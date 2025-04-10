using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPB_11.DataStructures.VPBDateTimeTemp;

namespace WPB_11.DataStructures
{
    public class VPBCurrType
    {
        public struct VPBCurrTypeStruct
        {
            public VPBDateTimeTempStruct DTT; // Дата и время
            public ushort CurrForce1; // Сила на датчике 1
            public ushort CurrForce2; // Сила на датчике 2
            public int CurrQ1; // Массив груза 1 
            public int CurrQ2; // Массив груза 2
            public byte CurrPercent1; // Процент нагрузки 1
            public byte CurrPercent2; // Процент нагрузки 2
            public byte CurrWind; // Сила ветра
            public byte SetupModeAndErrors; // Режим настройки и ошибки
            public ushort SummForce1; // Суммарное усилие 1
            public ushort SummForce2; // Суммарное усилие 2
            public string Temperature; // Температура
            public bool SetupMode; // Режим настройки
            public string Errors; // ошибки
            public byte WindForce; // Сила ветра
            public byte[] SensorNumbers; // Номера датчиков
            public byte[] Efforts; // Усилия

            // Конструктор, который принимает массив байтов
            public VPBCurrTypeStruct(byte[] packetData)
            {
                if (packetData.Length < 28)
                {
                    Debug.WriteLine($"VPBCurrTypeStruct конструктор {BitConverter.ToString(packetData)}");
                    //throw new ArgumentException("Недостаточная длина packetData. Ожидалось минимум 28 байт.");
                }

                DTT = new VPBDateTimeTempStruct
                {
                    Hour = BCDToDecimal(packetData[0]),    // Час
                    Minute = BCDToDecimal(packetData[1]),  // Минуты
                    Second = BCDToDecimal(packetData[2]),    // Секунды
                    Date = BCDToDecimal(packetData[3]),   // День
                    Month = BCDToDecimal(packetData[4]),  // Месяц
                    Year = BCDToDecimal(packetData[5]),   // Год  
                };

                CurrForce1 = BitConverter.ToUInt16(new byte[] { packetData[12], packetData[13] }, 0);
                CurrForce2 = BitConverter.ToUInt16(new byte[] { packetData[14], packetData[15] }, 0);
                CurrQ1 = BitConverter.ToInt32(packetData, 16);
                CurrQ2 = BitConverter.ToInt32(packetData, 20);
                //CurrPercent1 = packetData[24];
                //CurrPercent2 = packetData[25];


                // Вычисление суммарного усилия с обработкой переполнения
                if (CurrForce1 != 0 && CurrForce2 != 0)
                {
                    SummForce1 = (ushort)Math.Min((uint)CurrForce1 + (uint)CurrForce2, ushort.MaxValue);
                    SummForce2 = (ushort)Math.Min((uint)CurrForce1 + (uint)CurrForce2, ushort.MaxValue);
                }
                else
                {
                    SummForce1 = 0;
                    SummForce2 = 0;
                }

                // Добавляем режим настройки
                //SetupMode = (packetData[27] & 128) != 0; // Режим настройки

                // Добавляем ошибки
                //byte value = (byte)(packetData[27] & 127);
                Errors = "";

                // Добавляем температуру
                string value1 = packetData[6].ToString();
                float value2 = ((float)(packetData[7] >> 6) * 25);
                Temperature = $"{value1},{value2}";

                // Добавляем силу ветра
                //WindForce = (byte)(packetData[26] / 10); // Сила ветра

            }

            private int BCDToDecimal(byte bcd)
            {
                return (bcd >> 4 & 0x0F) * 10 + (bcd & 0x0F);
            }
        }

    }
}
