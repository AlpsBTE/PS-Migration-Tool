using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_difficulties")]
public partial class PlotsystemDifficulty
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(45)]
    public string Name { get; set; } = null!;

    [Column("multiplier")]
    public double Multiplier { get; set; }

    [Column("score_requirment", TypeName = "int(11)")]
    public int ScoreRequirment { get; set; }

    [InverseProperty("Difficulty")]
    public virtual ICollection<PlotsystemPlot> PlotsystemPlots { get; set; } = new List<PlotsystemPlot>();
}
