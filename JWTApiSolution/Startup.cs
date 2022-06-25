using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Configurations;
using CoreLayer.IdentityModels;
using CoreLayer.IRepositories;
using CoreLayer.IServices;
using CoreLayer.IUnitOfWorks;
using DataLayer;
using DataLayer.Repositories;
using DataLayer.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Services;
using SharedLayer.Configurations;

namespace JWTApiSolution
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<List<Client>>(Configuration.GetSection("Clients"));
            services.Configure<CustomTokenOption>(Configuration.GetSection("TokenOption"));
            var tokenOptions = Configuration.GetSection("TokenOption").Get<CustomTokenOption>();




            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped(typeof(IBaseRepositories<,>), typeof(BaseRepository<,>));
            services.AddScoped(typeof(IBaseService<,,>), typeof(BaseService<,,>));
            services.AddScoped<IUnitOfWork, UnitOfWorks>();

            services.AddDbContext<AppDbContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("DataLayer");
                });
            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(authenticationScheme:JwtBearerDefaults.AuthenticationScheme,
                opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = "www.serverapi.com",
                        ValidAudience = "www.serverapi.com",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),

                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero
                    };
                });

            var passwordOptions = new PasswordOptions()
            {
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonAlphanumeric = false,
                RequireUppercase = false,
                RequiredLength = 5,
                RequiredUniqueChars = 4
            };
            var userOptions = new UserOptions()
            {
                RequireUniqueEmail = true
            };
            services.AddIdentity<AppUser, AppRole>(identityOptions =>
                {
                    identityOptions.Password = passwordOptions;
                    identityOptions.User = userOptions;
                }).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


       

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWTApiSolution", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {




            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTApiSolution v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
