using Microsoft.AspNetCore.Mvc;
using FantasyApp.Services;

namespace FantasyApp.ViewComponents;

[ViewComponent(Name = "UpcomingMatches")]
public class UpcomingMatchesViewComponent : ViewComponent
{
    private readonly IApiService _apiService;

    public UpcomingMatchesViewComponent(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var upcomingMatches = await _apiService.GetPandaUpcomingMatches(3, 1);
        return View(upcomingMatches);
    }
}