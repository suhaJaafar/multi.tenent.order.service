using System.Text.Json;
using System.Text.Json.Serialization;
using IdentityService.Domain.Identity.ObjectValues;

namespace IdentityService.Domain.Identity.ObjectValues;

public class PhoneNumberJsonConverter : JsonConverter<PhoneNumber?>
{
    public override PhoneNumber? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrWhiteSpace(value) ? null : new PhoneNumber(value);
    }

    public override void Write(Utf8JsonWriter writer, PhoneNumber? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Value);
        }
    }
}

