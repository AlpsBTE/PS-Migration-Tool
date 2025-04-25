using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PS_Migration_Tool.Model.DatabaseV2;
using PS_Migration_Tool.Models;
using PS_Migration_Tool.Services;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var v1ConnectionString = config.GetConnectionString("V1")!;
var v2ConnectionString = config.GetConnectionString("V2")!;

var v1Context = new PlotSystemContext(new DbContextOptionsBuilder<PlotSystemContext>()
    .UseMySql(v1ConnectionString,
        ServerVersion.AutoDetect(v1ConnectionString),
        optionsBuilder => optionsBuilder.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(3),
            errorNumbersToAdd: null)
    ).Options);

var v2Context = new PlotSystemV2Context(new DbContextOptionsBuilder<PlotSystemV2Context>()
    .UseMySql(v2ConnectionString,
        ServerVersion.AutoDetect(v1ConnectionString),
        optionsBuilder => optionsBuilder.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(3),
            errorNumbersToAdd: null)
    ).Options);

var migrator = new MigrationService(v1Context, v2Context);
await migrator.MigrateAsync();