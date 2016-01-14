using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    public class Lap
    {
        /// <summary>
        /// 均值滤波结果
        /// </summary>
        public byte[,,] Lapla { get; private set; }
        private Conv[] _data;
        private readonly RsImage _img;

        /// <summary>
        /// 均值滤波
        /// </summary>
        /// <param name="r"></param>
        public Lap(RsImage r)
        {
            this._img = r;
            this.Lapla = new byte[r.BandsCount, r.Lines, r.Samples];

            CalcMean();
            FillData();
        }

        private void FillData()
        {
            var ori = _img.GetPicData();
            for (int b = 0; b < _img.BandsCount; b++)
            {
                var data = _data[b].ConvedData;
                for (int i = 0; i < _img.Lines; i++)
                {
                    for (int j = 0; j < _img.Samples; j++)
                    {
                        Lapla[b, i, j] = (byte) (data[i, j] + ori[b, i, j]);
                    }
                }
            }
        }

        private void CalcMean()
        {
            // 卷积核书P170-ENVI
            double[,] kernel =
            {
                {0, -1, 0},
                {-1, 4, -1},
                {0, -1, 0}
            };

            _data = new Conv[_img.BandsCount];
            for (int i = 0; i < _img.BandsCount; i++)
            {
                _data[i] = new Conv(_img.GetPicData(i + 1), kernel);
            }

        }

    }
}
