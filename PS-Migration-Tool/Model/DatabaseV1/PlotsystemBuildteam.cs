using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_buildteams")]
[Index("ApiKeyId", Name = "FK_132")]
public partial class PlotsystemBuildteam
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(45)]
    public string Name { get; set; } = null!;

    [Column("api_key_id", TypeName = "int(11)")]
    public int? ApiKeyId { get; set; }

    [ForeignKey("ApiKeyId")]
    [InverseProperty("PlotsystemBuildteams")]
    public virtual PlotsystemApiKey? ApiKey { get; set; }

    [InverseProperty("Buildteam")]
    public virtual ICollection<PlotsystemBuilderIsReviewer> PlotsystemBuilderIsReviewers { get; set; } = new List<PlotsystemBuilderIsReviewer>();

    [InverseProperty("Buildteam")]
    public virtual ICollection<PlotsystemBuildteamHasCountry> PlotsystemBuildteamHasCountries { get; set; } = new List<PlotsystemBuildteamHasCountry>();
}
