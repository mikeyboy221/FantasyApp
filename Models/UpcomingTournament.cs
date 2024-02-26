namespace FantasyApp.Models;

// (Get upcoming tournaments)
// https://api.pandascore.co/lol/tournaments/upcoming
public class UpcomingTournament
{
    public DateTime? begin_at { get; set; }
    public bool detailed_stats { get; set; }
    public DateTime? end_at { get; set; }
    public bool has_bracket { get; set; }
    public int id;
    public UpcomingTournament_League league;
    public int league_id;
    public bool live_supported;
    public List<UpcomingTournament_Match> matches { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    // public ?unsure of type? prizepool { get; set; }
    public UpcomingTournament_Serie serie { get; set; }
    public int serie_id { get; set; }
    public string slug { get; set; }
    public List<UpcomingTournament_Team> teams { get; set; }
    public string tier { get; set; }
    public UpcomingTournament_VideoGame videogame { get; set; }
    public string? videogame_title { get; set; }
    public int? winner_id { get; set; }
    public string? winner_type { get; set; }
}


// Child
public class UpcomingTournament_League
{
    public int id { get; set; }
    public string image_url { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
    public string url { get; set; }
}

public class UpcomingTournament_Stream
{
    public string embed_url { get; set; }
    public string language { get; set; }
    public bool main { get; set; }
    public bool official { get; set; }
    public string raw_url { get; set; }
}

public class UpcomingTournament_Live
{
    public DateTime? opens_at { get; set; }
    public bool supported { get; set; }
    public string url { get; set; }
}

public class UpcomingTournament_Match
{
    public DateTime? begin_at { get; set; }
    public bool detailed_stats { get; set; }
    public bool draw { get; set; }
    public DateTime? end_at { get; set; }
    public bool forfeit { get; set; }
    // public ?unsure of type? game_advantage { get; set; }
    public int id { get; set; }
    public UpcomingTournament_Live live { get; set; }
    public string match_type { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    public int number_of_games { get; set; }
    public DateTime? original_scheduled_at { get; set; }
    public bool rescheduled { get; set; }
    public DateTime? scheduled_at { get; set; }
    public string slug { get; set; }
    public string status { get; set; }
    public List<UpcomingTournament_Stream> streams_list { get; set; }
    public int tournament_id { get; set; }
    public int? winner_id { get; set; }
    public string winner_type { get; set; }
}

public class UpcomingTournament_Serie
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

public class UpcomingTournament_Team
{
    public string acronym { get; set; }
    public int id { get; set; }
    public string image_url { get; set; }
    public string location { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
}

public class UpcomingTournament_VideoGame
{
    public int id { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
}
