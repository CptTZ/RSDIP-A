using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    /// <summary>
    /// KT变换至少要用float存储
    /// </summary>
    public class KT
    {

        public double[,,] TransformedData { get; private set; }

        private readonly byte[,,] _oriData;
        private readonly byte _transType;
        private readonly double[,] _transMatrix;
        private double[] _minV, _maxV;

        private byte LinearSt(int i, double k, double v)
        {
            int d = (int) ((v - _minV[i])*k + 0.5);

            if (d < 0)
                return 0;
            else if (d > 255)
                return 255;
            else
                return (byte) d;
        }

        /// <summary>
        /// 获取KT变换后线性拉伸的结果
        /// </summary>
        /// <returns></returns>
        public byte[,,] GetLinearStretch()
        {
            int b = TransformedData.GetLength(0),
                h = TransformedData.GetLength(1),
                l = TransformedData.GetLength(2);

            byte[,,] res = new byte[b, h, l];

            for (int i = 0; i < b; i++)
            {
                double s = 255.0 / (_maxV[i] - _minV[i]);
                for (int j = 0; j < h; j++)
                {
                    for (int k = 0; k < l; k++)
                    {
                        res[i, j, k] = LinearSt(i, s, TransformedData[i, j, k]);
                    }
                }
            }

            return res;
        }

        private void GenerateNew()
        {
            int hang = _oriData.GetLength(1),
                lie = _oriData.GetLength(2);

            switch (this._transType)
            {
                case 4:
                case 7:
                    this.TransformedData =
                        new double[6, hang, lie];
                    this._minV = new double[6];
                    this._maxV = new double[6];
                    break;
                case 5:
                    this.TransformedData =
                        new double[4, hang, lie];
                    this._minV = new double[4];
                    this._maxV = new double[4];
                    break;
            }

            for (int i = 0; i < _minV.Length; i++)
            {
                _minV[i] = double.MaxValue;
                _maxV[i] = double.MinValue;
            }

        }

        /// <summary>
        /// 执行KT变换
        /// </summary>
        /// <param name="d"></param>
        /// <param name="t">4,5,7</param>
        public KT(byte[,,] d,byte t)
        {
            if (d.GetLength(0) != 7) 
                throw new ArgumentException("请使用原始的Landsat系列图像！");

            if (t != 4 && t != 5 && t != 7) 
                throw new ArgumentException("变换类型不支持！");

            this._transType = t;
            this._oriData = d;
            this._transMatrix = KT_Paras.GetMatrixByType(t);
           
            GenerateNew();
            K_T_Transform();
        }

        private void K_T_Transform()
        {
            for (int i = 0; i < this._oriData.GetLength(1); i++)
            {
                for (int j = 0; j < this._oriData.GetLength(2); j++)
                {
                    var result = MatBy(this._transMatrix, GetDataByRowCol(i, j));
                    AddR(result);
                    SaveData(result, i, j);
                }
            }
        }

        private void SaveData(double[,] d,int x,int y)
        {
            for (int i = 0; i < 4; i++)
            {
                TransformedData[i, x, y] = d[i, 0];
                _maxV[i] = d[i, 0] > _maxV[i] ? d[i, 0] : _maxV[i];
                _minV[i] = d[i, 0] < _minV[i] ? d[i, 0] : _minV[i];
            }
            if (this._transType != 5)
            {
                for (int i = 4; i < 6; i++)
                {
                    TransformedData[i, x, y] = d[i, 0];
                    _maxV[i] = d[i, 0] > _maxV[i] ? d[i, 0] : _maxV[i];
                    _minV[i] = d[i, 0] < _minV[i] ? d[i, 0] : _minV[i];
                }
            }
        }

        /// <summary>
        /// Landsat5-加常数项
        /// </summary>
        private void AddR(double[,] ds)
        {
            if (this._transType != 5) return;

            ds[0, 0] += 10.3695;
            ds[1, 0] += (-0.731);
            ds[2, 0] += (-3.3828);
            ds[3, 0] += 0.7879;
        }

        private double[,] GetDataByRowCol(int x, int y)
        {
            double[,] res = new double[6, 1];
            res[0, 0] = this._oriData[0, x, y];
            res[1, 0] = this._oriData[1, x, y];
            res[2, 0] = this._oriData[2, x, y];
            res[3, 0] = this._oriData[3, x, y];
            res[4, 0] = this._oriData[4, x, y];
            res[5, 0] = this._oriData[6, x, y];

            return res;
        }

        /// <summary>
        /// 矩阵相乘
        /// </summary>
        /// <param name="left">左</param>
        /// <param name="right">右</param>
        /// <returns></returns>
        private double[,] MatBy(double[,] left, double[,] right)
        {
            if (left.GetLength(1) != right.GetLength(0))
                throw new ArgumentException("矩阵不可乘！");

            double[,] result = new double[left.GetLength(0), right.GetLength(1)];

            for (int i = 0; i < left.GetLength(0); i++)
            {
                for (int j = 0; j < right.GetLength(1); j++)
                {
                    for (int k = 0; k < left.GetLength(1); k++)
                    {
                        result[i, j] += left[i, k] * right[k, j];
                    }
                }
            }

            return result;
        } 

    }
}
