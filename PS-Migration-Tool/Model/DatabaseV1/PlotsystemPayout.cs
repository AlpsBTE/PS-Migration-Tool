using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PS_Migration_Tool.Model.DatabaseV1;

[Table("plotsystem_payouts")]
public partial class PlotsystemPayout
{
    [Key]
    [Column("id", TypeName = "int(11)")]
    public int Id { get; set; }

    [Column("timeframe", TypeName = "enum('DAILY','WEEKLY','MONTHLY','YEARLY')")]
    public string Timeframe { get; set; } = null!;

    /// <summary>
    /// position on the leaderboard for this timeframe
    /// </summary>
    [Column("position", TypeName = "int(11)")]
    public int Position { get; set; }

    [Column("payout_amount")]
    [StringLength(100)]
    public string PayoutAmount { get; set; } = null!;
}
