using System.Text.Json.Serialization;

namespace ITProcesses.JsonSaveInfo;

public class AppSettings
{
    [JsonPropertyName("login")] public string? Email { get; set; } = string.Empty;

    [JsonPropertyName("password")] public string? Password { get; set; } = string.Empty;

}