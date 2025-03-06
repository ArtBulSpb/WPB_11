using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace WPB_11
{
    public class DeviceConnector
    {
        private static DeviceConnector _instance;
        private SerialPort _serialPort;
        public bool IsConnected { get; private set; }

        public event Action<string> OnDeviceConnected;

        private DeviceConnector(string portName)
        {
            _serialPort = new SerialPort(portName)
            {
                BaudRate = 9600,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None
            };
        }

        public static DeviceConnector Instance(string portName = "COM3")
        {
            if (_instance == null)
            {
                _instance = new DeviceConnector(portName);
            }
            return _instance;
        }

        public void Connect()
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                    IsConnected = true;
                    StartReading();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Disconnect()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                IsConnected = false;
            }
        }

        private void StartReading()
        {
            Thread readThread = new Thread(ReadData);
            readThread.IsBackground = true;
            readThread.Start();
        }

        private void ReadData()
        {
            while (IsConnected)
            {
                try
                {
                    string data = _serialPort.ReadLine(); // Чтение данных из порта
                    ProcessData(data);
                }
                catch (TimeoutException) { }
                catch (Exception ex)
                {
                    OnDeviceConnected?.Invoke($"Ошибка чтения данных: {ex.Message}");
                    Disconnect();
                }
            }
        }

        private void ProcessData(string data)
        {
            byte[] responseData = ConvertDataStringToByteArray(data);
            ProcessTimeResponse(responseData);
        }

        // Пример метода для преобразования строки данных в массив байтов
        private byte[] ConvertDataStringToByteArray(string data)
        {
            // Здесь вы можете реализовать логику для преобразования строки в массив байтов
            // Например, если данные приходят в шестнадцатеричном формате
            string[] stringBytes = data.Split(' ');
            byte[] bytes = new byte[stringBytes.Length];
            for (int i = 0; i < stringBytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(stringBytes[i], 16);
            }
            return bytes;
        }

        public void RequestCurrentTime()
        {
            if (!IsConnected)
            {
                OnDeviceConnected?.Invoke("Устройство не подключено.");
                return;
            }

            byte[] sendData = new byte[12];
            sendData[0] = 0x3F; // Начало пакета
            sendData[1] = 0x00; // ID устройства
            sendData[2] = 0x08; // Длина пакета
            sendData[3] = 0x06; // Команда для получения времени
            sendData[4] = (byte)DateTime.Now.Second;
            sendData[5] = (byte)DateTime.Now.Minute;
            sendData[6] = (byte)DateTime.Now.Hour;
            sendData[7] = (byte)DateTime.Now.DayOfWeek;
            sendData[8] = (byte)DateTime.Now.Day;
            sendData[9] = (byte)DateTime.Now.Month;
            sendData[10] = (byte)(DateTime.Now.Year - 2000);
            sendData[11] = 0; // CRC (пока не вычисляется)

            // Вычисление CRC
            for (int i = 0; i < 10; i++)
            {
                sendData[11] ^= sendData[i];
            }

            try
            {
                _serialPort.Write(sendData, 0, sendData.Length);
                Console.WriteLine("Запрос времени отправлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки данных: {ex.Message}");
            }
        }

        // Метод для обработки ответа на запрос времени
        private void ProcessTimeResponse(byte[] response)
        {
            if (response.Length < 12)
            {
                Console.WriteLine("Некорректный ответ на запрос времени.");
                return;
            }

            // Проверка CRC
            byte crc = 0;
            for (int i = 0; i < 10; i++)
            {
                crc ^= response[i];
            }

            if (crc != response[11])
            {
                Console.WriteLine("Ошибка CRC.");
                return;
            }

            // Получение времени
            DateTime deviceTime = new DateTime(
                2000 + response[10], // Год
                response[9],         // Месяц
                response[8],         // День
                response[6],         // Час
                response[5],         // Минуты
                 response[4]          // Секунды
        );

            Console.WriteLine($"Полученное время: {deviceTime}");
        }

        public string CheckDeviceStatus()
        {
            if (!IsConnected)
            {
                return "Устройство не подключено.";
            }
            else
            {
                return "Устройство подключено.";
            }
        }
    }

}

