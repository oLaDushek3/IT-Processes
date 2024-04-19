using System.Collections.Generic;
using ITProcesses.Models;
using Microsoft.Win32;
using OfficeOpenXml;

namespace ITProcesses.FilesGenerate;

public class XlsxGenerate
{
    public void GenerateExcel(List<UsersTask> users)
    {
        var package = new ExcelPackage();

        var sheet = package.Workbook.Worksheets.Add("Users");

        sheet.Cells["A1"].Value = "Имя";
        sheet.Cells["B1"].Value = "Фамилия";
        sheet.Cells["C1"].Value = "Отчество";
        sheet.Cells["D1"].Value = "Должность";
        
        sheet.Cells["A1:L1"].Style.Font.Bold = true;

        int i = 2;

        foreach (var user in users)
        {
            sheet.Cells[i, 1].Value = user.User.FirstName;
            sheet.Cells[i, 2].Value = user.User.LastName;
            sheet.Cells[i, 3].Value = user.User.MiddleName;
            sheet.Cells[i, 4].Value = user.User.Role;
            
            i++;
        }
        
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "Excel files(*.xlsx)|*.xlsx"
        };

        if (saveFileDialog.ShowDialog() == false)
            return;
        // получаем выбранный файл
        string filename = saveFileDialog.FileName;

         package.SaveAs(filename);
    }
}