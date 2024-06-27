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

[Authorize]
public class HomeController : Controller
{
    //private readonly ITokenAcquisition _tokenAcquisition;

    private readonly ILogger<HomeController> _logger;
    private readonly ITokenAcquisition _tokenAcquisition;

    public HomeController(ILogger<HomeController> logger, ITokenAcquisition tokenAcquisition)
    {
        _logger = logger;
        _tokenAcquisition = tokenAcquisition;
    }
    
    [AuthorizeForScopes(Scopes = new string[] {"api://cb7a0fe4-34a0-4f5e-b9d3-da3e4d1aa3ee/.default"})]
    public async Task<IActionResult> Index()
    {    
        //api://e13b8721-0e2f-4158-8e01-e93c0e97041e/user_impersonation
        /*Client Secret*/
        string[] scopes = new string[] { "api://cb7a0fe4-34a0-4f5e-b9d3-da3e4d1aa3ee/.default" };
        //var userAccessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] {"user.read"});

        // var msiClientId = "3887e849-de36-4e50-af35-156171d0e9a5";
        // var aadAppClientId = "cb7a0fe4-34a0-4f5e-b9d3-da3e4d1aa3ee";
        // var resourceTenantId = "9d26d957-ceda-49e0-b1b0-5d29bf8b5419";

        // ClientAssertionCredential assertion = new ClientAssertionCredential(resourceTenantId, 
        // aadAppClientId, new ManagedIdentityClientAssertion(msiClientId).GetSignedAssertion);

        // var credential = new DefaultAzureCredential();
     
        // var tokenExchangeRequest = new Azure.Core.TokenRequestContext(scopes);
        // var token = await credential.GetTokenAsync(tokenExchangeRequest);
        //Console.WriteLine(token);
        var token = await _tokenAcquisition.GetAccessTokenForUserAsync(new string[] {"api://cb7a0fe4-34a0-4f5e-b9d3-da3e4d1aa3ee/.default"});
        HttpClient httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);        
    

        // string[] scopes = new string[] { "api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default" };
 

        // // Define the scope of the token
        //string[] scopes = new string[] { "api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default" };

        

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
