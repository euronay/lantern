using System;
using System.Drawing;
using Lantern.Core.Colors;

namespace Lantern.Core.Patterns
{
    public class JumpHuePattern : IPattern
    {
        private Hsl _color;
        private double timer = 0;

        public JumpHuePattern()
        {
            _color = new Hsl(0,1,0.5);
        }

        public void Update(double delta)
        {
            timer += delta;
            if(timer >= 1)
            {
                // once the timer reaches one second, reset and change the hue
                timer = 0;
                _color.H = _color.H + 30;
            }
        }

        public Color Render(int index)
        {
            return _color.Rgb;
        }
        
    }
}