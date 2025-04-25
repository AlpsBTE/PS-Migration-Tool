using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_reviews")]
[Index("ReviewerUuid", Name = "fk_reviewer_uuid")]
public partial class PlotsystemReview
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("reviewer_uuid")]
    [StringLength(36)]
    public string ReviewerUuid { get; set; } = null!;

    [Column("rating")]
    [StringLength(45)]
    public string Rating { get; set; } = null!;

    [Column("feedback")]
    [StringLength(420)]
    public string Feedback { get; set; } = null!;

    [Column("review_date", TypeName = "datetime")]
    public DateTime ReviewDate { get; set; }

    [Column("sent", TypeName = "tinyint(4)")]
    public sbyte Sent { get; set; }

    [InverseProperty("Review")]
    public virtual ICollection<PlotsystemPlot> PlotsystemPlots { get; set; } = new List<PlotsystemPlot>();

    [ForeignKey("ReviewerUuid")]
    [InverseProperty("PlotsystemReviews")]
    public virtual PlotsystemBuilder ReviewerUu { get; set; } = null!;
}
