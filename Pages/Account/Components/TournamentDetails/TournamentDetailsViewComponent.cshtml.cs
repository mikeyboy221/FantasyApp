using Microsoft.AspNetCore.Mvc;

namespace FantasyApp.ViewComponents;

[ViewComponent(Name = "TournamentDetails")]
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