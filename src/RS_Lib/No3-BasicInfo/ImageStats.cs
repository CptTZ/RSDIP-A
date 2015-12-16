using System;

namespace RS_Lib
{
    /// <summary>
    /// 图像统计量
    /// </summary>
    public class ImageStats
    {
        private readonly int _bands;
        private readonly byte[, ,] _picData;
        private readonly int _lines;
        private readonly int _samples;

        /// <summary>
        /// 均值
        /// </summary>
        public double[] Mean { get; private set; }

        /// <summary>
        /// 标准差
        /// </summary>
        public double[] StdDev { get; private set; }

        /// <summary>
        /// 协方差
        /// </summary>
        public double[,] Covariance { get; private set; }

        /// <summary>
        /// 相关系数
        /// </summary>
        public double[,] Correlation { get; private set; }

        public ImageStats(RsImage p)
        {
            _bands = p.BandsCount;
            _lines = p.Lines;
            _samples = p.Samples;
            _picData = p.GetPicData();

            CalcStats();
        }

        /// <summary>
        /// 平均值
        /// </summary>
        private void CalcMean()
        {
            for (int i = 0; i < _bands; i++)
            {
                long tmpSum = 0;

                for (int j = 0; j < _lines; j++)
                {
                    for (int k = 0; k < _samples; k++)
                    {
                        tmpSum += _picData[i, j, k];
                    }
                }
                Mean[i] = tmpSum * 1.0 / (_lines * _samples);
            }
        }

        /// <summary>
        /// 标准差
        /// </summary>
        private void CalcStdDev()
        {
            for (int i = 0; i < _bands; i++)
            {
                double tmp = 0;

                for (int j = 0; j < _lines; j++)
                {
                    for (int k = 0; k < _samples; k++)
                    {
                        tmp += Math.Pow(_picData[i, j, k] - Mean[i], 2.0);
                    }
                }
                StdDev[i] = Math.Sqrt(tmp / (_lines * _samples));
            }
        }

        /// <summary>
        /// 协方差&相关系数矩阵
        /// </summary>
        private void CalcCovCor()
        {
            for (int p = 0; p < _bands; p++)
            {
                for (int q = 0; q < _bands; q++)
                {
                    double tmp = 0.0;
                    for (int i = 0; i < _lines; i++)
                    {
                        for (int j = 0; j < _samples; j++)
                        {
                            tmp += (_picData[p, i, j] - Mean[p]) *
                                         (_picData[q, i, j] - Mean[q]);
                        }
                    }
                    Covariance[p, q] = tmp / (_lines * _samples);
                    Correlation[p, q] = Covariance[p, q] / (StdDev[p] * StdDev[q]);
                }
            }
        }

        /// <summary>
        /// 计算统计参数-时间较长
        /// </summary>
        private void CalcStats()
        {
            Mean = new double[_bands];
            StdDev = new double[_bands];
            Correlation = new double[_bands, _bands];
            Covariance = new double[_bands, _bands];

            CalcMean();
            CalcStdDev();
            CalcCovCor();
        }

    }
}
