﻿// <auto-generated />
using System;
using FisTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FisTracker.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.12");

            modelBuilder.Entity("FisTracker.Data.Session", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("sessions");
                });

            modelBuilder.Entity("FisTracker.Data.TimeInput", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("HomeOffice")
                        .HasColumnType("tinyint(1)");

                    b.Property<TimeSpan?>("In")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan?>("LunchIn")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan?>("LunchOut")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan?>("Out")
                        .HasColumnType("time(6)");

                    b.HasKey("Date", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("timeinputs");
                });

            modelBuilder.Entity("FisTracker.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(83)
                        .HasColumnType("varchar(83)");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("FisTracker.Data.Session", b =>
                {
                    b.HasOne("FisTracker.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FisTracker.Data.TimeInput", b =>
                {
                    b.HasOne("FisTracker.Data.User", null)
                        .WithMany("TimeInputs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FisTracker.Data.User", b =>
                {
                    b.Navigation("TimeInputs");
                });
#pragma warning restore 612, 618
        }
    }
}
