using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infraestructure.ApiClients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("quiet-stone-2094", c =>
            {
                c.BaseAddress = new Uri("http://quiet-stone-2094.herokuapp.com/");
            });
            services.AddSingleton<ITransactionClient<IWebServicesEntity>, TransactionClient>();
            services.AddSingleton<IConversionClient<IWebServicesEntity>, ConversionClient>();
            services.AddSingleton<IApiClient<IWebServicesEntity, IWebServicesEntity>, ApiClients>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
