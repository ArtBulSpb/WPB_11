using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11.DataStructures
{
    class MqttClient
    {
        public struct MqttClientStruct
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string username; // Имя пользователя

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string pass; // Пароль

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string clientID; // ID клиента

            public ushort keepAliveInterval; // Интервал поддержания соединения
        }
    }
}
