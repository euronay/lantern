using System;
using System.Drawing;
using System.Threading;
using Newtonsoft.Json;

namespace Lantern.Core.Messaging
{
    public class LightCommandMessage
    {
        public LightCommand Command { get; set; }

        [JsonConverter(typeof(ColorArgbConverter))]
        public Color Color {get; set;}

        public TimeSpan Duration { get; set; }

        public TimeSpan Speed {get; set;}

        public LightCommandMessage()
        {
            Duration = Timeout.InfiniteTimeSpan;
            Speed = Timeout.InfiniteTimeSpan;
        }
    }
    
}