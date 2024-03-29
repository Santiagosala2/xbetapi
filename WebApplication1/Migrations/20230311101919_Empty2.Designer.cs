﻿// <auto-generated />
using System;
using DataStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuthServer.Migrations
{
    [DbContext(typeof(DataStoreContext))]
    [Migration("20230311101919_Empty2")]
    partial class Empty2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Bets.Models.Bet", b =>
                {
                    b.Property<int>("BetID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BetID"));

                    b.Property<string>("Climate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Completition")
                        .HasColumnType("datetime2");

                    b.Property<string>("FriendClimate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FriendID")
                        .HasColumnType("int");

                    b.Property<int?>("JudgeID")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<decimal>("Wager")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("When")
                        .HasColumnType("datetime2");

                    b.Property<int?>("WinnerID")
                        .HasColumnType("int");

                    b.HasKey("BetID");

                    b.HasIndex("FriendID");

                    b.HasIndex("JudgeID");

                    b.HasIndex("UserID");

                    b.HasIndex("WinnerID");

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("Friends.Models.Friendship", b =>
                {
                    b.Property<int>("FriendshipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FriendshipId"));

                    b.Property<int>("FromUserID")
                        .HasColumnType("int");

                    b.Property<bool>("Pending")
                        .HasColumnType("bit");

                    b.Property<int>("ToUserID")
                        .HasColumnType("int");

                    b.HasKey("FriendshipId");

                    b.HasIndex("FromUserID");

                    b.HasIndex("ToUserID");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("Users.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("DateOfBirth")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Wallet.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionID"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentProviderID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SourceUserID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("TargetUserID")
                        .HasColumnType("int");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionID");

                    b.HasIndex("SourceUserID");

                    b.HasIndex("TargetUserID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Bets.Models.Bet", b =>
                {
                    b.HasOne("Users.Models.User", "Friend")
                        .WithMany("FriendBets")
                        .HasForeignKey("FriendID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Users.Models.User", "Judge")
                        .WithMany("JudgeBets")
                        .HasForeignKey("JudgeID")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Users.Models.User", "User")
                        .WithMany("UserBets")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Users.Models.User", "Winner")
                        .WithMany("WinnerBets")
                        .HasForeignKey("WinnerID")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Friend");

                    b.Navigation("Judge");

                    b.Navigation("User");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("Friends.Models.Friendship", b =>
                {
                    b.HasOne("Users.Models.User", "FromUser")
                        .WithMany("FromUserFriendships")
                        .HasForeignKey("FromUserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Users.Models.User", "ToUser")
                        .WithMany("ToUserFriendships")
                        .HasForeignKey("ToUserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("Wallet.Models.Transaction", b =>
                {
                    b.HasOne("Users.Models.User", "SourceUser")
                        .WithMany("SystemTransactions")
                        .HasForeignKey("SourceUserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Users.Models.User", "TargetUser")
                        .WithMany("UserTransactions")
                        .HasForeignKey("TargetUserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("SourceUser");

                    b.Navigation("TargetUser");
                });

            modelBuilder.Entity("Users.Models.User", b =>
                {
                    b.Navigation("FriendBets");

                    b.Navigation("FromUserFriendships");

                    b.Navigation("JudgeBets");

                    b.Navigation("SystemTransactions");

                    b.Navigation("ToUserFriendships");

                    b.Navigation("UserBets");

                    b.Navigation("UserTransactions");

                    b.Navigation("WinnerBets");
                });
#pragma warning restore 612, 618
        }
    }
}
