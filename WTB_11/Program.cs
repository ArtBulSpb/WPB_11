using System;
using System.Drawing;
using System.Windows.Forms;
using WPB_11;
using WPB_11.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

public class MainForm : Form
{
    public MainForm()
    {
        this.BackColor = Color.White; // Цвет формы
        this.Icon = new Icon("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\VPBW_Icon.ico");
        this.Text = "ВПБ-А";
        string[] tabNames = {
            "Текущие значения",
            "Параметры прибора",
            "Параметры крана",
            "Долговременная информация",
            "Оперативная информация",
            "Перегрузки",
            "Настройка",
            "Дополнительно",
        };

        tabControl1 customTabControl = new tabControl1(tabNames)
        {
            Dock = DockStyle.Fill // Заполнить всю форму
        };

        this.Controls.Add(customTabControl); // Добавляем кастомный таб контрол в форму

        // Здесь можно добавить другие элементы управления или настройки формы.
    }

    

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new MainForm());
    }
}

   