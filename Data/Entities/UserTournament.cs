using System.ComponentModel.DataAnnotations;

namespace FantasyApp.Data.Entities;

public class UserTournament
{
    [Key]
    public required string Id { get; set; }
    public ICollection<UserTournamentTicket> UserTournamentTickets { get; set; } = new List<UserTournamentTicket>();

    public required string HostId { get; set; }
    public required string TournamentId { get; set; }

    public required string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Password { get; set; }
    public required int MaxParticipants { get; set; }
}