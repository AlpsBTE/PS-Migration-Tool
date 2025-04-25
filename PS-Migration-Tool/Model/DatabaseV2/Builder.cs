using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[Table("builder")]
[Index("Name", Name = "name", IsUnique = true)]
public partial class Builder
{
    [Key]
    [Column("uuid")]
    [StringLength(36)]
    public string Uuid { get; set; } = null!;

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("score", TypeName = "int(11)")]
    public int Score { get; set; }

    [Column("first_slot", TypeName = "int(11)")]
    public int? FirstSlot { get; set; }

    [Column("second_slot", TypeName = "int(11)")]
    public int? SecondSlot { get; set; }

    [Column("third_slot", TypeName = "int(11)")]
    public int? ThirdSlot { get; set; }

    [Column("plot_type", TypeName = "int(11)")]
    public int PlotType { get; set; }

    [InverseProperty("OwnerUu")]
    public virtual ICollection<Plot> PlotsNavigation { get; set; } = new List<Plot>();

    [ForeignKey("Uuid")]
    [InverseProperty("Uus")]
    public virtual ICollection<BuildTeam> BuildTeams { get; set; } = new List<BuildTeam>();

    [ForeignKey("Uuid")]
    [InverseProperty("Uus")]
    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();

    [ForeignKey("Uuid")]
    [InverseProperty("Uus")]
    public virtual ICollection<PlotReview> Reviews { get; set; } = new List<PlotReview>();
}
