
using System.Text.Json.Serialization;

namespace FantasyApp.Models.Leaguepedia;

public class PlayerInfo
{
    public string ID { get; set; }
    public string Player { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
    public string NativeName { get; set; }
    public string Country { get; set; }
    public string Nationality { get; set; }
    public string Age { get; set; }
    public string Role { get; set; }
    public string IsRetired { get; set; }
    [JsonPropertyName("FileName")]
    public string ImageFileName { get; set; }
}

public class ScoreboardPlayer
{
    public string? Champion { get; set; }
    public string? Kills { get; set; }
    public string? Deaths { get; set; }
    public string? Assists { get; set; }
    public string? Gold { get; set; }
    public string? CS { get; set; }
    public string? DamageToChampions { get; set; }
    public string? VisionScore { get; set; }
    public string? Role { get; set; }
    public DateTime? Time { get; set; }
    public string? PlayerWin { get; set; }
}

public class PlayerStats
{
    public PlayerInfo Player { get; set; }
    public string PlayerImageUrl { get; set; }
    public List<ScoreboardPlayer> Scoreboards { get; set; }

    public int Sum(string key)
    {
        switch(key)
        {
            case "Win":
                var wins = 0;
                Scoreboards.ForEach(delegate(ScoreboardPlayer scoreboard) {
                    if (scoreboard.PlayerWin == "Yes")
                    {
                        wins += 1;
                    }
                });
                return wins;
            case "Loss":
                var losses = 0;
                Scoreboards.ForEach(delegate(ScoreboardPlayer scoreboard) {
                    if (scoreboard.PlayerWin == "No")
                    {
                        losses += 1;
                    }
                });
                return losses;
            case "Damage":
                return Scoreboards.Sum(s => int.Parse(s.DamageToChampions));
            case "Gold":
                return Scoreboards.Sum(s => int.Parse(s.Gold));
            default:
                return 0;
        }
    }

    public int Avg(string key)
    {
        switch(key)
        {
            case "Damage":
                return Sum("Damage") / Scoreboards.Count;
            case "Gold":
                return Sum("Gold") / Scoreboards.Count;
            default:
                return 0;
        }
    }

    public double WinPercent()
    {
        if (Scoreboards.Count == 0)
        {
            return 0.0;
        }

        return Math.Round((double)Sum("Win") / Scoreboards.Count * 100, 1);
    }
}


