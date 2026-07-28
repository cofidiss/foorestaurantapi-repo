using FooRestaurantApi.APILayer.Extensions;
using FooRestaurantApi.APILayer.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;

namespace FooRestaurantApi.APILayer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterRequiredServices();


        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
 
            app.UseMiddleware<CustomErrorMiddleware>();


            if (env.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APILayer v1"));

            }

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

      
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();


            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
