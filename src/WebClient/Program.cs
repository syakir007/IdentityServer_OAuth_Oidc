using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:5001";

    options.ClientId = "web";
    options.ClientSecret = "secret";
    options.ResponseType = "code";

    options.SaveTokens = true;


    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("verification");
    options.Scope.Add("api1");
    options.Scope.Add("role");
    options.Scope.Add("offline_access");
    options.ClaimActions.MapJsonKey("email_verified", "email_verified");
    options.GetClaimsFromUserInfoEndpoint = true;

    options.SaveTokens = true;
});
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
