﻿using System;
using System.IO;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Infrastructure.Messaging;
using BusinessChat.Infrastructure.Services;
using BusinessChat.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BusinessChat.StooqWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
                            .ConfigureServices((services) =>
                            {
                                var configuration = new ConfigurationBuilder()
                                                        .SetBasePath(Directory.GetCurrentDirectory())
                                                        .AddJsonFile("appsettings.json", true, true)

                                                        //.AddJsonFile("appsettings.json", false)
                                                        .Build();
                                ConfigureServices(services, configuration);
                                services.AddHostedService<StockQueryResolverHostedService>();
                            });
            builder.Build().Run();
            Console.WriteLine("Press any key to terminate .");
            Console.ReadKey();
        }

        private static ServiceProvider ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<RabbitMqConfiguration>
(options => configuration.GetSection("RabbitMqConfiguration").Bind(options));
            services.Configure<MessagingConfiguration>
(options => configuration.GetSection("MessagingConfiguration").Bind(options));
            //services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConnectionSettings"));
            //services.Configure<MessagingConfiguration>(configuration.GetSection("MessagingConfiguration"));
            services.AddTransient<IMessageBroker, RabbitMqMessageBroker>();
            services.AddTransient<IStockQuery, StockQuery>();
            services.AddTransient<IStockResponse, StockResponse>();
            services.AddTransient<IStooqService, StooqService>();
            return services.BuildServiceProvider();
        }
    }
}
