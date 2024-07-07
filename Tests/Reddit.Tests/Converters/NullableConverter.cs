using Reddit.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reddit.Tests.Converters
{
    public class TestClassWithNullableField
    {
        [JsonPropertyName("int_value")]
        public required int intValue { get; set; }

        [JsonConverter(typeof(NullableConverter<string>))]
        [JsonPropertyName("string_value")]
        public string? stringValue { get; set; }
    }
    public class NullableConverterTest
    {
        static string nullable = """
            {
                "int_value": 9999,
                "string_value": ""
            }
            """;

        [Fact]
        void ConvertEmptyStringToNull()
        {
            TestClassWithNullableField expectedObject = new TestClassWithNullableField
            {
                intValue = 9999,
                stringValue = null
            };

            TestClassWithNullableField actualObject = JsonSerializer.Deserialize<TestClassWithNullableField>(nullable)!;

            Assert.Null(actualObject.stringValue);
            Assert.Equivalent(expectedObject, actualObject);
            Assert.Equal(JsonSerializer.Serialize(expectedObject), JsonSerializer.Serialize(actualObject));
        }

        static string normal = """
            {
                "int_value": 9999,
                "string_value": "test"
            }
            """;

        [Fact]
        void NormalDeserialize()
        {
            TestClassWithNullableField expectedObject = new TestClassWithNullableField
            {
                intValue = 9999,
                stringValue = "test"
            };

            TestClassWithNullableField actualObject = JsonSerializer.Deserialize<TestClassWithNullableField>(normal)!;

            Assert.Equivalent(expectedObject, actualObject);
            Assert.Equal(JsonSerializer.Serialize(expectedObject), JsonSerializer.Serialize(actualObject));
            Assert.Equal("""{"int_value":9999,"string_value":"test"}""", JsonSerializer.Serialize(actualObject));
        }
    }
}
