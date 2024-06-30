using System.Text.Json;

namespace DataCollector.Reddit.DataFormatter
{
    public static class DataFormatter
    {
        public static void Format(Thing thing, List<SFTTrainerData> dataList, ref SFTTrainerData data, string role)
        {
            var jsonData = (JsonElement)thing.Data;

            switch (thing.Kind)
            {
                case "Listing":
                    thing.Data = JsonSerializer.Deserialize<Listing>(jsonData)!;
                    Format((Listing)thing.Data, dataList, ref data, role);
                    break;

                case "t3":
                    thing.Data = JsonSerializer.Deserialize<Post>(jsonData)!;
                    Format((Post)thing.Data, dataList, ref data, role);
                    break;

                case "t1":
                    thing.Data = JsonSerializer.Deserialize<Comment>(jsonData)!;
                    Format((Comment)thing.Data, dataList, ref data, role);
                    break;
            }
        }

        public static void Format(Listing listing, List<SFTTrainerData> dataList, ref SFTTrainerData data, string role)
        {
            // Create a copy of messages
            var currentMessages = data.Messages.ToList();
            foreach (Thing thing in listing.Children)
            {
                data.Messages = currentMessages.ToList();
                Format(thing, dataList, ref data, role);
            }
        }

        public static void Format(Comment comment, List<SFTTrainerData> dataList, ref SFTTrainerData data, string role)
        {
            data.Messages.Add(new SFTTrainerData.Message { Role = role, Content = comment.Body });

            if (comment.Replies == null)
            {
                dataList.Add(data);
                return;
            }

            Format(comment.Replies, dataList, ref data, SFTTrainerData.Roles.SwapRoles(role));
        }

        public static void Format(Post post, List<SFTTrainerData> dataList, ref SFTTrainerData data, string role)
        {
            if (data.PostId is null)
            {
                data.PostId = post.Id;
            }

            data.Messages.Add(new SFTTrainerData.Message { Role = role, Content = $"{post.Title} {post.SelfText}" });
        }

        public static void Format(List<Thing> listOfThings, out List<SFTTrainerData> dataList)
        {
            dataList = new List<SFTTrainerData>();
            var data = new SFTTrainerData();
            DataFormatter.Format(listOfThings[0], dataList, ref data, SFTTrainerData.Roles.User);
            DataFormatter.Format(listOfThings[1], dataList, ref data, SFTTrainerData.Roles.Assistant);
        }
    }
}
