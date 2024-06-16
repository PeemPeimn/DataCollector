// Kind: "t1"
using System.Text.Json.Serialization;

public class Comment
{

    [JsonPropertyName("body")]
    public required string Body { get; set; }

    [JsonConverter(typeof(NullableConverter<Thing>))]
    [JsonPropertyName("replies")]
    public Thing? Replies { get; set; }

}