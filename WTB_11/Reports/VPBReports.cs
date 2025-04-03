using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FastReport;
using WPB_11.DataStructures;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WPB_11.Reports
{
    class VPBReports
    {
        private string _reportPath;

        public VPBReports(string reportPath)
        {
            _reportPath = reportPath;
        }

        public void GenerateReport(VPBCrane.VPBCraneStruct craneData)
        {
            // Создаем экземпляр отчета
            using (Report report = new Report())
            {
                try
                {
                    // Загружаем шаблон отчета
                    report.Load(_reportPath);
                    Debug.WriteLine("Load");

                    if (report.Pages.Count == 0)
                    {
                        MessageBox.Show("Отчет загружен, но не содержит страниц.");
                        return;
                    }

                    // Заполняем текстовые поля отчета
                    FillTextFields(report, craneData);

                    // Подготавливаем отчет
                    report.Prepare();

                    // Показать отчет
                    report.Show();
                }
                catch (Exception ex)
                {
                    // Обработка ошибок
                    MessageBox.Show($"Ошибка при генерации отчета: {ex.Message}");
                }
            }
        }


        private void FillTextFields(Report report, VPBCrane.VPBCraneStruct craneData)
        {
            // Заполнение текстовых полей отчета
            if (report.Pages.Count > 0)
            {
                var page = report.Pages[0];
                Debug.WriteLine($"Ошибка при генерации отчета: {System.Text.Encoding.UTF8.GetString(craneData.Crane)}");
                Debug.WriteLine($"loadgroup: {craneData.LoadGroup}");
                if (craneData.Crane != null)
                {
                    TextObject Crane = (TextObject)page.FindObject("Crane");
                    Crane.Text = System.Text.Encoding.UTF8.GetString(craneData.Crane);
                }
                if (craneData.CraneNumber != null)
                {
                    TextObject CraneNumber = (TextObject)page.FindObject("Crane_Number");
                    CraneNumber.Text = new string(craneData.CraneNumber);
                }
                if (craneData.VPBNumber != null)
                {
                    TextObject VPBNumber = (TextObject)page.FindObject("VPB_FactNum");
                    string valueToAssign = new string(craneData.VPBNumber);
                    string cleanValue = System.Text.RegularExpressions.Regex.Replace(valueToAssign, @"[^\d]", "");
                    VPBNumber.Text = cleanValue;

                }
                if (craneData.LoadGroup != 0)
                {
                    TextObject LoadGroup = (TextObject)page.FindObject("Load_Group");
                    tabCrane tabc = new tabCrane();
                    LoadGroup.Text = "A " + tabc.GetSelectedLoadingModes();
                    //Debug.WriteLine($"loadgroup: {(char)craneData.LoadGroup}");
                }
                if (craneData.ProgramVersion != 0)
                {
                    TextObject ProgramVersion = (TextObject)page.FindObject("VPB_SoftVersion");
                    ProgramVersion.Text = craneData.ProgramVersion.ToString();
                    //Debug.WriteLine($"version: {craneData.ProgramVersion}");
                }
                if (craneData.SetupDate != null)
                {
                    TextObject SetupDate = (TextObject)page.FindObject("VPB_SetupDate");
                    SetupDate.Text = BitConverter.ToString(craneData.SetupDate);
                }
                if (craneData.MaxQ1 != 0)
                {
                    TextObject MaxQ1 = (TextObject)page.FindObject("Memo177");
                    Debug.WriteLine($"MaxQ1: {MaxQ1}");
                    //MaxQ1.Text = craneData.MaxQ1.ToString();
                }

                TextObject ReadDataDateTime = (TextObject)page.FindObject("ReadDataDateTime");
                ReadDataDateTime.Text = DateTime.Now.ToString();
                TextObject Registrator = (TextObject)page.FindObject("Registrator");
                Registrator.Text = "ВПБ";
            }
            else
            {
                MessageBox.Show("Отчет не имеет страниц");
            }
        }
    }
}

