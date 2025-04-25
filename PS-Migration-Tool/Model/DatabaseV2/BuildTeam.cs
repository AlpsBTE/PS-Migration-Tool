using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[Table("build_team")]
[Index("ApiKey", Name = "api_key", IsUnique = true)]
public partial class BuildTeam
{
    [Key]
    [Column("build_team_id", TypeName = "int(11)")]
    public int BuildTeamId { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("api_key")]
    public string? ApiKey { get; set; }

    [Column("api_create_date", TypeName = "datetime")]
    public DateTime? ApiCreateDate { get; set; }

    [InverseProperty("BuildTeam")]
    public virtual ICollection<CityProject> CityProjects { get; set; } = new List<CityProject>();

    [InverseProperty("BuildTeam")]
    public virtual ICollection<Server> Servers { get; set; } = new List<Server>();

    [ForeignKey("BuildTeamId")]
    [InverseProperty("BuildTeams")]
    public virtual ICollection<ReviewToggleCriterion> CriteriaNames { get; set; } = new List<ReviewToggleCriterion>();

    [ForeignKey("BuildTeamId")]
    [InverseProperty("BuildTeams")]
    public virtual ICollection<Builder> Uus { get; set; } = new List<Builder>();
}
