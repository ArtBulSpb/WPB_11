using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace WPB_11
{
    class DeviceConnector
    {
        private SerialPort _serialPort;
        public bool IsConnected { get; private set; }

        public event Action<string> OnDeviceConnected;

        public DeviceConnector(string portName)
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

        public void Connect()
        {
            try
            {
                _serialPort.Open();
                IsConnected = true;
                StartReading();
            }
            catch (Exception ex)
            {
                OnDeviceConnected?.Invoke($"Устройство отключено.");
            }
        }

        public void Disconnect()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                IsConnected = false;
                OnDeviceConnected?.Invoke("Устройство отключено.");
            }
        }

        private void StartReading()
        {
            Thread readThread = new Thread(new ThreadStart(ReadData));
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
            // Обработка полученных данных
            Console.WriteLine($"Received data: {data}");
            // Здесь можно добавить логику для обработки пакетов данных
        }

        public string CheckDeviceStatus()
        {
            if (!IsConnected)
            {
                return "Устройство не подключено.";
            } else
            {
                return "Устройство подключено.";
            }
        }
    }
}

