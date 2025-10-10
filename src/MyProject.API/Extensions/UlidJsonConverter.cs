using System.Text.Json;
using System.Text.Json.Serialization;
using NUlid;

namespace MyProject.Api.Extensions;

public class UlidJsonConverter : JsonConverter<Ulid>
{
    public override Ulid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Ulid.Parse(value!);
    }

    public override void Write(Utf8JsonWriter writer, Ulid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}