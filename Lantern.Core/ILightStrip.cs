using System;
using System.Threading.Tasks;
using Lantern.Core.Patterns;

namespace Lantern.Core
{
    public interface ILightStrip
    {
        Task RunAsync(IPattern pattern, TimeSpan duration);

        void Stop();

        void Clear();
    }
}