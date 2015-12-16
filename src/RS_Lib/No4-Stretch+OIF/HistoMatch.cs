using System;

namespace RS_Lib
{
    /// <summary>
    /// 直方图规定化(直方图匹配)
    /// </summary>
    public class HistoMatch
    {
        private readonly byte[,] _oriImg;
        private readonly int[] _oriAcc;
        private readonly int[] _targetAcc;
        private readonly int[] _mappingTable;

        public byte[,] MatchedData { get; private set; }

        /// <summary>
        /// 构造函数-任意两波段或图像
        /// </summary>
        /// <param name="a">原始图像</param>
        /// <param name="b">拟合到的图像</param>
        public HistoMatch(byte[,] a, byte[,] b)
        {
            _oriImg = a;
            HistoData dat = new HistoData(a);
            _oriAcc = dat.GetAccHistogramData();

            dat = new HistoData(b);
            _targetAcc = dat.GetAccHistogramData();

            _mappingTable = new int[_oriAcc.Length];

            StartMatch();
        }

        private void StartMatch()
        {
            int k = 0;
            for (int i = 0; i < 256; i++)
            {
                double diffB = 1;
                for (int j = k; j < 256; j++)
                {
                    double diffA = Math.Abs(_oriAcc[i] - _targetAcc[j]);
                    // diffA==diffB
                    if (Math.Abs(diffA - diffB) < 1.0E-08)
                    {
                        diffB = diffA;
                        k = j;
                    }
                    else
                    {
                        k = (j - 1);
                        break;
                    }
                }
                if (k == 255)
                {
                    for (int l = i; l < 256; l++)
                    {
                        _mappingTable[l] = k;

                    }
                    break;
                }
                _mappingTable[i] = k;
            }

            MakeNewImg();
        }

        private void MakeNewImg()
        {
            MatchedData = new byte[_oriImg.GetLength(0), _oriImg.GetLength(1)];

            for (int i = 0; i < _oriImg.GetLength(0); i++)
            {
                for (int j = 0; j < _oriImg.GetLength(1); j++)
                {
                    var tmp = _mappingTable[_oriImg[i, j]];
                    if (tmp < 0)
                    {
                        MatchedData[i, j] = 0;
                    }
                    else if (tmp > 255)
                    {
                        MatchedData[i, j] = 255;
                    }
                    else
                    {
                        MatchedData[i, j] = (byte)tmp;
                    }
                }
            }
        }
    }
}
