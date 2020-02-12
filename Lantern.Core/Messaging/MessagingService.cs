using AsyncAwaitBestPractices;
using Lantern.Core.Devices;
using Lantern.Core.Patterns;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lantern.Core.Messaging
{
    public class MessagingService : IDisposable, IMessagingService
    {
        private IQueueClient _queueClient;
        private ILightStrip _lightStrip;
        private ILogger _logger;

        public MessagingService(IQueueClient queueClient, ILightStrip lightStrip, ILogger<MessagingService> logger)
        {
            _queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
            _lightStrip = lightStrip ?? throw new ArgumentNullException(nameof(lightStrip));
            _logger = logger;

            _logger.LogInformation("Registering message handler.");

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            var body = Encoding.UTF8.GetString(message.Body);
            _logger.LogInformation($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{body}");

            var command = JsonConvert.DeserializeObject<LightCommandMessage>(body);

            // DO STUFF
            _lightStrip.Stop();

            switch (command.Command)
            {
                case LightCommand.Off:
                    break;
                case LightCommand.On:
                    _lightStrip.RunAsync(new PlainColorPattern(Color.White),command.Duration).SafeFireAndForget();
                    break;
                case LightCommand.Color:
                    _lightStrip.RunAsync(new PlainColorPattern(command.Color), command.Duration).SafeFireAndForget();
                    break;
                case LightCommand.Rainbow:
                    _lightStrip.RunAsync(new RainbowPattern(16), command.Duration).SafeFireAndForget();
                    break;
            }

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);

        }

        // Use this handler to examine the exceptions received on the message pump.
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            _logger.LogDebug("Exception context for troubleshooting:");
            _logger.LogDebug($"- Endpoint: {context.Endpoint}");
            _logger.LogDebug($"- Entity Path: {context.EntityPath}");
            _logger.LogDebug($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        public async Task SendMessageAsync(LightCommandMessage message)
        {
            try
            {
                // Create a new message to send to the queue
                string messageBody = JsonConvert.SerializeObject(message);
                var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(messageBody));

                // Write the body of the message to the console
                _logger.LogInformation($"Sending message: {messageBody}");

                // Send the message to the queue
                await _queueClient.SendAsync(serviceBusMessage);

            }
            catch (Exception exception)
            {
                _logger.LogError($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        async void IDisposable.Dispose()
        {
            await _queueClient.CloseAsync();
        }
    }
}
