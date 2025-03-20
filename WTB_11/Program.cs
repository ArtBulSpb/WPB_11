using System;
using System.Drawing;
using System.Windows.Forms;
using WPB_11;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

public class MainForm : Form
{
    public MainForm()
    {
        this.BackColor = Color.White; // ���� �����
        this.Icon = new Icon("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\VPBW_Icon.ico");
        this.Text = "���-�";
        this.ClientSize = new Size(1350, 800); // ���������� ������ �������
        this.FormBorderStyle = FormBorderStyle.FixedSingle; // ��������� ��������� �������
        this.MaximizeBox = false;
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

   