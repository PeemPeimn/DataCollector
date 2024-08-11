using System.Text.Json.Serialization;

namespace Reddit.Models;

public class SftTrainerData
{
    public string? PostId { get; set; }
    public List<List<Message>> ConversationList { get; set; } = new List<List<Message>>();
    public List<Message> Conversation { get; set; } = new List<Message>();

    public static class Roles
    {
        public const string User = "user";
        public const string Assistant = "assistant";

        public static string SwapRoles(string role)
        {
            if (role == User)
            {
                return Assistant;
            }

            return User;
        }
    }

    public record struct Message
    {
        [JsonPropertyName("role")]
        public required string Role { get; set; }

        [JsonPropertyName("content")]
        public required string Content { get; set; }

    }
}
