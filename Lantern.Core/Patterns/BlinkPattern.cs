using System;
using System.Drawing;
using System.Threading;
using Lantern.Core.Colors;

namespace Lantern.Core.Patterns
{
    public class BlinkPattern : IPattern
    {
        private Color _color;
        private TimeSpan _blinkRate;

        private double _timer = 0;
        private bool _blinkOn;

        public BlinkPattern(Color color, TimeSpan blinkRate)
        {
            _color = color;
            _blinkRate = blinkRate;

            if(_blinkRate == Timeout.InfiniteTimeSpan)
                _blinkRate = TimeSpan.FromMilliseconds(200);
        }

        public void Update(double delta)
        {
            // timer in milliseconds
            _timer += delta * 1000;
            if(_timer >= _blinkRate.Milliseconds)
            {
                _blinkOn = !_blinkOn;
                _timer = 0;
            }
        }

        public Color Render(int index)
        {
            return _blinkOn ? _color : Color.Black;
        }
        
    }
}