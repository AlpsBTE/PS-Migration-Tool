using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_api_keys")]
[MySqlCollation("utf8mb4_unicode_ci")]
public partial class PlotsystemApiKey
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("api_key")]
    [StringLength(32)]
    public string ApiKey { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("ApiKey")]
    public virtual ICollection<PlotsystemBuildteam> PlotsystemBuildteams { get; set; } = new List<PlotsystemBuildteam>();
}
