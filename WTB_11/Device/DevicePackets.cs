﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WPB_11.DataStructures;
using static System.Net.Mime.MediaTypeNames;
using static WPB_11.DataStructures.RP;
using static WPB_11.DataStructures.VPBCrane;

namespace WPB_11.Device
{
    public class DevicePackets
    {
        private static DevicePackets _instance;
        public event Action<VPBCurrType.VPBCurrTypeStruct> DateTimeProcessed;
        public event Action<VPBCrane.VPBCraneStruct> VPBCraneProcessed;

        public static DevicePackets Instance()
        {
            if (_instance == null)
            {
                _instance = new DevicePackets();
            }
            return _instance;
        }

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


        public void ProcessVPBCrane(byte[] packetData)
        {
            Debug.WriteLine($"Начало обработки данных VPBCrane {BitConverter.ToString(packetData)}");

            // Краны
            RP.RPStruct rp = new RP.RPStruct();
            rp.VPBCrane = new VPBCrane.VPBCraneStruct();
            rp.VPBCrane.Crane = new byte[11];
            rp.VPBCrane.VPBNumber = new char[11];
            rp.VPBCrane.CraneNumber = new char[11];
            rp.VPBCrane.SetupDate = new byte[3];
            rp.VPBCrane.Sensors = new VPBSensors.VPBSensorsStruct[8];
            rp.VPBCrane.Cycles1 = new uint[15];
            rp.VPBCrane.Cycles2 = new uint[15];
            var CharacteristicByte1 = new byte[15];
            var CharacteristicByte2 = new byte[15];
            var SumQByte1 = new byte[15];
            var SumQByte2 = new byte[15];

            // Минимальный идентификатор
            for (int i = 0; i < 11; i++)
            {
                rp.VPBCrane.VPBNumber[i] = (char)packetData[i + 16];
            }

            // Номер крана
            for (int i = 0; i < 11; i++)
            {
                rp.VPBCrane.CraneNumber[i] = (char)packetData[i + 26];
            }

            // Дата настройки
            for (int i = 0; i < 3; i++)
            {
                rp.VPBCrane.SetupDate[i] = packetData[38 + i];
            }

            // Версия программы
            rp.VPBCrane.ProgramVersion = packetData[41];

            // Группа нагрузки
            rp.VPBCrane.LoadGroup = packetData[42];

            // Максимальная скорость
            rp.VPBCrane.MaxV = packetData[42];

            // Интегральная скорость
            rp.VPBCrane.IntegralV = packetData[43];

            // TPCHRPoint
            try
            {
                rp.VPBCrane.TpchrPoint = (uint)(packetData[45] +
                                                 packetData[46] * 256 +
                                                 packetData[47] * 65536 +
                                                 packetData[48] * 16777216);
            }
            catch { }

            for (int j = 0; j < 15; j++)
            {
                // Каждые 4 байта представляют одно значение LongWord
                int startIndex1 = 48 + j * 4; // Индексы для Cycles1
                int startIndex2 = 108 + j * 4; // Индексы для Cycles2

                // Преобразуем 4 байта в uint для Cycles1
                rp.VPBCrane.Cycles1[j] = BitConverter.ToUInt32(packetData, startIndex1);

                // Преобразуем 4 байта в uint для Cycles2
                rp.VPBCrane.Cycles2[j] = BitConverter.ToUInt32(packetData, startIndex2);
            }

            // Суммируем циклы
            rp.VPBCrane.SummCycles1 = 0;
            for (int j = 0; j < rp.VPBCrane.Cycles1.Length; j++)
            {
                rp.VPBCrane.SummCycles1 += rp.VPBCrane.Cycles1[j];
            }

            rp.VPBCrane.SummCycles2 = 0;
            for (int j = 0; j < rp.VPBCrane.Cycles2.Length; j++)
            {
                rp.VPBCrane.SummCycles2 += rp.VPBCrane.Cycles2[j];
            }

            // Характеристические числа
            for (int i = 0; i < 4; i++)
            {
                CharacteristicByte1[i] = (byte)packetData[168 + i];
                CharacteristicByte2[i] = (byte)packetData[172 + i];
            }
            rp.VPBCrane.CharacteristicNumber1 = BitConverter.ToSingle(CharacteristicByte1, 0);
            rp.VPBCrane.CharacteristicNumber2 = BitConverter.ToSingle(CharacteristicByte2, 0);


            // SummQ1
            for (int i = 0; i < 4; i++)
            {
                SumQByte1[i] = (byte)packetData[176 + i];
                SumQByte2[i] = (byte)packetData[180 + i];
            }
            rp.VPBCrane.SummQ1 = BitConverter.ToUInt32(SumQByte1);
            rp.VPBCrane.SummQ2 = BitConverter.ToUInt32(SumQByte2);

            // Время работы
            //rp.VPBCrane.OperatingTime = BitConverter.ToUInt32(packetData, 185);
            byte[] temp4bytes = new byte[4]; // Создаем массив для 4 байтов
            for (int i = 0; i < 4; i++)
            {
                temp4bytes[i] = BCDToByte(packetData[185 + i]); // Копируем данные из packetData
            }
            rp.VPBCrane.OperatingTime = BitConverter.ToUInt32(temp4bytes, 0); // Преобразуем байты в LongWord

            // Qmax1
            rp.VPBCrane.MaxQ1 = BitConverter.ToUInt32(packetData, 188);

            // Qmax2
            rp.VPBCrane.MaxQ2 = BitConverter.ToUInt32(packetData, 192);

            // Coeff1
            rp.VPBCrane.CoeffQ1 = (short)((packetData[197] << 8) + packetData[196]);

            // Additiv1
            rp.VPBCrane.AdditivQ1 = (short)((packetData[199] << 8) + packetData[198]);

            // Coeff2
            rp.VPBCrane.CoeffQ2 = (short)((packetData[201] << 8) + packetData[200]);
            // Additiv2
            rp.VPBCrane.AdditivQ2 = (short)((packetData[203] << 8) + packetData[202]);

            // Интегральная характеристика 1
            rp.VPBCrane.Integral1 = packetData[204];

            // Интегральная характеристика 2
            rp.VPBCrane.Integral2 = packetData[205];

            // Датчики
            for (int i = 0; i < 8; i++)
            {
                int baseIndex = 206 + i * 7;
                rp.VPBCrane.Sensors[i].Query = packetData[baseIndex];
                rp.VPBCrane.Sensors[i].SensorType = packetData[baseIndex + 1];
                rp.VPBCrane.Sensors[i].SensorAnswer = Convert.ToBoolean(packetData[baseIndex + 2]);
                rp.VPBCrane.Sensors[i].Data0Low = packetData[baseIndex + 3];
                rp.VPBCrane.Sensors[i].Data0High = packetData[baseIndex + 4];
                rp.VPBCrane.Sensors[i].Data1Low = packetData[baseIndex + 5];
                rp.VPBCrane.Sensors[i].Data1High = packetData[baseIndex + 6];
            }

            Debug.WriteLine("Событие VPBCraneProcessed вызывается");
            VPBCraneProcessed?.Invoke(rp.VPBCrane);
        }




        private int BCDToDecimal(byte bcd)
        {
            return (bcd >> 4 & 0x0F) * 10 + (bcd & 0x0F);
        }

        public static byte BCDToByte(byte bcd)
        {
            return (byte)(((bcd >> 4) * 10) + (bcd & 0x0F));
        }

        public static string ConvertIntsToString(int[] intArray)
        {
            // Конвертируем массив целых чисел в массив байтов
            byte[] byteArray = Array.ConvertAll(intArray, n => (byte)n);

            // Преобразуем байты в строку с использованием кодировки Windows-1251
            string windows1251String = Encoding.GetEncoding(1251).GetString(byteArray);

            // Преобразуем строку в массив байтов в кодировке UTF-16
            byte[] utf16Bytes = Encoding.Unicode.GetBytes(windows1251String);

            // Преобразуем обратно в строку для вывода
            return Encoding.Unicode.GetString(utf16Bytes);
        }
    }
}
