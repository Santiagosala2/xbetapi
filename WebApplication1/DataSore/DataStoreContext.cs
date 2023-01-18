global using Microsoft.EntityFrameworkCore;
using Wallet.Models;
using Users.Models;
using Friends.Models;

namespace DataStore
{
    public class DataStoreContext : DbContext
    {
        public DataStoreContext(DbContextOptions<DataStoreContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                   .HasOne(t => t.SourceUser)
                   .WithMany(u => u.SystemTransactions)
                   .HasForeignKey(t => t.SourceUserID)
                   .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Transaction>()
                   .HasOne(t => t.TargetUser)
                   .WithMany(u => u.UserTransactions)
                   .HasForeignKey(t => t.TargetUserID)
                   .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                   .HasOne(f => f.FromUser)
                   .WithMany(u => u.FromUserFriendships)
                   .HasForeignKey(t => t.FromUserID)
                   .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                   .HasOne(f => f.ToUser)
                   .WithMany(u => u.ToUserFriendships)
                   .HasForeignKey(t => t.ToUserID)
                   .OnDelete(DeleteBehavior.NoAction);

        }

            public DbSet<User> Users { get; set; }
            public DbSet<Transaction> Transactions { get; set; }
            public DbSet<Friendship> Friendships { get; set; }
    }
        
}