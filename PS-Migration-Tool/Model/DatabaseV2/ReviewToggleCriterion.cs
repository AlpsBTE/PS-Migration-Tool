﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[Table("review_toggle_criteria")]
public partial class ReviewToggleCriterion
{
    [Key]
    [Column("criteria_name")]
    public string CriteriaName { get; set; } = null!;

    [Column("is_optional")]
    public bool IsOptional { get; set; }

    [InverseProperty("CriteriaNameNavigation")]
    public virtual ICollection<ReviewContainsToggleCriterion> ReviewContainsToggleCriteria { get; set; } = new List<ReviewContainsToggleCriterion>();

    [ForeignKey("CriteriaName")]
    [InverseProperty("CriteriaNames")]
    public virtual ICollection<BuildTeam> BuildTeams { get; set; } = new List<BuildTeam>();
}
