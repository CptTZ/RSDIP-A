using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    public class GaussLow
    {
        public byte[,,] GaussLowData { get; private set; }

        private Conv[] _data;
        private readonly RsImage _img;
        private readonly int _o, _x, _y;
        private double[,] _kernel;

        /// <summary>
        /// 高斯低通
        /// </summary>
        /// <param name="r">图像</param>
        /// <param name="x">卷积行</param>
        /// <param name="y">卷积列</param>
        /// <param name="o">方差</param>
        public GaussLow(RsImage r, int x,int y,int o)
        {
            this._img = r;
            CheckSize(x, y, o);

            this._o = o;
            this._x = x;
            this._y = y;
            
            MakeKernel();

            GaussLowData = new byte[r.BandsCount, r.Lines, r.Samples];

            Calc();
            FillData();
        }

        private void FillData()
        {
            for (int b = 0; b < _img.BandsCount; b++)
            {
                var data = _data[b].GetLinearStretch();
                for (int i = 0; i < _img.Lines; i++)
                {
                    for (int j = 0; j < _img.Samples; j++)
                    {
                        GaussLowData[b, i, j] = data[i, j];
                    }
                }
            }
        }

        private void Calc()
        {
            _data = new Conv[_img.BandsCount];
            for (int i = 0; i < _img.BandsCount; i++)
            {
                _data[i] = new Conv(_img.GetPicData(i + 1), this._kernel);
            }
        }

        /// <summary>
        /// 生成卷积核-书P163
        /// </summary>
        private void MakeKernel()
        {
            this._kernel = new double[_x, _y];

            for (int i = 0; i < _x; i++)
            {
                for (int j = 0; j < _y; j++)
                {
                    _kernel[i, j] = ZTFB(i - _x / 2, j - _y / 2);
                }
            }
        }

        /// <summary>
        /// 正态分布计算
        /// </summary>
        /// <returns></returns>
        private double ZTFB(int xx, int yy)
        {
            double x = xx, y = yy;

            return Math.Exp(-(x * x + y * y) / (2 * _o * _o)) ;
        }

        private void CheckSize(int kx, int ky, int o)
        {
            if (kx < 3 || kx % 2 == 0 || ky < 3 || ky % 2 == 0 || o < 0)
            {
                throw new ArgumentException("高斯低通滤波区域必须为奇数，且大于3");
            }
        }
    }
}
