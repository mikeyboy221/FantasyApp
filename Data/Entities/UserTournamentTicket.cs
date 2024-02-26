using System.ComponentModel.DataAnnotations;

namespace FantasyApp.Data.Entities;

public class UserTournamentTicket
{
    [Key]
    public required string Id { get; set; }
    public TicketDraft? TicketDraft { get; set; }

    public required string UserTournamentId { get; set; }
    public UserTournament UserTournament { get; set; }

    public required string UserId { get; set; }
}