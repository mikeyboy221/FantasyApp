using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using FantasyApp.Services;
using FantasyApp.Data;
using FantasyApp.Data.Entities;

namespace FantasyApp.Pages.Tournament;

public class Draft
{
    public bool HasBeenDrafted { get; set; } = false;
    public string? UserId { get; set; }
    public int? Score { get; set; }
}

public class ScoreboardAndDraft
{
    public Dictionary<string, Models.Leaguepedia.PlayerScoreboard> Scoreboard { get; set; }
    public Dictionary<string, List<Draft>> DraftResults { get; set; }
}

public class ExpandedGame
{
    public string GameId;
    public string GameInMatch;

    public ScoreboardAndDraft Team1Scoreboard;
    public ScoreboardAndDraft Team2Scoreboard;
    public string Team1;
    public string Team2;
    public string Team1HeaderString;
    public string Team2HeaderString;
}

public class SelectedPlayer
{
    public string id;
    public string role;
}

public class UserScore
{
    public IdentityUser User;
    public TicketDraft TicketDraft;
    public Dictionary<string, int> Scores;
}

public class TournamentModel: PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IApiService _apiService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IDemoService _demoService;
    private readonly IPointsService _pointsService;

    public TournamentModel(
        ApplicationDbContext context,
        IApiService apiService,
        UserManager<IdentityUser> userManager,
        IDemoService demoService,
        IPointsService pointsService
    )
    {
        _context = context;
        _apiService = apiService;
        _userManager = userManager;
        _demoService = demoService;
        _pointsService = pointsService;
    }

    public required UserTournament? UserTournament;
    public required UserTournamentTicket? UserTournamentTicket;
    public required List<UserTournamentTicket> UserTournamentTickets;

    public required Models.Leaguepedia.Tournament Tournament;
    public required List<Models.Leaguepedia.TournamentMatch> Matches;

    [BindProperty]
    public required string UserId { get; set; }
    [BindProperty]
    public required string UserTournamentId { get; set; }
    [BindProperty]
    public required string TournamentId { get; set; }
    [BindProperty]
    public required string PageType { get; set; }

    public required int TournamentState;

    public ScoreboardAndDraft GetTeamScoreboardWithUserDraftResult(Dictionary<string, List<IdentityUser>> playerIdsAndUsers, List<string> playersList, List<Models.Leaguepedia.PlayerScoreboard> scoreboard)
    {
        // Construct scoreboard from matchgame scoreboard and team player list
        var teamScoreboard = new Dictionary<string, Models.Leaguepedia.PlayerScoreboard>()
        {
            { "Top", scoreboard.First(p => p.Role == "Top" && playersList.Contains(p.PlayerName)) },
            { "Jungle", scoreboard.First(p => p.Role == "Jungle" && playersList.Contains(p.PlayerName)) },
            { "Mid", scoreboard.First(p => p.Role == "Mid" && playersList.Contains(p.PlayerName)) },
            { "Bot", scoreboard.First(p => p.Role == "Bot" && playersList.Contains(p.PlayerName)) },
            { "Support", scoreboard.First(p => p.Role == "Support" && playersList.Contains(p.PlayerName)) }
        };

        // Initialize a new draft result for game
        var draftResults = new Dictionary<string, List<Draft>>();

        // Iterate through team scoreboard and assign draft result
        foreach (var roleAndPlayer in teamScoreboard)
        {
            var role = roleAndPlayer.Key;
            var playerId = roleAndPlayer.Value.PlayerName;

            draftResults[role] = [];

            // If player id is found in the drafted players table
            if (playerIdsAndUsers.ContainsKey(playerId))
            {
                // Append results for all users
                foreach (var user in playerIdsAndUsers[playerId])
                {
                    draftResults[role].Add(new Draft()
                    {
                        HasBeenDrafted = true,
                        UserId = playerIdsAndUsers[playerId][0].Id,
                        Score = 100
                    });
                }
            }
        }

        return new ScoreboardAndDraft()
        {
            Scoreboard = teamScoreboard,
            DraftResults = draftResults
        };
    }

    public async Task<IActionResult> OnGetAsync(string userTournamentId, string tournamentId, string pageType)
    {
        //Return user to login page if not logged in
        IdentityUser? user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        UserId = user.Id;
        UserTournamentId = userTournamentId;
        TournamentId = tournamentId;
        PageType = pageType;

        //Get user tournament
        UserTournament = await _context.UserTournament
            .FirstOrDefaultAsync(ut => ut.Id == userTournamentId);

        if (UserTournament == null)
        {
            return RedirectToPage("/Account/Dashboard");
        }

        //Get tournament information from middleware
        Tournament = await _apiService.GetLeaguepediaTournament(tournamentId);
        Matches = await _apiService.GetMatchScheduleForTournament(tournamentId);

        var currentDate = _demoService.GetCurrentDate();
        var tournamentHasStarted = DateOnly.FromDateTime(currentDate) > Tournament.DateStart;
        var tournamentHasFinished = Matches[Matches.Count - 1].HasFinished;

        TournamentState = tournamentHasStarted ? (tournamentHasFinished ? 2 : 1) : 0;

        //Check if user has ticket
        UserTournamentTicket = _context.UserTournamentTicket
            .FirstOrDefault(utt => utt.UserId == UserId && utt.UserTournamentId == UserTournamentId);

        UserTournamentTickets = _context.UserTournamentTicket
            .Where(utt => utt.UserTournamentId == UserTournamentId)
            .ToList();

       return Page();
    }

    public async Task<IActionResult> OnGetUserScores(string userTournamentId, string tournamentId)
    {
        //Get tournament games
        var _tournamentGames = await _apiService.GetTournamentGames(tournamentId);

        // Get UserTournamentTickets
        var userTournamentTickets = _context.UserTournamentTicket
            .Include(utt => utt.UserTournament) // load related UserTournament
            .Include(utt => utt.TicketDraft) // load related TicketDraft
            .Where(utt => utt.UserTournamentId == userTournamentId)
            .ToList();

        // Get ticket drafts for given tickets
        var _usersAndDrafts = userTournamentTickets
            .Select(utt => new UserScore
            {
                User = _context.Users.FirstOrDefault(u => u.Id == utt.UserId),
                TicketDraft = utt.TicketDraft,
                Scores = []
            })
            .ToArray();

        // Get ticket drafts for given tickets
        foreach (var userAndDraft in _usersAndDrafts)
        {
            var userDraft = new List<string> {
                userAndDraft.TicketDraft.TopPlayerId,
                userAndDraft.TicketDraft.JglPlayerId,
                userAndDraft.TicketDraft.MidPlayerId,
                userAndDraft.TicketDraft.BotPlayerId,
                userAndDraft.TicketDraft.SupPlayerId
            };

            userAndDraft.Scores = _pointsService.ProcessPlayerDraft(userDraft, _tournamentGames);
        }

        return Partial("_UserScoresPartial", _usersAndDrafts);
    }

    public async Task<IActionResult> OnGetExpandMatch(string matchId)
    {
        var matchGames = await _apiService.GetMatchGames(matchId);
        var userDrafts = await _context.TicketDraft
            .Include(td => td.UserTournamentTicket)
            .Where(d => d.UserTournamentTicket.UserTournamentId == UserTournamentId)
            .ToListAsync();

        var draftedPlayers = new Dictionary<string, List<IdentityUser>>();

        foreach (var ticketDraft in userDrafts)
        {
            var user = await _userManager.FindByIdAsync(ticketDraft.UserTournamentTicket.UserId);
            if (user == null)
            {
                continue;
            }

            var playerDraft = new List<string>()
            {
                ticketDraft.TopPlayerId,
                ticketDraft.JglPlayerId,
                ticketDraft.MidPlayerId,
                ticketDraft.BotPlayerId,
                ticketDraft.SupPlayerId
            };

            // Gets users selected players from ticket draft
            // Appeneds user id to drafted player in expanded game summary
            // Used to calculate the score obtained from each game
            // There is an error here if the rosters change between game rounds (Unsure if that is possible yet or not)

            foreach (var playerId in playerDraft)
            {
                var matchGame = matchGames[0];

                var draftedPlayerScoreboard = matchGame.Scoreboard.Find(p => p.PlayerName == playerId);
                if (draftedPlayerScoreboard != null)
                {
                    if (!draftedPlayers.ContainsKey(playerId))
                    {
                        draftedPlayers.Add(playerId, new List<IdentityUser>());
                    }

                    draftedPlayers[playerId].Add(user);
                }
            }
        }

        var expandedGames = new List<ExpandedGame>();

        foreach (var matchGame in matchGames)
        {
            var team1HeaderString = "";
            var team2HeaderString = "";

            if (matchGame.WinTeam == matchGame.Team1)
            {
                team1HeaderString = "(Victory)";
                team2HeaderString = "(Defeat)";
            }
            else
            {
                team1HeaderString = "(Defeat)";
                team2HeaderString = "(Victory)";
            }

            var team1Scoreboard = GetTeamScoreboardWithUserDraftResult(draftedPlayers, matchGame.Team1PlayersList, matchGame.Scoreboard);
            var team2Scoreboard = GetTeamScoreboardWithUserDraftResult(draftedPlayers, matchGame.Team2PlayersList, matchGame.Scoreboard);

            expandedGames.Add(new ExpandedGame()
            {
                GameId = matchGame.GameId,
                GameInMatch = matchGame.N_GameInMatch,

                Team1Scoreboard = team1Scoreboard,
                Team2Scoreboard = team2Scoreboard,

                Team1 = matchGame.Team1,
                Team1HeaderString = team1HeaderString,

                Team2 = matchGame.Team2,
                Team2HeaderString = team2HeaderString
            });
        }


        // foreach(var playerAndUsers in draftedPlayers)
        // {
        //     Console.WriteLine(playerAndUsers.Key);
        //     Console.WriteLine(playerAndUsers.Value.Count);
        // }

        return Partial("_ExpandedGamePartial", expandedGames);
    }

    public async Task<IActionResult> OnPostJoinTournamentAsync()
    {
        //Ensure Tournament has not begun
        var _tournament = await _apiService.GetLeaguepediaTournament(TournamentId);
        var currentDate = _demoService.GetCurrentDate();
        var tournamentHasStarted = DateOnly.FromDateTime(currentDate) > _tournament.DateStart;

        if (tournamentHasStarted)
        {
            return BadRequest();
        }


        // Check if user already has a ticket for this tournament
        var ticket = await _context.UserTournamentTicket
            .FirstOrDefaultAsync(t => t.UserId == UserId && t.UserTournamentId == UserTournamentId);

        if (ticket != null)
        {
            return BadRequest();
        }


        //Ensure UserTournament has not already reached max participants
        var userTournament = await _context.UserTournament
            .FirstOrDefaultAsync(ut => ut.Id == UserTournamentId);

        if (userTournament != null)
        {
            // Check if the tournament has reached the maximum number of participants
            int currentParticipants = await _context.UserTournamentTicket
                .CountAsync(t => t.UserTournamentId == UserTournamentId);

            if (currentParticipants >= userTournament.MaxParticipants)
            {
                // Tournament has reached the maximum number of participants
                return BadRequest();
            }
        }

        // If we reach here then we can create a new ticket for the user
        _context.UserTournamentTicket.Add(new UserTournamentTicket
        {
            Id = Guid.NewGuid().ToString(),
            UserId = UserId,
            UserTournamentId = UserTournamentId
        });
        await _context.SaveChangesAsync();


        return RedirectToPage("/Tournament/Tournament", new { userTournamentId = UserTournamentId, tournamentId = TournamentId, pageType = "live" });
    }

    public async Task<IActionResult> OnPostCreateDraftAsync()
    {
        //Return not found error if user is not logged in
        IdentityUser? user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
        {
            return NotFound();
        }

        //Ensure user has tournament ticket
        var userTournamentTicket = await _context.UserTournamentTicket
           .FirstOrDefaultAsync(t => t.UserId == user.Id && t.UserTournamentId == UserTournamentId);

        if (userTournamentTicket == null)
        {
            return BadRequest();
        }

        //Get selected players from request body
        List<SelectedPlayer> SelectedPlayers = JsonConvert.DeserializeObject<List<SelectedPlayer>>(await new StreamReader(Request.Body).ReadToEndAsync()) ?? [];

        var ticketDraft = new TicketDraft()
        {
            UserTournamentTicketId = userTournamentTicket.Id,
            TopPlayerId = SelectedPlayers.FirstOrDefault(p => p.role == "Top").id,
            MidPlayerId = SelectedPlayers.FirstOrDefault(p => p.role == "Mid").id,
            JglPlayerId = SelectedPlayers.FirstOrDefault(p => p.role == "Jungle").id,
            BotPlayerId = SelectedPlayers.FirstOrDefault(p => p.role == "Bot").id,
            SupPlayerId = SelectedPlayers.FirstOrDefault(p => p.role == "Support").id
        };

        //Write to database and save changes
        _context.TicketDraft.Add(ticketDraft);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Tournament/Tournament", new { userTournamentId = UserTournamentId, tournamentId = TournamentId, pageType = "live" });
    }
}