using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.Json;

namespace FantasyApp.Models.Leaguepedia;

public class TournamentMatch
{
    [JsonPropertyName("DateTime UTC")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime DateTime_UTC { get; set; }

    public string Team1 { get; set; }
    // public int Team1Score { get; set; }

    public string Team2 { get; set; }
    // public int Team2Score { get; set; }

    public string BestOf { get; set; }
    public string Round { get; set; }
    public string Phase { get; set; }

    public string MatchId { get; set; }
    public string UniqueMatch { get; set; }

    public bool HasFinished { get; set; } = false;
}

public class CustomDateTimeConverter: JsonConverter<DateTime>
{
    private readonly string _format = "yyyy-MM-dd hh:mm:ss";

    public CustomDateTimeConverter()
    {

    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
       return DateTime.ParseExact(reader.GetString(), _format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format, CultureInfo.InvariantCulture));
    }
}
