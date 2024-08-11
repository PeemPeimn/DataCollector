using Reddit.Models;
using System.Text.Json;

namespace Reddit.Converters
{
    public static class ObjectConverter
    {
        public static void Convert(Thing? thing)
        {
            if (thing == null) return;

            var jsonData = (JsonElement)thing.Data;

            switch (thing.Kind)
            {
                case "Listing":
                    thing.Data = jsonData.Deserialize<Listing>()!;
                    foreach (Thing t in ((Listing)thing.Data).Children)
                    {
                        Convert(t);
                    }
                    break;

                case "t3":
                    thing.Data = jsonData.Deserialize<Post>()!;
                    break;

                case "t1":
                    var comment = jsonData.Deserialize<Comment>()!;
                    Convert(comment.Replies);
                    thing.Data = comment;
                    break;
            }
        }
    }
}
