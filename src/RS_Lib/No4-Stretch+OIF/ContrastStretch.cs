
namespace RS_Lib
{
    /// <summary>
    /// 对比度拉伸
    /// 网上查的资料不知道对不对
    /// </summary>
    public class ContrastStretch
    {
        private readonly byte[,] _bandData;

        public byte[,] StretchedImg { get; private set; }

        public ContrastStretch(RsImage p, int band, double s, double o)
        {
            _bandData = p.GetPicData(band);

            DoStretch(s, o);
        }

        private void DoStretch(double scale, double offsite)
        {

            StretchedImg = new byte[_bandData.GetLength(0), _bandData.GetLength(1)];

            for (int i = 0; i < _bandData.GetLength(0); i++)
            {
                for (int j = 0; j < _bandData.GetLength(1); j++)
                {
                    int temp = (int) (scale*_bandData[i, j] + offsite + 0.5);

                    if (temp > 255)
                    {
                        StretchedImg[i, j] = 255;
                    }
                    else if (temp < 0)
                    {
                        StretchedImg[i, j] = 0;
                    }
                    else
                    {
                        StretchedImg[i, j] = (byte) temp;
                    }
                }
            }

        }

    }
}
