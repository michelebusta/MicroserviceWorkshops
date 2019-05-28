using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.EventHubs;
using Shared.Kafka;
using Shared.RabbitMQ;
using Shared.ServiceBus;
using System;

namespace IdentityManagementWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            if (string.Equals(Configuration["EventsSystem"], "servicebus", StringComparison.CurrentCultureIgnoreCase))
            {
                var section = Configuration.GetSection(nameof(ServiceBusProducerConfig));
                services.AddSingleton<IEventProducer>(sp => new ServiceBusProducer(sp.GetRequiredService<ILogger<ServiceBusProducer>>(), section));
            }
            else if (string.Equals(Configuration["EventsSystem"], "eventhub", StringComparison.CurrentCultureIgnoreCase))
            {
                var section = Configuration.GetSection(nameof(EventHubProducerConfig));
                services.AddSingleton<IEventProducer>(sp => new EventHubProducer(sp.GetRequiredService<ILogger<EventHubProducer>>(), section));
            }
            else if (string.Equals(Configuration["EventsSystem"], "rabbitmq", StringComparison.CurrentCultureIgnoreCase))
            {
                var section = Configuration.GetSection(nameof(RabbitMQProducerConfig));
                services.AddSingleton<IEventProducer>(sp => new RabbitMQProducer(sp.GetRequiredService<ILogger<RabbitMQProducer>>(), section));
            }
            else
            {
                services.AddSingleton<IEventProducer>(sp => new KafkaProducer(sp.GetRequiredService<ILogger<KafkaProducer>>(), Configuration.GetSection("kafka")));
            }
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
