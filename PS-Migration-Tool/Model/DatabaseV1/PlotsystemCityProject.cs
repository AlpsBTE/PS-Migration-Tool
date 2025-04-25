using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_city_projects")]
[Index("CountryId", Name = "fk_country_id")]
public partial class PlotsystemCityProject
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("country_id", TypeName = "int(11)")]
    public int CountryId { get; set; }

    [Column("name")]
    [StringLength(45)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    public string Description { get; set; } = null!;

    [Column("visible", TypeName = "tinyint(4)")]
    public sbyte Visible { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("PlotsystemCityProjects")]
    public virtual PlotsystemCountry Country { get; set; } = null!;

    [InverseProperty("CityProject")]
    public virtual ICollection<PlotsystemPlot> PlotsystemPlots { get; set; } = new List<PlotsystemPlot>();
}
