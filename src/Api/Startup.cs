using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Infraestructure.ApiClients;
using Infraestructure.Data;
using Infraestructure.Data.Repositories;
using Infraestructure.Interfaces;
using Infraestructure.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace PublicApi
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

            services.AddDbContext<TransSummaryContext>(op => op.UseInMemoryDatabase("TransactionSummaryDb"));
            services.AddScoped<ITransactionClient<TransactionModel>, TransactionClient>();
            services.AddScoped<IConversionClient<ConversionModel>, ConversionClient>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IConversionRepository, ConversionRepository>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IConversionService, ConversionService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Transaction Summary", Version = "v1" }));
            services.AddControllers();
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction Summary V1");
                config.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
