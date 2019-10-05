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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<ChatUser>().HasKey(f => f.Id);

            b.Entity<Friendship>().HasKey(f => new { f.User1ID, f.user2ID });
            b.Entity<Friendship>().HasOne(f => f.User1).WithMany(c => c.Friends).HasForeignKey(f => f.User1ID).OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(b);
        }
    }
}
