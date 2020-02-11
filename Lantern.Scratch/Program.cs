using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lantern.Core;
using Lantern.Core.Patterns;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using AsyncAwaitBestPractices;

namespace Lantern.Scratch
{
    class Program
    {
        const string ServiceBusConnectionString = "";
        const string QueueName = "commands";
        static IQueueClient _queueClient;
        static ILightStrip _lightStrip;

        static async Task Main(string[] args)
        {
            _queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            _lightStrip = new LightStrip(16);

            

            // Register the queue message handler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();

            Console.WriteLine("Starting Up...");

            // Flash LED Twice
            _lightStrip.RunAsync(new BlinkPattern(Color.Blue, TimeSpan.FromMilliseconds(200)), TimeSpan.FromSeconds(1)).SafeFireAndForget();

            Console.WriteLine("Press any key to close");
            await SendMessageAsync(new LightCommand{Command = LightCommandType.Color, Color = Color.Red});

            Console.Read();

            Console.WriteLine("Closing...");

            _lightStrip.Stop();
            await _queueClient.CloseAsync();

            
        }


        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            Console.WriteLine("Registering message handler.");

            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            var body = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{body}");

            var command = JsonConvert.DeserializeObject<LightCommand>(body);

            // DO STUFF
            _lightStrip.Stop();

            switch(command.Command)
            {
                case LightCommandType.Off:
                    break;
                case LightCommandType.On:
                    _lightStrip.RunAsync(new PlainColorPattern(Color.White), TimeSpan.Zero).SafeFireAndForget();
                    break;
                case LightCommandType.Color:
                    _lightStrip.RunAsync(new PlainColorPattern(command.Color), TimeSpan.Zero).SafeFireAndForget();
                    break;
                case LightCommandType.Rainbow:
                    _lightStrip.RunAsync(new RainbowPattern(16), TimeSpan.Zero).SafeFireAndForget();
                    break;
            }

            // Complete the message so that it is not received again.
            // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }

        // Use this handler to examine the exceptions received on the message pump.
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        static async Task SendMessageAsync(LightCommand command)
        {
            try
            {
                // Create a new message to send to the queue
                string messageBody = JsonConvert.SerializeObject(command);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                // Write the body of the message to the console
                Console.WriteLine($"Sending message: {messageBody}");

                // Send the message to the queue
                await _queueClient.SendAsync(message);

            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
