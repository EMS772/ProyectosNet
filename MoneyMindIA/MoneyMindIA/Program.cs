using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using MoneyMindIA.Configurations;
using MoneyMindIA.Models.Data;
using MoneyMindIA.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Base de datos
builder.Services.AddDbContext<MoneyMindDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

// Autenticación
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "CookieAuth";
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie("CookieAuth", options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Home/Error";
})
.AddGoogle(options => {
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.CallbackPath = "/signin-google"; // Debe coincidir con Google Cloud
});

// API DEEPSEEK:
builder.Services.AddHttpClient<DeepSeekService>();
builder.Services.Configure<DeepSeekConfig>(builder.Configuration.GetSection("DeepSeek"));

// Agregar al inicio:
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Configuration.AddUserSecrets<Program>();


var app = builder.Build();

// Middlewares
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();