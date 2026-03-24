using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using TareasMVC;
using TareasMVC.Servicios;



var builder = WebApplication.CreateBuilder(args);
var politicaUsusario = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

// Add services to the container.
builder.Services.AddControllersWithViews(
    opciones =>
    {
        opciones.Filters.Add(new AuthorizeFilter(politicaUsusario));
    }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
    
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication().AddMicrosoftAccount(opciones =>
{
    opciones.ClientId = builder.Configuration["MicrosoftClientId"];
    opciones.ClientSecret = builder.Configuration["MicrosoftClientSecret"];
});

builder.Services.AddAuthorization();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
     options.SignIn.RequireConfirmedAccount = false;
    }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/usuario/Login";
    options.LogoutPath = "/usuario/Login";
    options.AccessDeniedPath = "/usuario/Login";
});
builder.Services.AddLocalization(opciones =>
{
    opciones.ResourcesPath = "Recursos";
});

builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());




var app = builder.Build();

var CulturaUISoportadas = new[] { "es", "en" };

app.UseRequestLocalization(opciones =>
{
    opciones.DefaultRequestCulture = new RequestCulture("en");
    opciones.SupportedCultures = CulturaUISoportadas.Select(cultura => new CultureInfo(cultura)).ToList();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
