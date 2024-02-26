using Microsoft.AspNetCore.Mvc;

using FantasyApp.Services;

namespace FantasyApp.ViewComponents;

[ViewComponent(Name = "CreateDraft")]
public class CreateDraftViewComponent : ViewComponent
{
    private readonly IApiService _apiService;

    public CreateDraftViewComponent(IApiService apiService)
    {
        _apiService = apiService;
    }

    public class CreateDraftModel
    {
        public required string TournamentId;
        public required string UserTournamentId;
        public List<Models.Leaguepedia.TournamentRoster_Player> TournamentRoster = new();
        public Dictionary<string, string> Roles = new Dictionary<string, string>
        {
            { "Top", "TOP" },
            { "Mid", "MID" },
            { "Jungle", "JGL" },
            { "Bot", "ADC" },
            { "Support", "SUP"}
        };
    }

    public async Task<IViewComponentResult> InvokeAsync(string tournamentId, string userTournamentId)
    {
         // Initialize a dictionary to store players by their roles
        var rosters = await _apiService.GetLeaguepediaTournamentRoster(tournamentId);

        return View(new CreateDraftModel()
        {
            TournamentId = tournamentId,
            UserTournamentId = userTournamentId,
            TournamentRoster = rosters
        });
    }
}