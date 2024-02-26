namespace FantasyApp.Models;

// (Get upcoming tournaments)
// https://api.pandascore.co/lol/matches/upcoming?page[size]=3&page[number]=1
public class UpcomingMatch
{
    public string status { get; set; }
    public bool rescheduled { get; set; }
    public bool detailed_stats { get; set; }
    public List<UpcomingMatch_Game> games { get; set; }
    public string videogame_title { get; set; }
    public int serie_id { get; set; }
    public int tournament_id { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public DateTime? original_scheduled_at { get; set; }
    public List<UpcomingMatch_Result> results { get; set; }
    public string slug { get; set; }
    public UpcomingMatch_Team winner { get; set; }
    public int number_of_games { get; set; }
    public DateTime? end_at { get; set; }
    public bool forfeit { get; set; }
    public List<UpcomingMatch_Opponent> opponents { get; set; }
    public string winner_type { get; set; }
    public UpcomingMatch_VideoGame videogame { get; set; }
    public DateTime? modified_at { get; set; }
    public UpcomingMatch_League league { get; set; }
    public DateTime? begin_at { get; set; }
    public UpcomingMatch_VideoGameVersion videogame_version { get; set; }
    public DateTime? scheduled_at { get; set; }
    public string match_type { get; set; }
    public UpcomingMatch_Live live { get; set; }
    public UpcomingMatch_Serie serie { get; set; }
    public int? winner_id { get; set; }
    public bool draw { get; set; }
    public UpcomingMatch_Tournament tournament { get; set; }
    // public ?unsure of type? game_advantage { get; set; }
    public List<UpcomingMatch_Stream> streams { get; set; }

}


// Child
public class UpcomingMatch_Winner
{
    public object id;
    public string type;
}

public class UpcomingMatch_Game
    {
        public object begin_at;
        public bool complete;
        public bool detailed_stats;
        public object end_at;
        public bool finished;
        public bool forfeit;
        public int id;
        public object length;
        public int match_id;
        public int position;
        public string status;
        public UpcomingMatch_Winner winner;
        public string winner_type;
    }

public class UpcomingMatch_Result
{
    public int score { get; set; }
    public int team_id { get; set; }
}

public class UpcomingMatch_Team
{
    public string acronym { get; set; }
    public int id { get; set; }
    public string image_url { get; set; }
    public string location { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
}

public class UpcomingMatch_League
{
    public int id { get; set; }
    public string image_url { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
    public string url { get; set; }
}

public class UpcomingMatch_Opponent
{
    public UpcomingMatch_Team opponent { get; set; }
    public string type { get; set; }
}

public class UpcomingMatch_VideoGame
{
    public int id { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
}

public class UpcomingMatch_VideoGameVersion
{
    public bool current { get; set; }
    public string name { get; set; }
}

public class UpcomingMatch_Live
{
    public DateTime? opens_at { get; set; }
    public bool supported { get; set; }
    public string url { get; set; }
}

public class UpcomingMatch_Tournament
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

public class UpcomingMatch_Stream
{
    public string embed_url { get; set; }
    public string language { get; set; }
    public bool main { get; set; }
    public bool official { get; set; }
    public string raw_url { get; set; }
}

public class UpcomingMatch_Serie
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