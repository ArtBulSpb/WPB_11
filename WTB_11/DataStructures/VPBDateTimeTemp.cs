using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11.DataStructures
{
    public class VPBDateTimeTemp
    {
        public struct VPBDateTimeTempStruct
        {
            public int Hour;
            public int Minute;
            public int Second;
            public int Date;
            public int Month;
            public int Year;
            public byte Temperature_H;
            public byte Temperature_L;

            // Конструктор, который принимает массив байтов
            public VPBDateTimeTempStruct(byte[] data)
            {
                Hour = data[0];
                Minute = data[1];
                Second = data[2];
                Date = data[3];
                Month = data[4];
                Year = BitConverter.ToInt32(data, 5); // Предполагается, что год занимает 4 байта
                Temperature_H = data[9];
                Temperature_L = data[10];
            }
        }

    }
}
