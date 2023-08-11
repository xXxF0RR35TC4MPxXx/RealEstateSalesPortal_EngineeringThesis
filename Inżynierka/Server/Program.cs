using In¿ynierka.Shared;
using In¿ynierka_Services.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Text.Json.Serialization;

int maxRequestBodySize = 152428800;
var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
logger.Debug("Initializing web application");

var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
NLog.GlobalDiagnosticsContext.Set("LogDirectory", logPath);
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add configurations
    builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
    {
        var env = hostingContext.HostingEnvironment;

        config.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //load base settings
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true) //load local settings
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true) //load environment settings
                .AddEnvironmentVariables();
    });

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.AddSingleton<IWebHostBuilder, WebHostBuilder>();
    builder.Services.AddDistributedMemoryCache();

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = new TimeSpan(14, 0, 0, 0);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
    
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.Name = "UserLoginCookie";
        options.SlidingExpiration = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.IsEssential = true;
        options.ExpireTimeSpan = new TimeSpan(14, 0, 0, 0);
        options.Events.OnRedirectToLogin = (context) =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Cookie.HttpOnly = false;
    });

    builder.Services.AddCors(options => options.AddPolicy("corspolicy", build =>
    {
        build
        .AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(op => true)
        .AllowCredentials();
    }));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.RegisterServices(builder.Configuration);

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Version = "v1",
            Title = "API documentation",
            Description = "API documentation for the Offer app"
        });

        var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    });

    //AutoMapper
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);
    builder.Host.UseNLog();

    //increase the maximum size of request body to 50MB (default value is 30MB)
    builder.Services.Configure<KestrelServerOptions>(options =>
    {
        options.Limits.MaxRequestBodySize = maxRequestBodySize;
    });
    builder.Services.Configure<IISServerOptions>(options =>
    {
        options.MaxRequestBodySize = maxRequestBodySize;
    });
    builder.Services.Configure<FormOptions>(x =>
    {
        x.ValueLengthLimit = maxRequestBodySize;
        x.MultipartBodyLengthLimit = maxRequestBodySize;
        x.MultipartHeadersLengthLimit = maxRequestBodySize;
    });

    var app = builder.Build();
    app.UseCors("corspolicy");
    app.UseAuthentication();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    app.UseSession();
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "api/swagger";
    });

    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();
    app.UseRouting();
    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile("index.html");
    app.UseAuthorization();
    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}