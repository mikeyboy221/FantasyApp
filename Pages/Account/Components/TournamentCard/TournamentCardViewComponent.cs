using Microsoft.AspNetCore.Mvc;

using FantasyApp.Data.Entities;
using FantasyApp.Services;

namespace FantasyApp.ViewComponents;

[ViewComponent(Name = "TournamentCard")]
public class TournamentCardViewComponent : ViewComponent
{
    private readonly IApiService _apiService;

    public TournamentCardViewComponent(IApiService apiService)
    {
        _apiService = apiService;
    }

    public class ViewModel
    {
        public Models.Leaguepedia.Tournament? Tournament { get; set; }
        public UserTournament? UserTournament { get; set; }
    }

    public async Task<IViewComponentResult> InvokeAsync(UserTournament userTournament)
    {
        Models.Leaguepedia.Tournament tournament = await _apiService.GetLeaguepediaTournament(userTournament.TournamentId);

        return View(new ViewModel
        {
            Tournament = tournament,
            UserTournament = userTournament
        });
    }
}