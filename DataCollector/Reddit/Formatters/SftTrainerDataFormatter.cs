using Reddit.Models;
using static Reddit.Models.SftTrainerData;

namespace Reddit.Formatters;

public static class SftTrainerDataFormatter
{
    public static void Format(Thing thing, SftTrainerData data, string role)
    {
        switch (thing.Data)
        {
            case Listing:
                Format((Listing)thing.Data, data, role);
                break;

            case Post:
                Format((Post)thing.Data, data, role);
                break;

            case Comment:
                Format((Comment)thing.Data, data, role);
                break;
        }
    }

    public static void Format(Listing listing, SftTrainerData data, string role)
    {
        var conversation = new List<Message>(data.Conversation);
        foreach (Thing thing in listing.Children)
        {
            data.Conversation = new List<Message>(conversation);
            Format(thing, data, role);
        }
    }

    public static void Format(Post post, SftTrainerData data, string role)
    {
        var message = new Message
        {
            Role = role,
            Content = $"{post.Title} {post.SelfText}"
        };
        data.PostId = post.Id;
        data.Conversation.Add(message);
    }

    public static void Format(Comment comment, SftTrainerData data, string role)
    {
        data.Conversation.Add(new Message { Role = role, Content = comment.Body });

        if (comment.Replies == null)
        {
            data.ConversationList.Add(data.Conversation);
            return;
        }

        Format(comment.Replies, data, Roles.SwapRoles(role));
    }

    public static SftTrainerData Format(List<Thing> listOfThings)
    {
        var data = new SftTrainerData();
        Format(listOfThings[0], data, Roles.User);
        Format(listOfThings[1], data, Roles.Assistant);

        return data;
    }


}
