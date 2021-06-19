using Amazon.S3;
using DailyNewsServer.Core.Interfaces;
using DailyNewsServer.Core.Interfaces.Data;
using DailyNewsServer.Core.Models.Config;
using DailyNewsServer.Core.Services;
using DailyNewsServer.Data;
using DailyNewsServer.Data.Data;
using DailyNewsServer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics;
using System.Text;

namespace DailyNewsServer.Api
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
            services.AddDbContext<DailyNewsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                // use EnableSensitiveDataLogging to see data related used in query
                //.EnableSensitiveDataLogging()
                .LogTo(log => Debug.WriteLine(log)));
            services.AddScoped<DbContext, DailyNewsDbContext>();

            //Service Lifetimes https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-lifetimes
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IFilesRepository, FilesRepository>();

            services.AddControllers();

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Daily News", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

            services.AddHealthChecks();

            services.AddAWSService<IAmazonS3>(Configuration.GetAWSOptions());

            var TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:Secret"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = TokenValidationParameters;
            });

            services.AddAuthorization(cfg =>
            {
                //cfg.AddPolicy("ClearanceLevel1", policy => policy.RequireClaim("ClearanceLevel", "1", "2"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                             "Daily News v1"));

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
