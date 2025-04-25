using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[PrimaryKey("TutorialId", "Uuid")]
[Table("tutorial")]
public partial class Tutorial
{
    [Key]
    [Column("tutorial_id", TypeName = "int(11)")]
    public int TutorialId { get; set; }

    [Key]
    [Column("uuid")]
    [StringLength(36)]
    public string Uuid { get; set; } = null!;

    [Column("stage_id", TypeName = "int(11)")]
    public int StageId { get; set; }

    [Column("is_complete")]
    public bool IsComplete { get; set; }

    [Column("first_stage_start_date", TypeName = "datetime")]
    public DateTime FirstStageStartDate { get; set; }

    [Column("last_stage_complete_date", TypeName = "datetime")]
    public DateTime? LastStageCompleteDate { get; set; }
}
