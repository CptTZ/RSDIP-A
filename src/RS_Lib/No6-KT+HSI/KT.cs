using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Lib
{
    public class KT
    {

        /// <summary>
        /// 缨帽变换系数
        /// </summary>
        /// <param name="m">0:Landsat4; 1:Landsat5; 2:Landsat7</param>
        /// <returns></returns>
        public static double[,] GetMatrixByType(byte m)
        {
            double[,] matrix = new double[6, 6];
            switch (m)
            {
                case 0:
                    matrix[0, 0] = 0.3037; matrix[0, 1] = 0.2793; matrix[0, 2] = 0.4743; matrix[0, 3] = 0.5585; matrix[0, 4] = 0.5082; matrix[0, 5] = 0.1863;
                    matrix[1, 0] = -0.2848; matrix[1, 1] = -0.2435; matrix[1, 2] = -0.5436; matrix[1, 3] = 0.7243; matrix[1, 4] = 0.0840; matrix[1, 5] = -0.1800;
                    matrix[2, 0] = 0.1509; matrix[2, 1] = 0.1973; matrix[2, 2] = 0.3279; matrix[2, 3] = 0.3406; matrix[2, 4] = -0.7112; matrix[2, 5] = -0.4573;
                    matrix[3, 0] = -0.8242; matrix[3, 1] = -0.0849; matrix[3, 2] = 0.4392; matrix[3, 3] = -0.0580; matrix[3, 4] = 0.2012; matrix[3, 5] = -0.2768;
                    matrix[4, 0] = -0.3280; matrix[4, 1] = -0.0549; matrix[4, 2] = 0.1075; matrix[4, 3] = 0.1855; matrix[4, 4] = -0.4357; matrix[4, 5] = 0.8085;
                    matrix[5, 0] = 0.1084; matrix[5, 1] = -0.9022; matrix[5, 2] = 0.4120; matrix[5, 3] = 0.0573; matrix[5, 4] = -0.0251; matrix[5, 5] = 0.0238;

                    break;

                case 1:
                    matrix[0, 0] = 0.2909; matrix[0, 1] = 0.2493; matrix[0, 2] = 0.4806; matrix[0, 3] = 0.5568; matrix[0, 4] = 0.4438; matrix[0, 5] = 0.1706;
                    matrix[1, 0] = -0.2728; matrix[1, 1] = -0.2174; matrix[1, 2] = -0.5508; matrix[1, 3] = 0.7221; matrix[1, 4] = 0.0733; matrix[1, 5] = -0.1648;
                    matrix[2, 0] = 0.1446; matrix[2, 1] = 0.1761; matrix[2, 2] = 0.3322; matrix[2, 3] = 0.3396; matrix[2, 4] = -0.6210; matrix[2, 5] = -0.4186;
                    matrix[3, 0] = -0.8461; matrix[3, 1] = -0.0731; matrix[3, 2] = 0.4640; matrix[3, 3] = -0.0032; matrix[3, 4] = 0.0492; matrix[3, 5] = -0.0119;
                    for (int i = 4; i < 6; i++)
                    {
                        for (int j = 4; j < 6; j++)
                            matrix[i, j] = 0;
                    }
                    break;

                case 2:
                    matrix[0, 0] = 0.3561; matrix[0, 1] = 0.3972; matrix[0, 2] = 0.304; matrix[0, 3] = 0.6966; matrix[0, 4] = 0.2286; matrix[0, 5] = 0.1596;
                    matrix[1, 0] = -0.3344; matrix[1, 1] = -0.3544; matrix[1, 2] = -0.4556; matrix[1, 3] = 0.6966; matrix[1, 4] = -0.0242; matrix[1, 5] = -0.2630;
                    matrix[2, 0] = 0.2626; matrix[2, 1] = 0.2141; matrix[2, 2] = 0.0926; matrix[2, 3] = 0.0656; matrix[2, 4] = -0.7629; matrix[2, 5] = -0.5388;
                    matrix[3, 0] = -0.0805; matrix[3, 1] = -0.0498; matrix[3, 2] = 0.1950; matrix[3, 3] = -0.1327; matrix[3, 4] = 0.5752; matrix[3, 5] = -0.7775;
                    matrix[4, 0] = -0.7252; matrix[4, 1] = -0.0202; matrix[4, 2] = 0.6683; matrix[4, 3] = 0.0631; matrix[4, 4] = -0.1494; matrix[4, 5] = 0.0274;
                    matrix[5, 0] = 0.4000; matrix[5, 1] = -0.8172; matrix[5, 2] = 0.3832; matrix[5, 3] = 0.0602; matrix[5, 4] = -0.1095; matrix[5, 5] = 0.0985;

                    break;

                default:
                    return null;
            }

            return matrix;
        }

    }
}
