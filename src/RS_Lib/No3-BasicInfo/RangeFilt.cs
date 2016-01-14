using System;
using System.Collections.Generic;
using System.Linq;

namespace RS_Lib
{
    /// <summary>
    /// 极差纹理
    /// </summary>
    public class RangeFilt
    {
        public byte[,] JCWL { get; private set; }

        private readonly byte[,] _oriData;

        public RangeFilt(byte[,] o)
        {
            _oriData = o;
            JCWL = new byte[o.GetLength(0), o.GetLength(1)];

            FillData();
        }

        public RangeFilt(byte[,,] o, int band)
        {
            JCWL = new byte[o.GetLength(1), o.GetLength(2)];
            _oriData = new byte[o.GetLength(1), o.GetLength(2)];

            for (int i = 0; i < _oriData.GetLength(0); i++)
            {
                for (int j = 0; j < _oriData.GetLength(1); j++)
                {
                    _oriData[i, j] = o[band, i, j];
                }
            }

            FillData();
        }

        private void FillData()
        {
            for (int i = 0; i < _oriData.GetLength(0); i++)
            {
                for (int j = 0; j < _oriData.GetLength(1); j++)
                {
                    JCWL[i, j] = CalcNewValue(i, j);
                }
            }
        }

        private byte CalcNewValue(int x, int y)
        {
            List<byte> tmp = new List<byte>();

            // 3*3的范围
            for (int i = x - 1; i <= x + 1; i++) 
            {
                for (int j = y - 1; j < y + 1; j++)
                {
                    if (isInRange(i, j) == true)
                    {
                        tmp.Add(_oriData[i, j]);
                    }
                }
            }

            return (byte) (tmp.Max() - tmp.Min());
        }

        private bool isInRange(int i, int j)
        {
            return i >= 0 && j >= 0 && i < _oriData.GetLength(0) && j < _oriData.GetLength(1);
        }
    }
}
