using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using FantasyApp.Data.Entities;
using SQLitePCL;
using Microsoft.AspNetCore.Identity;


namespace FantasyApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserTournament> UserTournament { get; set; } = default!;
    public DbSet<UserTournamentTicket> UserTournamentTicket { get; set; } = default!;
    public DbSet<TicketDraft> TicketDraft { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Include default Identity configurations

        modelBuilder.Entity<UserTournamentTicket>()
            .HasOne(e => e.TicketDraft)
            .WithOne(e => e.UserTournamentTicket)
            .HasForeignKey<TicketDraft>(e => e.UserTournamentTicketId);

        modelBuilder.Entity<UserTournament>()
            .HasMany(e => e.UserTournamentTickets)
            .WithOne(e => e.UserTournament)
            .HasForeignKey(e => e.UserTournamentId)
            .IsRequired();
    }
}
