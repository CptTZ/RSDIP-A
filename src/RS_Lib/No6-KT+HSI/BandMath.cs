using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Expression;

namespace RS_Lib
{
    public class BandMath
    {

        public byte[,] CalculatedResult { get; private set; }

        private readonly float[,] _result;
        private readonly Formula _fm = new Formula();
        private readonly string _oriStatement;
        private readonly string[] _mark;
        private readonly byte[][,] _img;

        private void ProcessResult()
        {
            for (int i = 0; i < _result.GetLength(0); i++)
            {
                for (int j = 0; j < _result.GetLength(1); j++)
                {
                    if (_result[i, j] > 254.5)
                        CalculatedResult[i, j] = 255;
                    else if (_result[i, j] < 0)
                        CalculatedResult[i, j] = 0;
                    else
                        CalculatedResult[i, j] = (byte) (_result[i, j] + 0.5);
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

        private void CalcAll()
        {
            string exp = string.Copy(_oriStatement);

            for (int i = 0; i < _result.GetLength(0); i++)
            {
                for (int j = 0; j < _result.GetLength(1); j++)
                {
                    string tmp = string.Copy(exp);
                    for (int k = 0; k < _mark.Length; k++)
                    {
                        tmp = ProcessState(tmp, _mark[k], i, j, k);
                    }

                    _fm.Statement = tmp;
                    if (_fm.Evaluate() != null)
                        _result[i, j] = _fm.Result.AsFloat;
                }
            }
        }

        private string ProcessState(string raw, string mark, int i, int j, int index)
        {
            return raw.Replace(mark, _img[index][i, j].ToString());
        }


    }
}
