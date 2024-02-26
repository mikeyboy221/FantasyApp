using Microsoft.AspNetCore.Mvc;
using FantasyApp.Services;
using System.Runtime.CompilerServices;

namespace FantasyApp.ViewComponents;

[ViewComponent(Name = "DemoHeader")]
public class DemoHeaderViewComponent : ViewComponent
{
    private readonly IDemoService _demoService;

    public DemoHeaderViewComponent(IDemoService demoService)
    {
        _demoService = demoService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(_demoService.GetCurrentDate());
    }
}