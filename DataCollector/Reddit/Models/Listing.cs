using System.Text.Json.Serialization;

public class Listing
{

    [JsonPropertyName("children")]
    public required List<Thing> Children { get; set; }
}
