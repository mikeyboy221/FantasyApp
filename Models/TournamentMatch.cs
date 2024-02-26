using FantasyApp.Models;

// (Get tournament matches)
// https://api.pandascore.co/tournaments/{tournamentId}/matches
public class TournamentMatch
{
    public bool rescheduled { get; set; }
    public string status { get; set; }
    public string winner_type { get; set; }
    public List<TournamentMatch_Game> games { get; set; }
    public int id { get; set; }
    public int? number_of_games { get; set; }
    public int league_id { get; set; }
    public string? videogame_title { get; set; }
    public string name { get; set; }
    public string? winner { get; set; }
    public TournamentMatch_League league { get; set; }
    public string match_type { get; set; }
    // public ?unsure of type? game_advantage { get; set; }
    public bool forfeit { get; set; }
    public DateTime? modified_at { get; set; }
    public DateTime? end_at { get; set; }
    public TournamentMatch_Live live { get; set; }
    public DateTime? original_scheduled_at { get; set; }
    public TournamentMatch_Serie serie { get; set; }
    // public ?unsure of type? videogame_version { get; set; }
    public List<TournamentMatch_Opponents> opponents { get; set; }
    public int? winner_id { get; set; }
    public TournamentMatch_Tournament tournament { get; set; }
    public DateTime? scheduled_at { get; set; }
    public TournamentMatch_VideoGame videogame { get; set; }
    public bool detailed_stats { get; set; }
    public DateTime? begin_at { get; set; }
    public bool draw { get; set; }
    public int serie_id { get; set; }
    public string slug { get; set; }
    public List<TournamentMatch_Stream> streams { get; set; }
    public int tournament_id { get; set; }
    public List<TournamentMatch_Result> results { get; set; }
}

// Child
public class TournamentMatch_Tournament
{
    public DateTime? begin_at { get; set; }
    public bool detailed_stats { get; set; }
    public DateTime? end_at { get; set; }
    public bool has_bracket { get; set; }
    public int id { get; set; }
    public int league_id { get; set; }
    public bool live_supported { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    // public ?unsure of type? prizepool { get; set; }
    public int serie_id { get; set; }
    public string slug { get; set; }
    public string tier { get; set; }
    public int? winner_id { get; set; }
    public string winner_type { get; set; }
}

public class TournamentMatch_VideoGame
{
    public int id { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
}

public class TournamentMatch_Game
{
    public DateTime? begin_at { get; set; }
    public bool complete { get; set; }
    public bool detailed_stats { get; set; }
    public DateTime? end_at { get; set; }
    public bool finished { get; set; }
    public bool forfeit { get; set; }
    public int id { get; set; }
    public int? length { get; set; }
    public int match_id { get; set; }
    public int position { get; set; }
    public string status { get; set; }
    public TournamentMatch_GameWinner winner { get; set; }
    public string winner_type { get; set; }
}

public class TournamentMatch_GameWinner
{
    public int? id { get; set; }
    public string type { get; set; }
}

public class TournamentMatch_League
{
    public int id { get; set; }
    public string image_url { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
    public string url { get; set; }
}

public class TournamentMatch_Live
{
    public DateTime? opens_at { get; set; }
    public bool supported { get; set; }
    public string url { get; set; }
}

public class TournamentMatch_Opponents
{
    public TournamentMatch_Opponent opponent { get; set; }
    public string type { get; set; }
}

public class TournamentMatch_Opponent
{
    public string acronym { get; set; }
    public int id { get; set; }
    public string image_url { get; set; }
    public string location { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
}

public class TournamentMatch_Stream
{
    public string embed_url { get; set; }
    public string language { get; set; }
    public bool main { get; set; }
    public bool official { get; set; }
    public string raw_url { get; set; }
}

public class TournamentMatch_Result
{
    public int? score { get; set; }
    public string type { get; set; }
}

public class TournamentMatch_Serie
{
    public DateTime? begin_at { get; set; }
    public DateTime? end_at { get; set; }
    public string full_name { get; set; }
    public int id { get; set; }
    public int league_id { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    public string season { get; set; }
    public string slug { get; set; }
    public int? winner_id { get; set; }
    public string winner_type { get; set; }
    public int year { get; set; }
}