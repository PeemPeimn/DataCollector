// Base class for Reddit's data
using System.Text.Json.Serialization;

public class Thing
{
    [JsonPropertyName("kind")]
    public required string Kind { get; set; }

    [JsonPropertyName("data")]
    public required object Data { get; set; }
}