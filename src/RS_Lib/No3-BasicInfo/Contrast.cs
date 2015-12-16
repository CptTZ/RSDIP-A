
namespace RS_Lib
{
    /// <summary>
    /// 图像对比度
    /// </summary>
    public class Contrast
    {
        private readonly byte[,] _picData;
        private readonly int _hang, _lie;
        private double _contrast;

        /// <summary>
        /// 构造对比度计算
        /// </summary>
        /// <param name="p">二维图像</param>
        public Contrast(byte[,] p)
        {
            _hang = p.GetLength(0);
            _lie = p.GetLength(1);
            _picData = p;
            CalcContrast();
        }

        private void CalcContrast()
        {
            int tmpA = 0;
            int count = 0;

            for (int i = 0; i < _hang; i++)
            {
                for (int j = 0; j < _lie; j++)
                {
                    if (CheckRange(i - 1, j))
                    {
                        count++;
                        tmpA += (_picData[i, j] - _picData[i - 1, j]) * 
                            (_picData[i, j] - _picData[i - 1, j]);
                    }
                    if (CheckRange(i + 1, j))
                    {
                        count++;
                        tmpA += (_picData[i, j] - _picData[i + 1, j]) * 
                            (_picData[i, j] - _picData[i + 1, j]);
                    }
                    if (CheckRange(i, j - 1))
                    {
                        count++;
                        tmpA += (_picData[i, j] - _picData[i, j - 1]) * 
                            (_picData[i, j] - _picData[i, j - 1]);
                    }
                    if (CheckRange(i, j + 1))
                    {
                        count++;
                        tmpA += (_picData[i, j] - _picData[i, j + 1]) * 
                            (_picData[i, j] - _picData[i, j + 1]);
                    }
                    
                }
            }

            _contrast = (tmpA*1.0)/count;
        }

        /// <summary>
        /// 数据在不在图像内
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>在不在</returns>
        private bool CheckRange(int x, int y)
        {
            return (x >= 0) && (x < _hang) && (y >= 0 && y < _lie);
        }

        /// <summary>
        /// 返回某波段图像对比度
        /// </summary>
        /// <returns>Double</returns>
        public double GetImageContrast()
        {
            return _contrast;
        }

    }
}
