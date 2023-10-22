using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ITProcesses.JsonSaveInfo;

public class SaveInfo
{
    private static string _appSettings =>
        "appsettings.json";

    public static AppSettings? AppSettings => ReadAppSettings();
    
    public static void SaveSettings(AppSettings appSettings)
    {
        WriteAppSettings(appSettings);
    }

    private static AppSettings DefaultSettings => new()
    {
        UserName = null,
        Password = null
    };

    public static void CreateAppSettingsDefault()
    {
        if(!File.Exists(_appSettings))
            SaveSettings(DefaultSettings);
    }

    private static AppSettings? ReadAppSettings()
    {
        if (File.Exists(_appSettings))
        {
            using var sr = new StreamReader(_appSettings);

            var json = sr.ReadToEnd();

            return JsonSerializer.Deserialize<AppSettings>(json);
        }
        else
            return new AppSettings();
    }

    private static void WriteAppSettings(AppSettings appSettings)
    {
        using var sw = new StreamWriter(_appSettings);

        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(appSettings, options);

        sw.WriteAsync(json);
    }
}