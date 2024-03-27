using System.Text.Json;

using FantasyApp.Models;

namespace FantasyApp.Services;

public class ApiService : IApiService, IDisposable
{
    private readonly HttpClient client;
    private readonly string pandascoreApiAuthorizationToken = "hlo6gTkk6fdNDHU6qgPVhSvXIQt6PAdyfrrR9QJPjGVOS-ozfs0";

    public ApiService()
    {
        client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {pandascoreApiAuthorizationToken}");
    }

    // USING PANDA SCORE
    public async Task<T?> GetPandaApiResponse<T>(string url)
    {
        Console.WriteLine("API CALL: "+url);

        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content))
            {
                return JsonSerializer.Deserialize<T>(content);
            }
        }
        catch (HttpRequestException exception)
        {
            Console.WriteLine(exception.Message);
        }

        return default;
    }

    public async Task<List<UpcomingMatch>> GetPandaUpcomingMatches(int pageSize, int pageNumber)
    {
        var url = "https://api.pandascore.co/lol/matches/upcoming?page[size]="+pageSize+"&page[number]="+pageNumber;
        return await GetPandaApiResponse<List<UpcomingMatch>>(url) ?? new List<UpcomingMatch>();
    }


    // USING LEAGUEPEDIA API
    public async Task<T?> GetLeaguepediaApiResponse<T>(string url)
    {
        Console.WriteLine("Leaguepedia middleware API CALL: "+url);

        try
        {
            var response = await client.GetAsync("http://127.0.0.1:5000"+url);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content))
            {
                return JsonSerializer.Deserialize<T>(content);
            }
        }
        catch (HttpRequestException exception)
        {
            Console.WriteLine(exception.Message);
        }

        return default;
    }

    public async Task<List<Models.Leaguepedia.Tournament>> GetLeaguepediaUpcomingTournaments(DateOnly dateStart)
    {
        var startAt = dateStart.ToString("yyyy-MM-dd");
        var endAt = dateStart.AddYears(1).ToString("yyyy-MM-dd");

        var url = "/GetUpcomingTournaments?dateStart="+startAt+"&dateEnd="+endAt;
        return await GetLeaguepediaApiResponse<List<Models.Leaguepedia.Tournament>>(url) ?? new List<Models.Leaguepedia.Tournament>();
    }

    public async Task<Models.Leaguepedia.Tournament> GetLeaguepediaTournament(string tournamentId)
    {
        var url = "/Tournament?tournamentId="+tournamentId;
        return await GetLeaguepediaApiResponse<Models.Leaguepedia.Tournament>(url) ?? new Models.Leaguepedia.Tournament();
    }

    public async Task<List<Models.Leaguepedia.TournamentRoster_Player>> GetLeaguepediaTournamentRoster(string tournamentId)
    {
        var url = "/TournamentRoster?tournamentId="+tournamentId;
        var roster = await GetLeaguepediaApiResponse<List<Models.Leaguepedia.TournamentRoster_Player>>(url) ?? new List<Models.Leaguepedia.TournamentRoster_Player>();
        var noCoaches = roster.Where(entity => !entity.Role.Contains("Coach")).ToList();

        return noCoaches;
    }

    public async Task<List<Models.Leaguepedia.TournamentMatch>> GetMatchScheduleForTournament(string tournamentId)
    {
        var url = "/TournamentMatches?tournamentId="+tournamentId;
        return await GetLeaguepediaApiResponse<List<Models.Leaguepedia.TournamentMatch>>(url) ?? new List<Models.Leaguepedia.TournamentMatch>();

        // var response = await client.GetAsync("http://127.0.0.1:5000"+url);
        // response.EnsureSuccessStatusCode();

        // string content = await response.Content.ReadAsStringAsync();
        // if (!string.IsNullOrEmpty(content))
        // {
        //     return JsonConvert.DeserializeObject<List<Models.Leaguepedia.TournamentMatch>>(content, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss"});
        // }

        // return new List<Models.Leaguepedia.TournamentMatch>();
    }

    public async Task<List<Models.Leaguepedia.MatchGame>> GetMatchGames(string matchId)
    {
        var url = "/MatchGames?matchId="+matchId;
        return await GetLeaguepediaApiResponse<List<Models.Leaguepedia.MatchGame>>(url) ?? new List<Models.Leaguepedia.MatchGame>();
    }

    public async Task<Models.Leaguepedia.PlayerStats> GetPlayerStats(string playerName, string tournamentId)
    {
        Console.WriteLine("API CALL: "+playerName+" "+tournamentId);

        var url = "/PlayerStats?playerName="+playerName+"&tournamentId="+tournamentId;
        return await GetLeaguepediaApiResponse<Models.Leaguepedia.PlayerStats>(url) ?? new Models.Leaguepedia.PlayerStats();
    }

    public void Dispose()
    {
        client.Dispose();
    }
}

public interface IApiService
{
    public Task<List<UpcomingMatch>> GetPandaUpcomingMatches(int pageSize, int pageNumber);

    // Tournaments
    public Task<List<Models.Leaguepedia.Tournament>> GetLeaguepediaUpcomingTournaments(DateOnly dateStart);
    public Task<Models.Leaguepedia.Tournament> GetLeaguepediaTournament(string tournamentId);
    public Task<List<Models.Leaguepedia.TournamentRoster_Player>> GetLeaguepediaTournamentRoster(string tournamentId);

    // Matches
    public Task<List<Models.Leaguepedia.TournamentMatch>> GetMatchScheduleForTournament(string tournamentId);
    public Task<List<Models.Leaguepedia.MatchGame>> GetMatchGames(string matchId);

    // Players
    public Task<Models.Leaguepedia.PlayerStats> GetPlayerStats(string playerName, string tournamentId);
}