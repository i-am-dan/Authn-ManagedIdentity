using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using UI.Models;

namespace UI.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITokenAcquisition _tokenAcquisition;
    public HomeController(ILogger<HomeController> logger, ITokenAcquisition tokenAcquisition)
    {
        _logger = logger;
        _tokenAcquisition = tokenAcquisition;
    }

    [AuthorizeForScopes(Scopes = new string[] {"api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/Users.ReadWrite.All"})]
    public async Task<IActionResult> Index()
    {
        var token = await _tokenAcquisition.GetAccessTokenForUserAsync(new string[] {"api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/Users.ReadWrite.All"});
        HttpClient httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var jsonData = await httpClient.GetStringAsync("https://localhost:7198/WeatherForecast");

        ViewData["ApiResult"] = jsonData;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
