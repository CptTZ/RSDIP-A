using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RsNoAMain
{
    public partial class MainWindow
    {

        private void AddNewPic(byte[,,] d, string f)
        {
            this._image.Add(new RS_Lib.RsImage(f, d));
            this._fChoose.AddByMemoryFileName(f);
        }

    }
}
