using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DbContext
{
    public partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : IdentityDbContext<User, IdentityRole<int>, int>(options)
    {
        /// <summary>
        /// Set of all tracked sessions.
        /// </summary>

        public DbSet<Ad> Ads { get; set; } = null!;

        public DbSet<Bid> Bids { get; set; } = null!;

        public DbSet <Images> Images { get; set; } = null!;



        /// <inheritdoc cref="Microsoft.EntityFrameworkCore.DbContext.OnModelCreating"/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Bid>()
        .HasOne(b => b.User)
        .WithMany()
        .HasForeignKey(b => b.UserId)
        .OnDelete(DeleteBehavior.Restrict);

            // ============================================
            // SESSION ENTITY CONFIGURATION
            // ============================================


        }



    }
}
