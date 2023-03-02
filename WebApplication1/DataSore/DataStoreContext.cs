global using Microsoft.EntityFrameworkCore;
using Wallet.Models;
using Users.Models;
using Friends.Models;
using Bets.Models;

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

            modelBuilder.Entity<Bet>()
                   .HasOne(b => b.User)
                   .WithMany(u => u.UserBets)
                   .HasForeignKey(t => t.UserID)
                   .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bet>()
                   .HasOne(b => b.Friend)
                   .WithMany(u => u.FriendBets)
                   .HasForeignKey(t => t.FriendID)
                   .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bet>()
                  .HasOne(b => b.Judge)
                  .WithMany(u => u.JudgeBets)
                  .HasForeignKey(t => t.JudgeID)
                  .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bet>()
                  .HasOne(b => b.Winner)
                  .WithMany(u => u.WinnerBets)
                  .HasForeignKey(t => t.WinnerID)
                  .OnDelete(DeleteBehavior.NoAction);
        }

            public DbSet<User> Users { get; set; }
            public DbSet<Transaction> Transactions { get; set; }
            public DbSet<Friendship> Friendships { get; set; }
            public DbSet<Bet> Bets { get; set; }
    }
        
}