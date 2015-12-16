using System;

namespace RS_Lib
{
    /// <summary>
    /// 第一次作业-图像交织格式转换
    /// </summary>
    public class PicConvert
    {
        private readonly int _convType;
        private readonly byte[, ,] _data;
        private byte[] _convData = null;

        /// <summary>
        /// 构造图像转换器函数
        /// </summary>
        /// <param name="d">图像数据</param>
        /// <param name="t">转换格式(1-bsq,2-bip,3-bil</param>
        public PicConvert(RsImage d, int t)
        {
            this._data = d.GetPicData();
            this._convType = t;
            this.Convert();
        }

        /// <summary>
        /// 获取转换好的图像
        /// </summary>
        /// <returns>一维byte，供写入文件</returns>
        public byte[] GetConvertedData()
        {
            return _convData;
        }

        /// <summary>
        /// 获取转换目标的交织格式
        /// </summary>
        /// <returns></returns>
        public String GetTargetFormat()
        {
            switch (_convType)
            {
                case 1:
                    return "bsq";
                case 2:
                    return "bip";
                case 3:
                    return "bil";
            }
            return "Unknown";
        }
        private void Convert()
        {
            switch (_convType)
            {
                case 1:
                    toBSQ();
                    break;

                case 2:
                    toBIP();
                    break;

                case 3:
                    toBIL();
                    break;
            }
        }

        //转化为BIL格式
        private void toBIL()
        {
            _convData = new byte[_data.Length];
            long count = 0;
            for (int j = 0; j < _data.GetLength(1); j++)
            {
                for (int i = 0; i < _data.GetLength(0); i++)
                {
                    for (int k = 0; k < _data.GetLength(2); k++)
                    {
                        _convData[count] = _data[i, j, k];
                        count++;
                    }
                }
            }
        }

        //转化为BIP格式
        private void toBIP()
        {
            _convData = new byte[_data.Length];
            
            long count = 0;
            for (int j = 0; j < _data.GetLength(1); j++)
            {
                for (int k = 0; k < _data.GetLength(2); k++)
                {
                    for (int i = 0; i < _data.GetLength(0); i++)
                    {
                        _convData[count] = _data[i, j, k];
                        count++;
                    }
                }
            }
        }

        //转化为BSQ格式
        private void toBSQ()
        {
            _convData = new byte[_data.Length];
            long count = 0;
            for (int i = 0; i < _data.GetLength(0); i++)
            {
                for (int j = 0; j < _data.GetLength(1); j++)
                {
                    for (int k = 0; k < _data.GetLength(2); k++)
                    {
                        _convData[count] = _data[i, j, k];
                        count++;
                    }
                }
            }
        }
    }
}