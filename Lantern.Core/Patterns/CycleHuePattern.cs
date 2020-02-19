using System;
using System.Drawing;
using Lantern.Core.Colors;

namespace Lantern.Core.Patterns
{
    public class CycleHuePattern : IPattern
    {
        private Hsl _color;

        public CycleHuePattern()
        {
            _color = new Hsl(0,1,0.5);
        }

        public void Update(double delta)
        {
            // This adds 1 to the hue 10 times per second
            _color.H = _color.H + (10d * delta);
        }

        public Color Render(int index)
        {
            return _color.Rgb;
        }
        
    }
}