using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using WPB_11.DataStructures;

namespace WPB_11
{
    public class DeviceConnector
    {
        private static DeviceConnector _instance;
        private SerialPort _serialPort;
        public bool IsConnected { get; private set; }

        public event Action<string> OnDeviceConnected;
        private bool _waitingForTimeResponse = false;

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
                OnDeviceConnected?.Invoke($"Ошибка подключения: {ex.Message}");
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
                OnDeviceConnected?.Invoke("Запрос времени");
                _waitingForTimeResponse = true; // Установим флаг ожидания ответа
                StartReading();
            }
            catch (Exception ex)
            {
                OnDeviceConnected?.Invoke("ошибка отправки");
            }
        }

        private void ReadData()
        {
            OnDeviceConnected?.Invoke("ReadData");

            while (IsConnected)
            {
                OnDeviceConnected?.Invoke("попали в цикл");
                try
                {
                    OnDeviceConnected?.Invoke("попали в трай");

                    if (_serialPort.BytesToRead > 0)
                    {
                        byte[] response = new byte[12]; // Предположим, что ответ будет 12 байт
                        int bytesRead = _serialPort.Read(response, 0, response.Length); // Чтение ответа

                        OnDeviceConnected?.Invoke($"Прочитано байтов: {bytesRead}, данные: {BitConverter.ToString(response.Take(bytesRead).ToArray())}"); // Отладочное сообщение

                        if (bytesRead == response.Length)
                        {
                            if (_waitingForTimeResponse)
                            {
                                ProcessTimeResponse(response);
                                _waitingForTimeResponse = false; // Сброс флага ожидания
                            }
                            else
                            {
                                ProcessData(BitConverter.ToString(response)); // Обработка других данных
                            }
                        }
                        else
                        {
                            OnDeviceConnected?.Invoke("Некорректное количество байтов прочитано.");
                        }
                    }
                    else
                    {
                        OnDeviceConnected?.Invoke("Нет доступных данных для чтения.");
                    }
                }
                catch (TimeoutException)
                {
                    OnDeviceConnected?.Invoke("Таймаут при чтении данных.");
                }
                catch (Exception ex)
                {
                    OnDeviceConnected?.Invoke($"Ошибка чтения данных: {ex.Message}");
                    Disconnect();
                }
            }
        }



        

        private void ProcessTimeResponse(byte[] response)
        {
            if (response.Length < 12)
            {
                OnDeviceConnected?.Invoke("Некорректный ответ");
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
                OnDeviceConnected?.Invoke("ошибка crc");
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

            OnDeviceConnected?.Invoke("полученное время");
            OnDeviceConnected?.Invoke($"Текущее время устройства: {deviceTime}"); // Отправка времени в событие
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

