using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPB_11.DataStructures;

namespace WPB_11.Device
{
    public class DevicePackets
    {

        public event Action<string> DateTimeProcessed;
        public event Action<VPBCurrType.VPBCurrTypeStruct> VPBCurrProcessed;


        public void ProcessDateTimePacket(byte[] packetData)
        {
            Debug.WriteLine("ProcessDateTimePacket вызван");
            // Извлекаем дату и время, преобразуя BCD в десятичные значения
            DateTime VPBDateTime = VPBDateTimeToDateTime(
                BCDToDecimal(packetData[9]),  // Год
                BCDToDecimal(packetData[8]),  // Месяц
                BCDToDecimal(packetData[7]),  // День
                BCDToDecimal(packetData[4]),  // Час
                BCDToDecimal(packetData[5]),  // Минуты
                BCDToDecimal(packetData[6])    // Секунды
            );
            DateTimeProcessed?.Invoke($"Дата и время: {VPBDateTime}"); // Отображение даты и времени
        }


        public void ProcessVPBCurr(byte[] packetData)
        {
            Debug.WriteLine($"Обработка VPBCurr: {BitConverter.ToString(packetData)}"); // Отладочное сообщение

            // Проверяем CRC
            byte crc = 0;
            for (int i = 0; i < (packetData[2] * 256 + packetData[3] + 3); i++)
            {
                crc ^= packetData[i]; // CRC
            }

            if (crc != packetData[packetData[2] * 256 + packetData[3] + 4])
            {
                Console.WriteLine($"Ошибка CRC. Посчитал: {crc:X2}. Получил: {packetData[packetData[2] * 256 + packetData[3] + 4]:X2}");
                return; // Ошибка CRC
            }

            // Создаём экземпляр структуры
            VPBCurrType.VPBCurrTypeStruct sensorData = new VPBCurrType.VPBCurrTypeStruct
            {
                DTT = new VPBDateTimeTemp.VPBDateTimeTempStruct
                {
                    Hour = packetData[5],
                    Minute = packetData[6],
                    Second = packetData[7],
                    Date = packetData[8],
                    Month = packetData[9],
                    Year = packetData[10]
                },
                CurrForce1 = BitConverter.ToUInt16(new byte[] { packetData[13], packetData[14] }, 0),
                CurrForce2 = BitConverter.ToUInt16(new byte[] { packetData[15], packetData[16] }, 0),
                CurrQ1 = BitConverter.ToInt32(packetData, 17),
                CurrQ2 = BitConverter.ToInt32(packetData, 21),
                CurrPercent1 = packetData[25],
                CurrPercent2 = packetData[26]
            };

            Debug.WriteLine($"Данные датчиков: {sensorData}"); // Отладочное сообщение
            VPBCurrProcessed?.Invoke(sensorData);
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

        private string ConvertBCDToDecimalString(byte[] bcdData)
        {
            StringBuilder decimalValue = new StringBuilder();

            for (int i = 0; i < bcdData.Length; i++)
            {
                // Преобразуем каждый байт BCD в десятичное значение
                decimalValue.Append(BCDToDecimal(bcdData[i]));

                // Добавляем разделитель, если это не последний элемент
                if (i < bcdData.Length - 1)
                {
                    decimalValue.Append(", "); // Используйте ", " или любой другой разделитель по вашему выбору
                }
            }

            return decimalValue.ToString();
        }

    }
}
