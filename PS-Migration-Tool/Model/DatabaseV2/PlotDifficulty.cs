using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[Table("plot_difficulty")]
public partial class PlotDifficulty
{
    [Key]
    [Column("difficulty_id")]
    public string DifficultyId { get; set; } = null!;

    [Column("multiplier")]
    [Precision(4, 2)]
    public decimal? Multiplier { get; set; }

    [Column("score_requirement", TypeName = "int(11)")]
    public int ScoreRequirement { get; set; }

    [InverseProperty("Difficulty")]
    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();
}
