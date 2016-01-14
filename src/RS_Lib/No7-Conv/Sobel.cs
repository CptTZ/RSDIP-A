using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    public class Sobel
    {
        /// <summary>
        /// Sobel
        /// </summary>
        public byte[,,] SobelData { get; private set; }
        private Conv[] _dataX, _dataY;
        private readonly RsImage _img;

        /// <summary>
        /// Sobel
        /// </summary>
        /// <param name="r"></param>
        public Sobel(RsImage r)
        {
            this._img = r;
            this.SobelData = new byte[r.BandsCount, r.Lines, r.Samples];

            CalcX();
            CalcY();

            FillData();
        }

        private void FillData()
        {
            for (int b = 0; b < _img.BandsCount; b++)
            {
                var x = _dataX[b].ConvedData;
                var y = _dataY[b].ConvedData;

                for (int i = 0; i < _img.Lines; i++)
                {
                    for (int j = 0; j < _img.Samples; j++)
                    {
                        SobelData[b, i, j] = (byte) (Math.Abs(x[i, j]) + Math.Abs(y[i, j]) + 0.5);
                    }
                }
            }
        }

        private void CalcX()
        {
            // 卷积核书P169
            double[,] kernel =
            {
                {-1, 0, 1},
                {-2, 0, 2},
                {-1, 0, 1}
            };

            _dataX = new Conv[_img.BandsCount];
            for (int i = 0; i < _img.BandsCount; i++)
            {
                _dataX[i] = new Conv(_img.GetPicData(i + 1), kernel);
            }

        }

        private void CalcY()
        {
            // 卷积核书P169
            double[,] kernel =
            {
                {-1, -2, -1},
                {0, 0, 0},
                {1, 2, 1}
            };

            _dataY = new Conv[_img.BandsCount];
            for (int i = 0; i < _img.BandsCount; i++)
            {
                _dataY[i] = new Conv(_img.GetPicData(i + 1), kernel);
            }

        }
    }
}
