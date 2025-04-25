using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_builders")]
[MySqlCollation("utf8mb4_unicode_ci")]
public partial class PlotsystemBuilder
{
    [Key]
    [Column("uuid")]
    [StringLength(36)]
    [MySqlCollation("utf8mb4_general_ci")]
    public string Uuid { get; set; } = null!;

    [Column("name")]
    [StringLength(16)]
    public string Name { get; set; } = null!;

    [Column("score", TypeName = "int(11)")]
    public int Score { get; set; }

    [Column("completed_plots", TypeName = "int(11)")]
    public int CompletedPlots { get; set; }

    [Column("first_slot", TypeName = "int(11)")]
    public int? FirstSlot { get; set; }

    [Column("second_slot", TypeName = "int(11)")]
    public int? SecondSlot { get; set; }

    [Column("third_slot", TypeName = "int(11)")]
    public int? ThirdSlot { get; set; }

    [Column("lang")]
    [StringLength(5)]
    public string? Lang { get; set; }

    [Column("setting_plot_type", TypeName = "int(11)")]
    public int? SettingPlotType { get; set; }

    [InverseProperty("BuilderUu")]
    public virtual ICollection<PlotsystemBuilderIsReviewer> PlotsystemBuilderIsReviewers { get; set; } = new List<PlotsystemBuilderIsReviewer>();

    [InverseProperty("OwnerUu")]
    public virtual ICollection<PlotsystemPlot> PlotsystemPlots { get; set; } = new List<PlotsystemPlot>();

    [InverseProperty("PlayerUu")]
    public virtual ICollection<PlotsystemPlotsTutorial> PlotsystemPlotsTutorials { get; set; } = new List<PlotsystemPlotsTutorial>();

    [InverseProperty("ReviewerUu")]
    public virtual ICollection<PlotsystemReview> PlotsystemReviews { get; set; } = new List<PlotsystemReview>();
}
