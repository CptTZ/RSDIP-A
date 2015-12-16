using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    /// <summary>
    /// RGB转HSI
    /// </summary>
    public class RGBHSI
    {
        internal struct Pixel
        {
            public double H, S, I;
        }
        
        public double[,,] HSIData { get; private set; }


        private readonly byte[] _band;
        private readonly byte[,,] _oriData;

        public RGBHSI(byte[,,] d, byte[] b)
        {
            if (b.Length != 3) throw new ArgumentException("选中波段必须为3个！");

            _oriData = d;
            _band = b;
            HSIData = new double[3, d.GetLength(1), d.GetLength(2)];
        }

        private void TransRGB()
        {
            for (int i = 0; i < _oriData.GetLength(1); i++)
            {
                for (int j = 0; j < _oriData.GetLength(2); j++)
                {
                    var a = RGB2HSI(
                        _oriData[_band[0], i, j],
                        _oriData[_band[1], i, j],
                        _oriData[_band[2], i, j]);

                    HSIData[0, i, j] = a.H;
                    HSIData[1, i, j] = a.S;
                    HSIData[2, i, j] = a.I;
                }
            }
        }

        private Pixel RGB2HSI(double R, double G, double B)
        {
            R = R / 255;
            G = G / 255;
            B = B / 255;

            double theta = Math.Acos(0.5 * ((R - G) + (R - B)) / Math.Sqrt((R - G) * (R - G) + (R - B) * (G - B))) / (2 * Math.PI);

            return new Pixel
            {
                H = (B <= G) ? theta : (1 - theta),
                S = 1 - 3 * Math.Min(Math.Min(R, G), B) / (R + G + B),
                I = (R + G + B) / 3
            }; 
        }


    }
}
