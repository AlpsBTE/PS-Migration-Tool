using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_plots")]
[Index("CityProjectId", Name = "fkIdx_57")]
[Index("OwnerUuid", Name = "fkIdx_60")]
[Index("ReviewId", Name = "fkIdx_70")]
[Index("DifficultyId", Name = "fkIdx_82")]
public partial class PlotsystemPlot
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("city_project_id", TypeName = "int(11)")]
    public int CityProjectId { get; set; }

    [Column("difficulty_id", TypeName = "int(11)")]
    public int DifficultyId { get; set; }

    [Column("review_id", TypeName = "int(11)")]
    public int? ReviewId { get; set; }

    [Column("owner_uuid")]
    [StringLength(36)]
    public string? OwnerUuid { get; set; }

    [Column("member_uuids")]
    [StringLength(110)]
    public string? MemberUuids { get; set; }

    [Column("status", TypeName = "enum('unclaimed','unfinished','unreviewed','completed')")]
    public string Status { get; set; } = null!;

    [Column("mc_coordinates")]
    [StringLength(255)]
    public string McCoordinates { get; set; } = null!;

    [Column("score", TypeName = "int(11)")]
    public int? Score { get; set; }

    [Column("last_activity", TypeName = "datetime")]
    public DateTime? LastActivity { get; set; }

    [Column("create_date", TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column("create_player")]
    [StringLength(36)]
    public string CreatePlayer { get; set; } = null!;

    [Column("pasted", TypeName = "tinyint(4)")]
    public sbyte Pasted { get; set; }

    [Column("outline")]
    public string? Outline { get; set; }

    [Column("type", TypeName = "int(11)")]
    public int Type { get; set; }

    [Column("version")]
    public double? Version { get; set; }

    [ForeignKey("CityProjectId")]
    [InverseProperty("PlotsystemPlots")]
    public virtual PlotsystemCityProject CityProject { get; set; } = null!;

    [ForeignKey("DifficultyId")]
    [InverseProperty("PlotsystemPlots")]
    public virtual PlotsystemDifficulty Difficulty { get; set; } = null!;

    [ForeignKey("OwnerUuid")]
    [InverseProperty("PlotsystemPlots")]
    public virtual PlotsystemBuilder? OwnerUu { get; set; }

    [ForeignKey("ReviewId")]
    [InverseProperty("PlotsystemPlots")]
    public virtual PlotsystemReview? Review { get; set; }
}
