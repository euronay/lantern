using System;
using System.Drawing;
using System.Threading;
using Lantern.Core.Colors;

namespace Lantern.Core.Patterns
{
    public class ChasePattern : IPattern
    {
        private Hsl _color;
        private TimeSpan _chaseRate;
        private int _offset = 0;
        private double _timer = 0;

        private Color[] _buffer;

        public ChasePattern(Color color, int length, TimeSpan chaseRate)
        {
            _color = new Hsl(color);
            _buffer = new Color[length];
            _chaseRate = chaseRate;

            if(_chaseRate == Timeout.InfiniteTimeSpan)
                _chaseRate = TimeSpan.FromMilliseconds(50);

            Fill();
        }

        public void Update(double delta)
        {
            // Every 200ms, move the light on one
            _timer += delta * 1000;
            if(_timer >= _chaseRate.Milliseconds)
            {
                Fill();
                _timer = 0;
            }
        }

        public Color Render(int index)
        {
            return _buffer[index];
        }

        private void Fill()
        {
            if(_offset >= _buffer.Length)
                _offset = 0;


            for(int i=0; i<_buffer.Length; i++)
            {
                if (i == _offset)
                {
                    _buffer[i] = _color.Rgb;
                }
                else
                {
                    _buffer[i] = Color.Black;
                }
            }

            _offset ++;
        }
        
    }
}