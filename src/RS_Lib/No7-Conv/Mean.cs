using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    public class Mean
    {
        /// <summary>
        /// 均值滤波结果
        /// </summary>
        public byte[,,] MeanFilter { get; private set; }
        private Conv[] _data;
        private readonly RsImage _img;

        /// <summary>
        /// 均值滤波
        /// </summary>
        /// <param name="r"></param>
        public Mean(RsImage r)
        {
            this._img = r;
            this.MeanFilter = new byte[r.BandsCount, r.Lines, r.Samples];

            CalcMean();
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
                        MeanFilter[b, i, j] = data[i, j];
                    }
                }
            }
        }

        private void CalcMean()
        {
            // 卷积核书P160
            double[,] kernel =
            {
                {0.125, 0.125, 0.125},
                {0.125, 0, 0.125},
                {0.125, 0.125, 0.125}
            };

            _data = new Conv[_img.BandsCount];
            for (int i = 0; i < _img.BandsCount; i++)
            {
                _data[i] = new Conv(_img.GetPicData(i + 1), kernel);
            }

        }

    }
}
