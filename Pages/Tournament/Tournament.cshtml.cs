using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using FantasyApp.Models;
using FantasyApp.Services;
using FantasyApp.Data;
using FantasyApp.Data.Entities;
using System.Text.RegularExpressions;

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

public class TournamentModel: PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IApiService _apiService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IDemoService _demoService;

    public TournamentModel(
        ApplicationDbContext context,
        IApiService apiService,
        UserManager<IdentityUser> userManager,
        IDemoService demoService
    )
    {
        _context = context;
        _apiService = apiService;
        _userManager = userManager;
        _demoService = demoService;
    }

    public required UserTournament? UserTournament;
    public required List<IdentityUser> Users;

    public required Models.Leaguepedia.Tournament Tournament;
    public required List<Models.Leaguepedia.TournamentMatch> Matches;

    public required string UserId;
    public required string UserTournamentId;
    public required string TournamentId;
    public required string PageType;

    public required int TournamentState;

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
        var tournamentHasFinished = Matches[Matches.Count-1].HasFinished;

        TournamentState = tournamentHasStarted ? (tournamentHasFinished ? 2 : 1) : 0;


        //Check if user has ticket
        var userTournamentTicket = _context.UserTournamentTicket
            .FirstOrDefault(utt => utt.UserId == UserId && utt.UserTournamentId == UserTournamentId);

        //Check if user has draft


       return Page();
    }
}