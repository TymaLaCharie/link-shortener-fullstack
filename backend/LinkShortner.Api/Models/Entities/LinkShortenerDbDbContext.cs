using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LinkShortner.Api.Models.Entities;

public partial class LinkShortenerDbDbContext : DbContext
{
    public LinkShortenerDbDbContext()
    {
    }

    public LinkShortenerDbDbContext(DbContextOptions<LinkShortenerDbDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Click> Clicks { get; set; }

    public virtual DbSet<Url> Urls { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Click>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clicks__3214EC07A8627530");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ClickedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Url).WithMany(p => p.Clicks)
                .HasForeignKey(d => d.UrlId)
                .HasConstraintName("FK_Clicks_Urls");
        });

        modelBuilder.Entity<Url>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Urls__3214EC076665FE80");

            entity.HasIndex(e => e.ShortCode, "IX_Urls_ShortCode").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ShortCode).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Urls)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Urls_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC074DD3FCF8");

            entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Surname).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
