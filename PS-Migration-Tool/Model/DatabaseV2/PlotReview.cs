using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[Table("plot_review")]
[Index("PlotId", Name = "plot_id")]
public partial class PlotReview
{
    [Key]
    [Column("review_id", TypeName = "int(11)")]
    public int ReviewId { get; set; }

    [Column("plot_id", TypeName = "int(11)")]
    public int PlotId { get; set; }

    [Column("rating")]
    [StringLength(7)]
    public string Rating { get; set; } = null!;

    [Column("feedback")]
    [StringLength(256)]
    public string? Feedback { get; set; }

    [Column("reviewed_by")]
    [StringLength(36)]
    public string ReviewedBy { get; set; } = null!;

    [Column("review_date", TypeName = "datetime")]
    public DateTime ReviewDate { get; set; }

    [ForeignKey("PlotId")]
    [InverseProperty("PlotReviews")]
    public virtual Plot Plot { get; set; } = null!;

    [ForeignKey("ReviewId")]
    [InverseProperty("Reviews")]
    public virtual ICollection<ReviewToggleCriterion> CriteriaNames { get; set; } = new List<ReviewToggleCriterion>();

    [ForeignKey("ReviewId")]
    [InverseProperty("Reviews")]
    public virtual ICollection<Builder> Uus { get; set; } = new List<Builder>();
}
