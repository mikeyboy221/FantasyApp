using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using FantasyApp.Services;
using System.Drawing.Printing;
using FantasyApp.Models;

namespace FantasyApp.Pages;

public class LookupModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IApiService _apiService;

    public LookupModel(ILogger<IndexModel> logger, IApiService apiService)
    {
        _logger = logger;
        _apiService = apiService;
    }

    public bool ShowModal { get; set; }
    public int Year { get; set; }
    public string? TournamentId { get; set;}
    public List<Models.Leaguepedia.TournamentRoster_Player> TournamentPlayers { get; set; } = [];

    public Models.Leaguepedia.PlayerStats PlayerStats { get; set; }

    public List<int> Years { get; set; }
    public List<Models.Leaguepedia.Tournament> Tournaments { get; set; }

    public async Task<IActionResult> OnGetAsync(int? year, string? tournamentId, string? playerName)
    {
        //Get years from current year -1 (Skip current year because season will not have ended)
        Years = [];
        for (var dt=DateTime.Now.AddYears(-1); dt.Year >= 2011; dt = dt.AddYears(-1))
        {
            Years.Add(dt.Year);
        }

        //Set current year & tournament
        Year = year ?? Years[0];

        //Get tournaments for the set year
        Tournaments = await _apiService.GetLeaguepediaUpcomingTournaments(new DateOnly(Year, 1, 1));


        if (tournamentId != null)
        {
            TournamentId = tournamentId;
            TournamentPlayers = await _apiService.GetLeaguepediaTournamentRoster(TournamentId);

            if (playerName != null)
            {
                ShowModal = true;
                PlayerStats = await _apiService.GetPlayerStats(playerName, tournamentId);
            }
            else
            {
                ShowModal = false;
            }
        }


        return Page();
    }
}
