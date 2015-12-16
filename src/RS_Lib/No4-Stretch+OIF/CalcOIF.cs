using System;
using System.Collections.Generic;

namespace RS_Lib
{
    /// <summary>
    /// OIF计算
    /// </summary>
    public class CalcOIF
    {
        private readonly ImageStats _stats;
        private readonly int _imgBands;

        public List<String[]> ResultOIF { get; private set; }

        public CalcOIF(RsImage img)
        {
            _imgBands = img.BandsCount;
            _stats = new ImageStats(img);
            ResultOIF=new List<string[]>();

            CalcOif();
        }

        private String[] MakeDesc(int i, int j, int k)
        {
            String[] desc = new string[2];

            desc[0] = (i + 1) + ", " + (1 + j) + ", " + (k + 1);
            return desc;
        }

        private void CalcOif()
        {
            double[] stdDev = _stats.StdDev;
            double[,] corr = _stats.Correlation;
            for (int i = 0; i < _imgBands; i++)
            {
                for (int j = 0; j < _imgBands; j++)
                {
                    for (int k = 0; k < _imgBands; k++)
                    {
                        String[] line = MakeDesc(i, j, k);

                        double a = stdDev[i] + stdDev[j] + stdDev[k];
                        double b = corr[i, j] + corr[j, k] + corr[i, k];

                        double result = a/b;
                        line[1] = Math.Round(result, 5).ToString();
                        ResultOIF.Add(line);
                    }
                }
            }
        }
    }
}
