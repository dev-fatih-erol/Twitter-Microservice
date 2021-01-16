using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.EventBus;
using AspNetCore.EventBus.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using UserInfo.Api.Data;
using UserInfo.Api.Data.Configurations;
using UserInfo.Api.Data.Services;
using UserInfo.Api.IntegrationEvents.EventHandling;
using UserInfo.Api.IntegrationEvents.Events;

namespace UserInfo.Api
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
            services.AddEventBusRabbitMQ(o =>
            {
                o.HostName = "localhost";
                o.UserName = "guest";
                o.Password = "guest";
                o.QueueName = "event_bus_queue";
                o.BrokerName = "event_bus";
                o.RetryCount = 5;
            });

            services.AddTransient<UserRegisterEventHandler>();

            services.Configure<MongoConfiguration>(Configuration.GetSection(nameof(MongoConfiguration)));

            services.AddSingleton<IMongoConfiguration>(o => o.GetRequiredService<IOptions<MongoConfiguration>>().Value);

            services.AddSingleton<UserInfoDbContext>();

            services.AddSingleton<IInfoService, InfoService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserInfo.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserInfo.Api v1"));
            }

            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<UserRegisterEvent, UserRegisterEventHandler>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
