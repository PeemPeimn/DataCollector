using Reddit.Formatters;
using System.Text.Json;
using Xunit;

namespace Reddit.Tests.DataFormatterTests
{
    public class FormatPostTest
    {
        [Fact]
        public void DataPostIdIsNull()
        {
            string json = """
            {
                "kind": "t3",
                "data": {
                    "id": "test_id",
                    "title": "You'll never guess what this mosquito did today.",
                    "selftext": "That's crazy. That's messed up."
                }
            }
            """;

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

        }

        [Fact]
        public void DataPostIdIsNotNull()
        {
            string json = """
            {
                "kind": "t3",
                "data": {
                    "id": "test_id",
                    "title": "You'll never guess what this mosquito did today.",
                    "selftext": "That's crazy. That's messed up."
                }
            }
            """;

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

        }
    }

}