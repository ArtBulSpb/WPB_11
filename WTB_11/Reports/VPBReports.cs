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
                    VPBNumber.Text = new string(craneData.VPBNumber);
                }
                if (craneData.LoadGroup != 0)
                {
                    TextObject LoadGroup = (TextObject)page.FindObject("Load_Group");
                    LoadGroup.Text = ((char)craneData.LoadGroup).ToString();
                }
                if (craneData.ProgramVersion != 0)
                {
                    TextObject ProgramVersion = (TextObject)page.FindObject("VPB_SoftVersion");
                    ProgramVersion.Text = ((char)craneData.ProgramVersion).ToString();
                }
                if (craneData.SetupDate != null)
                {
                    TextObject SetupDate = (TextObject)page.FindObject("VPB_SetupDate");
                    SetupDate.Text = System.Text.Encoding.UTF8.GetString(craneData.SetupDate);
                }

                TextObject ReadDataDateTime = (TextObject)page.FindObject("ReadDataDateTime");
                ReadDataDateTime.Text = DateTime.Now.ToString();
            }
            else
            {
                MessageBox.Show("Отчет не имеет страниц");
            }
        }
    }
}

