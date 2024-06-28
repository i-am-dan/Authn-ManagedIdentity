using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace UI.Controllers;

// [Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    //It ensures that the user is asked for consent if needed, and incrementally.
    //[AuthorizeForScopes(Scopes = new string[] {"api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default"})]
    // [AuthorizeForScopes(Scopes = new string[] {"user.read"})]
    public async Task<IActionResult> Index()
    {   
        //If we want to use scope and use access_token
        //https://learn.microsoft.com/en-us/azure/devops/integrate/get-started/authentication/service-principal-managed-identity?view=azure-devops
        var credential = new DefaultAzureCredential(
        new DefaultAzureCredentialOptions
        {
            TenantId = "9d26d957-ceda-49e0-b1b0-5d29bf8b5419",
            ManagedIdentityClientId = "3887e849-de36-4e50-af35-156171d0e9a5"
        });
        
        var token = await credential.GetTokenAsync(new Azure.Core.TokenRequestContext(new[] { "cb7a0fe4-34a0-4f5e-b9d3-da3e4d1aa3ee/.default" }));
    

        // Use the access token to call a protected web API.
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);        


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

    // public static async Task<TokenResponse> GetToken()  
    // {
    //     //https://learn.microsoft.com/en-us/azure/app-service/overview-managed-identity?tabs=portal%2Chttp
    //     var resource_uri = "cb7a0fe4-34a0-4f5e-b9d3-da3e4d1aa3ee/.default";
    //     var client_id = "3887e849-de36-4e50-af35-156171d0e9a5";
    //     var identity_header = Environment.GetEnvironmentVariable("IDENTITY_HEADER");
    //     var identity_endpoint = Environment.GetEnvironmentVariable("IDENTITY_ENDPOINT");
    //     var tokenURI = identity_endpoint + "?resource=" + resource_uri + "&client_id="+ client_id + "&api-version=2019-08-01";


    //     HttpClient client = new HttpClient();   
    //     client.DefaultRequestHeaders.Add("X-IDENTITY-HEADER", identity_header);
    //     var response = await client.GetStringAsync(String.Format("{0}/MSI/token?resource={1}&api-version={2}&clientid={3}", identity_endpoint, resource_uri, "2019-08-01", client_id));
        
    //     var options = new JsonSerializerOptions
    //     {
    //         PropertyNameCaseInsensitive = true
    //     };
        

    //     TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(response, options);

    //     try {
    //         return tokenResponse;
    //     }
    //     catch (HttpRequestException) // Non success
    //     {
    //         Console.WriteLine("An error occurred.");
    //     }
    //     catch (NotSupportedException) // When content type is not valid
    //     {
    //         Console.WriteLine("The content type is not supported.");
    //     }
    //     catch (JsonException) // Invalid JSON
    //     {
    //         Console.WriteLine("Invalid JSON.");
    //     }

    //     return null;
    // }
}

public class TokenResponse {
    public String? Access_token {get; set;}
}