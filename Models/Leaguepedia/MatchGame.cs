using System.Text.Json.Serialization;

namespace FantasyApp.Models.Leaguepedia;

public class MatchGame
{
    public string GameId { get; set; }

    [JsonPropertyName("N GameInMatch")]
    public string N_GameInMatch { get; set; }

    public string Team1 { get; set; }
    public string Team1Players { get; set; }
    public List<string> Team1PlayersList
    {
        get => Team1Players.Split(',').ToList();
        set => Team1Players = value != null ? string.Join(',', value) : null;
    }

    public string Team2 { get; set; }
    public string Team2Players { get; set; }
    public List<string> Team2PlayersList
    {
        get => Team2Players.Split(',').ToList();
        set => Team2Players = value != null ? string.Join(',', value) : null;
    }

    public string WinTeam { get; set; }

    public string UniqueMatch { get; set; }
    public List<PlayerScoreboard> Scoreboard { get; set; }
}

public class PlayerScoreboard
{
    [JsonPropertyName("Link")]
    public string PlayerName { get; set; }

    public string Role { get; set; }
    public string PlayerWin { get; set; }

    public string Champion { get; set; }
    public string Kills { get; set; }
    public string Deaths { get; set; }
    public string Assists { get; set; }
    public string Gold { get; set; }
    public string CS { get; set; }
    public string VisionScore { get; set; }
    public string DamageToChampions { get; set; }
}



