using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11.DataStructures
{
    class MqttServer
    {
        public struct MqttServerStruct
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string host; // Хост

            public ushort port; // Порт
            public ushort connect; // Подключение
        }
    }
}
