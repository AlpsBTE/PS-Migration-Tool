using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[PrimaryKey("ReviewId", "CriteriaName")]
[Table("review_contains_toggle_criteria")]
[Index("CriteriaName", Name = "criteria_name")]
public partial class ReviewContainsToggleCriterion
{
    [Key]
    [Column("review_id", TypeName = "int(11)")]
    public int ReviewId { get; set; }

    [Key]
    [Column("criteria_name")]
    public string CriteriaName { get; set; } = null!;

    [Column("is_checked")]
    public bool IsChecked { get; set; }

    [ForeignKey("CriteriaName")]
    [InverseProperty("ReviewContainsToggleCriteria")]
    public virtual ReviewToggleCriterion CriteriaNameNavigation { get; set; } = null!;

    [ForeignKey("ReviewId")]
    [InverseProperty("ReviewContainsToggleCriteria")]
    public virtual PlotReview Review { get; set; } = null!;
}
