using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WPB_11.DataStructures;

namespace WPB_11.Device
{
    public class DevicePackets
    {

        public event Action<VPBCurrType.VPBCurrTypeStruct> DateTimeProcessed;
        public event Action<VPBCurrType.VPBCurrTypeStruct> VPBCurrProcessed;


        public void ProcessDateTimePacket(byte[] packetData)
        {
            Debug.WriteLine($"ProcessDateTimePacket вызван {BitConverter.ToString(packetData)}");
            // Создание структуры для текущих значений
            VPBCurrType.VPBCurrTypeStruct sensorData = new VPBCurrType.VPBCurrTypeStruct
            {
                DTT = new VPBDateTimeTemp.VPBDateTimeTempStruct
                {
                    Hour = BCDToDecimal(packetData[4]),  // Час
                    Minute = BCDToDecimal(packetData[5]),  // Минуты
                    Second = BCDToDecimal(packetData[6]),    // Секунды
                    Date = BCDToDecimal(packetData[7]),  // День
                    Month = BCDToDecimal(packetData[8]),  // Месяц
                    Year = BCDToDecimal(packetData[9]),  // Год  
                },
                CurrForce1 = BitConverter.ToUInt16(new byte[] { packetData[12], packetData[13] }, 0),
                CurrForce2 = BitConverter.ToUInt16(new byte[] { packetData[14], packetData[15] }, 0),
                CurrQ1 = BitConverter.ToInt32(packetData, 16),
                CurrQ2 = BitConverter.ToInt32(packetData, 20),
                CurrPercent1 = packetData[24],
                CurrPercent2 = packetData[25],
                
            };
            // Вычисление суммарного усилия с обработкой переполнения
            if (sensorData.CurrForce1 != 0 && sensorData.CurrForce1 != 0)
            {
                Debug.WriteLine($"IF вызван {BitConverter.ToString(packetData)}");
                sensorData.SummForce1 = (ushort)Math.Min((uint)sensorData.CurrForce1 + (uint)sensorData.CurrForce2, ushort.MaxValue);
                sensorData.SummForce2 = (ushort)Math.Min((uint)sensorData.CurrForce1 + (uint)sensorData.CurrForce2, ushort.MaxValue);
            }
            // Добавляем режим настройки
            sensorData.SetupMode = (packetData[27] & 128) != 0; // Режим настройки

            // Добавляем ошибки
            byte value = (byte)(packetData[27] & 127);
            sensorData.Errors = value.ToString();

            // Добавляем температуру
            string value1 = packetData[10].ToString(); 
            float value2 = ((float)(packetData[11] >> 6) * 25);
            sensorData.Temperature = $"{value1},{value2}"; 

            // Добавляем силу ветра
            sensorData.WindForce = (byte)(packetData[26] / 10); // Сила ветра

            // Датчики
            byte[] sensorNumbers = new byte[4];
            byte[] efforts = new byte[4];

            // Получение номеров датчиков
            for (int i = 0; i < 4; i++)
            {
                sensorNumbers[i] = (byte)packetData[17 + i]; // номера датчиков
            }

            // Получение усилия
            for (int i = 0; i < 4; i++)
            {
                efforts[i] = (byte)packetData[21 + i]; // усилие
            }

            DateTimeProcessed?.Invoke(sensorData); // Отображение даты и времени
        }


        private DateTime VPBDateTimeToDateTime(int year, int month, int date, int hour, int minute, int second)
        {
            int fullYear = 2000 + year; // Предполагаем, что год начинается с 2000

            // Проверка на допустимость значений
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(month), $"Месяц должен быть в диапазоне от 1 до 12. {year} {month} {date}");
            }

            // Проверка на количество дней в месяце
            int daysInMonth = DateTime.DaysInMonth(fullYear, month);
            if (date < 1 || date > daysInMonth)
            {
                throw new ArgumentOutOfRangeException(nameof(date), $"День должен быть в диапазоне от 1 до {daysInMonth} для месяца {month}.");
            }

            // Проверка на допустимость времени
            if (hour < 0 || hour > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(hour), "Час должен быть в диапазоне от 0 до 23.");
            }
            if (minute < 0 || minute > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(minute), "Минуты должны быть в диапазоне от 0 до 59.");
            }
            if (second < 0 || second > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(second), "Секунды должны быть в диапазоне от 0 до 59.");
            }

            //OnDeviceConnected?.Invoke($"Полученные данные: Год={fullYear}, Месяц={month}, День={date}, Час={hour}, Минуты={minute}, Секунды={second}");
            return new DateTime(fullYear, month, date, hour, minute, second);
        }


        private int BCDToDecimal(byte bcd)
        {
            return (bcd >> 4 & 0x0F) * 10 + (bcd & 0x0F);
        }

    }
}
