using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Expression;

namespace RS_Lib
{
    public class BandMath
    {

        public byte[,] CalculatedResult { get; private set; }

        private readonly float[,] _result;
        private readonly string _oriStatement;
        private readonly string[] _mark;
        private readonly byte[][,] _img;
        private float _minV, _maxV, _s;

        private byte LinearSt(float v)
        {
            int d = (int)((v - _minV) * _s + 0.5);

            if (d < 0)
                return 0;
            else if (d > 255)
                return 255;
            else
                return (byte)d;
        }

        private void FindMaxMin()
        {
            _minV = float.MaxValue;
            _maxV = float.MinValue;

            foreach (var t in _result)
            {
                if (t > _maxV) _maxV = t;
                if (t < _minV) _minV = t;
            }

            _s = 255.0f / (_maxV - _minV);
        }

        /// <summary>
        /// 结果线性拉伸
        /// </summary>
        private void ProcessResult()
        {
            FindMaxMin();

            for (int i = 0; i < _result.GetLength(0); i++)
            {
                for (int j = 0; j < _result.GetLength(1); j++)
                {
                    CalculatedResult[i, j] = LinearSt(_result[i, j]);
                }
            }
        }

        /// <summary>
        /// BandMath类
        /// </summary>
        public BandMath(string st, string[] d, byte[][,] m)
        {
            this._oriStatement = "=" + st;
            this._mark = d;
            this._img = m;
            this._result = new float[_img[0].GetLength(0), _img[0].GetLength(1)];
            this.CalculatedResult = new byte[_img[0].GetLength(0), _img[0].GetLength(1)];

            CalcAll();
            ProcessResult();
        }

        private void CheckInput()
        {
            if (_img.Any(t => t.GetLength(0) != t.GetLength(1)))
            {
                throw new ArgumentException("参与波段运算的图像，其长宽必须一样！");
            }
        }

        private void CalcAll()
        {
            CheckInput();
            string exp = string.Copy(_oriStatement);

            // 图像分成四部分算
            Thread[] calcThreads =
            {
                new Thread(() => Calc1(exp)),
                new Thread(() => Calc2(exp)),
                new Thread(() => Calc3(exp)),
                new Thread(() => Calc4(exp))
            };

            foreach (var t in calcThreads)
            {
                t.Start();
            }

            foreach (var t in calcThreads)
            {
                t.Join();
            }
        }

        #region 4个子计算过程
        private void Calc1(string exp)
        {
            Formula f1 = new Formula();
            for (int i = 0; i < _result.GetLength(0) / 2; i++)
            {
                for (int j = 0; j < _result.GetLength(1) / 2; j++)
                {
                    string tmp = string.Copy(exp);
                    for (int k = 0; k < _mark.Length; k++)
                    {
                        tmp = ProcessSubstitude(tmp, _mark[k], i, j, k);
                    }

                    f1.Statement = tmp;
                    if (f1.Evaluate() != null)
                        _result[i, j] = f1.Result.AsFloat;
                }
            }
        }

        private void Calc2(string exp)
        {
            Formula f2 = new Formula();
            for (int i = 0; i < _result.GetLength(0) / 2; i++)
            {
                for (int j = _result.GetLength(1) / 2; j < _result.GetLength(1); j++)
                {
                    string tmp = string.Copy(exp);
                    for (int k = 0; k < _mark.Length; k++)
                    {
                        tmp = ProcessSubstitude(tmp, _mark[k], i, j, k);
                    }

                    f2.Statement = tmp;
                    if (f2.Evaluate() != null)
                        _result[i, j] = f2.Result.AsFloat;
                }
            }
        }

        private void Calc3(string exp)
        {
            Formula f3 = new Formula();
            for (int i = _result.GetLength(0) / 2; i < _result.GetLength(0); i++)
            {
                for (int j = 0; j < _result.GetLength(1) / 2; j++)
                {
                    string tmp = string.Copy(exp);
                    for (int k = 0; k < _mark.Length; k++)
                    {
                        tmp = ProcessSubstitude(tmp, _mark[k], i, j, k);
                    }

                    f3.Statement = tmp;
                    if (f3.Evaluate() != null)
                        _result[i, j] = f3.Result.AsFloat;
                }
            }
        }

        private void Calc4(string exp)
        {
            Formula f4 = new Formula();
            for (int i = _result.GetLength(0) / 2; i < _result.GetLength(0); i++)
            {
                for (int j = _result.GetLength(1) / 2; j < _result.GetLength(1); j++)
                {
                    string tmp = string.Copy(exp);
                    for (int k = 0; k < _mark.Length; k++)
                    {
                        tmp = ProcessSubstitude(tmp, _mark[k], i, j, k);
                    }

                    f4.Statement = tmp;
                    if (f4.Evaluate() != null)
                        _result[i, j] = f4.Result.AsFloat;
                }
            }
        }
        #endregion

        private string ProcessSubstitude(string raw, string mark, int i, int j, int index)
        {
            return raw.Replace(mark, _img[index][i, j].ToString());
        }


    }
}
