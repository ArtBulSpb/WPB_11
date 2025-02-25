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
        this.BackColor = Color.White; // ���� �����
        this.Icon = new Icon("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\VPBW_Icon.ico");
        this.Text = "���-�";
        string[] tabNames = {
            "������� ��������",
            "��������� �������",
            "��������� �����",
            "�������������� ����������",
            "����������� ����������",
            "����������",
            "���������",
            "�������������",
        };

        tabControl1 customTabControl = new tabControl1(tabNames)
        {
            Dock = DockStyle.Fill // ��������� ��� �����
        };

        this.Controls.Add(customTabControl); // ��������� ��������� ��� ������� � �����

        // ����� ����� �������� ������ �������� ���������� ��� ��������� �����.
    }

    

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new MainForm());
    }
}

   