using System.Text.Json.Serialization;

public struct SFTTrainerData
{
    public SFTTrainerData()
    {
        Messages = new List<Message>();
    }

    public static class Roles
    {
        public const string User = "user";
        public const string Assistant = "assistant";

        public static string SwapRoles(string role)
        {
            if (role == User)
            {
                return Roles.Assistant;
            }

            return Roles.User;
        }
    }

    public record struct Message
    {
        [JsonPropertyName("role")]
        public required string Role { get; set; }

        [JsonPropertyName("content")]
        public required string Content { get; set; }

    }

    public string? PostId { get; set; }

    public List<Message> Messages { get; set; } = new List<Message>();
}
