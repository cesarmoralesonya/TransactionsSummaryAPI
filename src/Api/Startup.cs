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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace PublicApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient(Configuration.GetValue<string>("ConfigApp:QuietStoneClient"), c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("ConfigApp:QuietStoneUrl"));
            });

            services.AddDbContext<TransSummaryContext>(op => op.UseInMemoryDatabase("TransactionSummaryDb"));
            services.AddSingleton<ITransactionClient<TransactionModel>, TransactionClient>();
            services.AddSingleton<IConversionClient<ConversionModel>, ConversionClient>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IConversionRepository, ConversionRepository>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IConversionService, ConversionService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Transaction Summary", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddControllers();
            services.AddLogging();
            services.AddApplicationInsightsTelemetry();
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
                config.ConfigObject.AdditionalItems.Add("syntaxHighlight", false); //Turns off syntax highlight which causing performance issues...
                config.ConfigObject.AdditionalItems.Add("theme", "agate"); //Reverts Swagger UI 2.x  theme which is simpler not much performance benefit...
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
