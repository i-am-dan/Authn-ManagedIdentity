using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

//DO NOT TOUCH
// builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
//     .EnableTokenAcquisitionToCallDownstreamApi(new string[] {"api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/Users.ReadWrite.All"})
//     .AddInMemoryTokenCaches();

//Add services to the container.
//var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ');
//WORKS
// builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
//     .EnableTokenAcquisitionToCallDownstreamApi(new string[] {"api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default"})
//     .AddInMemoryTokenCaches();

// builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
//     .EnableTokenAcquisitionToCallDownstreamApi(new string[] {"api://915f3e69-455d-4c0e-95d0-c9f8f2bef59d/.default"})
//     .AddInMemoryTokenCaches();

// Managed Identity
// builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration)
//         .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
//         .AddDownstreamApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
//         .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews(options =>
{
    // var policy = new AuthorizationPolicyBuilder()
    //     .RequireAuthenticatedUser()
    //     .Build();

    // options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddRazorPages();//.AddMicrosoftIdentityUI();


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

// app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
