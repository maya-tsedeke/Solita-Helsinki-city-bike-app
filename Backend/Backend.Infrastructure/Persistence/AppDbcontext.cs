using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Infrastructure.Persistence
{
    public class AppDbcontext : DbContext
    {
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Trip> Trips { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("DefaultConnection");
        //}


        public AppDbcontext(DbContextOptions<AppDbcontext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Journey>()
          .HasKey(j => j.Id);

            modelBuilder.Entity<Station>()
                .HasKey(s => s.ID);

            modelBuilder.Entity<Journey>()
                .HasOne(j => j.DepartureStation)
                .WithMany(s => s.DepartureJourneys)
                .HasForeignKey(j => j.DepartureStationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Journey>()
                .HasOne(j => j.ReturnStation)
                .WithMany(s => s.ReturnJourneys)
                .HasForeignKey(j => j.ReturnStationId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Trip>()
               .HasKey(s => s.Id);
        }
    }
    
}
