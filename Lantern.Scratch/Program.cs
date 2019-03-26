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

            Console.WriteLine("Running Cycle pattern");
            device.RunAsync(new CycleHuePattern());

            while(!Console.KeyAvailable) {}
            device.Stop();

            Console.ReadKey();

            Console.WriteLine("Running Jump pattern");
            device.RunAsync(new JumpHuePattern());

            while(!Console.KeyAvailable) {}
            device.Stop();
        }
    }
}
