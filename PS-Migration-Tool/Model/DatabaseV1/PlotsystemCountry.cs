using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_countries")]
[Index("ServerId", Name = "fkIdx_38")]
public partial class PlotsystemCountry
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("server_id", TypeName = "int(11)")]
    public int ServerId { get; set; }

    [Column("name")]
    [StringLength(45)]
    public string Name { get; set; } = null!;

    [Column("head_id")]
    [StringLength(10)]
    public string? HeadId { get; set; }

    [Column("continent", TypeName = "enum('europe','asia','africa','oceania','south america','north america')")]
    public string Continent { get; set; } = null!;

    [InverseProperty("Country")]
    public virtual ICollection<PlotsystemBuildteamHasCountry> PlotsystemBuildteamHasCountries { get; set; } = new List<PlotsystemBuildteamHasCountry>();

    [InverseProperty("Country")]
    public virtual ICollection<PlotsystemCityProject> PlotsystemCityProjects { get; set; } = new List<PlotsystemCityProject>();

    [ForeignKey("ServerId")]
    [InverseProperty("PlotsystemCountries")]
    public virtual PlotsystemServer Server { get; set; } = null!;
}
