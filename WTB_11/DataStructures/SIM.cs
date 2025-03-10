using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11.DataStructures
{
    class SIM
    {
        public struct SIMStruct
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string apn; // APN

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string apn_user; // Имя пользователя APN

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string apn_pass; // Пароль APN
        }
    }
}
