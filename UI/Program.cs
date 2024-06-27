using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

//DO NOT TOUCH
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi(new string[] {"api://cb7a0fe4-34a0-4f5e-b9d3-da3e4d1aa3ee/.default"})
    .AddInMemoryTokenCaches();

//wire up graph call

//https://learn.microsoft.com/en-us/dotnet/azure/sdk/authentication/azure-hosted-apps?tabs=azure-portal%2Cazure-app-service%2Ccommand-line

//https://www.youtube.com/watch?v=2cWIxn-LOp8
// builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));


builder.Services.AddSingleton(new DefaultAzureCredential());

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

builder.Services.AddRazorPages();//.AddMicrosoftIdentityUI();
// AddMvcOptions(options => {
//     var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//     options.Filters.Add(new AuthorizeFilter(policy));
// })
// .AddMicrosoftIdentityUI();        


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
