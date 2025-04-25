using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_builder_is_reviewer")]
[Index("BuilderUuid", Name = "FK_138")]
[Index("BuildteamId", Name = "FK_141")]
public partial class PlotsystemBuilderIsReviewer
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("builder_uuid")]
    [StringLength(36)]
    public string BuilderUuid { get; set; } = null!;

    [Column("buildteam_id", TypeName = "int(11)")]
    public int BuildteamId { get; set; }

    [ForeignKey("BuilderUuid")]
    [InverseProperty("PlotsystemBuilderIsReviewers")]
    public virtual PlotsystemBuilder BuilderUu { get; set; } = null!;

    [ForeignKey("BuildteamId")]
    [InverseProperty("PlotsystemBuilderIsReviewers")]
    public virtual PlotsystemBuildteam Buildteam { get; set; } = null!;
}
