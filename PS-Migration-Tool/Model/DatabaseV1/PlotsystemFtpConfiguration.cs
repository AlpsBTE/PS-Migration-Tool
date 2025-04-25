using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_ftp_configurations")]
public partial class PlotsystemFtpConfiguration
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("schematics_path")]
    [StringLength(255)]
    public string? SchematicsPath { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string Address { get; set; } = null!;

    [Column("port", TypeName = "int(11)")]
    public int Port { get; set; }

    [Column("isSFTP", TypeName = "tinyint(4)")]
    public sbyte IsSftp { get; set; }

    [Column("username")]
    [StringLength(255)]
    public string Username { get; set; } = null!;

    [Column("password")]
    [StringLength(255)]
    public string Password { get; set; } = null!;

    [InverseProperty("FtpConfiguration")]
    public virtual ICollection<PlotsystemServer> PlotsystemServers { get; set; } = new List<PlotsystemServer>();
}
