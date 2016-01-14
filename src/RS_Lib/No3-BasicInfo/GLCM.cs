
namespace RS_Lib
{
    /// <summary>
    /// 灰度共生矩阵
    /// </summary>
    public class GLCM
    {
        public byte[,] GLCMdata { get; private set; }

        private readonly int _dx, _dy, _hang, _lie;
        private double _max, _min;
        private readonly byte[,] _imgData;
        // 8位量化:0-255
        private readonly double[,] _glcm = new double[256, 256];

        /// <summary>
        /// 灰度共生矩阵实例化
        /// </summary>
        /// <param name="r">某波段</param>
        /// <param name="dx">偏移x</param>
        /// <param name="dy">偏移y</param>
        public GLCM(byte[,] r, int dx, int dy)
        {
            _max = double.MinValue;
            _min = double.MaxValue;
            _imgData = r;
            _hang = r.GetLength(0);
            _lie = r.GetLength(1);
            _dx = dx;
            _dy = dy;
            GLCMdata = new byte[256, 256];

            CalcGLCM();
            Process();
        }

        private void CalcGLCM()
        {
            int count = 0;
            int[,] tmp = new int[256, 256];

            for (int x = 0; x < _hang; x++)
            {
                for (int y = 0; y < _lie; y++)
                {
                    if (!CheckBoundary(x, y)) continue;
                    count++;
                    tmp[_imgData[x, y], _imgData[x + _dx, y + _dy]]++;
                }
            }

            // 均一化
            for (int i = 0; i < tmp.GetLength(0); i++)
            {
                for (int j = 0; j < tmp.GetLength(1); j++)
                {
                    var r = tmp[i, j] * 255.0 / count;
                    _glcm[i, j] = r;

                    if (r < _min) _min = r;
                    if (r > _max) _max = r;
                }
            }

        }

        /// <summary>
        /// 检查是不是出界了
        /// </summary>
        /// <param name="x">原始x</param>
        /// <param name="y">原始y</param>
        /// <returns>好的回复True</returns>
        private bool CheckBoundary(int x, int y)
        {
            return (x + _dx < _hang) && (y + _dy < _lie);
        }

        /// <summary>
        /// 线性拉伸结果
        /// </summary>
        private void Process()
        {
            double s = 255.0 / (_max - _min);

            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    GLCMdata[i, j] = LinearSt(s, _glcm[i, j]);
                }
            }
        }

        private byte LinearSt(double k, double v)
        {
            int d = (int)((v - _min) * k + 0.5);

            if (d < 0)
                return 0;
            else if (d > 255)
                return 255;
            else
                return (byte)d;
        }

    }
}
