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

var builder = WebApplication.CreateBuilder(args); // builder WebApplication türünde bir nesne ve web uygulamasýný baþlatýrken kullanýlan bir yapýlandýrýcý.
var connectionString = builder.Configuration.GetConnectionString("BillingDb") ?? throw new InvalidOperationException("Connection string 'BillingContextConnection' not found.");

// IConfigurationBuilder ; yapýlandýrma kaynaklarýný tanýmlamak için kullanýlan bir interface
// builder.Configuration ; yapýlandýrýcýnýn yapýlandýrma ayarlarýna eriþmek için kullanýlan bir özellik.
IConfigurationBuilder configBuilder = builder.Configuration; //configBuilder IConfigurationBuilder tipinde yapýlandýrma
                                                             //ayarlarýný temsil eden bir nesneye atanýr bu sayede yapýlandýrma
                                                             //ayarlarýna eriþebiliriz
configBuilder.Sources.Clear(); // Yapýlandýrýcýda daha önce eklenmiþ olan tüm yapýlandýrma kaynaklarýný temizler.
configBuilder.AddJsonFile("appsettings.json") // IConfigurationBuilder nesnesine = configBuilder'a Json dosyasý ekler.
                                              // Uygulamanýn yapýlandýrma ayarlarýnýn merkezi bir dosyadan eriþilebilmesini saðlar.
    .AddJsonFile("appsettings.development.json", true) // Eðer belirtilen dosya ortada yoksa opsiyonel olduðu için uygulama çalýþmaya devam eder
    .AddUserSecrets(Assembly.GetExecutingAssembly()) // Kullancýya özel gizli anahtarlarý nesnenin içine ekliyor 
    .AddEnvironmentVariables() // nesneye ortam deðiþkenlerini ekliyor. 
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
