using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PokedexApi.Interfaces;
using PokedexApi.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexApi
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

            services.AddControllers();

            services.AddHealthChecks(); // Register ASP.Net Core Healthcheck Middleware for container liveness checks. Basic healthchecks not tied to any subsytems. Access at '/health'

            // OpenApi SwaggerDoc Generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PokedexApi", Version = "v1" });
            });

            #region RegisterServices
            // Registers the PokemonService as a Dependency Injection container 
            // As we are using the service for HTTP REST API Calls, we register the service as an HTTP client
            services.AddHttpClient<IPokemonService, PokemonService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Use Developer exception page and swagger Page if Development. Otherwise give the user a generic error page
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokedexApi v1"));
            }

            // Enables request logging for Serilog as we suppressed the chatty ASP.NET Core information logging in Program.cs
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            // Map Endpoints using Controllers according to .Net Core Convention
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health"); //Map Healthcheck
            });
        }
    }
}
