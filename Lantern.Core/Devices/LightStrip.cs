using System;
using System.Device.Spi;
using System.Device.Spi.Drivers;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
using Lantern.Core.Patterns;

namespace Lantern.Core.Devices
{
    public class LightStrip : ILightStrip
    {
        public int LedCount { get; set; }
        
        public Ws2812b Device { get; private set; }
        private CancellationTokenSource _cancellationTokenSource;

        public LightStrip(int count)
        {
            LedCount = count;

            var settings = new SpiConnectionSettings(0, 0) {
                ClockFrequency = 2_400_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };
            
		    // Create a Neo Pixel x8 stick on spi 0.0
            var spi = new UnixSpiDevice(settings);
            
            Device = new Ws2812b(spi, count);
        }

        public async Task RunAsync(IPattern pattern)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            await RunInternalAsync(pattern);
        }

        public async Task RunAsync(IPattern pattern, TimeSpan duration)
        {
            _cancellationTokenSource = new CancellationTokenSource(duration);

            await RunInternalAsync(pattern);
        }

        private async Task RunInternalAsync(IPattern pattern)
        {
            await Task.Run(() => {
                long currentTick = DateTime.Now.Ticks;
                long lastTick = DateTime.Now.Ticks;
                double delta = 0;

                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    currentTick = DateTime.Now.Ticks;
                    delta = (double)(currentTick - lastTick) / (double)TimeSpan.TicksPerSecond;
                    lastTick = currentTick;

                    pattern.Update(delta);

                    BitmapImage img = Device.Image;
                    img.Clear();
                    for (int i = 0; i < LedCount; i++)
                    {
                        img.SetPixel(i, 0, pattern.Render(i));
                    }
                    Device.Update();
                }
                Clear();
            }, _cancellationTokenSource.Token);
        }

        public void Stop()
        {
            if(_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();

            Clear();
        }

        public void Clear()
        {
            BitmapImage img = Device.Image;
            img.Clear();
            Device.Update();
        }
    }
}
