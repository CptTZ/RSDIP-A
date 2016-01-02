using System;
using System.IO;
using System.Text;

namespace RS_Lib
{
    /// <summary>
    /// 通用-遥感图像类
    /// </summary>
    public partial class RsImage
    {
        #region 属性和字段
        /// <summary>
        /// 波段数
        /// </summary>
        public int BandsCount { get; private set; }
        /// <summary>
        /// 数据类型（暂不用）
        /// </summary>
        public int DataType { get; private set; }
        /// <summary>
        /// 数据描述
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// 数据交织类型(BSQ/BIP/BIL)
        /// </summary>
        public string Interleave { get; private set; }
        /// <summary>
        /// 行数
        /// </summary>
        public int Lines { get; private set; } 
        /// <summary>
        /// 列数
        /// </summary>
        public int Samples { get; private set; }
        public int XStart { get; private set; }
        public int YStart { get; private set; }

        public string FileName { get; private set; }
        
        private readonly string _dataFilePath;
        private readonly string _headerFilePath;
        private byte[,,] _picData;
        #endregion

        #region Getter模块
        public string GetFilePath()
        {
            return _dataFilePath;
        }

        /// <summary>
        /// 遥感图像整体数据
        /// </summary>
        /// <returns></returns>
        public byte[, ,] GetPicData()
        {
            byte[,,] res = new byte[_picData.GetLength(0), _picData.GetLength(1), _picData.GetLength(2)];

            for (int i = 0; i < _picData.GetLength(0); i++)
            {
                for (int j = 0; j < _picData.GetLength(1); j++)
                {
                    for (int k = 0; k < _picData.GetLength(2); k++)
                    {
                        res[i, j, k] = _picData[i, j, k];
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// 遥感图像特定波段数据(1~n)
        /// </summary>
        /// <param name="band">波段号</param>
        /// <returns></returns>
        public byte[,] GetPicData(int band)
        {
            if (band-1 > this.BandsCount) return null;

            byte[,] data = new byte[Lines, Samples];
            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Samples; j++)
                {
                    data[i, j] = _picData[band-1, i, j];
                }
            }

            return data;
        }
        
        #endregion Getter模块

        /// <summary>
        /// 遥感图像构造函数
        /// </summary>
        /// <param name="filePath">遥感HDR文件头的地址</param>
        public RsImage(String filePath)
        {
            FileInfo fi = new FileInfo(filePath);

            this.FileName = fi.Name;
            this._headerFilePath = filePath;
            this._dataFilePath = filePath.Substring(0, filePath.LastIndexOf('.'));

            this.ReadMetaData();
            this.ReadPic();
        }

        /// <summary>
        /// 创建元信息内容，允许指定交织方法，为空表示不修改
        /// </summary>
        /// <param name="interleave"></param>
        /// <returns></returns>
        public String BuildMetaData(String interleave)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ENVI");
            // 描述块
            sb.AppendLine("description = {");
            sb.AppendLine(this.Description +
                          " Prog By Gp.A @ " + DateTime.Now.TimeOfDay +
                          ", " + DateTime.Now.DayOfYear + "}");
            // 七大块
            sb.AppendLine("samples = " + this.Samples);
            sb.AppendLine("lines = " + this.Lines);
            sb.AppendLine("bands = " + this.BandsCount);
            sb.AppendLine("data type = " + this.DataType);
            if (String.IsNullOrWhiteSpace(interleave))
            {
                sb.AppendLine("interleave = " + this.Interleave);
            }
            else
            {
                sb.AppendLine("interleave = " + interleave);
            }
            sb.AppendLine("x start = " + this.XStart);
            sb.AppendLine("y start = " + this.YStart);
            // 不知道什么块
            sb.AppendLine("header offset = 0");
            sb.AppendLine("file type = ENVI Standard");
            sb.AppendLine("sensor type = Unknown");
            sb.AppendLine("byte order = 0");

            return sb.ToString();
        }

        /// <summary>
        /// 获取=后的内容
        /// </summary>
        /// <param name="rawStr"></param>
        /// <returns></returns>
        private String getDataAfterEqual(String rawStr)
        {
            return rawStr.Split('=')[1].Trim();
        }

        #region 三大格式读取
        private void readBIL(BinaryReader rsFileBinData)
        {
            for (int j = 0; j < Lines; j++)
            {
                for (int i = 0; i < BandsCount; i++)
                {
                    for (int k = 0; k < Samples; k++)
                    {
                        _picData[i, j, k] = rsFileBinData.ReadByte();
                    }
                }
            }
        }

        private void readBIP(BinaryReader rsFileBinData)
        {
            for (int j = 0; j < Lines; j++)
            {
                for (int k = 0; k < Samples; k++)
                {
                    for (int i = 0; i < BandsCount; i++)
                    {
                        _picData[i, j, k] = rsFileBinData.ReadByte();
                    }
                }
            }
        }

        private void readBSQ(BinaryReader rsFileBinData)
        {
            for (int i = 0; i < BandsCount; i++)
            {
                for (int j = 0; j < Lines; j++)
                {
                    for (int k = 0; k < Samples; k++)
                    {
                        _picData[i, j, k] = rsFileBinData.ReadByte();
                    }
                }
            }
        }
        #endregion

        private void ReadMetaData()
        {
            StreamReader headReader = null;

            try
            {
                headReader = new StreamReader(this._headerFilePath);
                String metaLineData = null;
                while ((metaLineData = headReader.ReadLine()) != null)
                {
                    if (metaLineData.Contains("samples"))
                    {
                        this.Samples = int.Parse(getDataAfterEqual(metaLineData));
                    }
                    else if (metaLineData.Contains("lines"))
                    {
                        this.Lines = int.Parse(getDataAfterEqual(metaLineData));
                    }
                    else if (metaLineData.Contains("bands"))
                    {
                        this.BandsCount = int.Parse(getDataAfterEqual(metaLineData));
                    }
                    else if (metaLineData.Contains("x start"))
                    {
                        this.XStart = int.Parse(getDataAfterEqual(metaLineData));
                    }
                    else if (metaLineData.Contains("y start"))
                    {
                        this.YStart = int.Parse(getDataAfterEqual(metaLineData));
                    }
                    else if (metaLineData.Contains("data type"))
                    {
                        this.DataType = int.Parse(getDataAfterEqual(metaLineData));
                    }
                    else if (metaLineData.Contains("interleave"))
                    {
                        this.Interleave = getDataAfterEqual(metaLineData);
                    }
                    else if (metaLineData.Contains("description"))
                    {
                        StringBuilder sb = new StringBuilder(getDataAfterEqual(metaLineData).Replace("{", ""));
                        String tmp;
                        while ((tmp = headReader.ReadLine()).Contains("}") == false)
                        {
                            sb.Append(tmp);
                        }
                        sb.Append(tmp.Trim().Replace("}", ""));
                        this.Description = sb.ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                headReader?.Close();
            }
        }

        /// <summary>
        /// 读取二进制图片信息
        /// </summary>
        private void ReadPic()
        {
            BinaryReader rsFileBinData = null;
            try
            {
                rsFileBinData = new BinaryReader(new FileStream(
                    this._dataFilePath, FileMode.Open, FileAccess.Read));
                rsFileBinData.BaseStream.Position = 0;
                this._picData = new byte[BandsCount, Lines, Samples];

                switch (Interleave)
                {
                    case "bil":
                        readBIL(rsFileBinData);
                        break;

                    case "bip":
                        readBIP(rsFileBinData);
                        break;

                    case "bsq":
                        readBSQ(rsFileBinData);
                        break;

                    default:
                        return;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                rsFileBinData?.Close();
            }
        }

    }
}