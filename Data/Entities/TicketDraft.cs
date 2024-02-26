using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantasyApp.Data.Entities;

public class TicketDraft
{
    [Key]
    public string UserTournamentTicketId { get; set; }
    public UserTournamentTicket UserTournamentTicket { get; set; }

    [Required]
    public required string TopPlayerId { get; set; }

    [Required]
    public required string JglPlayerId { get; set; }

    [Required]
    public required string MidPlayerId { get; set; }

    [Required]
    public required string BotPlayerId { get; set; }

    [Required]
    public required string SupPlayerId { get; set; }
}