using System.Text.Json.Serialization;

namespace Cookbook.Core.Excpetions;

public class Error
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("detail")]
    public string Detail { get; set; }
}
