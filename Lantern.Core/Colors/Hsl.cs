using System;
using System.Drawing;

namespace Lantern.Core.Colors
{
    /// <summary>
    /// Color class using hue, saturation and lightness.
    /// </summary>
    /// <remarks>Conversion code taken from http://james-ramsden.com/convert-from-hsl-to-rgb-colour-codes-in-c/</remarks>
    public class Hsl
    {
        private double _h;
        private double _s;
        private double _l;

        /// <summary>Hue</summary>
        public double H 
        { 
            get => _h; 
            set
            {
                if(value < 0)
                {
                    _h = 360;
                }
                else if (value > 360)
                {
                    _h = 0;
                }
                else
                {
                    _h = value;
                }
            }
        }

        /// <summary>Saturation</summary>
        public double S 
        { 
            get => _s; 
            set
            {
                if(value < 0)
                {
                    _s = 1;
                }
                else if (value > 1)
                {
                    _s = 0;
                }
                else
                {
                    _s = value;
                }
            }
        }

        /// <summary>Lightness</summary>
        public double L 
        { 
            get => _l; 
            set
            {
                if(value < 0)
                {
                    _l = 1;
                }
                else if (value > 1)
                {
                    _l = 0;
                }
                else
                {
                    _l = value;
                }
            }
        }

        /// <summary>Creates new HSL value with blank values equivalent to black</summary>
        public Hsl()
        {
            H = 0;
            S = 0;
            L = 0;
        }

        /// <summary>Creates a new HSL value with given hue, saturation and lightness </summary>
        public Hsl(double hue, double saturation, double lightness)
        {
            H = hue;
            S = saturation;
            L = lightness;
        }

        /// <summary>Creates a new HSL value with given RGB color </summary>
        public Hsl(Color color)
        {
            Rgb = color;
        }

        public Color Rgb
        {
            get
            {
                var color = ConvertHslToRgb();
                return color;
            }
            set
            {
                ConvertRgbToHsl(value);
            }
        }

        private Color ConvertHslToRgb()
        {
            double h = _h/360;
            double r = 0, g = 0, b = 0;
            if (_l != 0)
            {
                if (_s == 0)
                    r = g = b = _l;
                else
                {
                    double temp2;
                    if (_l < 0.5)
                        temp2 = _l * (1.0 + _s);
                    else
                        temp2 = _l + _s - (_l * _s);

                    double temp1 = 2.0 * _l - temp2;

                    r = GetColorComponent(temp1, temp2, h + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, h);
                    b = GetColorComponent(temp1, temp2, h - 1.0 / 3.0);
                }
            }
            return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
        }

        private double GetColorComponent(double temp1, double temp2, double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;

            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            else
                return temp1;
        }

        private void ConvertRgbToHsl(Color color)
        {
            H = color.GetHue();
            S = color.GetSaturation();
            L = color.GetBrightness();
        }
    }
}
