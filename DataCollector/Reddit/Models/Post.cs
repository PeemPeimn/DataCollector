// Kind: "t3"
using System.Text.Json.Serialization;

public class Post
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("permalink")]
    public required string PermaLink { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("selftext")]
    public required string SelfText { get; set; }

}