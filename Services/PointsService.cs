using Microsoft.Build.FileSystem;

namespace FantasyApp.Services;

public class PointsService : IPointsService
{
    public PointsService()
    {

    }

    private static Dictionary<string, int> PointsMap = new Dictionary<string, int>
    {
        { "Kill", 10 },
        { "Assist", 3 },
        { "VisionScore", 1 },
        { "Damage", 2 },
        { "Gold", 1 },
        { "Win", 10 },
    };

    public int GetPointsForScorebard(Models.Leaguepedia.PlayerScoreboard scoreboard)
    {
        int totalPoints = 0;

        foreach (var property in PointsMap)
        {
            switch (property.Key)
            {
                case "Kill":
                    totalPoints += int.TryParse(scoreboard.Kills, out int killsValue) ? killsValue * property.Value : 0;
                    break;
                case "Assist":
                    totalPoints += int.TryParse(scoreboard.Assists, out int assistsValue) ? assistsValue * property.Value : 0;
                    break;
                case "VisionScore":
                    totalPoints += int.TryParse(scoreboard.VisionScore, out int visionScoreValue) ? visionScoreValue * property.Value : 0;
                    break;
                case "Damage":
                    totalPoints += int.TryParse(scoreboard.DamageToChampions, out int damageValue) ? damageValue * property.Value / 1000 : 0;
                    break;
                case "Gold":
                    totalPoints += int.TryParse(scoreboard.Gold, out int goldValue) ? goldValue * property.Value / 1000 : 0;
                    break;
                case "Win":
                    totalPoints += scoreboard.PlayerWin == "Win" ? property.Value : 0;
                    break;
            }
        }

        return totalPoints;
    }

    public Dictionary<string, int> ProcessPlayerDraft(List<string> userDraft, List<Models.Leaguepedia.MatchGame> tournamentGames)
    {
        var playerScores = new Dictionary<string, int>();

        foreach (var playerName in userDraft)
        {
            var playerScoreboards = tournamentGames.SelectMany(game => game.Scoreboard)
                .Where(scoreboard => scoreboard.PlayerName == playerName)
                .ToList();

            int totalPoints = 0;
            foreach (var scoreboard in playerScoreboards)
            {
                int points = GetPointsForScorebard(scoreboard);
                totalPoints += points;
            }

            playerScores[playerName] = totalPoints;
        }

        foreach (var player in playerScores)
        {
            Console.WriteLine(player.Key + " " + player.Value);
        }

        return playerScores;
    }
}

public interface IPointsService
{
    public Dictionary<string, int> ProcessPlayerDraft(List<string> userDraft, List<Models.Leaguepedia.MatchGame> tournamentGames);
    public int GetPointsForScorebard(Models.Leaguepedia.PlayerScoreboard scoreboard);
}