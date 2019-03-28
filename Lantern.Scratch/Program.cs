using System;
using Lantern.Core;
using Lantern.Core.Patterns;

namespace Lantern.Scratch
{
    class Program
    {
        static void Main(string[] args)
        {
            var device = new LightStrip(30);

            Console.WriteLine("Running Rainbow pattern");
            device.RunAsync(new RainbowPattern(30));

            while(!Console.KeyAvailable) {}
            device.Stop();

            Console.ReadKey();

        }
    }
}
