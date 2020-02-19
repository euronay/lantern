using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Lantern.Core.Patterns;
using Lantern.Core.Devices;
using Lantern.Core.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.ServiceBus;

namespace Lantern.Scratch
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Up...");
            Console.WriteLine("Press any key to close");

            // Get config
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();

            var serviceBusConnectionString = configuration.GetValue<string>("serviceBusConnectionString");
            var queue = configuration.GetValue<string>("ServiceBusQueue");
            var ledCount = configuration.GetValue<int>("LedCount");
            var useFakeDevice = configuration.GetValue<bool>("UseFakeDevice");

            // Setup IOC
            var serviceCollection = new ServiceCollection()
                .AddLogging(opt => opt.AddConsole())
                .AddSingleton<IQueueClient>(_ => new QueueClient(serviceBusConnectionString, queue))
                .AddSingleton<IMessagingService, MessagingService>();

            if(useFakeDevice)
            {
                serviceCollection.AddSingleton<ILightStrip, FakeLightStrip>();
            }
            else
            {
                serviceCollection.AddSingleton<ILightStrip>(provider => new LightStrip(ledCount, provider.GetRequiredService<ILogger<LightStrip>>()));
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Flash LED Twice
            var lightStrip = serviceProvider.GetService<ILightStrip>();
            await lightStrip.RunAsync(new ChasePattern(Color.Blue, 16, Timeout.InfiniteTimeSpan), TimeSpan.FromSeconds(1));

            Console.WriteLine("Light check OK");

            // Send a message to ourselves           
            var messagingService = serviceProvider.GetService<IMessagingService>();
            await messagingService.SendMessageAsync(new LightCommandMessage{ Command = LightCommand.Chase, Color = Color.Green, Duration = TimeSpan.FromSeconds(1) });

            Console.Read();

            Console.WriteLine("Closing...");

            //TODO make this do this on dispose?
            lightStrip.Stop();
        }


       
    }
}
