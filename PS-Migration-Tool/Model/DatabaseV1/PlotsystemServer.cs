using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_servers")]
[Index("FtpConfigurationId", Name = "fkIdx_30")]
public partial class PlotsystemServer
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("ftp_configuration_id", TypeName = "int(11)")]
    public int? FtpConfigurationId { get; set; }

    [Column("name")]
    [StringLength(45)]
    public string Name { get; set; } = null!;

    [ForeignKey("FtpConfigurationId")]
    [InverseProperty("PlotsystemServers")]
    public virtual PlotsystemFtpConfiguration? FtpConfiguration { get; set; }

    [InverseProperty("Server")]
    public virtual ICollection<PlotsystemCountry> PlotsystemCountries { get; set; } = new List<PlotsystemCountry>();
}
