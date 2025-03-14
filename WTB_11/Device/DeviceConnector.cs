using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using WPB_11.DataStructures;

namespace WPB_11.Device
{
    public class DeviceConnector
    {
        private static DeviceConnector _instance;
        private SerialPort _serialPort;
        public bool IsConnected { get; private set; }

        public event Action<string> OnDeviceConnected;
        private DevicePackets _devicePackets; // Добавляем поле для хранения экземпляра DevicePackets

        private DeviceConnector(string portName, DevicePackets devicePackets)
        {
            _devicePackets = devicePackets; // Сохраняем переданный экземпляр
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

        public static DeviceConnector Instance(string portName = "COM3", DevicePackets devicePackets = null)
        {
            if (_instance == null)
            {
                _instance = new DeviceConnector(portName, devicePackets);
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
                        //OnDeviceConnected?.Invoke("Доступные порты: " + string.Join(", ", ports));

                        _serialPort.Open();
                        IsConnected = true;
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

        

        private void SendData(byte[] data)
        {
            //OnDeviceConnected?.Invoke("SendData");
            if (!IsConnected || !_serialPort.IsOpen)
            {
                OnDeviceConnected?.Invoke("Устройство не подключено или порт закрыт. Невозможно отправить данные.");
                return;
            }

            try
            {
                _serialPort.Write(data, 0, data.Length);
                //OnDeviceConnected?.Invoke($"Данные отправлены. {BitConverter.ToString(data)}");
            }
            catch (Exception ex)
            {
                //OnDeviceConnected?.Invoke("Ошибка при отправке данных: " + ex.Message);
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


        public void Request(byte[] sendData)
        {
            //OnDeviceConnected?.Invoke("Отправляю команду для получения даты и времени.");
            if (IsConnected)
            {
                //OnDeviceConnected?.Invoke($"Отправляю данные: {BitConverter.ToString(sendData)}");

                try
                {
                    SendData(sendData);
                    Thread.Sleep(100); // Задержка для обработки команды устройством

                    // Ожидание ответа от устройства
                    byte[] response = ReceiveData(); // Получите ответ от устройства
                    if (response != null)
                    {
                        ProcessReceivedData(response);
                        // OnDeviceConnected?.Invoke($"Получен ответ: {BitConverter.ToString(response)}");
                    }
                    else
                    {
                        // OnDeviceConnected?.Invoke("Ответ не получен.");
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

            // Извлекаем айди пакета из четвертого байта
            byte packetId = packetData[3];

            switch (packetId)
            {
                case 1: // пакет дата время температура
                    _devicePackets.ProcessDateTimePacket(packetData);
                    break;

                case 3: // пакет все про прибо и кран
                    _devicePackets.ProcessVPBCurr(packetData);
                    break;

                default:
                    OnDeviceConnected?.Invoke("Неизвестный тип пакета.");
                    break;
            }
        }



    }

}

