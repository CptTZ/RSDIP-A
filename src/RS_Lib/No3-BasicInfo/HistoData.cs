
namespace RS_Lib
{
    /// <summary>
    /// 直方图相关
    /// </summary>
    public class HistoData
    {
        private readonly byte[] _picData;
        private int[] _histoDat;
        private int[] _accHistoDat;

        /// <summary>
        /// 构造直方图数据
        /// </summary>
        public HistoData(RsImage img, int band)
        {
            byte[,] p = img.GetPicData(band);
            int hang = p.GetLength(0);
            int lie = p.GetLength(1);
            _picData = new byte[hang * lie];

            int count = 0;
            for (int i = 0; i < hang; i++)
            {
                for (int j = 0; j < lie; j++)
                {
                    _picData[count++] = p[i, j];
                }
            }

            CalcHistoData();
        }

        /// <summary>
        /// 构造直方图数据
        /// </summary>
        /// <param name="p">二维图像</param>
        public HistoData(byte[,] p)
        {
            int hang = p.GetLength(0);
            int lie = p.GetLength(1);
            _picData = new byte[hang * lie];

            int count = 0;
            for (int i = 0; i < hang; i++)
            {
                for (int j = 0; j < lie; j++)
                {
                    _picData[count++] = p[i, j];
                }
            }

            CalcHistoData();
        }

        private void CalcHistoData()
        {
            _histoDat=new int[256];
            _accHistoDat=new int[256];

            foreach (byte t in _picData)
            {
                _histoDat[t]++;
            }

            // 对齐下
            _accHistoDat[0] = _histoDat[0];
            for (int i = 1; i < _histoDat.Length; i++)
            {
                _accHistoDat[i] = _accHistoDat[i - 1] + _histoDat[i];
            }

        }

        /// <summary>
        /// 联合直方图-静态类
        /// </summary>
        /// <param name="i1">图像1</param>
        /// <param name="i2">图像2</param>
        /// <returns>X,Y</returns>
        public static int[,] GetUniHistrogramData(byte[,] i1, byte[,] i2)
        {
            // 两图像行列数必须一样
            if (i1.GetLength(0) != i2.GetLength(0) || i1.GetLength(1) != i2.GetLength(1))
            {
                return null;
            }

            // 像素值: 8位量化为0-255
            int[,] uniHisto = new int[256, 256];

            for (int i = 0; i < i2.GetLength(0); i++)
            {
                for (int j = 0; j < i1.GetLength(1); j++)
                {
                    uniHisto[i1[i, j], i2[i, j]]++;
                }
            }

            // TO-DO: 缩放范围到0-255?
            return uniHisto;
        }

        /// <summary>
        /// 累积直方图
        /// </summary>
        /// <returns>0-255</returns>
        public int[] GetAccHistogramData()
        {
            return _accHistoDat;
        }

        /// <summary>
        /// 直方图
        /// </summary>
        /// <returns>0-255</returns>
        public int[] GetHistogramData()
        {
            return _histoDat;
        }
    }
}
