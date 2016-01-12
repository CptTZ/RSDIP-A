using System;
using System.Collections.Generic;
using System.Linq;

namespace RS_Lib
{
    /// <summary>
    /// 卷积运算
    /// </summary>
    public class Conv
    {
        /// <summary>
        /// 卷积后原始结果
        /// </summary>
        public double[,] ConvedData { get; private set; }

        private int kSizeX, kSizeY;
        private double _resMax, _resMin;
        private readonly byte[,] _oriData;
        private readonly double[,] _kernel;
        private readonly double _valueIfNotInRange;

        /// <summary>
        /// 卷积运算
        /// </summary>
        /// <param name="d">原始数据</param>
        /// <param name="b">波段号</param>
        /// <param name="k">卷积核</param>
        /// <param name="vinir">出界时算什么</param>
        public Conv(byte[,,] d, int b, double[,] k, double vinir = 0)
        {
            _oriData = new byte[d.GetLength(1), d.GetLength(2)];

            for (int i = 0; i < d.GetLength(1); i++)
            {
                for (int j = 0; j < d.GetLength(2); j++)
                {
                    _oriData[i, j] = d[b, i, j];
                }
            }

            this._valueIfNotInRange = vinir;
            this._kernel = k;

            CalcConv();
        }

        /// <summary>
        /// 卷积运算
        /// </summary>
        /// <param name="d">原始数据+波段号</param>
        /// <param name="k">卷积核</param>
        /// <param name="vinir">出界时算什么</param>
        public Conv(byte[,] d, double[,] k, double vinir = 0)
        {
            this._oriData = d;
            this._valueIfNotInRange = vinir;
            this._kernel = k;

            CalcConv();
        }

        /// <summary>
        /// 卷积后线性拉伸的结果
        /// </summary>
        public byte[,] GetLinearStretch()
        {
            int h = ConvedData.GetLength(0),
                l = ConvedData.GetLength(1);
            byte[,] res = new byte[h, l];

            double s = 255.0 / (_resMax - _resMin);
            for (int j = 0; j < h; j++)
            {
                for (int k = 0; k < l; k++)
                {
                    res[j, k] = LinearSt(s, ConvedData[j, k]);
                }
            }

            return res;
        }

        private byte LinearSt(double k, double v)
        {
            int d = (int)((v - _resMin) * k + 0.5);

            if (d < 0)
                return 0;
            else if (d > 255)
                return 255;
            else
                return (byte)d;
        }

        /// <summary>
        /// 进行卷积运算
        /// </summary>
        private void CalcConv()
        {
            CheckBound(out kSizeX, out kSizeY);

            this.ConvedData = new double[_oriData.GetLength(0), _oriData.GetLength(1)];

            for (int i = 0; i < _oriData.GetLength(0); i++)
            {
                for (int j = 0; j < _oriData.GetLength(1); j++)
                {
                    this.ConvedData[i, j] = CalcNewValue(i, j);
                }   
            }
        }

        /// <summary>
        /// 卷积结果计算
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private double CalcNewValue(int i, int j)
        {
            double result = 0;
            for (int kI = i - kSizeX/2; kI <= i + kSizeX/2; kI++)
            {
                for (int kJ = j - kSizeY/2; kJ <= j + kSizeY/2; kJ++)
                {
                    double oriV = isInRange(kI, kJ) ? this._oriData[kI, kJ] : this._valueIfNotInRange;
                    result += oriV*this._kernel[kI - i + kSizeX/2, kJ - j + kSizeY/2];
                }
            }

            if (result < _resMin) _resMin = result;
            if (result > _resMax) _resMax = result;

            return result;
        }
        
        private void CheckBound(out int kx, out int ky)
        {
            _resMax = double.MinValue;
            _resMin = double.MaxValue;

            kx = this._kernel.GetLength(0);
            ky = this._kernel.GetLength(1);

            if (kx < 3 || kx%2 == 0 || ky < 3 || ky%2 == 0)
            {
                throw new ArgumentException("卷积核必须为奇数，且大于3");
            }
        }

        /// <summary>
        /// 数据是否在范围里
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private bool isInRange(int i, int j)
        {
            return i >= 0 && j >= 0 && i < _oriData.GetLength(0) && j < _oriData.GetLength(1);
        }
    }
}
