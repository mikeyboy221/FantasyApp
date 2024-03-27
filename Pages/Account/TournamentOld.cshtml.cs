// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Newtonsoft.Json;


// using FantasyApp.Models;
// using FantasyApp.Services;
// using FantasyApp.Data;
// using FantasyApp.Data.Entities;
// using Microsoft.Data.Sqlite;

// namespace FantasyApp.Pages.Account;

// public class TournamentModel : PageModel
// {
//     private readonly ApplicationDbContext _context;
//     private readonly IApiService _apiService;
//     private readonly UserManager<IdentityUser> _userManager;
//     private readonly IDemoService _demoService;

//     public TournamentModel(
//         ApplicationDbContext context,
//         IApiService apiService,
//         UserManager<IdentityUser> userManager,
//         IDemoService demoService,
//     )
//     {
//         _context = context;
//         _apiService = apiService;
//         _userManager = userManager;
//         _demoService = demoService;
//     }

//     private Task<IdentityUser?> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

//     public UserTournament? UserTournament;
//     public Models.Leaguepedia.Tournament Tournament;
//     public List<IdentityUser> UsersInTournament;

//     [BindProperty]
//     public required string UserId { get; set; }
//     [BindProperty]
//     public required string UserTournamentId { get; set; }
//     [BindProperty]
//     public required string TournamentId { get; set; }
//     public required string PageType { get; set; }

//     public async Task<IActionResult> OnGetAsync(string userTournamentId, string tournamentId, string pageType)
//     {
//         var user = await GetCurrentUserAsync();
//         if (user == null)
//         {
//             return NotFound();
//         }

//         UserId = user.Id;
//         UserTournamentId = userTournamentId;
//         TournamentId = tournamentId;
//         PageType = pageType;

//         string userQuery = @"
//             SELECT AspNetUsers.*
//             FROM AspNetUsers
//             JOIN UserTournamentTicket ON AspNetUsers.Id = UserTournamentTicket.UserId
//             WHERE UserTournamentTicket.UserTournamentId = @UserTournamentId";

//         UsersInTournament = await _context.Users
//             .FromSqlRaw(userQuery, new SqliteParameter("@UserTournamentId", UserTournamentId))
//             .ToListAsync();

//         UserTournament = await _context.UserTournament
//             .FirstOrDefaultAsync(m => m.Id == UserTournamentId);

//         if (UserTournament == null)
//         {
//             return NotFound();
//         }

//         Tournament = await _apiService.GetLeaguepediaTournament(TournamentId);

//         var currentDate = _demoService.GetCurrentDate();


//         if (DateOnly.FromDateTime(currentDate) < Tournament.DateStart)
//         {
//             tournamentState = -1;
//         } elseif (DateOnly.FromDateTime(currentDate) > Tournament.)

//         if (Tournament == null)
//         {
//             return NotFound();
//         }

//         return Page();
//     }

//     public class ExpandedGame
//     {
//         public string GameId;
//         public string GameInMatch;

//         public ScoreboardAndDraft Team1Scoreboard;
//         public ScoreboardAndDraft Team2Scoreboard;
//         public string Team1;
//         public string Team2;
//         public string Team1HeaderString;
//         public string Team2HeaderString;
//     }

//     public class ScoreboardAndDraft
//     {
//         public Dictionary<string, Models.Leaguepedia.PlayerScoreboard> Scoreboard { get; set; }
//         public Dictionary<string, List<Draft>> DraftResults { get; set; }
//     }

//     public class Draft
//     {
//         public bool HasBeenDrafted { get; set; } = false;
//         public string? UserId { get; set; }
//         public int? Score { get; set; }
//     }

//     public ScoreboardAndDraft GetTeamScoreboardWithUserDraftResult(Dictionary<string, List<IdentityUser>> playerIdsAndUsers, List<string> playersList, List<Models.Leaguepedia.PlayerScoreboard> scoreboard)
//     {
//         // Construct scoreboard from matchgame scoreboard and team player list
//         var teamScoreboard = new Dictionary<string, Models.Leaguepedia.PlayerScoreboard>()
//         {
//             { "Top", scoreboard.First(p => p.Role == "Top" && playersList.Contains(p.PlayerName)) },
//             { "Jungle", scoreboard.First(p => p.Role == "Jungle" && playersList.Contains(p.PlayerName)) },
//             { "Mid", scoreboard.First(p => p.Role == "Mid" && playersList.Contains(p.PlayerName)) },
//             { "Bot", scoreboard.First(p => p.Role == "Bot" && playersList.Contains(p.PlayerName)) },
//             { "Support", scoreboard.First(p => p.Role == "Support" && playersList.Contains(p.PlayerName)) }
//         };

//         // Initialize a new draft result for game
//         var draftResults = new Dictionary<string, List<Draft>>();

//         // Iterate through team scoreboard and assign draft result
//         foreach (var roleAndPlayer in teamScoreboard)
//         {
//             var role = roleAndPlayer.Key;
//             var playerId = roleAndPlayer.Value.PlayerName;

//             draftResults[role] = [];

//             // If player id is found in the drafted players table
//             if (playerIdsAndUsers.ContainsKey(playerId))
//             {
//                 // Append results for all users
//                 foreach (var user in playerIdsAndUsers[playerId])
//                 {
//                     draftResults[role].Add(new Draft()
//                     {
//                         HasBeenDrafted = true,
//                         UserId = playerIdsAndUsers[playerId][0].Id,
//                         Score = 100
//                     });
//                 }
//             }
//         }

//         return new ScoreboardAndDraft()
//         {
//             Scoreboard = teamScoreboard,
//             DraftResults = draftResults
//         };
//     }

//     public async Task<IActionResult> OnGetExpandMatch(string userTournamentId, string matchId)
//     {
//         var matchGames = await _apiService.GetMatchGames(matchId);
//         var userDrafts = await _context.TicketDraft
//             .Include(td => td.UserTournamentTicket)
//             .Where(d => d.UserTournamentTicket.UserTournamentId == userTournamentId)
//             .ToListAsync();



//         var draftedPlayers = new Dictionary<string, List<IdentityUser>>();

//         foreach (var ticketDraft in userDrafts)
//         {
//             var user = await _userManager.FindByIdAsync(ticketDraft.UserTournamentTicket.UserId);
//             if (user == null)
//             {
//                 continue;
//             }

//             var playerDraft = new List<string>()
//             {
//                 ticketDraft.TopPlayerId,
//                 ticketDraft.JglPlayerId,
//                 ticketDraft.MidPlayerId,
//                 ticketDraft.BotPlayerId,
//                 ticketDraft.SupPlayerId
//             };

//             // Gets users selected players from ticket draft
//             // Appeneds user id to drafted player in expanded game summary
//             // Used to calculate the score obtained from each game
//             // There is an error here if the rosters change between game rounds (Unsure if that is possible yet or not)

//             foreach (var playerId in playerDraft)
//             {
//                 var matchGame = matchGames[0];

//                 var draftedPlayerScoreboard = matchGame.Scoreboard.Find(p => p.PlayerName == playerId);
//                 if (draftedPlayerScoreboard != null)
//                 {
//                     if (!draftedPlayers.ContainsKey(playerId))
//                     {
//                         draftedPlayers.Add(playerId, new List<IdentityUser>());
//                     }

//                     draftedPlayers[playerId].Add(user);
//                 }
//             }
//         }

//         var expandedGames = new List<ExpandedGame>();

//         foreach (var matchGame in matchGames)
//         {
//             var team1HeaderString = "";
//             var team2HeaderString = "";

//             if (matchGame.WinTeam == matchGame.Team1)
//             {
//                 team1HeaderString = "(Victory)";
//                 team2HeaderString = "(Defeat)";
//             }
//             else
//             {
//                 team1HeaderString = "(Defeat)";
//                 team2HeaderString = "(Victory)";
//             }


//             var team1Scoreboard = GetTeamScoreboardWithUserDraftResult(draftedPlayers, matchGame.Team1PlayersList, matchGame.Scoreboard);
//             var team2Scoreboard = GetTeamScoreboardWithUserDraftResult(draftedPlayers, matchGame.Team2PlayersList, matchGame.Scoreboard);

//             expandedGames.Add(new ExpandedGame()
//             {
//                 GameId = matchGame.GameId,
//                 GameInMatch = matchGame.N_GameInMatch,

//                 Team1Scoreboard = team1Scoreboard,
//                 Team2Scoreboard = team2Scoreboard,

//                 Team1 = matchGame.Team1,
//                 Team1HeaderString = team1HeaderString,

//                 Team2 = matchGame.Team2,
//                 Team2HeaderString = team2HeaderString
//             });
//         }


//         foreach(var playerAndUsers in draftedPlayers)
//         {
//             Console.WriteLine(playerAndUsers.Key);
//             Console.WriteLine(playerAndUsers.Value.Count);
//         }

//         return Partial("_ExpandedGamePartial", expandedGames);
//     }

//     public async Task<IActionResult> OnPostAsync()
//     {
//         // Check if user already has a ticket for this tournament
//         var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";

//         // Print the URL to the console
//         Console.WriteLine($"Create draft. URL: {url}");

//         var ticket = await _context.UserTournamentTicket
//             .FirstOrDefaultAsync(t => t.UserId == UserId && t.UserTournamentId == UserTournamentId);
//         if (ticket != null)
//         {
//             Console.WriteLine("User already has ticket for this tournament");
//             return RedirectToPage("/Account/Tournament", new { userTournamentId = UserTournamentId, tournamentId = TournamentId, pageType = "live" });
//         }

//         // Create tournament ticket for user
//         _context.UserTournamentTicket.Add(new UserTournamentTicket
//         {
//             Id = Guid.NewGuid().ToString(),
//             UserId = UserId,
//             UserTournamentId = UserTournamentId
//         });
//         await _context.SaveChangesAsync();

//         return RedirectToPage("/Account/Tournament", new { userTournamentId = UserTournamentId, tournamentId = TournamentId, pageType = "live" });
//     }

//     public class SelectedPlayer
//     {
//         public string id;
//         public string role;
//     }

//     public async Task<IActionResult> OnPostCreateDraftAsync()
//     {
//         var user = await GetCurrentUserAsync();
//         if (user == null)
//         {
//             return NotFound();
//         }

//         string body = await new StreamReader(Request.Body).ReadToEndAsync();
//         Console.WriteLine($"Create draft. Body: {body}");

//         List<SelectedPlayer> SelectedPlayers = JsonConvert.DeserializeObject<List<SelectedPlayer>>(body) ?? [];

//         var userTournamentTicket = await _context.UserTournamentTicket
//            .FirstOrDefaultAsync(t => t.UserId == user.Id && t.UserTournamentId == UserTournamentId);

//         if (userTournamentTicket == null)
//         {
//             Console.WriteLine("User does not have ticket for this tournament");
//             return BadRequest();
//         }

//         var ticketDraft = new TicketDraft()
//         {
//             UserTournamentTicketId = userTournamentTicket.Id,
//             TopPlayerId = SelectedPlayers.FirstOrDefault(p => p.role == "Top").id,
//             MidPlayerId = SelectedPlayers.FirstOrDefault(p => p.role == "Mid").id,
//             JglPlayerId = SelectedPlayers.FirstOrDefault(p => p.role == "Jungle").id,
//             BotPlayerId = SelectedPlayers.FirstOrDefault(p => p.role == "Bot").id,
//             SupPlayerId = SelectedPlayers.FirstOrDefault(p => p.role == "Support").id
//         };

//         _context.TicketDraft.Add(ticketDraft);
//         await _context.SaveChangesAsync();

//         return RedirectToPage("/Account/Tournament", new { userTournamentId = UserTournamentId, tournamentId = TournamentId, pageType = "live" });
//     }

// }