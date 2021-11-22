using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace BaseGateway
{
    public class Startup  
    {  
        public void ConfigureServices(IServiceCollection services)  
        {  
            services.AddOcelot().AddCacheManager(settings =>
            {  
                settings.WithDictionaryHandle();  
            });  
        }  
  
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)  
        {  
            if (env.IsDevelopment())  
            {  
                app.UseDeveloperExceptionPage();  
            }  
            app.UseRouting();  
            app.UseEndpoints(endpoints =>  
            {  
                endpoints.MapControllers();  
            });  
            await app.UseOcelot();  
        }  
    }  
}