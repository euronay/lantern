using System;
using System.Threading.Tasks;
using Lantern.Core.Patterns;

namespace Lantern.Core.Devices
{
    public interface ILightStrip
    {
        Task RunAsync(IPattern pattern);

        Task RunAsync(IPattern pattern, TimeSpan duration);

        void Stop();

        void Clear();
    }
}