using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[Table("plot")]
[Index("CityProjectId", Name = "city_project_id")]
[Index("DifficultyId", Name = "difficulty_id")]
[Index("OwnerUuid", Name = "owner_uuid")]
public partial class Plot
{
    [Key]
    [Column("plot_id", TypeName = "int(11)")]
    public int PlotId { get; set; }

    [Column("city_project_id")]
    public string CityProjectId { get; set; } = null!;

    [Column("difficulty_id")]
    public string DifficultyId { get; set; } = null!;

    [Column("owner_uuid")]
    [StringLength(36)]
    public string? OwnerUuid { get; set; }

    [Column("status", TypeName = "enum('unclaimed','unfinished','unreviewed','completed')")]
    public string Status { get; set; } = null!;

    [Column("outline_bounds", TypeName = "text")]
    public string OutlineBounds { get; set; } = null!;

    [Column("initial_schematic", TypeName = "mediumblob")]
    public byte[] InitialSchematic { get; set; } = null!;

    [Column("complete_schematic", TypeName = "mediumblob")]
    public byte[]? CompleteSchematic { get; set; }

    [Column("last_activity_date", TypeName = "datetime")]
    public DateTime? LastActivityDate { get; set; }

    [Column("is_pasted")]
    public bool IsPasted { get; set; }

    [Column("mc_version")]
    [StringLength(8)]
    public string? McVersion { get; set; }

    [Column("plot_version")]
    public double PlotVersion { get; set; }

    [Column("plot_type", TypeName = "int(11)")]
    public int PlotType { get; set; }

    [Column("created_by")]
    [StringLength(36)]
    public string CreatedBy { get; set; } = null!;

    [Column("create_date", TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [ForeignKey("CityProjectId")]
    [InverseProperty("Plots")]
    public virtual CityProject CityProject { get; set; } = null!;

    [ForeignKey("DifficultyId")]
    [InverseProperty("Plots")]
    public virtual PlotDifficulty Difficulty { get; set; } = null!;

    [ForeignKey("OwnerUuid")]
    [InverseProperty("PlotsNavigation")]
    public virtual Builder? OwnerUu { get; set; }

    [InverseProperty("Plot")]
    public virtual ICollection<PlotReview> PlotReviews { get; set; } = new List<PlotReview>();

    [ForeignKey("PlotId")]
    [InverseProperty("Plots")]
    public virtual ICollection<Builder> Uus { get; set; } = new List<Builder>();
}
