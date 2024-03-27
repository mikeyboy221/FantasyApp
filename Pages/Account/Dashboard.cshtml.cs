using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using FantasyApp.Services;
using FantasyApp.Models;
using FantasyApp.Data;
using FantasyApp.Data.Entities;
using NuGet.Protocol.Core.Types;


namespace FantasyApp.Pages.Account;

public class DashboardModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly IDemoService _demoService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

    public DashboardModel(
        IApiService apiService,
        IDemoService demoService,
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _apiService = apiService;
        _demoService = demoService;
        _context = context;
        _userManager = userManager;
    }

    public IList<UserTournament> JoinedUserTournaments { get; set; } = default!;
    public IList<UserTournament> UserTournaments { get; set; } = default!;
    public List<Models.Leaguepedia.Tournament> UpcomingTournaments { get; set; }

    [BindProperty]
    public CreateTournamentForm CreateUserTournamentInput { get; set; }

    public class CreateTournamentForm
    {
        public required string TournamentId { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = null!;
        public string Password { get; set; } = null!;
        // public DateTime ScoringStartDate { get; set; }
        // public DateTime ScoringEndDate { get; set; }
    }

    private static string GetDayWithSuffix(int day)
    {
        if (day >= 11 && day <= 13)
        {
            return $"{day}th";
        }

        switch (day % 10)
        {
            case 1:
                return $"{day}st";
            case 2:
                return $"{day}nd";
            case 3:
                return $"{day}rd";
            default:
                return $"{day}th";
        }
    }

    public string GetHeaderDateString()
    {
        DateOnly currentDate = DateOnly.FromDateTime(_demoService.GetCurrentDate());

        return $"Today is the {GetDayWithSuffix(currentDate.Day)} of {currentDate:MMMM}";
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;

        var currentDate = DateOnly.FromDateTime(_demoService.GetCurrentDate());

        UpcomingTournaments = await _apiService.GetLeaguepediaUpcomingTournaments(currentDate);
        UserTournaments = await _context.UserTournament.ToListAsync();

        JoinedUserTournaments = await _context.UserTournament
            .Include(ut => ut.UserTournamentTickets)
            .Join(
                _context.UserTournamentTicket,
                ut => ut.Id,
                utt => utt.UserTournamentId,
                (ut, utt) => new { UserTournament = ut, UserTournamentTicket = utt }
            )
            .Where(joined => joined.UserTournamentTicket.UserId == userId)
            .Select(joined => joined.UserTournament)
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostCreateUserTournamentAsync()
    {
        if (ModelState.IsValid)
        {

            var user = await GetCurrentUserAsync();
            var userId = user.Id;
            var tournamentOverviewPage = CreateUserTournamentInput.TournamentId;

            var userTournament = new UserTournament
            {
                Id = Guid.NewGuid().ToString(),
                HostId = userId,
                TournamentId = tournamentOverviewPage,
                Name = CreateUserTournamentInput.Name,
                Description = CreateUserTournamentInput.Description,
                Password = CreateUserTournamentInput.Password,
                MaxParticipants = 10
            };

            _context.UserTournament.Add(userTournament);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Dashboard");
        }

        Console.WriteLine("Invalid model state");
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        foreach (var error in errors)
        {
            Console.WriteLine(error.ErrorMessage);
        }

        return RedirectToPage("/Account/Dashboard");
    }
}