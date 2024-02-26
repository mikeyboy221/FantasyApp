using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using FantasyApp.Models;
using FantasyApp.Services;
using FantasyApp.Data;
using FantasyApp.Data.Entities;


namespace FantasyApp.ViewComponents;

[ViewComponent(Name = "TournamentLive")]
public class TournamentLiveViewComponent : ViewComponent
{

    private readonly ApplicationDbContext _context;
    private readonly IApiService _apiService;
    private readonly IDemoService _demoService;
    private readonly UserManager<IdentityUser> _userManager;

    private Task<IdentityUser?> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

    public TournamentLiveViewComponent(
        ApplicationDbContext context,
        IApiService apiService,
        IDemoService demoService,
        UserManager<IdentityUser> userManager
    )
    {
        _context = context;
        _apiService = apiService;
        _demoService = demoService;
        _userManager = userManager;
    }

    public class ViewModel
    {
        public string UserTournamentId;
        public string TournamentId;

        public bool UserHasTicket = true;
        public bool UserHasDraft = true;
        public bool HasFinished = false;

        public Tournament Tournament = default!;
        public UserAndDraft[] UsersAndDrafts;
        public Dictionary<string, Models.Leaguepedia.TournamentRoster_Player> UserDraft;
        public List<Models.Leaguepedia.TournamentMatch> Matches = default!;
    }

    public class UserAndDraft
    {
        public IdentityUser User;
        public TicketDraft TicketDraft;
    }

    public class Match
    {
        public Team Team1;
        public Team Team2;
        public int Id;
        public DateTime ScheduledAt;
        public string MatchTime;
        public string MatchDate;
    }

    public class Team
    {
        public string TeamName;
        public string TeamAcronym;
        public string TeamImage;
        public Dictionary<string, Tournament_Player> Players = new Dictionary<string, Tournament_Player>();
    }

    public async Task<IViewComponentResult> InvokeAsync(string tournamentId, string userTournamentId)
    {
        var user = await GetCurrentUserAsync();
        var userId = user.Id;

        var ticket = await _context.UserTournamentTicket
            .FirstOrDefaultAsync(t => t.UserId == userId && t.UserTournamentId == userTournamentId);

        if (ticket == null)
        {
            return View(new ViewModel() { UserHasTicket = false });
        }

        // Check if player has drafted a team
        TicketDraft? TicketDraft = await _context.TicketDraft
            .FirstOrDefaultAsync(t => t.UserTournamentTicketId == ticket.Id);

        if (TicketDraft == null)
        {
            return View(new ViewModel()
            {
                UserHasDraft = false,
                UserTournamentId = userTournamentId,
                TournamentId = tournamentId
            });
        }

        // Get all tournament tickets
        var userTournamentTickets = _context.UserTournamentTicket
            .Include(utt => utt.UserTournament) // load related UserTournament
            .Include(utt => utt.TicketDraft) // load related TicketDraft
            .Where(utt => utt.UserTournamentId == userTournamentId)
            .ToList();

        // Get userIds and ticketDrafts from UserTournamentTickets
        var usersAndTicketDrafts = userTournamentTickets
            .Select(utt => new UserAndDraft
            {
                User = _context.Users.FirstOrDefault(u => u.Id == utt.UserId),
                TicketDraft = utt.TicketDraft
            })
            .ToArray();


        // Get players in draft
        var draftedPlayerIds = new Dictionary<string, string>()
        {
            {"Top", TicketDraft.TopPlayerId},
            {"Jgl", TicketDraft.JglPlayerId},
            {"Mid", TicketDraft.MidPlayerId},
            {"Bot", TicketDraft.BotPlayerId},
            {"Sup", TicketDraft.SupPlayerId}
        };

        var roster = await _apiService.GetLeaguepediaTournamentRoster(tournamentId);
        var userDraft = new Dictionary<string, Models.Leaguepedia.TournamentRoster_Player>();

        roster.ForEach(p =>
        {
            foreach (var playerId in draftedPlayerIds)
            {
                if (p.ID == playerId.Value)
                {
                    userDraft.Add(playerId.Key, p);
                }
            }
        });

        // Get tournament matches
        var matches = await _apiService.GetMatchScheduleForTournament(tournamentId);
        var currentDate = _demoService.GetCurrentDate();

        matches.ForEach(m =>
        {
            if (currentDate > m.DateTime_UTC)
            {
                m.HasFinished = true;
            }

            // Console.WriteLine(m.Team1+" vs "+m.Team2+" @ "+m.DateTime_UTC.ToString("yyyy-MM-dd h:mm:ss"));
        });

        return View(new ViewModel()
        {
            UserTournamentId = userTournamentId,
            TournamentId = tournamentId,

            // Tournament = tournament,
            UsersAndDrafts = usersAndTicketDrafts,
            UserDraft = userDraft,
            Matches = matches,
        });
    }
}