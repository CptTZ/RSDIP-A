using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    public class Grad
    {
        public byte[,,] GradData { get; private set; }

        private Conv[] _data;
        private readonly RsImage _img;
        private readonly int _o, _x, _y;

        /// <summary>
        /// 梯度倒数加权
        /// </summary>
        public Grad(RsImage r, int x, int y, int o)
        {
            this._img = r;
            CheckSize(x, y, o);

            this._o = o;
            this._x = x;
            this._y = y;

            MakeKernel();

            GradData = new byte[r.BandsCount, r.Lines, r.Samples];

        }

        private double[,] MakeKernel()
        {
            double[,] kern = new double[_x, _y];

            for (int i = 0; i < _x; i++)
            {
                for (int j = 0; j < _y; j++)
                {
                    
                }
            }

            return kern;
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
