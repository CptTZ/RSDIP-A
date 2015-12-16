
namespace RS_Lib
{
    /// <summary>
    /// 2%灰度拉伸
    /// </summary>
    public class Stretch
    {
        private readonly double[] _posibilities;
        private readonly byte[,] _bandData;
        
        public byte[,] StretchedBandData { get; private set; }

        /// <summary>
        /// 拉伸
        /// </summary>
        /// <param name="data">图像</param>
        /// <param name="band">波段：1~n</param>
        public Stretch(byte[,,] data, int band)
        {
            _posibilities = new double[256];
            _bandData = new byte[data.GetLength(1), data.GetLength(2)];

            for (int i = 0; i < data.GetLength(1); i++)
            {
                for (int j = 0; j < data.GetLength(2); j++)
                {
                    _bandData[i, j] = data[band, i, j];
                }
            }

            HistoData hd = new HistoData(_bandData);
            // 累计直方图255处为总像素数
            long totalPixel = hd.GetAccHistogramData()[255];

            for (int i = 0; i < _posibilities.Length; i++)
            {
                // 乘100化为百分比的值
                _posibilities[i] = (100.0) * (hd.GetAccHistogramData()[i]) / (totalPixel * 1.0);
            }

            StretchImg();
        }

        /// <summary>
        /// 开始拉伸,默认2%
        /// </summary>
        private void StretchImg(double left = 2, double right = 98)
        {
            #region 找到2%处
            int a = 0, b = 255;
            int i = 0;
            // 大概2%处
            while (_posibilities[i] < left)
            {
                i++;
            }
            a = i - 1;

            i = 255;
            // 大概98%处
            while (_posibilities[i] > right) 
            {
                i--;
            }
            b = i;
            #endregion

            TwoPercentStretch(a, b);
        }

        /// <summary>
        /// 计算拉伸后图像
        /// </summary>
        private void TwoPercentStretch(int a, int b, int c = 0, int d = 255)
        {
            StretchedBandData = new byte[_bandData.GetLength(0), _bandData.GetLength(1)];

            for (int i = 0; i < _bandData.GetLength(0); i++)
            {
                for (int j = 0; j < _bandData.GetLength(1); j++)
                {
                    double tmp = (1.0*(d - c)*(_bandData[i, j] - a)/(1.0*(b - a)) + c);

                    // 2015.11.26-之前数值处理错误，要注意大于255和小于0的情况
                    if (tmp > 255)
                        StretchedBandData[i, j] = 255;
                    else if (tmp < 0)
                        StretchedBandData[i, j] = 0;
                    else
                        StretchedBandData[i, j] = (byte) (tmp + 0.5);
                }
            }
        }
    }
}
