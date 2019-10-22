using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using ChatApp.Models;

namespace ChatApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ChatUser, IdentityRole<int>,int>
    {
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<ChatUser>().HasKey(f => f.Id);
            b.Entity<Message>().HasKey(msg => msg.ID);

            b.Entity<Friendship>().HasOne(f => f.Owner).WithMany().HasForeignKey(f => f.OwnerID).OnDelete(DeleteBehavior.NoAction);
            b.Entity<Friendship>().HasOne(f => f.Friend).WithMany().HasForeignKey(f => f.FriendID);

            base.OnModelCreating(b);
        }
    }
}
