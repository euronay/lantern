using System;
using System.Drawing;
using System.Threading.Tasks;
using Lantern.Core.Patterns;
using Microsoft.Azure.ServiceBus;
using AsyncAwaitBestPractices;
using Lantern.Core.Devices;
using Lantern.Core.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Lantern.Scratch
{
    class Program
    {
        const string ServiceBusConnectionString = "";
        const string QueueName = "commands";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Up...");

            var serviceProvider = new ServiceCollection()
                .AddLogging(opt => opt.AddConsole())
                .AddSingleton<IQueueClient>(_ => new QueueClient(ServiceBusConnectionString, QueueName))
                //.AddSingleton<ILightStrip>(_ => new LightStrip(16))
                .AddSingleton<ILightStrip, FakeLightStrip>()
                .AddSingleton<IMessagingService, MessagingService>()
                .BuildServiceProvider();

            var lightStrip = serviceProvider.GetService<ILightStrip>();

            // Flash LED Twice
            lightStrip.RunAsync(new BlinkPattern(Color.Blue, TimeSpan.FromMilliseconds(200)), TimeSpan.FromSeconds(1)).SafeFireAndForget();

            Console.WriteLine("Press any key to close");
            var messagingService = serviceProvider.GetService<IMessagingService>();
            await messagingService.SendMessageAsync(new LightCommandMessage{Command = LightCommand.Color, Color = Color.Red});

            Console.Read();

            Console.WriteLine("Closing...");

            //TODO make this do this on dispose?
            lightStrip.Stop();
        }


       
    }
}
