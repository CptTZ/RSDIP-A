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
        private struct Pixel
        {
            public double H, S, I;
        }
        
        public double[,,] HSIData { get; private set; }

        private readonly byte[] _band;
        private readonly byte[,,] _oriData;
        private double[] _minV, _maxV;

        private byte LinearSt(int i, double k, double v)
        {
            int d = (int)((v - _minV[i]) * k + 0.5);

            if (d < 0)
                return 0;
            else if (d > 255)
                return 255;
            else
                return (byte)d;
        }

        public byte[,,] GetLinearStretch()
        {
            int h = HSIData.GetLength(1),
                l = HSIData.GetLength(2);
            
            byte[,,] res = new byte[3, h, l];

            for (int i = 0; i < 3; i++)
            {
                double s = 255.0 / (_maxV[i] - _minV[i]);
                for (int j = 0; j < h; j++)
                {
                    for (int k = 0; k < l; k++)
                    {
                        res[i, j, k] = LinearSt(i, s, HSIData[i, j, k]);
                    }
                }
            }

            return res;
        }

        public RGBHSI(byte[,,] d, byte[] b)
        {
            if (b.Length != 3) throw new ArgumentException("选中波段必须为3个！");

            _oriData = d;
            _band = b;
            HSIData = new double[3, d.GetLength(1), d.GetLength(2)];

            InitMaxMin();
            TransRGB();
        }

        private void InitMaxMin()
        {
            _minV = new double[3];
            _maxV = new double[3];

            for (int i = 0; i < _minV.Length; i++)
            {
                _minV[i] = double.MaxValue;
                _maxV[i] = double.MinValue;
            }
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

                    if (_minV[0] > a.H) _minV[0] = a.H;
                    if (_minV[1] > a.S) _minV[1] = a.S;
                    if (_minV[2] > a.I) _minV[2] = a.I;
                    if (_maxV[0] < a.H) _maxV[0] = a.H;
                    if (_maxV[1] < a.S) _maxV[1] = a.S;
                    if (_maxV[2] < a.I) _maxV[2] = a.I;

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
            
            double min = Math.Min(Math.Min(R, G), B),
                max = Math.Max(Math.Max(R, G), B),
                dt = max - min;

            Pixel px = new Pixel {I = max};

            // max!=0
            if (Math.Abs(max) > 1e-9)
            {
                px.S = dt/max;
            }
            else
            {
                // I无法计算
                px.S = 0;
                px.H = -1;
                return px;
            }

            if (Math.Abs(R - max) < 1e-9)
            {
                px.H = (G - B)/dt;
            }
            else if(Math.Abs(G - max) < 1e-9)
            {
                px.H = 2 + (B - R)/dt;
            }
            else
            {
                px.H = 4 + (R - G)/dt;
            }

            px.H *= 60;
            if (px.H < 0)
                px.H += 360;

            return px;
        }


    }
}
