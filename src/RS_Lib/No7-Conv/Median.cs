using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    public class Median
    {
        public byte[,,] MedianData { get; private set; }

        private readonly int _sizeX, _sizeY;
        private readonly byte[,,] _img;

        /// <summary>
        /// 中值滤波
        /// </summary>
        /// <param name="r">图像</param>
        /// <param name="x">行大小</param>
        /// <param name="y">列大小</param>
        public Median(RsImage r, int x, int y)
        {
            this._img = r.GetPicData();
            this._sizeX = x;
            this._sizeY = y;
            this.MedianData = new byte[r.BandsCount, r.Lines, r.Samples];

            CheckSize(x, y);
            CalcMedian();
        }

        private byte CalcNewValue(int i, int j, int b)
        {
            List<byte> tmp = new List<byte>();

            for (int kI = i - _sizeX / 2; kI <= i + _sizeX / 2; kI++)
            {
                for (int kJ = j - _sizeY / 2; kJ <= j + _sizeY / 2; kJ++)
                {
                    if (isInRange(kI, kJ)) 
                        tmp.Add(_img[b, kI, kJ]);
                }
            }
            tmp.Sort();

            return tmp[tmp.Count/2];
        }

        private void CalcMedian()
        {
            for (int b = 0; b < _img.GetLength(0); b++)
            {
                for (int i = 0; i < _img.GetLength(1); i++)
                {
                    for (int j = 0; j < _img.GetLength(2); j++)
                    {
                        this.MedianData[b, i, j] = CalcNewValue(i, j, b);
                    }
                }
            }
        }

        private void CheckSize(int kx, int ky)
        {
            if (kx < 3 || kx % 2 == 0 || ky < 3 || ky % 2 == 0)
            {
                throw new ArgumentException("中值滤波区域必须为奇数，且大于3");
            }
        }

        private bool isInRange(int i, int j)
        {
            return i >= 0 && j >= 0 && i < _img.GetLength(1) && j < _img.GetLength(2);
        }

    }
}
