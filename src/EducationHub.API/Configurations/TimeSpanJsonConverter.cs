using System.Text.Json;
using System.Text.Json.Serialization;

namespace EducationHub.API.Configurations
{
    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
                return TimeSpan.Zero;

            if (TimeSpan.TryParse(value, out var result))
                return result;

            throw new JsonException($"Não foi possível converter '{value}' para TimeSpan. Use o formato HH:mm:ss (ex: 40:00:00)");
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(@"hh\:mm\:ss"));
        }
    }
}
