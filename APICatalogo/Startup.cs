using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Errors;
using APICatalogo.Filters;
using AutoMapper;
using APICatalogo.Repository;
using APICatalogo.Repository.DTOs.Mappings;

namespace APICatalogo
{
    public class Startup
    {
        public IConfiguration configuration { get; }

        public Startup(IConfiguration config)
        {
            configuration = config;
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
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

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));
            });
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
