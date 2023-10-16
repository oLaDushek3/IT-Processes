using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ITProcesses.JsonSaveInfo;

public class SaveInfo
{
    private static string _AppSettings =>
        "appsettings.json";

    private static void WriteAppSettings(AppSettings appSettings)
    {
        using var sw = new StreamWriter(_AppSettings);

        var options = new JsonSerializerOptions
        {
            Encoder =
                JavaScriptEncoder.Create(
                    UnicodeRanges.All),
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(appSettings, options);

        sw.WriteAsync(json);
    }
}