using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityService.Domain.Identity.ObjectValues;

public class PasswordJsonConverter : JsonConverter<Password>
{
    public override Password? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrWhiteSpace(value) ? null : new Password(value);
    }

    public override void Write(Utf8JsonWriter writer, Password value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}

