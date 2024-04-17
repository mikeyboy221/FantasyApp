using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using FantasyApp.Services;
using FantasyApp.Data;
using FantasyApp.Data.Entities;

namespace FantasyApp.ViewComponents;

public class TournamentLiveModel
{
    public bool UserHasTicket;
    public int TournamentState;
    public Dictionary<string, Models.Leaguepedia.TournamentRoster_Player>? UserTicketDraft;
    public string TournamentId;
    public string UserTournamentId;
    public UserAndDraft[] UsersAndDrafts;

    public List<Models.Leaguepedia.TournamentMatch> Matches;
    public Models.Leaguepedia.Tournament Tournament;
}

public class UserAndDraft
{
    public IdentityUser User;
    public TicketDraft TicketDraft;
}

public class TournamentLiveViewComponent: ViewComponent
{
    private readonly ApplicationDbContext _context;
    private readonly IApiService _apiService;
    private readonly IDemoService _demoService;
    private readonly UserManager<IdentityUser> _userManager;

    private Dictionary<string, Models.Leaguepedia.TournamentRoster_Player> _userTicketDraft = [];

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

    public async Task<IViewComponentResult> InvokeAsync(
        int tournamentState,
        string userId,
        UserTournament userTournament,

        Models.Leaguepedia.Tournament Tournament,
        List<Models.Leaguepedia.TournamentMatch> Matches
    )
    {
        var userTournamentTicket = await _context.UserTournamentTicket
            .FirstOrDefaultAsync(t => t.UserId == userId && t.UserTournamentId == userTournament.Id);

        if (userTournamentTicket != null)
        {
            TicketDraft? ticketDraft = await _context.TicketDraft
                .FirstOrDefaultAsync(t => t.UserTournamentTicketId == userTournamentTicket.Id);

            if (ticketDraft != null)
            {
                Dictionary<string, string> userDraftedPlayerIds = new()
                {
                    {"Top", ticketDraft.TopPlayerId},
                    {"Jgl", ticketDraft.JglPlayerId},
                    {"Mid", ticketDraft.MidPlayerId},
                    {"Bot", ticketDraft.BotPlayerId},
                    {"Sup", ticketDraft.SupPlayerId}
                };

                var roster = await _apiService.GetLeaguepediaTournamentRoster(userTournament.TournamentId);
                roster.ForEach(p =>
                {
                    foreach (var playerId in userDraftedPlayerIds)
                    {
                        if (p.ID == playerId.Value)
                        {
                            _userTicketDraft.Add(playerId.Key, p);
                        }
                    }
                });
            }
        }

        // Get all tournament tickets
        var userTournamentTickets = _context.UserTournamentTicket
            .Include(utt => utt.UserTournament) // load related UserTournament
            .Include(utt => utt.TicketDraft) // load related TicketDraft
            .Where(utt => utt.UserTournamentId == userTournament.Id)
            .ToList();

        // Get userIds and ticketDrafts from UserTournamentTickets
        var usersAndTicketDrafts = userTournamentTickets
            .Select(utt => new UserAndDraft
            {
                User = _context.Users.FirstOrDefault(u => u.Id == utt.UserId),
                TicketDraft = utt.TicketDraft
            })
            .ToArray();

        var currentDate = _demoService.GetCurrentDate();
        Matches.ForEach(m =>
        {
            if (currentDate > m.DateTime_UTC)
            {
                m.HasFinished = true;
            }
        });

        return View(new TournamentLiveModel {
            TournamentState = tournamentState,
            UserHasTicket = userTournamentTicket != null,
            UserTicketDraft = _userTicketDraft,
            TournamentId = userTournament.TournamentId,
            UserTournamentId = userTournament.Id,
            UsersAndDrafts = usersAndTicketDrafts,

            Matches = Matches,
            Tournament = Tournament
        });
    }
}