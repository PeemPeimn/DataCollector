using Reddit.Formatters;
using Reddit.Models;
using System.Text.Json;

namespace Reddit.Tests.DataFormatterTests
{
    public class FormatPostTest
    {
        const string json = """
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
        [InlineData(json)]
        public void DataPostIdIsNull(string json)
        {
            Thing thing = JsonSerializer.Deserialize<Thing>(json)!;

            List<SFTTrainerData> dataList = new List<SFTTrainerData>();
            SFTTrainerData data = new SFTTrainerData();
            string role = "test_role";
            DataFormatter.Format(thing, dataList, ref data, role);

            SFTTrainerData expectedData = new SFTTrainerData
            {
                PostId = "test_id",
                Messages = new List<SFTTrainerData.Message>
                {
                    new SFTTrainerData.Message {
                        Role = "test_role",
                        Content = "You'll never guess what this mosquito did today. That's crazy. That's messed up." }
                }
            };

            Assert.Equivalent(expectedData, data, true);
            Assert.Equal(typeof(Post), thing.Data.GetType());

        }

        [Theory]
        [InlineData(json)]
        public void DataPostIdIsNotNull(string json)
        {
            Thing thing = JsonSerializer.Deserialize<Thing>(json)!;

            List<SFTTrainerData> dataList = new List<SFTTrainerData>();
            SFTTrainerData data = new SFTTrainerData { PostId = "not_null" };
            string role = "test_role";
            DataFormatter.Format(thing, dataList, ref data, role);

            SFTTrainerData expectedData = new SFTTrainerData
            {
                PostId = "not_null",
                Messages = new List<SFTTrainerData.Message>
                {
                    new SFTTrainerData.Message {
                        Role = "test_role",
                        Content = "You'll never guess what this mosquito did today. That's crazy. That's messed up." }
                }
            };

            Assert.Equivalent(expectedData, data, true);
            Assert.Equal(typeof(Post), thing.Data.GetType());

        }
    }

    public class FormatCommentTest
    {
        const string emptyReplies = """
            {
                "kind": "t1",
                "data": {
                    "body": "Harrison Temple",
                    "replies": ""
                }
            }
            """;

        [Theory]
        [InlineData(emptyReplies)]
        public void RepliesIsAnEmptyString(string json)
        {
            Thing thing = JsonSerializer.Deserialize<Thing>(json)!;

            List<SFTTrainerData> dataList = new List<SFTTrainerData>();
            SFTTrainerData data = new SFTTrainerData();
            string role = SFTTrainerData.Roles.User;

            SFTTrainerData expectedData = new SFTTrainerData
            {
                PostId = null,
                Messages = new List<SFTTrainerData.Message>
                {
                    new SFTTrainerData.Message {
                        Role = SFTTrainerData.Roles.User,
                        Content = "Harrison Temple" }
                }
            };

            List<SFTTrainerData> expectedDataList = new List<SFTTrainerData>();
            expectedDataList.Add(expectedData);

            DataFormatter.Format(thing, dataList, ref data, role);

            Assert.Equivalent(expectedData, data, true);
            Assert.Equivalent(expectedDataList, dataList);
            Assert.Equal(typeof(Comment), thing.Data.GetType());
            Assert.Null(((Comment)thing.Data).Replies);

        }

        const string withReplies = """
            {
                "kind": "t1",
                "data": {
                    "body": "Harrison Temple",
                    "replies": {
                        "kind": "Truth",
                        "data": "He's not real."
                    }
                }
            }
            """;

        [Theory]
        [InlineData(withReplies, SFTTrainerData.Roles.User)]
        [InlineData(withReplies, SFTTrainerData.Roles.Assistant)]
        public void DataPostIdIsNotNull(string json, string role)
        {
            Thing thing = JsonSerializer.Deserialize<Thing>(json)!;

            List<SFTTrainerData> dataList = new List<SFTTrainerData>();
            SFTTrainerData data = new SFTTrainerData();

            SFTTrainerData expectedData = new SFTTrainerData
            {
                PostId = null,
                Messages = new List<SFTTrainerData.Message>
                {
                    new SFTTrainerData.Message {
                        Role = role,
                        Content = "Harrison Temple" }
                }
            };

            List<SFTTrainerData> expectedDataList = new List<SFTTrainerData>();

            DataFormatter.Format(thing, dataList, ref data, role);

            Assert.Equivalent(expectedData, data, true);
            Assert.Equivalent(expectedDataList, dataList);
            Assert.Equal(typeof(Comment), thing.Data.GetType());
            Assert.NotNull(((Comment)thing.Data).Replies);

        }
    }

    public class ListingFormatTest
    {
        const string json = """
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
        [InlineData(json)]
        public void CheckReferenceEqualityAndEachTypeOfChildren(string json)
        {
            Thing thing = JsonSerializer.Deserialize<Thing>(json)!;

            List<SFTTrainerData> dataList = new List<SFTTrainerData>();
            SFTTrainerData data = new SFTTrainerData();

            SFTTrainerData expectedData = new SFTTrainerData();
            string role = SFTTrainerData.Roles.User;

            SFTTrainerData refData = data;

            DataFormatter.Format(thing, dataList, ref data, role);

            Listing list = (Listing)thing.Data;

            Assert.False(refData.Equals(data));
            Assert.Equal(typeof(Comment), list.Children[0].Data.GetType());
            Assert.Equal(typeof(Post), list.Children[1].Data.GetType());
            Assert.Equal(typeof(Listing), list.Children[2].Data.GetType());
        }

    }

    public class EntryPointListOfThingsFormatterTest
    {

        const string json = """"
            [
                {
                    "kind": "test_1",
                    "data": ""
                },
                {
                    "kind": "test_2",
                    "data": ""
                }
            ]
            """";

        [Fact]
        public void TestVariableCreation()
        {
            List<SFTTrainerData> dataList;

            List<Thing> listOfThings = JsonSerializer.Deserialize<List<Thing>>(json)!;

            List<SFTTrainerData> expectedDataList = new List<SFTTrainerData>();

            DataFormatter.Format(listOfThings, out dataList);

            Assert.Equivalent(expectedDataList, dataList);

        }
    }
}
