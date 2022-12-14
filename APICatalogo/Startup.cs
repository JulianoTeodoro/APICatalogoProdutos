using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Errors;
using APICatalogo.Filters;
using AutoMapper;
using APICatalogo.Repository;
using APICatalogo.Logging;

using APICatalogo.Repository.DTOs.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APICatalogo
{
    public class Startup
    {
        public IConfiguration configuration { get; }

        public Startup(IConfiguration config)
        {
            configuration = config;
        }

        public void Configure(WebApplication app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();
            app.UseHsts();

            app.UseAuthorization();

            app.MapControllers();

            loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
            {
                LogLevel = LogLevel.Information,
            }));

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));
            });
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidAudience = configuration["TokenConfiguration:Audience"],
                        ValidIssuer = configuration["TokenConfiguration:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"])
                        )
                    };
            });

            services.AddCors();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
           
            services.AddScoped<ApiLoggingFilter>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton(mapper);

        }

    }
}
