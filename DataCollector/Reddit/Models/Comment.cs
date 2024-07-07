// Kind: "t1"
using Reddit.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Models;

public class Comment
{

    [JsonPropertyName("body")]
    public required string Body { get; set; }

    [JsonConverter(typeof(NullableConverter<Thing>))]
    [JsonPropertyName("replies")]
    public Thing? Replies { get; set; }

}