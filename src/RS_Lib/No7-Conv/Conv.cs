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
        public double[,] ConvedData { get; private set; }

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
            if (k.GetLength(0) != k.GetLength(1))
            {
                throw new ArgumentException("卷积核必须为正方形！");
            }
            this._kernel = k;
        }

        /// <summary>
        /// 卷积运算
        /// </summary>
        /// <param name="d">原始数据+波段号</param>
        /// <param name="k">卷积核</param>
        /// <param name="vinir">出界时算什么</param>
        public Conv(byte[,] d, double[,] k, double vinir)
        {
            this._oriData = d;
            this._valueIfNotInRange = vinir;
            if (k.GetLength(0) != k.GetLength(1))
            {
                throw new ArgumentException("卷积核必须为正方形！");
            }
            this._kernel = k;
        }
       
        /// <summary>
        /// 数据是否在范围里
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private bool isInRange(int i, int j)
        {
            return (i >= 0 && j >= 0 && i + 1 <= _oriData.GetLength(0) && j + 1 <= _oriData.GetLength(1));
        }
    }
}
