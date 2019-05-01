using Iris.Rms.Data;
using Iris.Rms.Interfaces;
using Iris.Rms.Service;
using Iris.Rms.Voice;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iris.Rms.Web.Host
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
            string connectionString = Configuration.GetConnectionString("RmsConnectionString");
            services.AddDbContext<RmsDbContext>(cfg =>
            {
                cfg.UseSqlServer(connectionString);
            });


            services.AddTransient<DataSeeder>();
            services.AddTransient<CommandEvaluator>();
            services.AddTransient<IRmsService, RmsService>();
            services.AddTransient<IVoiceRms, VoiceRms>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "MDM Swagger Endpoint", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MDM Swagger Endpoint");
            });
            app.UseCors(builder => builder.AllowAnyOrigin());
            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default", "{controller}/{action}/{id?}",
                    new { Controller = "Home", Action = "Index" });
            });

            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                DataSeeder seeder = scope.ServiceProvider.GetService<DataSeeder>();
                seeder.Seed().Wait();

                IVoiceRms voiceRms = scope.ServiceProvider.GetService<IVoiceRms>();
                voiceRms.Listen();

            }

        }
    }
}
