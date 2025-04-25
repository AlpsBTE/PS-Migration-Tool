using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[Table("country")]
public partial class Country
{
    [Key]
    [Column("country_code")]
    [StringLength(2)]
    public string CountryCode { get; set; } = null!;

    [Column("continent", TypeName = "enum('EU','AS','AF','OC','SA','NA')")]
    public string Continent { get; set; } = null!;

    [Column("material")]
    [StringLength(255)]
    public string Material { get; set; } = null!;

    [Column("custom_model_data")]
    [StringLength(255)]
    public string? CustomModelData { get; set; }

    [InverseProperty("CountryCodeNavigation")]
    public virtual ICollection<CityProject> CityProjects { get; set; } = new List<CityProject>();
}
