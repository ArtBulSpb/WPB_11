using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WPB_11
{
    class TabSettings
    {
        public void ShowTabContent(Panel contentPanel, string[] TabNames)
        {
            contentPanel.Controls.Clear();

            var roundedButton = new DoubleRoundedButton
            {
                LeftText = "Передач данных по Can",
                RightText = "Передача данных на сервер",
                Size = new Size(480, 40),
                Dock = DockStyle.None,
                Margin = new Padding(100, 0, 0, 10)
            };
            roundedButton.LeftButtonClick += (s, e) => MessageBox.Show("Нажата кнопка 'Обновить'!");
            roundedButton.RightButtonClick += (s, e) => MessageBox.Show("Нажата кнопка 'Печать'!");

            ArrowButton sendButton = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\send.JPG", 90, 200);
            sendButton.Click += (s, e) =>
            {
                MessageBox.Show("Посылаем команду...");
            };

            ArrowButton responseButton = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\response.JPG", 90, 200);
            responseButton.Click += (s, e) =>
            {
                MessageBox.Show("Получаем ответ...");
            };

            ArrowButton reloadButton = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\reload.JPG", 90, 200) { Height = 40};
            reloadButton.Click += (s, e) =>
            {
                MessageBox.Show("Обновляем...");
            };

            var atComandList = new CustomCheckedListBox("AT Команды") { Margin = new Padding(25, 0, 25, 0) , Height = 60};
            atComandList.AddItem("AT");
            atComandList.AddItem("ATEO");
            atComandList.AddItem("IMEI");
            atComandList.AddItem("Наличие модема");
            atComandList.AddItem("Статус сим");
            atComandList.AddItem("Регистрация в сети");
            atComandList.AddItem("Инфо о операторе");
            atComandList.AddItem("Состояние gprs");
            atComandList.AddItem("Ip адрес");


            var APN = new TextBoxWithButton("APN") { PlaceholderText = "значение появляется при подключении прибора" };
            var APNUser = new TextBoxWithButton("APN USER") { PlaceholderText = "значение появляется при подключении прибора" };
            var APNPassword = new TextBoxWithButton("APN PASSWORD") { PlaceholderText = "значение появляется при подключении прибора" };
            var host = new TextBoxWithButton("Host") { PlaceholderText = "значение появляется при подключении прибора" };
            var port = new TextBoxWithButton("Port") { PlaceholderText = "значение появляется при подключении прибора" };
            var username = new TextBoxWithButton("Username") { PlaceholderText = "значение появляется при подключении прибора" };
            var password = new TextBoxWithButton("Password") { PlaceholderText = "значение появляется при подключении прибора" };
            var comand = new RoundedTextBox("Команда") { PlaceholderText = "значение появляется при подключении прибора" };
            var response = new RoundedTextBox("Ответ от устройства") { PlaceholderText = "значение появляется при подключении прибора" };
            /*var atButton = new SingleRoundedButton
            {
                ButtonText = "AT",
                Size = new Size(200, 50),
                Dock = DockStyle.None,
            };
            var ateoButton = new SingleRoundedButton
            {
                ButtonText = "ATEO",
                Size = new Size(200, 50),
                Dock = DockStyle.None,
            };
            var imeiButton = new SingleRoundedButton
            {
                ButtonText = "IMEI",
                Size = new Size(200, 50),
                Dock = DockStyle.None,
            };
            var modemButton = new SingleRoundedButton
            {
                ButtonText = "Наличие модема",
                Size = new Size(200, 50),
                Dock = DockStyle.None,
            };
            var simStatusButton = new SingleRoundedButton
            {
                ButtonText = "Наличие модема",
                Size = new Size(200, 50),
                Dock = DockStyle.None,
            };
            var onlineRegistrationButton = new SingleRoundedButton
            {
                ButtonText = "Регистрация в сети",
                Size = new Size(200, 50),
                Dock = DockStyle.None,
            };
            var operatorInformationButton = new SingleRoundedButton
            {
                ButtonText = "Инфо о операторе",
                Size = new Size(200, 50),
                Dock = DockStyle.None,
            };
            var gprsStatusButton = new SingleRoundedButton
            {
                ButtonText = "Состояние gprs",
                Size = new Size(200, 50),
                Dock = DockStyle.None,
            };
            var IpButton = new SingleRoundedButton
            {
                ButtonText = "Ip адрес",
                Size = new Size(200, 50),
                Dock = DockStyle.None,
            };*/


            TableLayoutPanel layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3, // Один столбец для DataGridView
                RowCount = 7,    // Два ряда: один для таблицы, другой для кнопки
                Padding = new Padding(20),
                AutoSize = true
            };

            Panel separator1 = new Panel // отделяет дату и время от остального
            {
                Height = 1, // Высота разделителя
                Dock = DockStyle.Top, // Расположение сверху
                BackColor = Color.FromArgb(224, 224, 224), // Цвет E0E0E0
                Margin = new Padding(0, 0, 0, 10)
            };
            Panel separator2 = new Panel // отделяет дату и время от остального
            {
                Height = 1, // Высота разделителя
                Dock = DockStyle.Top, // Расположение сверху
                BackColor = Color.FromArgb(224, 224, 224), // Цвет E0E0E0
                Margin = new Padding(0, 10, 0, 10)
            };
            Panel separator3 = new Panel // отделяет дату и время от остального
            {
                Height = 1, // Высота разделителя
                Dock = DockStyle.Top, // Расположение сверху
                BackColor = Color.FromArgb(224, 224, 224), // Цвет E0E0E0
                Margin = new Padding(0, 10, 0, 10)
            };

            FlowLayoutPanel APNPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(20, 0, 0, 0)
            };
            APNPanel.Controls.Add(APN);
            APNPanel.Controls.Add(APNUser);
            APNPanel.Controls.Add(APNPassword);

            FlowLayoutPanel serverPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(20, 0, 0, 0)
            };
            serverPanel.Controls.Add(host);
            serverPanel.Controls.Add(port);


            FlowLayoutPanel userPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(20, 0, 0, 0)
            };
            userPanel.Controls.Add(username);
            userPanel.Controls.Add(password);

            /*FlowLayoutPanel atPanel1 = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(20, 0, 0, 0)
            };
            atPanel1.Controls.Add(atButton);
            atPanel1.Controls.Add(ateoButton);
            atPanel1.Controls.Add(imeiButton);

            FlowLayoutPanel atPanel2 = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(20, 0, 0, 0)
            };
            atPanel2.Controls.Add(modemButton);
            atPanel2.Controls.Add(simStatusButton);
            atPanel2.Controls.Add(onlineRegistrationButton);

            FlowLayoutPanel atPanel3 = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(20, 0, 0, 0)
            };
            atPanel3.Controls.Add(operatorInformationButton);
            atPanel3.Controls.Add(gprsStatusButton);
            atPanel3.Controls.Add(IpButton);*/

            FlowLayoutPanel commandPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(20, 0, 0, 0)
            };
            commandPanel.Controls.Add(comand);
            commandPanel.Controls.Add(sendButton);

            FlowLayoutPanel reloadPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(20, 0, 0, 0)
            };
            reloadPanel.Controls.Add(atComandList);
            reloadPanel.Controls.Add(reloadButton);

            FlowLayoutPanel responsePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(20, 0, 0, 0)
            };
            responsePanel.Controls.Add(response);
            responsePanel.Controls.Add(responseButton);

            layoutPanel.Controls.Add(roundedButton, 0, 0);
            layoutPanel.SetColumnSpan(roundedButton, 3);
            layoutPanel.Controls.Add(separator1, 0, 1);
            layoutPanel.SetColumnSpan(separator1, 3);
            layoutPanel.Controls.Add(APNPanel, 0, 2);
            layoutPanel.Controls.Add(serverPanel, 1, 2);
            layoutPanel.Controls.Add(userPanel, 2, 2);
            layoutPanel.Controls.Add(separator2, 0, 3);
            layoutPanel.SetColumnSpan(separator2, 3);
            /*layoutPanel.Controls.Add(atPanel1, 0, 4);
            layoutPanel.Controls.Add(atPanel2, 1, 4);
            layoutPanel.Controls.Add(atPanel3, 2, 4);*/
            layoutPanel.Controls.Add(commandPanel, 0, 4);
            layoutPanel.Controls.Add(reloadPanel, 1, 4);
            layoutPanel.Controls.Add(responsePanel, 2, 4);

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Для кнопки
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Для разделителя
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Для APNPanel
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Для serverPanel
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Для userPanel
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Для разделителей
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Для других панелей

            contentPanel.Controls.Add(layoutPanel);
        }
    }
}
