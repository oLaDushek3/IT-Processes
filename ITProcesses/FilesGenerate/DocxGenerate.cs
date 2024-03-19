using System.Diagnostics;
using System.Drawing;
using System.Linq;
using ITProcesses.Models;
using ITProcesses.Services;
using Microsoft.Win32;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace ITProcesses.FilesGenerate;

public class DocxGenerate
{
    private readonly ItprocessesContext _context = new ItprocessesContext();

    public async void GeneratePeopleFromTask(Tasks tasks)
    {
        TaskService taskService = new TaskService(_context);

        var users = await taskService.GetAllUsersFromTask(tasks.Id);

        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "Word files(*.docx)|*.docx"
        };

        if (saveFileDialog.ShowDialog() == false)
            return;
        // получаем выбранный файл
        string filename = saveFileDialog.FileName;

        string path = filename;

        DocX document = DocX.Create(path);

        Paragraph paragraph = document.InsertParagraph();
        Paragraph paragraph20 = document.InsertParagraph();
        Paragraph paragraph1 = document.InsertParagraph();

        paragraph.AppendLine(
                "Государственное бюджетное профессиональное\nобразовательное учреждение Московской области\n«Серпуховский колледж»")
            .UnderlineColor(System.Drawing.Color.Black)
            .Font("Times New Roman")
            .FontSize(12)
            .Alignment = Alignment.center;

        paragraph.AppendLine($"ПРИКАЗ ")
            .Bold()
            .FontSize(12)
            .Bold()
            .Font("Times New Roman")
            .Alignment = Alignment.center;


        paragraph.AppendLine($"{tasks.DateStartTimestamp.Date} № {tasks.Id}")
            .Bold()
            .FontSize(12)
            .Font("Times New Roman")
            .UnderlineColor(Color.Black)
            .Alignment = Alignment.center;

        paragraph.AppendLine("г. Серпухов")
            .FontSize(12)
            .Font("Times New Roman")
            .Alignment = Alignment.center;

        paragraph20.AppendLine($"О назначении сотрудников для выполнения задачи {tasks.Name}")
            .FontSize(12)
            .Font("Times New Roman")
            .Alignment = Alignment.left;

        paragraph20.AppendLine("ПРИКАЗЫВАЮ")
            .Bold()
            .FontSize(12)
            .Bold()
            .Font("Times New Roman")
            .Alignment = Alignment.center;

        paragraph1.AppendLine($"Назначить для выполнения задачи {tasks.Name} следующих сотрудников:")
            .FontSize(12)
            .Font("Times New Roman")
            .Alignment = Alignment.left;

        Table table = document.AddTable(users.Count + 1, 4);

        table.Rows[0]
            .Cells[0]
            .Paragraphs
            .First()
            .Append("№\nп/п")
            .Font("Times New Roman")
            .FontSize(12)
            .Bold();

        table.Rows[0]
            .Cells[1]
            .Paragraphs
            .First()
            .Append("ФИО сотрудника")
            .Font("Times New Roman")
            .FontSize(12)
            .Bold();

        table.Rows[0]
            .Cells[2]
            .Paragraphs
            .First()
            .Append("Задача")
            .Font("Times New Roman")
            .FontSize(12)
            .Bold();

        table.Rows[0]
            .Cells[3]
            .Paragraphs
            .First()
            .Append("Даты\nучастия")
            .Font("Times New Roman")
            .FontSize(12)
            .Bold();

        int count = 1;

        foreach (var user in users)
        {
            table.Rows[count]
                .Cells[0]
                .Paragraphs
                .First()
                .Append(count.ToString())
                .Font("Times New Roman")
                .FontSize(12);

            table.Rows[count]
                .Cells[1]
                .Paragraphs
                .First()
                .Append($"{user.User.LastName} {user.User.FirstName} \n{user.User.MiddleName}")
                .FontSize(12)
                .Font("Times New Roman");

            table.Rows[count]
                .Cells[2]
                .Paragraphs
                .First()
                .Append(tasks.Name)
                .FontSize(12)
                .Font("Times New Roman");

            table.Rows[count]
                .Cells[3]
                .Paragraphs
                .First()
                .Append($"{tasks.DateStartTimestamp.Date} -\n{tasks.DateEndTimestamp.Date}")
                .FontSize(12)
                .Font("Times New Roman");

            count++;
        }

        table.Alignment = Alignment.center;

        document.InsertTable(table);

        Paragraph paragraph12 = document.InsertParagraph();

        paragraph12.AppendLine("Директор ГБПОУ МО\n«Серпуховский колледж»")
            .FontSize(12)
            .Font("Times New Roman")
            .Alignment = Alignment.left;

        paragraph12.Append("\t\t\t\t\t\t\tТ.В. Федорова")
            .FontSize(12)
            .Font("Times New Roman");

        Paragraph paragraph13 = document.InsertParagraph();

        paragraph13.AppendLine("С приказом ознакомлены:")
            .FontSize(12)
            .Font("Times New Roman")
            .Alignment = Alignment.left;

        Paragraph paragraph14 = document.InsertParagraph();

        foreach (var user in users)
        {
            paragraph14.AppendLine("jdsakjdksajlkdjsa")
                .FontSize(12)
                .Font("Times New Roman")
                .Color(Color.White)
                .UnderlineColor(Color.Black)
                .Alignment = Alignment.right;

            paragraph14.Append($"{user.User.LastName} {user.User.FirstName[0]}. {user.User.MiddleName[0]}.")
                .FontSize(12)
                .Font("Times New Roman")
                .Alignment = Alignment.right;
        }

        document.Save();

        Process.Start(new ProcessStartInfo
        {
            FileName = filename,
            UseShellExecute = true
        });
    }
}