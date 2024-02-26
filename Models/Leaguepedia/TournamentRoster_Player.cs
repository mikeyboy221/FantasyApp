using System.Collections.Generic;
using System.Linq;

namespace FantasyApp.Models.Leaguepedia;

public class TournamentRoster_Player
{
    public string ID { get; set; }

    public string Name  { get; set; }
    public string Player { get; set; }

    public string Team { get; set; }

    // Handles the case where player has multiple roles
    public string Role { get; set; }
    public List<string> Roles  {
        get => Role.Split(',').Select(r => r.Trim()).ToList();
    }

    public string N_PlayerInTeam { get; set; }

}