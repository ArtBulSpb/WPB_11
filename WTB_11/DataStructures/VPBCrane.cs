using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11.DataStructures
{
    public class VPBCrane
    {
        public struct VPBCraneStruct
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public byte[] Crane; // Название крана

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public char[] VPBNumber; // Номер прибора

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public char[] CraneNumber; // Номер крана

            public byte[] SetupDate; // Дата настройки (3 байта)
            public byte ProgramVersion; // Версия ПО
            public byte LoadGroup; // Группа нагрузки
            public byte MaxV; // Максимальная скорость
            public byte IntegralV; // Интегральная скорость
            public uint TpchrPoint; // Адрес последней записи в ТПЧР
            public uint[] Cycles1; // Циклы 1
            public uint[] Cycles2; // Циклы 2
            public uint SummCycles1 { get; set; }
            public uint SummCycles2 { get; set; }
            public Single CharacteristicNumber1; // Характеристика 1
            public Single CharacteristicNumber2; // Характеристика 2
            public uint SummQ1; // Сумма нагрузки 1
            public uint SummQ2; // Сумма нагрузки 2
            public uint OperatingTime; // Наработка крана
            public uint MaxQ1; // Максимальная нагрузка 1
            public uint MaxQ2; // Максимальная нагрузка 2
            public short CoeffQ1; // Коэффициент для массы 1
            public short AdditivQ1; // Добавка для массы 1
            public short CoeffQ2; // Коэффициент для массы 2
            public short AdditivQ2; // Добавка для массы 2
            public byte Integral1; // Интегральная скорость 1
            public byte Integral2; // Интегральная скорость 2
            public VPBSensors.VPBSensorsStruct[] Sensors; // Датчики
        }

    }
}
