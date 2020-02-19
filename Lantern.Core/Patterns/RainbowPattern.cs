using System;
using System.Drawing;
using Lantern.Core.Colors;

namespace Lantern.Core.Patterns
{
    public class RainbowPattern : IPattern
    {
        private Color[] _colors = new Color[]{Color.Red, Color.OrangeRed, Color.Yellow, Color.Green, Color.Cyan, Color.Blue, Color.DarkViolet, Color.Magenta};

        private Color[] _rainbow;
        private int _offset = 0;

        private double _timer = 0;

        public RainbowPattern(int length)
        {
            _rainbow = new Color[length];
            FillRainbow();
        }

        public void Update(double delta)
        {
            _timer += delta;
            if(_timer >= 0.1)
            {
                FillRainbow();
                _timer = 0;
            }
        }

        public Color Render(int index)
        {
            return _rainbow[index];
        }

        private void FillRainbow()
        {
            if(_offset >= _colors.Length)
                _offset = 0;

            var currentColor = _offset;

            for(int i=0; i<_rainbow.Length; i++)
            {
                _rainbow[i] = _colors[currentColor];
                currentColor ++;

                if(currentColor >= _colors.Length)
                    currentColor = 0;
            }

            _offset ++;
        }
        
    }
}