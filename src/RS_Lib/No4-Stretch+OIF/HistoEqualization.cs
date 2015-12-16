
namespace RS_Lib
{
    /// <summary>
    /// 快速直方图均衡化
    /// 仅用于8位量化
    /// </summary>
    public class HistoEqualization
    {
        private readonly byte[,] _bandData;
        private readonly int[] _accData;
        private int[] _mappingTable;

        public byte[,] EqualedData { get; private set; }

        public HistoEqualization(RsImage img, int band)
        {
            _bandData = img.GetPicData(band);
            HistoData hd = new HistoData(img, band);
            _accData = hd.GetAccHistogramData();

            StartEqualization();
        }

        /// <summary>
        /// 创建映射关系
        /// </summary>
        private void EstablishMapping()
        {
            _mappingTable = new int[_accData.Length];
            // 累计直方图255时为总计像素
            int totalPix = _accData[255];

            for (int i = 0; i < 256; i++)
            {
                _mappingTable[i] = (int) (0.5 + 255.0*_accData[i]/totalPix);
            }
        }

        private void StartEqualization()
        {
            EstablishMapping();

            EqualedData = new byte[_bandData.GetLength(0), _bandData.GetLength(1)];

            for (int i = 0; i < _bandData.GetLength(0); i++)
            {
                for (int j = 0; j < _bandData.GetLength(1); j++)
                {
                    var tmp = _mappingTable[_bandData[i, j]];
                    if (tmp < 0)
                    {
                        EqualedData[i, j] = 0;
                    }
                    else if (tmp > 255)
                    {
                        EqualedData[i, j] = 255;
                    }
                    else
                    {
                        EqualedData[i, j] = (byte) tmp;
                    }
                }
            }
        }
    }
}
