using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11.DataStructures
{
    class DeviceData
    {
        public struct TimeResponse
        {
            public int Year;
            public int Month;
            public int Day;
            public int Hour;
            public int Minute;
            public int Second;

            public DateTime ToDateTime()
            {
                return new DateTime(Year + 2000, Month, Day, Hour, Minute, Second);
            }
        }

        public struct DeviceStatus
        {
            public bool IsConnected;
            public string Message;
        }
    }
}
