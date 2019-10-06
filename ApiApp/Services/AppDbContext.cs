using EventManager.ApiApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.ApiApp.Services {
    public class AppDbContext : DbContext {
        public DbSet<Event> Events { get; set; }
        public DbSet<Guest> Guests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Event>(x => x.ToTable("Event"));
        }
    }
}
