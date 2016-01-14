using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    public class Grad
    {
        public byte[,,] GradData { get; private set; }
        
        private readonly byte[,,] _oriData;
        private readonly RsImage _img;

        /// <summary>
        /// 梯度倒数加权-3*3
        /// </summary>
        public Grad(RsImage r)
        {
            this._img = r;
            this._oriData = r.GetPicData();

            GradData = new byte[r.BandsCount, r.Lines, r.Samples];
            ScanHoleImg();
        }

        private void ScanHoleImg()
        {
            for (int b = 0; b < _img.BandsCount; b++)
            {
                for (int j = 0; j < _img.Lines; j++)
                {
                    for (int k = 0; k < _img.Samples; k++)
                    {
                        double[,] kern; byte[,] part;

                        MakeKernel(b, j, k, out kern, out part);
                        double sum = 0;

                        for (int xx = 0; xx < 3; xx++)
                        {
                            for (int yy = 0; yy < 3; yy++)
                            {
                                sum += part[xx, yy]*kern[xx, yy];
                            }
                        }

                        if (sum > 245.5)
                        {
                            sum = 245.5;
                        }

                        GradData[b, j, k] = (byte) (sum + 0.5);
                    }
                }
            }
        }
        
        private void MakeKernel(int b, int x, int y, out double[,] kern, out byte[,] res)
        {
            kern = new double[3, 3];
            res = new byte[3, 3];
            double total = 0;

            for (int i = x - 1; i <= x + 1; i++) 
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    byte oriV = isInRange(i, j) ? _oriData[b, i, j] : (byte) 0;
                    res[i + 1 - x, j + 1 - y] = oriV;
                    var tmp = DaoShuFenZhiYi(oriV, _oriData[b, x, y]);
                    kern[i + 1 - x, j + 1 - y] = tmp;
                    total += tmp;
                }
            }
            total *= 2;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    kern[i, j] = kern[i, j] / total;
                }
            }
            kern[1, 1] = 0.5;
        }

        private double DaoShuFenZhiYi(byte a, byte b)
        {
            if (a - b == 0) return 0;
            return 1.0 / Math.Abs(a - b);
        }
        

        private bool isInRange(int i, int j)
        {
            return i >= 0 && j >= 0 && i < _oriData.GetLength(1) && j < _oriData.GetLength(2);
        }
    }
}
