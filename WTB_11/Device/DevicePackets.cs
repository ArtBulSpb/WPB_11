using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WPB_11.DataStructures;
using static System.Net.Mime.MediaTypeNames;
using static WPB_11.DataStructures.RP;
using static WPB_11.DataStructures.VPBCrane;
using static WPB_11.DataStructures.VPBDateTimeTemp;

namespace WPB_11.Device
{
    public class DevicePackets
    {
        private static DevicePackets _instance;
        public event Action<VPBCurrType.VPBCurrTypeStruct> DateTimeProcessed;
        public event Action<VPBCrane.VPBCraneStruct> VPBCraneProcessed;
        public event Action<VPBCurrType.VPBCurrTypeStruct[]> TPCHRProcessed;
        public event Action<VPBCurrType.VPBCurrTypeStruct[]> TPProcessed;

        DateTime HighDateTime = new DateTime(1800, 01, 01, 0, 0, 0);
        DateTime LowDateTime = new DateTime(2400, 01, 01, 0, 0, 0);

        private bool isUpdatingTable = false;


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
            //Debug.WriteLine($"ProcessDateTimePacket вызван {BitConverter.ToString(packetData)}");
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
            //Debug.WriteLine($"Начало обработки данных VPBCrane {BitConverter.ToString(packetData)}");

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
                rp.VPBCrane.VPBNumber[i] = (char)packetData[i + 15];
            }

            // Номер крана
            for (int i = 0; i < 11; i++)
            {
                rp.VPBCrane.CraneNumber[i] = (char)packetData[i + 26];
            }

            // Дата настройки
            for (int i = 0; i < 3; i++)
            {
                rp.VPBCrane.SetupDate[i] = packetData[37 + i];
            }

            // Версия программы
            rp.VPBCrane.ProgramVersion = packetData[40];

            // Группа нагрузки
            rp.VPBCrane.LoadGroup = packetData[41];

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
                temp4bytes[i] = packetData[184 + i]; // Копируем данные из packetData

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

            // TpchrPoint
            rp.VPBCrane.TpchrPoint = (uint)packetData[44]
                          + ((uint)packetData[45] << 8)
                          + ((uint)packetData[46] << 16)
                          + ((uint)packetData[47] << 24);

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

        private RequestQueue requestQueue = new RequestQueue();

        public void ProcessTPCHRPacket(byte[] packetData)
        {
            //Debug.WriteLine($"ProcessTPCHRPacket вызван {BitConverter.ToString(packetData)}");
            if (isUpdatingTable) return; // Если уже обновляется, выходим

            isUpdatingTable = true;
            RP.RPStruct rp = new RP.RPStruct();
            rp.TPCHR = new VPBCurrType.VPBCurrTypeStruct[5449];

            var TempTPCHRKadr = new byte[24];
            int ReadKadrPacket = 0;

            while (ReadKadrPacket < 544)
            {
                for (int I = 0; I < 10; I++)
                {
                    for (int j = 0; j < 24; j++)
                    {
                        TempTPCHRKadr[j] = (byte)packetData[4 + I * 24 + j];
                    }

                    int index = I + ReadKadrPacket * 10;

                    if (index >= rp.TPCHR.Length)
                    {
                        break; // или завершите обработку
                    }
                    rp.TPCHR[index] = new VPBCurrType.VPBCurrTypeStruct(TempTPCHRKadr);
                }

                // Добавляем запрос в очередь
                //Debug.WriteLine($"ReadKadrPacketProcess {ReadKadrPacket}");
                requestQueue.Enqueue(ReadKadrPacket);
                ReadKadrPacket++;
            }
            isUpdatingTable = false;
            // Обработка события
            TPCHRProcessed?.Invoke(rp.TPCHR);
        }


        public void ProcessTPPacket(byte[] packetData)
        {
            //Debug.WriteLine($"ProcessTPCHRPacket вызван {BitConverter.ToString(packetData)}");
            if (isUpdatingTable) return; // Если уже обновляется, выходим

            isUpdatingTable = true;
            RP.RPStruct rp = new RP.RPStruct();
            rp.TPCHR = new VPBCurrType.VPBCurrTypeStruct[5449];

            var TempTPCHRKadr = new byte[24];
            int ReadKadrPacket = 0;

            while (ReadKadrPacket < 544)
            {
                for (int I = 0; I < 10; I++)
                {
                    for (int j = 0; j < 24; j++)
                    {
                        TempTPCHRKadr[j] = (byte)packetData[4 + I * 24 + j];
                    }

                    int index = I + ReadKadrPacket * 10;

                    if (index >= rp.TPCHR.Length)
                    {
                        break; // или завершите обработку
                    }
                    rp.TPCHR[index] = new VPBCurrType.VPBCurrTypeStruct(TempTPCHRKadr);
                }

                // Добавляем запрос в очередь
                //Debug.WriteLine($"ReadKadrPacketProcess {ReadKadrPacket}");
                requestQueue.Enqueue(ReadKadrPacket);
                ReadKadrPacket++;
            }
            isUpdatingTable = false;
            // Обработка события
            TPCHRProcessed?.Invoke(rp.TPCHR);
        }


        private int BCDToDecimal(byte bcd)
        {
            return (bcd >> 4 & 0x0F) * 10 + (bcd & 0x0F);
        }

        public static byte BCDToByte(byte bcd)
        {
            return (byte)(((bcd >> 4) * 10) + (bcd & 0x0F));
        }

        public DateTime ToDateTime(VPBDateTimeTempStruct dateTimeStruct)
        {
            int year = BCDToDecimal((byte)dateTimeStruct.Year) + 2000; // Предполагается, что год хранится в формате BCD
            int month = BCDToDecimal((byte)dateTimeStruct.Month);
            int day = BCDToDecimal((byte)dateTimeStruct.Date);
            int hour = BCDToDecimal((byte)dateTimeStruct.Hour);
            int minute = BCDToDecimal((byte)dateTimeStruct.Minute);
            int second = BCDToDecimal((byte)dateTimeStruct.Second);

            return new DateTime(year, month, day, hour, minute, second);
        }



    }
}
