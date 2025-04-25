using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_buildteam_has_countries")]
[Index("BuildteamId", Name = "FK_115")]
[Index("CountryId", Name = "FK_118")]
public partial class PlotsystemBuildteamHasCountry
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("country_id", TypeName = "int(11)")]
    public int CountryId { get; set; }

    [Column("buildteam_id", TypeName = "int(11)")]
    public int BuildteamId { get; set; }

    [ForeignKey("BuildteamId")]
    [InverseProperty("PlotsystemBuildteamHasCountries")]
    public virtual PlotsystemBuildteam Buildteam { get; set; } = null!;

    [ForeignKey("CountryId")]
    [InverseProperty("PlotsystemBuildteamHasCountries")]
    public virtual PlotsystemCountry Country { get; set; } = null!;
}
