using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPB_11.DataStructures;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WPB_11.Device
{
    class DeviceCommands
    {
        public static readonly byte[] RequestDateTime = new byte[5] { 0x3F, 0x00, 0x01, 0x01, 0x3F }; // Команда для запроса даты, времени и температуры
        public static readonly byte[] SetDateTime = CreateSetDateTime(); // Команда для установки даты и времени
        public static readonly byte[] RequestVPBCrane = new byte[5] { 0x3F, 0x00, 0x01, 0x03, 0x3D }; //запрос все про прибор и кран (VPBCrane)
        public static readonly byte[] RequestTPP = new byte[5] { 0x3F, 0x00, 0x01, 0x04, 0x3A }; //Запрос на TPPoint и TPSize(TPInfo) 
        public static readonly byte[] SetDeviceNumber = new byte[5] { 0x3F, 0x00, 0x01, 0x04, 0x3A }; //установить номер прибора 3F 00 0C 05 …data 11 bytes…. KC
        //public static readonly byte[] SetCraneMark = CreateSetCraneMark(); // установить марку крана
        //public static readonly byte[] SetCraneNumber = CreateSetCraneNumber(); // установить номер крана


        public static byte[] CreateSetDateTime() 
        {
            byte[] sendData = new byte[12];

            sendData[0] = 0x3F; // Начало команды
            sendData[1] = 0x00; // Следующий байт
            sendData[2] = 0x08; // Длина данных
            sendData[3] = 0x02; // Код или тип данных

            // Заполнение данными в формате BCD
            sendData[4] = IntToBCD(DateTime.Now.Second);
            sendData[5] = IntToBCD(DateTime.Now.Minute);
            sendData[6] = IntToBCD(DateTime.Now.Hour);
            sendData[7] = IntToBCD((int)DateTime.Now.DayOfWeek); // День недели (0 = Воскресенье)
            sendData[8] = IntToBCD(DateTime.Now.Day);
            sendData[9] = IntToBCD(DateTime.Now.Month);
            sendData[10] = IntToBCD(DateTime.Now.Year - 2000); // Год (последние две цифры)

            // Вычисление контрольной суммы
            sendData[11] = 0; // Инициализация контрольной суммы
            for (int i = 0; i < 11; i++)
            {
                sendData[11] ^= sendData[i]; // XOR для всех байт, кроме контрольной суммы
            }

            return sendData;
        }

        public static byte[] CreateSetCraneMark(tabCrane tabC)
        {
            byte[] sendData = new byte[16];
            sendData[0] = 0x3F; // Начальная часть команды
            sendData[1] = 0x00;
            sendData[2] = 0x0C;
            sendData[3] = 0x07;

            // Заполняем 11 байтов пробелами (0x20)
            for (int i = 0; i < 11; i++)
            {
                sendData[i + 4] = 0x20; // 0x20 - это пробел
            }

            // Проверяем, что craneInfoMark не равен null
            if (tabC.craneInfoMark != null)
            {
                string craneMark = tabC.craneInfoMark.Text;
                for (int i = 0; i < craneMark.Length && i < 11; i++)
                {
                    sendData[i + 4] = (byte)craneMark[i]; // Преобразуем символ в байт
                }
            }
            else
            {
                throw new InvalidOperationException("craneInfoMark is not initialized.");
            }

            // Вычисляем контрольную сумму
            byte checksum = 0;
            for (int i = 0; i < 15; i++) // XOR всех байтов, кроме последнего
            {
                checksum ^= sendData[i];
            }
            sendData[15] = checksum;

            return sendData;
        }


        public static byte[] CreateSetCraneNumber()
        {
            byte[] sendData = new byte[16];
            sendData[0] = 0x3F; // Начальная часть команды
            sendData[1] = 0x00;
            sendData[2] = 0x0C;
            sendData[3] = 0x08;

            // Заполняем 11 байтов пробелами (0x20)
            for (int i = 0; i < 11; i++)
            {
                sendData[i + 4] = 0x20; // 0x20 - это пробел
            }

            // Копируем текст номера крана из LabeledEdit3 в массив sendData
            tabCrane tabC = new tabCrane();
            string craneNumber = tabC.craneInfoNumber.Text;
            for (int i = 0; i < craneNumber.Length && i < 11; i++)
            {
                sendData[i + 4] = (byte)craneNumber[i]; // Преобразуем символ в байт
            }

            // Вычисляем контрольную сумму
            byte checksum = 0;
            for (int i = 0; i < 15; i++) // XOR всех байтов, кроме последнего
            {
                checksum ^= sendData[i];
            }
            sendData[15] = checksum;

            return sendData;
        }

        public static byte IntToBCD(int value)
        {
            return (byte)(value / 10 << 4 | value % 10); // Преобразование в BCD
        }


    }


}
