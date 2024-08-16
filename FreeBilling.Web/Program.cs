using FreeBilling.Web.Data;
using FreeBilling.Web.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

IConfigurationBuilder configBuilder = builder.Configuration;
configBuilder.Sources.Clear();
configBuilder.AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.development.json", true)
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .AddEnvironmentVariables()
    .AddCommandLine(args);


builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailService, DevTimeEmailService>();
//Transient = it will generate everytime when we need it
builder.Services.AddDbContext<BillingContext>();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapRazorPages();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Welcome to FreeBilling");
//});

app.Run();
