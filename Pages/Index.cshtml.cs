using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using FantasyApp.Services;

namespace FantasyApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IApiService _apiService;

    public IndexModel(ILogger<IndexModel> logger, IApiService apiService)
    {
        _logger = logger;
        _apiService = apiService;
    }
}
