
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using POS.Application;
using POS.Application.BackgroundJobs;
using POS.Application.Contract.FileStorages;
using POS.Application.PermissionHandler;
using POS.Domain;
using POS.EFCore;
using POS.EFCore.DatabaseSeeders;
using System.Text;

namespace POS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDomain();
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);


            //Add Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });


            //var ss = builder.Configuration.Get<string>("OriginSettings:AppUrl");
            var corsOrigins = builder.Configuration.GetValue<string>("OriginSettings:AppUrl") ?? "http://localhost:4200";
            // Add CORS services
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "_myAllowSpecificOrigins",
                    policy =>
                    {
                               policy.WithOrigins(corsOrigins) // Specify the allowed origins
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .WithExposedHeaders("Content-Disposition");
                    });
            });
            //-------------

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "POS_";
            });


            // Add controllers or other services
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });
            builder.Services.AddEndpointsApiExplorer();
            //=================== Swagger =======================//
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

 
            
            builder.Services.AddAntiforgery();
            var app = builder.Build();

            app.UseHangfireDashboard("/hangfire");

            RecurringJob.AddOrUpdate<OutboxCleanupJob>("outbox-cleanup", job => job.Run(), Cron.Daily);

            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
                await seeder.SeedAsync();
            }
            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<RolePermissionSeeder>();
                await seeder.SeedAsync();
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }
            var fileStorage =
            builder.Configuration
           .GetSection("FileStorage")
           .Get<FileStorage>();


            var uploadsFolder = fileStorage?.UploadsFolder;
            var requestPath = fileStorage?.RequestPath;

            if (!Directory.Exists(uploadsFolder) && uploadsFolder != null)
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder)),
                RequestPath = requestPath
            });
            app.UseAntiforgery();
            app.UseCors("_myAllowSpecificOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
