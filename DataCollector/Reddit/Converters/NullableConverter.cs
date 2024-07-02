using System.Text.Json;
using System.Text.Json.Serialization;

public class NullableConverter<T> : JsonConverter<T?> where T : class
{
    static readonly byte[] Empty = Array.Empty<byte>();

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (reader.ValueTextEquals(Empty))
                return null;
        }
        return JsonSerializer.Deserialize<T>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value, options);
}