using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyWebModels.Database;
using MyWebAPI.Services;
using MyWebAPI.Services.Account;
using MyWebAPI.Services.SocialAuth;
using MyWebModels.Sittings;
using MyWebModels.Models.Account;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyWebAPI
{
    public class Startup
    {
        //for social auth.
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>(true, reloadOnChange: true);

            var configuration = builder.Build();
            Configuration = configuration;
        }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // for access-controle-allowed-origin
            services.AddCors();

            //Configuration from AppSettings
            services.Configure<JWT>(Configuration.GetSection("JWT"));

            services.AddScoped<FacebookAuthService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICountryServices, CountryServices>();
            services.AddScoped<ICityServices, CityServices>();

            services.AddScoped<IChatServices, ChatServices>();
            services.AddScoped<IMessageServices, MessageServices>();

            //services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Adding DB Context with MSSQL
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));


            // add identity.
            services.AddIdentity<AppUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 4;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireDigit = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                //option.SignIn.RequireConfirmedEmail = true;
                //option.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();



            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

            services
                .AddAuthentication(options =>
                {
                    ///TODO: If you plan to use both cookie and JWT auth on this API, you can use this attribute
                    ///[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                    };

                })
                .AddFacebook(facebook =>
                {
                    facebook.AppId = Configuration["JWT:Authentication_Facebook_AppId"];
                    facebook.AppSecret = Configuration["JWT:Authentication_Facebook_AppSecret"];
                    facebook.SaveTokens = true;
                })
                .AddGoogle(google =>
                {
                    google.ClientSecret = Configuration["JWT:Authentication_Google_ClientSecret"];
                    google.ClientId = Configuration["JWT:Authentication_Google_ClientId"];
                    google.Scope.Add("profile");
                    google.Events.OnCreatingTicket = (context) =>
                    {
                        var picture = context.User.GetProperty("picture").GetString();

                        context.Identity.AddClaim(new Claim("picture", picture));

                        return Task.CompletedTask;
                    };
                    google.SaveTokens = true;
                });

            

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyWebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // for access-controle-allowed-origin
            app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
