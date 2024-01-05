using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using UnitedPolling.Services;
using UnitedPolling.DataContext;
public class Startup
{
    public IConfiguration _configuration { get; }

    public Startup(WebApplicationBuilder builder)
    {
        _configuration = builder.Configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var azureAd = _configuration.GetSection("AzureAd");

        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-XSRF-TOKEN";
            options.Cookie.Name = "__Host-X-XSRF-TOKEN";
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        services.AddHttpClient(); 
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });
        services.AddOptions();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(_configuration)
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddInMemoryTokenCaches();

        services.AddAuthorization();

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "United Polling", Version = "v1" });

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    AuthorizationCode = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri($"{azureAd["Instance"]}/{azureAd["TenantId"]}/oauth2/v2.0/authorize"),
                        TokenUrl = new Uri($"{azureAd["Instance"]}/{azureAd["TenantId"]}/oauth2/v2.0/token"),
                        Scopes = { { $"api://{azureAd["ClientId"]}/access_as_user", "Access as user" } },
                    }
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new string[] { }
                }
            });
        });

        services.AddDbContext<UnitedPollingDbContext>(options =>
            options.UseSqlServer(_configuration.GetValue<string>("ConnectionString")));

        //services.Configure<Configurations.SendGrid>(builder.Configuration.GetSection("SendGrid"));
        services.AddTransient<IEmailSender, EmailSender>();

        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IUserService, UserService>();
    }

    public void Configure(WebApplication app)
    {
        var azureAd = _configuration.GetSection("AzureAd");

        app.UseCors("AllowAll");

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId(azureAd["ClientId"]);
                c.OAuthUsePkce();
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        // Apply database migrations on startup
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<UnitedPollingDbContext>();
            dbContext.Database.Migrate();
        }
    }
}