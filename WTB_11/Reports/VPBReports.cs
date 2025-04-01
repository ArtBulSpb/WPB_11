using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
            var page = report.Pages[0];
           

            TextObject Crane = (TextObject)page.FindObject("Crane");
            Crane.Text = System.Text.Encoding.UTF8.GetString(craneData.Crane);

            TextObject CraneNumber = (TextObject)page.FindObject("CraneNumber");
            CraneNumber.Text = new string(craneData.CraneNumber);

            TextObject VPBNumber = (TextObject)page.FindObject("VPBNumber");
            VPBNumber.Text = new string(craneData.VPBNumber);

        }
    }
}

