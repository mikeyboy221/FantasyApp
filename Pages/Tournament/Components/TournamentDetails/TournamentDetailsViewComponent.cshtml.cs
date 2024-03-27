using Microsoft.AspNetCore.Mvc;

namespace FantasyApp.ViewComponents;

public class TournamentDetailsViewComponent : ViewComponent
{
    public TournamentDetailsViewComponent()
    {
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}