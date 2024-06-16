public struct SFTTrainerData
{
    public SFTTrainerData()
    {
        Messages = new List<Message>();
    }

    public SFTTrainerData(string id, List<Message> messages)
    {
        Id = id;
        Messages = messages;
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

    public record Message
    {
        public required string Role { get; set; }

        public required string Content { get; set; }

        //public Message(string role, string content)
        //{
        //    Role = role;
        //    Content = content;
        //}
    }

    public string? Id { get; set; }

    public List<Message> Messages { get; set; } = new List<Message>();
}
