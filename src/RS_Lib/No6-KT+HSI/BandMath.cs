using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Expression;

namespace RS_Lib
{
    public class BandMath
    {

        public byte[,,] CalculatedResult { get; private set; }

        private readonly double[,,] _result;
        private Formula _fm = new Formula();
        private readonly string _oriStatement;
        private readonly string[] _mark;
        private readonly RsImage[] _img;

        /// <summary>
        /// BandMath类
        /// </summary>
        /// <param name="st"></param>
        /// <param name="data"></param>
        public BandMath(string st, Dictionary<string, RsImage> data)
        {
            this._oriStatement = st;
            this._mark = data.Keys.ToArray();
            this._img = data.Values.ToArray();
        }


    }
}
