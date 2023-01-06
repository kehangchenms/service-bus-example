using AsbMassNetCoreQueue.Contracts;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AsbMassNetCoreQueue
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
            var connectionString = "Endpoint=sb://sweetbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=100LIF8+xSXwRCnqkOr9dR8Hn4wYgyMywzn0DFjE37M=";

            var newOrdersQueue = "tsys-msg";

            // create the bus using Azure Service bus
            var azureServiceBus = Bus.Factory.CreateUsingAzureServiceBus(busFactoryConfig =>
            {
                busFactoryConfig.Host(connectionString);

                // specify the message of Order object to be sent to a specific queue
                busFactoryConfig.Message<Order>(configTopology =>
                {
                    configTopology.SetEntityName(newOrdersQueue);
                });
            });

            services.AddMassTransit
                (
                    config =>
                    {
                        config.AddBus(provider => azureServiceBus);
                    }
                );

            services.AddSingleton<ISendEndpointProvider>(azureServiceBus);
            services.AddSingleton<IBus>(azureServiceBus);

            services.AddControllers();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
