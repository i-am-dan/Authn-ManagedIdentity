using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.AppConfig;
using Microsoft.Identity.Web;
using UI.Models;
using Azure.Identity;
using Azure.Core;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;

namespace UI.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ITokenAcquisition _tokenAcquisition;

    private readonly ILogger<HomeController> _logger;
    
    //private readonly IDownstreamApi _downstreamApi;

    // private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(ILogger<HomeController> logger, ITokenAcquisition tokenAcquisition)
    {
        _logger = logger;
        _tokenAcquisition = tokenAcquisition;
        // _httpClientFactory = httpClientFactory;
        //_downstreamApi = downstreamApi;
    }
    
    [AuthorizeForScopes(Scopes = new string[] {"api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default"})]
    public async Task<IActionResult> Index()
    {
        /*This is trying managed identity*/
        //var token = await GetAccessTokenAsync("api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d");
        
        /*Client Secret*/
        var token = await _tokenAcquisition.GetAccessTokenForUserAsync(new string[] {"api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default"});
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);        
        
        /*Managed Identity*/
        // var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions{
        //     ExcludeEnvironmentCredential = true,
        //     ExcludeManagedIdentityCredential = true,
        //     ExcludeVisualStudioCredential = true,
        //     ExcludeAzureCliCredential = true,
        //     ExcludeAzurePowerShellCredential = true,
        //     ExcludeSharedTokenCacheCredential=true
        // });


        //var credential = new DefaultAzureCredential();
        //string[] scopes = new string[] { "api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default" };
 

        // // Define the scope of the token
        //string[] scopes = new string[] { "api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default" };
        string[] scopes = new string[] { "api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default" };

        // // Get the token
        //var accessToken = await credential.GetTokenAsync(new Azure.Core.TokenRequestContext(scopes));

        /*Local*/
        //var jsonData = await httpClient.GetStringAsync("https://api-azureauthsample.azurewebsites.net/WeatherForecast");
        var jsonData = await httpClient.GetStringAsync("https://localhost:7111/WeatherForecast");

          
        ViewData["ApiResult"] = jsonData;
        
        return View();
    }

    private static IManagedIdentityApplication CreateManagedIdentityApplication()
    {
           
        return ManagedIdentityApplicationBuilder.Create(ManagedIdentityId.SystemAssigned).Build();
    
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
