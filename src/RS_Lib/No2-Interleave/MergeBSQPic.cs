using System;
using System.Text;

namespace RS_Lib
{
    /// <summary>
    /// BSQ图像合并
    /// </summary>
    public class MergeBSQPic
    {
        private readonly String[] _fileNames;
        private byte[] _finalPic;
        private RsImage[] _mergeImages;
        private int _numOfFiles;
        /// <summary>
        /// 构造一个图像合并类
        /// </summary>
        /// <param name="fn"></param>
        public MergeBSQPic(String[] fn)
        {
            this._fileNames = fn;
            OpenFiles();
        }

        /// <summary>
        /// 构建元数据
        /// </summary>
        /// <returns></returns>
        public String BuildMetaData()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("ENVI");
            sb.AppendLine("header offset = 0");
            sb.AppendLine("file type = ENVI Standard");
            sb.AppendLine("data type = 1");
            sb.AppendLine("interleave = bsq");
            sb.AppendLine("sensor type = Unknown");
            sb.AppendLine("byte order = 0");
            sb.AppendLine("samples = " + _mergeImages[0].Samples);
            sb.AppendLine("lines = " + _mergeImages[0].Lines);
            sb.AppendLine("bands = " + _numOfFiles);
            sb.AppendLine("description = {");
            sb.AppendLine("Merged By TonyZ, Gp.A @ " +
                DateTime.Now.TimeOfDay + ", " + DateTime.Now.DayOfYear + " }");

            return sb.ToString();
        }

        public RsImage[] GetAllPics()
        {
            return _mergeImages;
        }

        public byte[] GetFinalPic()
        {
            return _finalPic;
        }

        public int GetNumOfFiles()
        {
            return _numOfFiles;
        }
        /// <summary>
        /// 合并本类中所有的遥感图像
        /// </summary>
        public void MergePic()
        {
            long len = _numOfFiles * _mergeImages[0].Lines * _mergeImages[0].Samples;
            _finalPic = new byte[len];

            long count = 0;
            for (int z = 0; z < _numOfFiles; z++)
            {
                byte[, ,] tmp = _mergeImages[z].GetPicData();
                for (int i = 0; i < tmp.GetLength(0); i++)
                {
                    for (int j = 0; j < tmp.GetLength(1); j++)
                    {
                        for (int k = 0; k < tmp.GetLength(2); k++)
                        {
                            _finalPic[count] = tmp[i, j, k];
                            count++;
                        }
                    }
                }
            }
        }

        private void OpenFiles()
        {
            _numOfFiles = _fileNames.GetLength(0);
            _mergeImages = new RsImage[_numOfFiles];

            for (int i = 0; i < _numOfFiles; i++)
            {
                _mergeImages[i] = new RsImage(_fileNames[i]);
                if (_mergeImages[i].Lines != _mergeImages[0].Lines |
                    _mergeImages[i].Samples != _mergeImages[0].Samples |
                    _mergeImages[i].Interleave != "bsq")
                {
                    _mergeImages = null;
                    return;
                }
                if (_mergeImages[i].GetPicData() == null)
                {
                    _mergeImages = null;
                    return;
                }
            }
        }
    }
}