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
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace UI.Controllers;

// [Authorize]
public class HomeController : Controller
{
    //private readonly ITokenAcquisition _tokenAcquisition;

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    // [AuthorizeForScopes(Scopes = new string[] {"api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default"})]
    public async Task<IActionResult> Index()
    {     
        /*This is trying managed identity*/
        // var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions{
        //     ExcludeEnvironmentCredential = true,
        //     ExcludeManagedIdentityCredential = true,
        //     ExcludeVisualStudioCredential = true,
        //     ExcludeAzurePowerShellCredential = true,
        //     ExcludeSharedTokenCacheCredential= true,
        //     ExcludeWorkloadIdentityCredential= true,
        //     ExcludeAzureCliCredential = true,
        //     ExcludeAzureDeveloperCliCredential = true
        // });

        //var atoken = await credential.GetTokenAsync(new Azure.Core.TokenRequestContext(new string[] {"api://e13b8721-0e2f-4158-8e01-e93c0e97041e/user_impersonation"}));
        //var actoken = atoken.ToString();
        
        /*Client Secret*/
        //var token = await _tokenAcquisition.GetAccessTokenForUserAsync(new string[] {"api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default"});
        HttpClient httpClient = new HttpClient();
        //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", actoken);        
    

        //var credential = new DefaultAzureCredential();
        //string[] scopes = new string[] { "api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default" };
 

        // // Define the scope of the token
        //string[] scopes = new string[] { "api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default" };
        //string[] scopes = new string[] { "api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default" };

        // Get the token
        //var accessToken = await credential.GetTokenAsync(new Azure.Core.TokenRequestContext());

        /*Local*/
        var jsonData = await httpClient.GetStringAsync("https://api-azureauthsample.azurewebsites.net/WeatherForecast");
        //var jsonData = await httpClient.GetStringAsync("https://localhost:7111/WeatherForecast");
          
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
