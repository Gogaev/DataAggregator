using System;
using System.Collections.Generic;
using DataAggregator.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAggregator.Infrastructure.Db;

public partial class AggregatorDbContext : DbContext
{
    public AggregatorDbContext(DbContextOptions<AggregatorDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer_101> Customer_101s { get; set; }

    public virtual DbSet<Customer_145> Customer_145s { get; set; }

    public virtual DbSet<Customer_2> Customer_2s { get; set; }

    public virtual DbSet<EventTypes_2> EventTypes_2s { get; set; }

    public virtual DbSet<Events_101> Events_101s { get; set; }

    public virtual DbSet<Events_145> Events_145s { get; set; }

    public virtual DbSet<Events_2> Events_2s { get; set; }

    public virtual DbSet<NotificationsBroker> NotificationsBrokers { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer_101>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC071A490EE0");

            entity.ToTable("Customer_101");

            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(128);
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(128);
            entity.Property(e => e.Salutation).HasMaxLength(10);
        });

        modelBuilder.Entity<Customer_145>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Customer_145");

            entity.Property(e => e.Email).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Password).HasMaxLength(128);
            entity.Property(e => e.UserId).HasMaxLength(128);
        });

        modelBuilder.Entity<Customer_2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07B80FB17A");

            entity.ToTable("Customer_2");

            entity.Property(e => e.Email).HasMaxLength(128);
            entity.Property(e => e.JobPosition).HasMaxLength(128);
            entity.Property(e => e.PasswordHash).HasMaxLength(128);
        });

        modelBuilder.Entity<EventTypes_2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventTyp__3214EC071059562C");

            entity.ToTable("EventTypes_2");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(64);
        });

        modelBuilder.Entity<Events_101>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events_1__3214EC0716F87422");

            entity.ToTable("Events_101");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.EventDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Events_145>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events_1__3214EC071EE5CAFC");

            entity.ToTable("Events_145");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CustomerId).HasMaxLength(128);
            entity.Property(e => e.EventDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Events_2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events_2__3214EC074CCF77E5");

            entity.ToTable("Events_2");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.EventDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<NotificationsBroker>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("NotificationsBroker");

            entity.Property(e => e.Email).HasMaxLength(128);
            entity.Property(e => e.FinHash).HasMaxLength(128);
            entity.Property(e => e.FirstName).HasMaxLength(128);
            entity.Property(e => e.LastName).HasMaxLength(128);
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tenants__3214EC07D294EADA");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.OrganisationName).HasMaxLength(128);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
