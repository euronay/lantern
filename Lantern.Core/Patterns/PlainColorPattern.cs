using System;
using System.Drawing;
using Lantern.Core.Colors;

namespace Lantern.Core.Patterns
{
    public class PlainColorPattern : IPattern
    {
        private Color _color;

        public PlainColorPattern(Color color)
        {
            _color = color;
        }

        public void Update(double delta)
        {
 
        }

        public Color Render(int index)
        {
            return _color;
        }
        
    }
}