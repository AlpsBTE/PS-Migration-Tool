using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PS_Migration_Tool.Model.DatabaseV2;

public partial class PlotSystemV2Context : DbContext
{
    public PlotSystemV2Context()
    {
    }

    public PlotSystemV2Context(DbContextOptions<PlotSystemV2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<BuildTeam> BuildTeams { get; set; }

    public virtual DbSet<Builder> Builders { get; set; }

    public virtual DbSet<CityProject> CityProjects { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Plot> Plots { get; set; }

    public virtual DbSet<PlotDifficulty> PlotDifficulties { get; set; }

    public virtual DbSet<PlotReview> PlotReviews { get; set; }

    public virtual DbSet<ReviewContainsToggleCriterion> ReviewContainsToggleCriteria { get; set; }

    public virtual DbSet<ReviewToggleCriterion> ReviewToggleCriteria { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<SystemInfo> SystemInfos { get; set; }

    public virtual DbSet<Tutorial> Tutorials { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<BuildTeam>(entity =>
        {
            entity.HasKey(e => e.BuildTeamId).HasName("PRIMARY");

            entity.HasMany(d => d.CriteriaNames).WithMany(p => p.BuildTeams)
                .UsingEntity<Dictionary<string, object>>(
                    "BuildTeamUsesToggleCriterion",
                    r => r.HasOne<ReviewToggleCriterion>().WithMany()
                        .HasForeignKey("CriteriaName")
                        .HasConstraintName("build_team_uses_toggle_criteria_ibfk_2"),
                    l => l.HasOne<BuildTeam>().WithMany()
                        .HasForeignKey("BuildTeamId")
                        .HasConstraintName("build_team_uses_toggle_criteria_ibfk_1"),
                    j =>
                    {
                        j.HasKey("BuildTeamId", "CriteriaName")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("build_team_uses_toggle_criteria");
                        j.HasIndex(new[] { "CriteriaName" }, "criteria_name");
                        j.IndexerProperty<int>("BuildTeamId")
                            .HasColumnType("int(11)")
                            .HasColumnName("build_team_id");
                        j.IndexerProperty<string>("CriteriaName").HasColumnName("criteria_name");
                    });

            entity.HasMany(d => d.Uus).WithMany(p => p.BuildTeams)
                .UsingEntity<Dictionary<string, object>>(
                    "BuildTeamHasReviewer",
                    r => r.HasOne<Builder>().WithMany()
                        .HasForeignKey("Uuid")
                        .HasConstraintName("build_team_has_reviewer_ibfk_2"),
                    l => l.HasOne<BuildTeam>().WithMany()
                        .HasForeignKey("BuildTeamId")
                        .HasConstraintName("build_team_has_reviewer_ibfk_1"),
                    j =>
                    {
                        j.HasKey("BuildTeamId", "Uuid")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("build_team_has_reviewer");
                        j.HasIndex(new[] { "Uuid" }, "uuid");
                        j.IndexerProperty<int>("BuildTeamId")
                            .HasColumnType("int(11)")
                            .HasColumnName("build_team_id");
                        j.IndexerProperty<string>("Uuid")
                            .HasMaxLength(36)
                            .HasColumnName("uuid");
                    });
        });

        modelBuilder.Entity<Builder>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");
        });

        modelBuilder.Entity<CityProject>(entity =>
        {
            entity.HasKey(e => e.CityProjectId).HasName("PRIMARY");

            entity.Property(e => e.IsVisible).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.BuildTeam).WithMany(p => p.CityProjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("city_project_ibfk_1");

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.CityProjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("city_project_ibfk_2");

            entity.HasOne(d => d.ServerNameNavigation).WithMany(p => p.CityProjects)
                .HasPrincipalKey(p => p.ServerName)
                .HasForeignKey(d => d.ServerName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("city_project_ibfk_3");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryCode).HasName("PRIMARY");
        });

        modelBuilder.Entity<Plot>(entity =>
        {
            entity.HasKey(e => e.PlotId).HasName("PRIMARY");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("current_timestamp()");
            entity.Property(e => e.Status).HasDefaultValueSql("'unclaimed'");

            entity.HasOne(d => d.CityProject).WithMany(p => p.Plots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plot_ibfk_1");

            entity.HasOne(d => d.Difficulty).WithMany(p => p.Plots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plot_ibfk_2");

            entity.HasOne(d => d.OwnerUu).WithMany(p => p.PlotsNavigation).HasConstraintName("plot_ibfk_3");

            entity.HasMany(d => d.Uus).WithMany(p => p.Plots)
                .UsingEntity<Dictionary<string, object>>(
                    "BuilderIsPlotMember",
                    r => r.HasOne<Builder>().WithMany()
                        .HasForeignKey("Uuid")
                        .HasConstraintName("builder_is_plot_member_ibfk_2"),
                    l => l.HasOne<Plot>().WithMany()
                        .HasForeignKey("PlotId")
                        .HasConstraintName("builder_is_plot_member_ibfk_1"),
                    j =>
                    {
                        j.HasKey("PlotId", "Uuid")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("builder_is_plot_member");
                        j.HasIndex(new[] { "Uuid" }, "uuid");
                        j.IndexerProperty<int>("PlotId")
                            .HasColumnType("int(11)")
                            .HasColumnName("plot_id");
                        j.IndexerProperty<string>("Uuid")
                            .HasMaxLength(36)
                            .HasColumnName("uuid");
                    });
        });

        modelBuilder.Entity<PlotDifficulty>(entity =>
        {
            entity.HasKey(e => e.DifficultyId).HasName("PRIMARY");

            entity.Property(e => e.Multiplier).HasDefaultValueSql("'1.00'");
        });

        modelBuilder.Entity<PlotReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PRIMARY");

            entity.Property(e => e.ReviewDate).HasDefaultValueSql("current_timestamp()");

            entity.HasOne(d => d.Plot).WithMany(p => p.PlotReviews).HasConstraintName("plot_review_ibfk_1");

            entity.HasMany(d => d.Uus).WithMany(p => p.Reviews)
                .UsingEntity<Dictionary<string, object>>(
                    "BuilderHasReviewNotification",
                    r => r.HasOne<Builder>().WithMany()
                        .HasForeignKey("Uuid")
                        .HasConstraintName("builder_has_review_notification_ibfk_2"),
                    l => l.HasOne<PlotReview>().WithMany()
                        .HasForeignKey("ReviewId")
                        .HasConstraintName("builder_has_review_notification_ibfk_1"),
                    j =>
                    {
                        j.HasKey("ReviewId", "Uuid")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("builder_has_review_notification");
                        j.HasIndex(new[] { "Uuid" }, "uuid");
                        j.IndexerProperty<int>("ReviewId")
                            .HasColumnType("int(11)")
                            .HasColumnName("review_id");
                        j.IndexerProperty<string>("Uuid")
                            .HasMaxLength(36)
                            .HasColumnName("uuid");
                    });
        });

        modelBuilder.Entity<ReviewContainsToggleCriterion>(entity =>
        {
            entity.HasKey(e => new { e.ReviewId, e.CriteriaName })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.HasOne(d => d.CriteriaNameNavigation).WithMany(p => p.ReviewContainsToggleCriteria).HasConstraintName("review_contains_toggle_criteria_ibfk_2");

            entity.HasOne(d => d.Review).WithMany(p => p.ReviewContainsToggleCriteria).HasConstraintName("review_contains_toggle_criteria_ibfk_1");
        });

        modelBuilder.Entity<ReviewToggleCriterion>(entity =>
        {
            entity.HasKey(e => e.CriteriaName).HasName("PRIMARY");
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => new { e.BuildTeamId, e.ServerName })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.HasOne(d => d.BuildTeam).WithMany(p => p.Servers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("server_ibfk_1");
        });

        modelBuilder.Entity<SystemInfo>(entity =>
        {
            entity.HasKey(e => e.SystemId).HasName("PRIMARY");

            entity.Property(e => e.LastUpdate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()");
        });

        modelBuilder.Entity<Tutorial>(entity =>
        {
            entity.HasKey(e => new { e.TutorialId, e.Uuid })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.Property(e => e.FirstStageStartDate).HasDefaultValueSql("current_timestamp()");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
