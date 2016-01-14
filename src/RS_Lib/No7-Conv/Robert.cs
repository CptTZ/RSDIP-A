using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    public class Robert
    {

        public byte[,,] RobertData { get; private set; }

        private readonly byte[,,] _oriData;
        private readonly RsImage _img;

        private readonly double[,,] _robA, _robB;


        public Robert(RsImage img)
        {
            this._img = img;
            this._oriData = img.GetPicData();
            this.RobertData = new byte[img.BandsCount, img.Lines, img.Samples];
            this._robA = new double[img.BandsCount, img.Lines, img.Samples];
            this._robB = new double[img.BandsCount, img.Lines, img.Samples];

            ConvForEven();
            FillData();
        }

        private void FillData()
        {
            for (int b = 0; b < _img.BandsCount; b++)
            {
                for (int i = 0; i < _img.Lines; i++)
                {
                    for (int j = 0; j < _img.Samples; j++)
                    {
                        RobertData[b, i, j] = (byte) (Math.Abs(_robA[b, i, j]) + Math.Abs(_robB[b, i, j]) + 0.5);
                    }
                }
            }
        }

        private
            void ConvForEven()
        {
            double[,] xA = {{1, 0}, {0, -1}},
                xB = {{0, 1}, {-1, 0}};

            for (int b = 0; b < _img.BandsCount; b++)
            {
                for (int i = 0; i < _img.Lines; i++)
                {
                    for (int j = 0; j < _img.Samples; j++)
                    {
                        _robA[b, i, j] = CalcNewValue(b, i, j, xA);
                        _robB[b, i, j] = CalcNewValue(b, i, j, xB);
                    }
                }
            }
        }

        private double CalcNewValue(int b,int x, int y, double[,] kern)
        {
            double res = 0;

            for (int i = x; i < x + 2; i++) 
            {
                for (int j = y; j < y + 2; j++)
                {
                    if (isInRange(i, j))
                        res += _oriData[b, i, j]*kern[i - x, j - y];
                }
            }

            return res;
        }

        private bool isInRange(int i, int j)
        {
            return i >= 0 && j >= 0 && i < _oriData.GetLength(1) && j < _oriData.GetLength(2);
        }

    }
}
