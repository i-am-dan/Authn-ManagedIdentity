# AzureAuthSamples

## General Overview
This repository shows coding samples on how to implement OAuth2 patterns using Microsot Identity Services and Azure Active Directory.  There are two main programs:
- TestUI - Shows a sample web application which requires a user to sign in
- TestAPI - A sample web API which requires an access token from a signed in user

Prerequisites
- Azure Access (with permissions to create applications in Azure Active Directory)
- Visual Studio Code with C# extension

This code can be cloned and used directly one the Azure application regisrations are in place.  Optionally, instructions have been given which will allow the user to create these samples by hand.

For questions or comments, please contact: Joe Welch (jwelch@launchcg.com)

## Instructions On Setting Up Samples

## Instructions On Creating Samples By Hand

### Phase 1: Create and register UI

- Step 1: Create app registration in Azure for UI. 
    - Choose name (AzureAuthSampleUI for example)
    - Set account type to: Accounts in this organizational directory only
    - Create redirect UI: Web platform, https://localhost:{myport}/signin-oidc  where {myport} is your desired port (e.g. 54440)
    - Under Authentication tab, check both 'Access tokens' and 'ID tokens'
    - On the overview tab, make note of the tenant (or directory) id.  You'll need that later...
- Step 2: Generate a dotnet sample app with Auth.  Run: dotnet new webapp --auth MultiOrg
- Step 3: Modify app to connect to AAD
    - In the appsettings.json file, modify the AzureAd section to fill in the following information
        - Instance => https://login.microsoftonline.com/
        - TenantId => Tenant Id copied from step 1
        - ClientId => The client id of your registered app
        - CallbackPath => /signin-oidc
    - Modify Properties/launchSettings.json to ensure the https port is set to {myport} (i.e. the port you specified in Step 1)

    Run application locally!  You should either be logged in automatically or prompted to login.

### Phase 2: Create and register API for a downstream API Service

- Step 1: Create app registration for API
    - Provide name for registration, don't fill out redirect UI
    - Switch to Explose an API tab
        - Select 'Set' under Application ID URI.  Use the default which is of the form: api://{App Guid}
        - Add a scope.  
            - Name = Users.ReadWrite.All
            - Add sample text for consent boxes
            - Click button to Add Scope
        - Authorize client app to call API
            - Click 'Add a client application' button
            - Add client ID from UI to the client id textbox
            - Select the scope (Users.ReadWrite.All)
- Step 2: Generate a dotnet sample app for the webapi.  Run: dotnet new webapi --auth SingleOrg
- Step 3: Optional - To verify sample is working, disable authentication and authorization.  Comment out the following lines:
    - Program.cs: builder.Services.AddAuthentication, and lines containing app.UseAuthentication and app.UseAuthorization
    - Controllers/WeatherForecastController.cs: Attributes Authorize and RequireScope preceding controller class
    - Run locally and navigate to {host}/WeatherForecast    - You should see a valid result
    - Reenable lines commented out, and re-run sample       - You should an error 401, unauthorized user
- Step 4: Add Tenant Id and client Id to AppSettings.json

### Phase 3: Modify UI to call API
- Step 1: Generate and save secret in UI App Registration
    - Modify client UI registration to add a secret.  Copy the value of the secret - after this is generated, Azure will no longer show this

- Step 2: Modify Program.cs to enable getting tokens to call the API.
    In Program.cs, add the following lines after .AddMicrosoftIdentityWeb :
    ```
    .EnableTokenAcquisitionToCallDownstreamApi(new string[] {"api://{Guid API}/Users.ReadWrite.All"})
    .AddInMemoryTokenCaches();
    ```
- Step 3: Add Token interfaces to HomeController.cs via dependency injection
    - In HomeController.cs add the following code: 
    ```
    private readonly ITokenAcquisition _tokenAcquisition;
    public HomeController(ILogger<HomeController> logger, ITokenAcquisition tokenAcquisition)
    {
        _logger = logger;
        _tokenAcquisition = tokenAcquisition;
    }
    ```
- Step 4: Modify the Index Method of HomeController.cs to obtain the token and call the API
    - Add this code to the Index method.  Be sure port is set to proper address to call the local API.
    ```
        public async Task<IActionResult> Index()
    {
        var token = await _tokenAcquisition.GetAccessTokenForUserAsync(new string[] {"api://babdd027-ceb1-49a3-a58c-39e1f557874b/Users.ReadWrite.All"});
        HttpClient httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var jsonData = await httpClient.GetStringAsync("https://localhost:7012/WeatherForecast");

        ViewData["ApiResult"] = jsonData;

        return View();
    }
    ```
- Step 5: Before the Index method of the HomeController, add the attribute: [AuthorizeForScopes(Scopes = new string[] {"api://{API Guid}/Users.ReadWrite.All"})]
- Step 6: Add the API result to the index page in Views/Home/Index.cshtml
    ```
    <div>Api Result</div>
    <div>@ViewData["ApiResult"]</div>
    ```

# How to Setup AAD B2C
### Phase 1: Ensure that the Microsoft Azure Active Directory Provider is registered for the subscription
- Step 1: Open the Azure CLI and login to the subscription: az login
- Step 2: View the list of providers: az provider list --output table
- Step 3: If Microsoft.AzureActiveDirectory is not registered, register it: az provider register --namespace Microsoft.AzureActiveDirectory



