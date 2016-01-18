using System;

namespace RS_Lib
{
    /// <summary>
    /// 直方图规定化(直方图匹配)
    /// </summary>
    public class HistoMatch
    {
        private readonly byte[,] _oriImg;
        private readonly int[] _oriAcc, _targetAcc, _mappingTable;


        public byte[,] MatchedData { get; private set; }

        /// <summary>
        /// 构造函数-任意两波段或图像
        /// </summary>
        /// <param name="a">原始图像</param>
        /// <param name="b">拟合到的图像</param>
        public HistoMatch(byte[,] a, byte[,] b)
        {
            _oriImg = a;

            _oriAcc = new HistoData(a).GetAccHistogramData();
            _targetAcc = new HistoData(b).GetAccHistogramData();
            _mappingTable = new int[256];
            
            GMLMatch();
        }

        private void GMLMatch()
        {
            int[,] srcMin = new int[256, 256];
            int lastStartY = 0, lastEndY = 0, startY = 0, endY = 0;

            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    srcMin[j, i] = Math.Abs(_oriAcc[i] - _targetAcc[j]);
                }
            }

            for (int x = 0; x < 256; x++)
            {
                int minValue = srcMin[x, 0];

                for (int y = 0; y < 256; y++)
                {
                    if (minValue > srcMin[x, y]) 
                    {
                        endY = y;
                        minValue = srcMin[x, y];
                    }
                }

                if (startY == lastStartY && endY == lastEndY) continue;

                for (int i = startY; i <= endY; i++)
                {
                    _mappingTable[i] = x; 
                }

                lastStartY = startY;
                lastEndY = endY;
                startY = lastEndY + 1;
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
