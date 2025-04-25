using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV2;

[Table("system_info")]
public partial class SystemInfo
{
    [Key]
    [Column("system_id", TypeName = "int(11)")]
    public int SystemId { get; set; }

    [Column("db_version")]
    public double DbVersion { get; set; }

    [Column("current_plot_version")]
    public double CurrentPlotVersion { get; set; }

    [Column("last_update", TypeName = "timestamp")]
    public DateTime LastUpdate { get; set; }

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }
}
