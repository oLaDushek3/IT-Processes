using System.Text.Json.Serialization;

namespace ITProcesses.JsonSaveInfo;

public class AppSettings
{
    [JsonPropertyName("username")] public string? UserName { get; set; } = string.Empty;

    [JsonPropertyName("password")] public string? Password { get; set; } = string.Empty;
    
    [JsonPropertyName("currentProject")] public int CurrentProject { get; set; }
    
    [JsonPropertyName("currentTheme")] public string CurrentTheme { get; set; } = string.Empty;
}