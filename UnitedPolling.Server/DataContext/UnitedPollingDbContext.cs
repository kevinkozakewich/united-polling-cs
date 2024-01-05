using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using UnitedPolling.DataContext.Models;

namespace UnitedPolling.DataContext
{
    public partial class UnitedPollingDbContext : DbContext
    {
        public UnitedPollingDbContext(
            DbContextOptions options) : base(options)
        {
            if (!Database.IsInMemory())
            { this.ChangeTracker.LazyLoadingEnabled = false; }
        }

        public DbSet<Poll> Poll { get; set; }
        public DbSet<PollAdministrator> PollAdministrators { get; set; }
        public DbSet<PollParticipant> PollParticipants { get; set; }
        public DbSet<PollQuestion> PollQuestion { get; set; }
        public DbSet<PollQuestionOption> PollQuestionOption { get; set; }
        public DbSet<PollQuestionResponse> PollQuestionResponse { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Poll>()
            .HasOne(b => b.CreatedUser)
            .WithMany(b => b.CreatedUsers)
            .HasForeignKey(b => b.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Poll>()
            .HasOne(b => b.UpdatedUser)
            .WithMany(b => b.UpdatedUsers)
            .HasForeignKey(b => b.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
