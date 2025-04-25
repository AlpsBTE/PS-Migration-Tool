using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[PrimaryKey("BuildTeamId", "ServerName")]
[Table("server")]
[Index("ServerName", Name = "server_name", IsUnique = true)]
public partial class Server
{
    [Key]
    [Column("build_team_id", TypeName = "int(11)")]
    public int BuildTeamId { get; set; }

    [Key]
    [Column("server_name")]
    public string ServerName { get; set; } = null!;

    [ForeignKey("BuildTeamId")]
    [InverseProperty("Servers")]
    public virtual BuildTeam BuildTeam { get; set; } = null!;

    [InverseProperty("ServerNameNavigation")]
    public virtual ICollection<CityProject> CityProjects { get; set; } = new List<CityProject>();
}
