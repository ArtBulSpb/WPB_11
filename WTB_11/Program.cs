using System;
using System.Drawing;
using System.Windows.Forms;
using WPB_11;
using WPB_11.Properties;

public class MainForm : Form
{
    public MainForm()
    {
        this.BackColor = Color.White; // ���� �����

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

   