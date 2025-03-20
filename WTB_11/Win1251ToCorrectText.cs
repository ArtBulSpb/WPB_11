using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPB_11.DataStructures;

namespace WPB_11
{
    public class Win1251ToCorrectText
    {
        public string GetNormalText(byte[]byteStr)
        {
            return CodePagesEncodingProvider.Instance.GetEncoding(1251).GetString(byteStr);
        }
    }
}
