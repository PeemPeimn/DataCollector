using System.Text.Json.Serialization;

namespace Reddit.Models;

public class Listing
{

    [JsonPropertyName("children")]
    public required List<Thing> Children { get; set; }
}
