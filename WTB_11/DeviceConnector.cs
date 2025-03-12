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
                Handshake = Handshake.None,
                ReadTimeout = 3000,
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

        private readonly object _lock = new object();

        public void Connect()
        {
            lock (_lock)
            {
                if (IsConnected)
                {
                    OnDeviceConnected?.Invoke("Подключение уже в процессе...");
                    return;
                }

                IsConnected = true;

                try
                {
                    if (!_serialPort.IsOpen)
                    {
                        // Проверяем доступные порты
                        string[] ports = SerialPort.GetPortNames();
                        OnDeviceConnected?.Invoke("Доступные порты: " + string.Join(", ", ports));

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
        }

        public void Disconnect()
        {
            lock (_lock)
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                    IsConnected = false;
                }
            }
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

        private void StartReading()
        {
            Thread readThread = new Thread(ReadData);
            readThread.IsBackground = true; // Поток будет завершен при закрытии приложения
            readThread.Start();
        }

        private void SendData(byte[] data)
        {
            OnDeviceConnected?.Invoke("SendData");
            if (!IsConnected || !_serialPort.IsOpen)
            {
                OnDeviceConnected?.Invoke("Устройство не подключено или порт закрыт. Невозможно отправить данные.");
                return;
            }

            try
            {
                _serialPort.Write(data, 0, data.Length);
                OnDeviceConnected?.Invoke($"Данные отправлены. {BitConverter.ToString(data)}");
            }
            catch (Exception ex)
            {
                OnDeviceConnected?.Invoke("Ошибка при отправке данных: " + ex.Message);
            }
        }


        private void ReadData()
        {
            OnDeviceConnected?.Invoke("ReadData started.");
            while (IsConnected)
            {
                try
                {
                    if (_serialPort.BytesToRead > 0)
                    {
                        byte[] response = new byte[13]; // Предполагаем, что ответ будет 13 байт

                        // Проверяем, достаточно ли данных для чтения
                        if (_serialPort.BytesToRead >= response.Length)
                        {
                            int bytesRead = _serialPort.Read(response, 0, response.Length);
                            if (bytesRead == response.Length)
                            {
                                OnDeviceConnected?.Invoke("Прочитали данные.");
                                ProcessReceivedData(response);
                            }
                            else
                            {
                                OnDeviceConnected?.Invoke("Некорректное количество байтов прочитано.");
                            }
                        }
                        else
                        {
                            OnDeviceConnected?.Invoke("Недостаточно данных для чтения.");
                        }
                    }
                    else
                    {
                     
                        //OnDeviceConnected?.Invoke($"Прочитано 0 ");
                        Thread.Sleep(100); // Задержка перед повторной проверкой
                    }
                }
                catch (TimeoutException)
                {
                    OnDeviceConnected?.Invoke("Таймаут при чтении данных.");
                }
                catch (IOException ioEx)
                {
                    OnDeviceConnected?.Invoke($"Ошибка ввода-вывода: {ioEx.Message}");
                    Disconnect();
                    break;
                }
                catch (Exception ex)
                {
                    OnDeviceConnected?.Invoke($"Ошибка чтения данных: {ex.Message}");
                    Disconnect();
                    break;
                }
            }
        }


        private byte[] ReceiveData()
        {
            // Предположим, что у вас есть объект SerialPort
            if (_serialPort.IsOpen)
            {
                try
                {
                    // Дождаться получения данных
                    int bytesToRead = _serialPort.BytesToRead;
                    if (bytesToRead > 0)
                    {
                        byte[] buffer = new byte[bytesToRead];
                        _serialPort.Read(buffer, 0, bytesToRead);
                        return buffer; // Возвращаем полученные данные
                    }
                }
                catch (Exception ex)
                {
                    OnDeviceConnected?.Invoke($"Ошибка при получении данных: {ex.Message}");
                }
            }
            return null; // Возвращаем null, если нет данных
        }


        public void RequestDateTime()
        {
            OnDeviceConnected?.Invoke("Отправляю команду для получения даты и времени.");
            if (IsConnected)
            {
                byte[] sendData = new byte[5] { 0x3F, 0x00, 0x01, 0x01, 0x3F }; // Команда для запроса даты и времени

                OnDeviceConnected?.Invoke("Отправляю команду для получения даты и времени.");
                OnDeviceConnected?.Invoke($"Отправляю данные: {BitConverter.ToString(sendData)}");

                try
                {
                    SendData(sendData);
                    Thread.Sleep(100); // Задержка для обработки команды устройством

                    // Ожидание ответа от устройства
                    byte[] response = ReceiveData(); // Получите ответ от устройства
                    if (response != null)
                    {
                        OnDeviceConnected?.Invoke($"Получен ответ: {BitConverter.ToString(response)}");
                    }
                    else
                    {
                        //OnDeviceConnected?.Invoke("Ответ не получен.");
                    }
                }
                catch (Exception ex)
                {
                    OnDeviceConnected?.Invoke($"Ошибка при отправке данных: {ex.Message}");
                    return;
                }
            }
        }



        private void ProcessReceivedData(byte[] packetData)
        {
            // Проверяем, что пакет содержит достаточно данных
            if (packetData.Length < 13)
            {
                OnDeviceConnected?.Invoke("Недостаточно данных в пакете.");
                return;
            }

            // Извлекаем дату и время
            DateTime VPBDateTime = VPBDateTimeToDateTime(
                packetData[8],  // Год
                packetData[9],  // Месяц
                packetData[10], // День
                packetData[5],  // Час
                packetData[6],  // Минуты
                packetData[7]   // Секунды
            );

            OnDeviceConnected?.Invoke($"Дата и время: {VPBDateTime}"); // Отображение даты и времени
                                                               // Вы можете обновить UI элементы здесь, например, текстовые поля
        }

        private DateTime VPBDateTimeToDateTime(byte year, byte month, byte date, byte hour, byte minute, byte second)
        {
            int fullYear = 2000 + year; // Предполагаем, что год начинается с 2000
            return new DateTime(fullYear, month, date, hour, minute, second);
        }

    }

}

