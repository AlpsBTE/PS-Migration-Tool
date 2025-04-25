using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PS_Migration_Tool.Model.DatabaseV1;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PS_Migration_Tool.Models;

public partial class PlotSystemContext : DbContext
{
    public PlotSystemContext()
    {
    }

    public PlotSystemContext(DbContextOptions<PlotSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PlotsystemApiKey> PlotsystemApiKeys { get; set; }

    public virtual DbSet<PlotsystemBuilder> PlotsystemBuilders { get; set; }

    public virtual DbSet<PlotsystemBuilderIsReviewer> PlotsystemBuilderIsReviewers { get; set; }

    public virtual DbSet<PlotsystemBuildteam> PlotsystemBuildteams { get; set; }

    public virtual DbSet<PlotsystemBuildteamHasCountry> PlotsystemBuildteamHasCountries { get; set; }

    public virtual DbSet<PlotsystemCityProject> PlotsystemCityProjects { get; set; }

    public virtual DbSet<PlotsystemCountry> PlotsystemCountries { get; set; }

    public virtual DbSet<PlotsystemDifficulty> PlotsystemDifficulties { get; set; }

    public virtual DbSet<PlotsystemFtpConfiguration> PlotsystemFtpConfigurations { get; set; }

    public virtual DbSet<PlotsystemPayout> PlotsystemPayouts { get; set; }

    public virtual DbSet<PlotsystemPlot> PlotsystemPlots { get; set; }

    public virtual DbSet<PlotsystemPlotsTutorial> PlotsystemPlotsTutorials { get; set; }

    public virtual DbSet<PlotsystemReview> PlotsystemReviews { get; set; }

    public virtual DbSet<PlotsystemServer> PlotsystemServers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<PlotsystemApiKey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()");
        });

        modelBuilder.Entity<PlotsystemBuilder>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.Property(e => e.SettingPlotType).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<PlotsystemBuilderIsReviewer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.BuilderUu).WithMany(p => p.PlotsystemBuilderIsReviewers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_136");

            entity.HasOne(d => d.Buildteam).WithMany(p => p.PlotsystemBuilderIsReviewers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_139");
        });

        modelBuilder.Entity<PlotsystemBuildteam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.ApiKey).WithMany(p => p.PlotsystemBuildteams).HasConstraintName("FK_plotsystem_buildteams_plotsystem_api_keys");
        });

        modelBuilder.Entity<PlotsystemBuildteamHasCountry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Buildteam).WithMany(p => p.PlotsystemBuildteamHasCountries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_113");

            entity.HasOne(d => d.Country).WithMany(p => p.PlotsystemBuildteamHasCountries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_116");
        });

        modelBuilder.Entity<PlotsystemCityProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Visible).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.Country).WithMany(p => p.PlotsystemCityProjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_country_id");
        });

        modelBuilder.Entity<PlotsystemCountry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Server).WithMany(p => p.PlotsystemCountries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_37");
        });

        modelBuilder.Entity<PlotsystemDifficulty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Multiplier).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<PlotsystemFtpConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.IsSftp).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<PlotsystemPayout>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Position).HasComment("position on the leaderboard for this timeframe");
        });

        modelBuilder.Entity<PlotsystemPlot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Status).HasDefaultValueSql("'unclaimed'");
            entity.Property(e => e.Type).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.CityProject).WithMany(p => p.PlotsystemPlots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_56");

            entity.HasOne(d => d.Difficulty).WithMany(p => p.PlotsystemPlots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_81");

            entity.HasOne(d => d.OwnerUu).WithMany(p => p.PlotsystemPlots).HasConstraintName("FK_59");

            entity.HasOne(d => d.Review).WithMany(p => p.PlotsystemPlots).HasConstraintName("FK_69");
        });

        modelBuilder.Entity<PlotsystemPlotsTutorial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("current_timestamp()");

            entity.HasOne(d => d.PlayerUu).WithMany(p => p.PlotsystemPlotsTutorials)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_12");
        });

        modelBuilder.Entity<PlotsystemReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.ReviewDate).HasDefaultValueSql("current_timestamp()");

            entity.HasOne(d => d.ReviewerUu).WithMany(p => p.PlotsystemReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_reviewer_uuid");
        });

        modelBuilder.Entity<PlotsystemServer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.FtpConfiguration).WithMany(p => p.PlotsystemServers).HasConstraintName("FK_29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
