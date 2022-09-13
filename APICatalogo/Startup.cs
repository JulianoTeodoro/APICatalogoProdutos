using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Errors;
using APICatalogo.Filters;

namespace APICatalogo
{
    public class Startup
    {
        public IConfiguration configuration { get; }

        public Startup(IConfiguration config)
        {
            configuration = config;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }

    }
}
