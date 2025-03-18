using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11.DataStructures
{
    public class VPBSensors
    {
        public struct VPBSensorsStruct
        {
            public byte Query; // Запрос
            public byte SensorType; // Тип датчика
            public bool SensorAnswer; // Ответ датчика
            public byte Data0Low;
            public byte Data0High;
            public byte Data1Low;
            public byte Data1High;
        }

    }
}
