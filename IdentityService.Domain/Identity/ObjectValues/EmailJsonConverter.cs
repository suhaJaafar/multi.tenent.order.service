using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityService.Domain.Identity.ObjectValues;

public class EmailJsonConverter : JsonConverter<Email>
{
    public override Email? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrWhiteSpace(value) ? null : new Email(value);
    }

    public override void Write(Utf8JsonWriter writer, Email value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}

