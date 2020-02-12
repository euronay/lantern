using Lantern.Core.Patterns;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lantern.Core.Devices
{
    public class FakeLightStrip : ILightStrip
    {
        private ILogger _logger;

        public FakeLightStrip(ILogger<FakeLightStrip> logger)
        {
            _logger = logger;
        }

        public void Clear()
        {
            _logger.LogInformation("Fake logger: Clearing LEDs");
        }

        public Task RunAsync(IPattern pattern)
        {
            _logger.LogInformation($"Fake logger: Running Pattern {pattern.GetType().Name}");
            return Task.CompletedTask;
        }

        public Task RunAsync(IPattern pattern, TimeSpan duration)
        {
            _logger.LogInformation($"Fake logger: Running Pattern {pattern.GetType().Name} for duration {duration.TotalMilliseconds}ms");
            return Task.CompletedTask;
        }

        public void Stop()
        {
            _logger.LogInformation("Fake logger: Stop");
        }
    }

}
