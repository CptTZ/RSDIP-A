using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    /// <summary>
    /// 卷积运算
    /// </summary>
    public class Conv
    {
        public double[,] ConvedData;

        private readonly byte[,] _oriData;
        private readonly double[,] _kernel;

        public Conv(byte[,,] d, int b, double[,] k)
        {
            _oriData = new byte[d.GetLength(1), d.GetLength(2)];

            for (int i = 0; i < d.GetLength(1); i++)
            {
                for (int j = 0; j < d.GetLength(2); j++)
                {
                    _oriData[i, j] = d[b, i, j];
                }
            }

            this._kernel = k;
        }

        public Conv(byte[,] d, double[,] k)
        {
            this._oriData = d;
            this._kernel = k;
        }
       

        private bool isInRange(int i, int j, double valueIfNotInRange=0)
        {
            return (i >= 0 && j >= 0 && i + 1 <= _oriData.GetLength(0) && j + 1 <= _oriData.GetLength(1));
        }
    }
}
