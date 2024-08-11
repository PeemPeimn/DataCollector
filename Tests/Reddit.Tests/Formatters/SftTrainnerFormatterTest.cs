using Reddit.Formatters;
using Reddit.Models;

namespace Reddit.Tests.Formatters;

public class SftTrainnerFormatterTest
{
    public class FormatPostTest
    {
        [Fact]
        public void CheckData()
        {
            var post = new Post
            {
                Id = "1a2b3c",
                Title = "title",
                SelfText = "selftext",
            };
            var data = new SftTrainerData();
            var expectedData = new SftTrainerData
            {
                PostId = post.Id,
                Conversation = new List<SftTrainerData.Message> {
                    new SftTrainerData.Message
                    {
                        Role = SftTrainerData.Roles.User,
                        Content = $"{post.Title} {post.SelfText}"
                    }
                }
            };

            SftTrainerDataFormatter.Format(post, data, SftTrainerData.Roles.User);

            Assert.Equivalent(expectedData, data);
        }
    }

    public class FormatCommentTest
    {

        [Fact]
        public void HasReplies()
        {
            var comment = new Comment
            {
                Body = "test",
                Replies = new Thing
                {
                    Kind = "test",
                    Data = "test"
                }
            };

            var data = new SftTrainerData();
            var expectedData = new SftTrainerData();
            expectedData.Conversation.
                Add(new SftTrainerData.Message
                {
                    Role = SftTrainerData.Roles.User,
                    Content = comment.Body
                });

            SftTrainerDataFormatter.Format(comment, data, SftTrainerData.Roles.User);

            Assert.Equivalent(expectedData, data);
        }

        [Fact]
        public void NoReplies()
        {
            var comment = new Comment
            {
                Body = "test",
                Replies = null,
            };

            var data = new SftTrainerData();
            var expectedData = new SftTrainerData();
            expectedData.Conversation.
                Add(new SftTrainerData.Message
                {
                    Role = SftTrainerData.Roles.User,
                    Content = comment.Body
                });
            expectedData.ConversationList.Add(expectedData.Conversation);

            SftTrainerDataFormatter.Format(comment, data, SftTrainerData.Roles.User);

            Assert.Equivalent(expectedData, data);
        }
    }

    public class FormatListingTest
    {

        [Fact]
        public void ConversationIsCopied()
        {
            var data = new SftTrainerData();
            data.Conversation.
                Add(new SftTrainerData.Message
                {
                    Role = SftTrainerData.Roles.User,
                    Content = "test"
                });
            var conversation = data.Conversation;
            var listing = new Listing
            {
                Children = new List<Thing> {
                    new Thing { Kind = "test", Data = "test"}
                }
            };

            SftTrainerDataFormatter.Format(listing, data, SftTrainerData.Roles.User);

            Assert.True(conversation != data.Conversation);
        }
    }

    public class EndToEndFormatTest
    {
        private static List<Thing> listOfThings = new List<Thing> {
            new Thing {
                Kind = "t3",
                Data = new Post {
                    Id = "1a2b3c",
                    SelfText = "selftext",
                    Title = "title",
                }
            },
            new Thing {
                Kind = "listing",
                Data = new Listing {
                    Children = new List<Thing> {
                        new Thing {
                            Kind = "t1",
                            Data = new Comment {
                                Body = "comment1",
                                Replies = new Thing {
                                    Kind = "t1",
                                    Data = new Comment {
                                        Body = "comment1-1"
                                    }
                                },
                            },
                        },
                        new Thing {
                            Kind = "t1",
                            Data = new Comment {
                                Body = "comment2",
                                Replies = null,
                            },
                        }
                    }
                }
            },
        };

        [Fact]
        public void EndToEndTest()
        {
            var expectedData = new SftTrainerData
            {
                PostId = "1a2b3c",
                ConversationList = new List<List<SftTrainerData.Message>>
                {
                    new List<SftTrainerData.Message> {
                        new SftTrainerData.Message {
                            Role = SftTrainerData.Roles.User,
                            Content = $"title selftext",
                        },
                        new SftTrainerData.Message {
                            Role = SftTrainerData.Roles.Assistant,
                            Content = "comment1"
                        },
                        new SftTrainerData.Message {
                            Role = SftTrainerData.Roles.User,
                            Content = "comment1-1"
                        }
                    },
                    new List<SftTrainerData.Message> {
                        new SftTrainerData.Message {
                            Role = SftTrainerData.Roles.User,
                            Content = $"title selftext",
                        },
                        new SftTrainerData.Message {
                            Role = SftTrainerData.Roles.Assistant,
                            Content = "comment2"
                        },
                    }
                }
            };


            var data = SftTrainerDataFormatter.Format(listOfThings);

            Assert.Equivalent(expectedData.PostId, data.PostId);
            Assert.Equivalent(expectedData.ConversationList, data.ConversationList);

        }
    }
}

