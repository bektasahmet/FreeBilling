using FluentValidation;
using FreeBilling.Data.Entities;
using FreeBilling.Web.Apis;
using FreeBilling.Web.Data;
using FreeBilling.Web.Models;
using FreeBilling.Web.Services;
using Mapster;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FreeBilling.Web.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args); // builder WebApplication t�r�nde bir nesne ve web uygulamas�n� ba�lat�rken kullan�lan bir yap�land�r�c�.
var connectionString = builder.Configuration.GetConnectionString("BillingDb") ?? throw new InvalidOperationException("Connection string 'BillingContextConnection' not found.");

// IConfigurationBuilder ; yap�land�rma kaynaklar�n� tan�mlamak i�in kullan�lan bir interface
// builder.Configuration ; yap�land�r�c�n�n yap�land�rma ayarlar�na eri�mek i�in kullan�lan bir �zellik.
IConfigurationBuilder configBuilder = builder.Configuration; //configBuilder IConfigurationBuilder tipinde yap�land�rma
                                                             //ayarlar�n� temsil eden bir nesneye atan�r bu sayede yap�land�rma
                                                             //ayarlar�na eri�ebiliriz
configBuilder.Sources.Clear(); // Yap�land�r�c�da daha �nce eklenmi� olan t�m yap�land�rma kaynaklar�n� temizler.
configBuilder.AddJsonFile("appsettings.json") // IConfigurationBuilder nesnesine = configBuilder'a Json dosyas� ekler.
                                              // Uygulaman�n yap�land�rma ayarlar�n�n merkezi bir dosyadan eri�ilebilmesini sa�lar.
    .AddJsonFile("appsettings.development.json", true) // E�er belirtilen dosya ortada yoksa opsiyonel oldu�u i�in uygulama �al��maya devam eder
    .AddUserSecrets(Assembly.GetExecutingAssembly()) // Kullanc�ya �zel gizli anahtarlar� nesnenin i�ine ekliyor 
    .AddEnvironmentVariables() // nesneye ortam de�i�kenlerini ekliyor. 
    .AddCommandLine(args);


builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailService, DevTimeEmailService>();
//Transient = it will generate everytime when we need it
builder.Services.AddDbContext<BillingContext>();

builder.Services.AddIdentityApiEndpoints<TimeBillUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<BillingContext>();

builder.Services.AddAuthentication()
  .AddJwtBearer(cfg =>
  {
      cfg.TokenValidationParameters = new TokenValidationParameters()
      {
          ValidIssuer = builder.Configuration["Token:Issuer"],
          ValidAudience = builder.Configuration["Token:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"]!))
      };
  });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("api", cfg =>
    {
        cfg.RequireAuthenticatedUser();
        cfg.AddAuthenticationSchemes(IdentityConstants.BearerScheme);
    });

builder.Services.AddAuthorization(cfg => 
{
    cfg.AddPolicy("ApiPolicy", bldr =>
    {
        bldr.RequireAuthenticatedUser();
        bldr.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    });
});


builder.Services.AddScoped<IBillingRepository, BillingRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<TimeBillModel>();

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetEntryAssembly()!);




var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapRazorPages();


if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Welcome to FreeBilling");
//});

TimeBillsApi.Register(app);
AuthApi.Register(app);
EmployeesApi.Register(app);

app.MapControllers();

app.MapFallbackToPage("/customerBilling");
app.MapGroup("api/auth")
    .MapIdentityApi<TimeBillUser>();

app.Run();
