using System;
using System.Drawing;
using Lantern.Core.Colors;

namespace Lantern.Core.Patterns
{
    public class HalloweenPattern : IPattern
    {
        private Hsl _color1 = new Hsl(Color.GreenYellow);
        private Hsl _color2 = new Hsl(Color.DarkGreen);

        private Hsl[] _colors;
        

        private double _timer = 0;

        public HalloweenPattern(int length)
        {
            _colors = new Hsl[length];

                for(int i=0; i<length; i++)
                {
                var h = _color1.H + (int)((_color2.H - _color1.H) * i / length);
                var s = _color1.S + (int)((_color2.S - _color1.S) * i / length);
                var l = _color1.L + (int)((_color2.S - _color1.L) * i / length);

                _colors[i] = new Hsl(h,s,l);
                }
        }

        public void Update(double delta)
        {
            _timer += delta;
            if(_timer >= 0.1)
            {
//                FillRainbow();
                _timer = 0;
            }
        }

        public Color Render(int index)
        {
            return _colors[index].Rgb;
        }

        
    }
}