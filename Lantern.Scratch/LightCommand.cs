using System.Drawing;
using Newtonsoft.Json;

namespace Lantern.Scratch
{
    public class LightCommand
    {
        public LightCommandType Command { get; set; }

        [JsonConverter(typeof(ColorArgbConverter))]
        public Color Color {get; set;}
    }
    
}