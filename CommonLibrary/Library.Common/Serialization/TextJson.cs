using Log.Common;
using System.Text.Json;

namespace Library.Common.Serialization;

/// <summary>
/// https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/how-to
/// </summary>
public static class TextJson
{
    public static string FileName { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "\\" +
        AppDomain.CurrentDomain.FriendlyName + ".json";

    public static JsonSerializerOptions SerializerOptions { get; set; } = new() {
        WriteIndented = true
    };

    public static T? Deserialize<T>()
    {
        if(File.Exists(FileName))
        {
            try
            {
                using var fileStream = File.Open(FileName, FileMode.Open);
                return JsonSerializer.Deserialize<T>(fileStream, SerializerOptions);
            }
            catch(Exception ex)
            {
                ExTrace.Warning(Texts.DeserializationFailed, FileName, ex);
            }
        }
        return default;
    }

    public static void Serialize<T>(T jsonClass)
    {
        try
        {
            using var fileStream = File.Open(FileName, FileMode.Create);
            JsonSerializer.Serialize(fileStream, jsonClass, SerializerOptions);
        }
        catch(Exception ex)
        {
            ExTrace.Warning(Texts.SerializationFailed, FileName, ex);
        }
    }
}