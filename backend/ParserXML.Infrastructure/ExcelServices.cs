using OfficeOpenXml;
using ParserXML.Core.DTOs;
using Microsoft.AspNetCore.Http;
using ParserXML.Core.Providers;

namespace ParserXML.Infrastructure
{
    public class ExcelServices : IExcelService
    {

        public ExcelServices()
        {
        }

      
        public void ExportToExcel(List<TableDataDto> dataList, string filePath)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            int column = 1;

         
            worksheet.Cells[1, column++].Value = "Name";
            worksheet.Cells[1, column++].Value = "Vorname";
            worksheet.Cells[1, column++].Value = "Geschlecht";
            worksheet.Cells[1, column++].Value = "Alter";
            worksheet.Cells[1, column++].Value = "Adresse";
            worksheet.Cells[1, column++].Value = "Geburtsort";
            worksheet.Cells[1, column++].Value = "Telefonnummer";
           worksheet.Cells[1, column++].Value = "Mobilnummer";
            worksheet.Cells[1, column++].Value = "Kommentar";

            for (int i = 0; i < dataList.Count; i++)
            {
                column = 1;
                var data = dataList[i];

                worksheet.Cells[i + 2, column++].Value = data.Name;
                 worksheet.Cells[i + 2, column++].Value = data.Vorname;
                worksheet.Cells[i + 2, column++].Value = data.Geschlecht;
                worksheet.Cells[i + 2, column++].Value = data.Alter;
                worksheet.Cells[i + 2, column++].Value = data.Adresse;
                worksheet.Cells[i + 2, column++].Value = data.Geburtsort;
                worksheet.Cells[i + 2, column++].Value = data.Telefonnummer;
                worksheet.Cells[i + 2, column++].Value = data.Mobilnummer;
                worksheet.Cells[i + 2, column++].Value = data.Kommentar;
            }

            package.SaveAs(new FileInfo(filePath));
        }


        public void AppendToExistingExcel(List<TableDataDto> dataList, string filePath, List<string> selectedNodes)
        {
            using var package = new ExcelPackage(new FileInfo(filePath));

            string newSheetName = $"Appended_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
            var worksheet = package.Workbook.Worksheets.Add(newSheetName);

            int columnIndex = 1;
            if (selectedNodes.Contains("Name")) worksheet.Cells[1, columnIndex++].Value = "Name";
            if (selectedNodes.Contains("Alter")) worksheet.Cells[1, columnIndex++].Value = "Alter";
            if (selectedNodes.Contains("Adresse")) worksheet.Cells[1, columnIndex++].Value = "Adresse";
            if (selectedNodes.Contains("Telefonnummer")) worksheet.Cells[1, columnIndex++].Value = "Telefonnummer";

            int startRow = 2;

            for (int i = 0; i < dataList.Count; i++)
            {
                var data = dataList[i];
                columnIndex = 1; 

                if (selectedNodes.Contains("Name") && !string.IsNullOrEmpty(data.Name))
                    worksheet.Cells[startRow + i, columnIndex++].Value = data.Name;
                if (selectedNodes.Contains("Alter") && data.Alter != 0)
                    worksheet.Cells[startRow + i, columnIndex++].Value = data.Alter;
                if (selectedNodes.Contains("Adresse") && !string.IsNullOrEmpty(data.Adresse))
                    worksheet.Cells[startRow + i, columnIndex++].Value = data.Adresse;
                if (selectedNodes.Contains("Telefonnummer") && !string.IsNullOrEmpty(data.Telefonnummer))
                    worksheet.Cells[startRow + i, columnIndex++].Value = data.Telefonnummer;
            }

            package.Save();
            Console.WriteLine($"Successfully appended entries to {filePath} in sheet '{newSheetName}'.");
        }
    }
}
