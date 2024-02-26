namespace FantasyApp.Models;

// (Get tournament roster)
// https://api.pandascore.co/tournaments/12600/rosters
public class Rosters
{
    public List<Roster_Team> rosters;
    public string type { get; set; }
}


// Child
public class Roster_Team
{
    public string acronym { get; set; }
    public Roster_VideoGame current_videogame { get; set; }
    public int id { get; set; }
    public string image_url { get; set; }
    public string location { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    public List<Roster_Player> players { get; set; }
    public string slug { get; set; }
}

public class Roster_VideoGame
{
    public int id { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
}

public class Roster_Player
{
    public bool active { get; set; }
    public int age { get; set; }
    public string birthday { get; set; }
    public string first_name { get; set; }
    public int id { get; set; }
    public string image_url { get; set; }
    public string last_name { get; set; }
    public DateTime? modified_at { get; set; }
    public string name { get; set; }
    public string nationality { get; set; }
    public string role { get; set; }
    public string slug { get; set; }
}
