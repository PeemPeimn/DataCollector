using Reddit.Converters;
using Reddit.Models;
using System.Text.Json;

namespace Reddit.Tests.Converters
{
    public class ConvertPostTest
    {
        const string postJson = """
            {
                "kind": "t3",
                "data": {
                    "id": "test_id",
                    "title": "You'll never guess what this mosquito did today.",
                    "selftext": "That's crazy. That's messed up."
                }
            }
            """;
        const string notPostJson = """
            {
                "kind": "t4",
                "data": {
                    "id": "test_id",
                    "title": "You'll never guess what this mosquito did today.",
                    "selftext": "That's crazy. That's messed up."
                }
            }
            """;

        [Theory]
        [InlineData(postJson, true)]
        [InlineData(notPostJson, false)]
        public void ConvertToPost(string json, bool isPost)
        {
            Thing thing = JsonSerializer.Deserialize<Thing>(json)!;
            ObjectConverter.Convert(thing);
            Assert.Equal(isPost, thing.Data is Post);
        }
    }

    public class ConvertCommentTest
    {
        const string commentEmptyRepliesJson = """
            {
                "kind": "t1",
                "data": {
                    "body": "Harrison Temple",
                    "replies": ""
                }
            }
            """;

        const string commentNullRepliesJson = """
            {
                "kind": "t1",
                "data": {
                    "body": "Harrison Temple",
                    "replies": null
                }
            }
            """;

        const string commentHasRepliesJson = """
            {
                "kind": "t1",
                "data": {
                    "body": "Harrison Temple",
                    "replies": {
                        "kind": "t3",
                        "data": {
                            "id": "test_id",
                            "title": "You'll never guess what this mosquito did today.",
                            "selftext": "That's crazy. That's messed up."
                        }
                    }
                }
            }
            """;


        const string notCommentJson = """
            {
                "kind": "t3",
                "data": {
                    "id": "test_id",
                    "title": "You'll never guess what this mosquito did today.",
                    "selftext": "That's crazy. That's messed up."
                }
            }
            """;

        [Theory]
        [InlineData(commentEmptyRepliesJson, true, false)]
        [InlineData(commentNullRepliesJson, true, false)]
        [InlineData(commentHasRepliesJson, true, true)]
        [InlineData(notCommentJson, false, false)]
        public void ConvertToComment(string json, bool isComment, bool hasReplies)
        {
            Thing thing = JsonSerializer.Deserialize<Thing>(json)!;
            ObjectConverter.Convert(thing);
            Assert.Equal(isComment, thing.Data is Comment);
            if (isComment)
            {
                Assert.Equal(hasReplies, ((Comment)thing.Data).Replies != null);
            }
        }
    }

    public class ConvertListingTest
    {
        const string listingJson = """
                {
                    "kind": "Listing",
                    "data": {
                        "children": [
                            {
                                "kind": "t1",
                                "data": {
                                    "body": "Harrison Temple",
                                    "replies": ""
                                }
                            },
                            {
                                "kind": "t3",
                                "data": {
                                    "id": "test_id",
                                    "title": "You'll never guess what this mosquito did today.",
                                    "selftext": "That's crazy. That's messed up."
                                }
                            },
                            {
                                "kind": "Listing",
                                "data": {
                                    "children": []
                                }
                            }
                        ]
                    }
                }
                """;

        [Theory]
        [InlineData(listingJson)]
        public void CheckEachChildrenType(string json)
        {
            Thing thing = JsonSerializer.Deserialize<Thing>(json)!;

            ObjectConverter.Convert(thing);

            Listing list = (Listing)thing.Data;

            Assert.Equal(typeof(Comment), list.Children[0].Data.GetType());
            Assert.Equal(typeof(Post), list.Children[1].Data.GetType());
            Assert.Equal(typeof(Listing), list.Children[2].Data.GetType());
        }
    }

    //public class ConvertCommentTest
    //{
    //    const string emptyReplies = """
    //        {
    //            "kind": "t1",
    //            "data": {
    //                "body": "Harrison Temple",
    //                "replies": ""
    //            }
    //        }
    //        """;

    //    [Theory]
    //    [InlineData(emptyReplies)]
    //    public void RepliesIsAnEmptyString(string json)
    //    {
    //        Thing thing = JsonSerializer.Deserialize<Thing>(json)!;

    //        List<SFTTrainerData> dataList = new List<SFTTrainerData>();
    //        SFTTrainerData data = new SFTTrainerData();
    //        string role = SFTTrainerData.Roles.User;

    //        SFTTrainerData expectedData = new SFTTrainerData
    //        {
    //            PostId = null,
    //            Messages = new List<SFTTrainerData.Message>
    //            {
    //                new SFTTrainerData.Message {
    //                    Role = SFTTrainerData.Roles.User,
    //                    Content = "Harrison Temple" }
    //            }
    //        };

    //        List<SFTTrainerData> expectedDataList = new List<SFTTrainerData>();
    //        expectedDataList.Add(expectedData);

    //        ObjectConverter.Convert(thing, dataList, data, role);

    //        Assert.Equivalent(expectedData, data, true);
    //        Assert.Equivalent(expectedDataList, dataList);
    //        Assert.Equal(typeof(Comment), thing.Data.GetType());
    //        Assert.Null(((Comment)thing.Data).Replies);

    //    }

    //    const string withReplies = """
    //        {
    //            "kind": "t1",
    //            "data": {
    //                "body": "Harrison Temple",
    //                "replies": {
    //                    "kind": "Truth",
    //                    "data": "He's not real."
    //                }
    //            }
    //        }
    //        """;

    //    [Theory]
    //    [InlineData(withReplies, SFTTrainerData.Roles.User)]
    //    [InlineData(withReplies, SFTTrainerData.Roles.Assistant)]
    //    public void DataPostIdIsNotNull(string json, string role)
    //    {
    //        Thing thing = JsonSerializer.Deserialize<Thing>(json)!;

    //        List<SFTTrainerData> dataList = new List<SFTTrainerData>();
    //        SFTTrainerData data = new SFTTrainerData();

    //        SFTTrainerData expectedData = new SFTTrainerData
    //        {
    //            PostId = null,
    //            Messages = new List<SFTTrainerData.Message>
    //            {
    //                new SFTTrainerData.Message {
    //                    Role = role,
    //                    Content = "Harrison Temple" }
    //            }
    //        };

    //        List<SFTTrainerData> expectedDataList = new List<SFTTrainerData>();

    //        ObjectConverter.Convert(thing, dataList, data, role);

    //        Assert.Equivalent(expectedData, data, true);
    //        Assert.Equivalent(expectedDataList, dataList);
    //        Assert.Equal(typeof(Comment), thing.Data.GetType());
    //        Assert.NotNull(((Comment)thing.Data).Replies);

    //    }
    //}

    //public class ListingConvertTest
    //{
    //    const string json = """
    //        {
    //            "kind": "Listing",
    //            "data": {
    //                "children": [
    //                    {
    //                        "kind": "t1",
    //                        "data": {
    //                            "body": "Harrison Temple",
    //                            "replies": ""
    //                        }
    //                    },
    //                    {
    //                        "kind": "t3",
    //                        "data": {
    //                            "id": "test_id",
    //                            "title": "You'll never guess what this mosquito did today.",
    //                            "selftext": "That's crazy. That's messed up."
    //                        }
    //                    },
    //                    {
    //                        "kind": "Listing",
    //                        "data": {
    //                            "children": []
    //                        }
    //                    }
    //                ]
    //            }
    //        }
    //        """;

    //    [Theory]
    //    [InlineData(json)]
    //    public void CheckEachChildrenType(string json)
    //    {
    //        Thing thing = JsonSerializer.Deserialize<Thing>(json)!;

    //        List<SFTTrainerData> dataList = new List<SFTTrainerData>();
    //        SFTTrainerData data = new SFTTrainerData();

    //        SFTTrainerData expectedData = new SFTTrainerData();
    //        string role = SFTTrainerData.Roles.User;

    //        ObjectConverter.Convert(thing, dataList, data, role);

    //        Listing list = (Listing)thing.Data;

    //        Assert.Equal(typeof(Comment), list.Children[0].Data.GetType());
    //        Assert.Equal(typeof(Post), list.Children[1].Data.GetType());
    //        Assert.Equal(typeof(Listing), list.Children[2].Data.GetType());
    //    }

    //}

    //public class EntryPointListOfThingsConvertterTest
    //{

    //    const string json = """"
    //        [
    //            {
    //                "kind": "test_1",
    //                "data": ""
    //            },
    //            {
    //                "kind": "test_2",
    //                "data": ""
    //            }
    //        ]
    //        """";

    //    [Fact]
    //    public void TestVariableCreation()
    //    {

    //        List<Thing> listOfThings = JsonSerializer.Deserialize<List<Thing>>(json)!;

    //        List<SFTTrainerData> expectedDataList = new List<SFTTrainerData>();

    //        var dataList = ObjectConverter.Convert(listOfThings);

    //        Assert.Equivalent(expectedDataList, dataList);

    //    }
    //}
}
