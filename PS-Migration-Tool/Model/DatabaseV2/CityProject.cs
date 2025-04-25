using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[Table("city_project")]
[Index("BuildTeamId", Name = "build_team_id")]
[Index("CountryCode", Name = "country_code")]
[Index("ServerName", Name = "server_name")]
public partial class CityProject
{
    [Key]
    [Column("city_project_id")]
    public string CityProjectId { get; set; } = null!;

    [Column("build_team_id", TypeName = "int(11)")]
    public int BuildTeamId { get; set; }

    [Column("country_code")]
    [StringLength(2)]
    public string CountryCode { get; set; } = null!;

    [Column("server_name")]
    public string ServerName { get; set; } = null!;

    [Required]
    [Column("is_visible")]
    public bool? IsVisible { get; set; }

    [ForeignKey("BuildTeamId")]
    [InverseProperty("CityProjects")]
    public virtual BuildTeam BuildTeam { get; set; } = null!;

    [ForeignKey("CountryCode")]
    [InverseProperty("CityProjects")]
    public virtual Country CountryCodeNavigation { get; set; } = null!;

    [InverseProperty("CityProject")]
    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();

    [ForeignKey("ServerName")]
    [InverseProperty("CityProjects")]
    public virtual Server ServerNameNavigation { get; set; } = null!;
}
