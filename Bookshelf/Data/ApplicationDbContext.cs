using System;
using System.Collections.Generic;
using System.Text;
using Bookshelf.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookshelf.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }

        protected override void
        OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 1,
                Name = "Fiction"
            });
            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 2,
                Name = "Non-Fiction"
            });
            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 3,
                Name = "SciFi"
            });
            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 4,
                Name = "Poetry"
            });
            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 5,
                Name = "Horror"
            });
            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 6,
                Name = "Romance"
            });
            modelBuilder.Entity<Genre>().HasData(new Genre()
            {
                Id = 7,
                Name = "Murder Mystery"
            });
        }
    }
}
