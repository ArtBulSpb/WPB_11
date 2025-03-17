using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11.DataStructures
{
    public class VPBCurrType
    {
        public struct VPBCurrTypeStruct
        {
            public VPBDateTimeTemp.VPBDateTimeTempStruct DTT; // Дата и время
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
            public byte WindForce; // Сила ветра (можно использовать byte, если это значение в пределах 0-255)
        }


    }
}
