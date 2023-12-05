using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CleanArchitecture.Infrastructure.Converters;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly?>
{
    private const string FullTimeFormat = "HH:mm:ss.FFFFFFF";
    private const string HourAndMinFormat = "HH:mm";
    public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        if (TimeOnly.TryParseExact(value, FullTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeOnly result))
        {
            return result;
        }

        return TimeOnly.ParseExact(value, HourAndMinFormat, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString(FullTimeFormat, CultureInfo.InvariantCulture));
        }
    }
}