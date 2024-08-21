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

var builder = WebApplication.CreateBuilder(args); // builder WebApplication türünde bir nesne ve web uygulamasını başlatırken kullanılan bir yapılandırıcı.
var connectionString = builder.Configuration.GetConnectionString("BillingDb") ?? throw new InvalidOperationException("Connection string 'BillingContextConnection' not found.");

// IConfigurationBuilder ; yapılandırma kaynaklarını tanımlamak için kullanılan bir interface
// builder.Configuration ; yapılandırıcının yapılandırma ayarlarına erişmek için kullanılan bir özellik.
IConfigurationBuilder configBuilder = builder.Configuration; //configBuilder IConfigurationBuilder tipinde yapılandırma
                                                             //ayarlarını temsil eden bir nesneye atanır bu sayede yapılandırma
                                                             //ayarlarına erişebiliriz
configBuilder.Sources.Clear(); // Yapılandırıcıda daha önce eklenmiş olan tüm yapılandırma kaynaklarını temizler.
configBuilder.AddJsonFile("appsettings.json") // IConfigurationBuilder nesnesine = configBuilder'a Json dosyası ekler.
                                              // Uygulamanın yapılandırma ayarlarının merkezi bir dosyadan erişilebilmesini sağlar.
    .AddJsonFile("appsettings.development.json", true) // Eğer belirtilen dosya ortada yoksa opsiyonel olduğu için uygulama çalışmaya devam eder
    .AddUserSecrets(Assembly.GetExecutingAssembly()) // Kullancıya özel gizli anahtarları nesnenin içine ekliyor 
    .AddEnvironmentVariables() // nesneye ortam değişkenlerini ekliyor. 
    .AddCommandLine(args);


builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailService, DevTimeEmailService>();
//Transient = it will generate everytime when we need it
builder.Services.AddDbContext<BillingContext>();

builder.Services.AddIdentityApiEndpoints<TimeBillUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
})
    .AddEntityFrameworkStores<BillingContext>();

builder.Services.AddAuthentication()
    .AddBearerToken();

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
builder.Services.AddValidatorsFromAssemblyContaining<TimeBillModel> ();

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetEntryAssembly()!);




var app = builder.Build();

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

app.MapControllers();

app.MapGroup("api/auth")
    .MapIdentityApi<TimeBillUser>();

app.Run();
