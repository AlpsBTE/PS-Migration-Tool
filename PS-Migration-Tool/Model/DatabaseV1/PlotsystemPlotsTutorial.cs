using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_plots_tutorial")]
[Index("PlayerUuid", Name = "FK_142")]
public partial class PlotsystemPlotsTutorial
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("player_uuid")]
    [StringLength(36)]
    public string PlayerUuid { get; set; } = null!;

    [Column("tutorial_id", TypeName = "int(11)")]
    public int TutorialId { get; set; }

    [Column("stage_id", TypeName = "int(11)")]
    public int StageId { get; set; }

    [Column("is_completed", TypeName = "tinyint(4)")]
    public sbyte IsCompleted { get; set; }

    [Column("create_date", TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column("last_stage_complete_date", TypeName = "datetime")]
    public DateTime? LastStageCompleteDate { get; set; }

    [Column("complete_date", TypeName = "datetime")]
    public DateTime? CompleteDate { get; set; }

    [ForeignKey("PlayerUuid")]
    [InverseProperty("PlotsystemPlotsTutorials")]
    public virtual PlotsystemBuilder PlayerUu { get; set; } = null!;
}
