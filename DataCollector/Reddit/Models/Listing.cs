namespace DataCollector.Reddit.Models
{
    public class Listing : Thing
    {
        public string? Before { get; set; }

        public string? After { get; set; }

        public required List<Thing> Children { get; set; }
    }
}
