using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11.DataStructures
{
    public class RP
    {
        public struct RPStruct
        {
            public VPBCrane.VPBCraneStruct VPBCrane;
            public VPBCurrType.VPBCurrTypeStruct[] TPCHR; // Массив TPCHR
            public VPBCurrType.VPBCurrTypeStruct[] TP; // Массив TP
        }

    }
}
