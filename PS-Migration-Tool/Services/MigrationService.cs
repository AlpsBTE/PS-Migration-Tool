using System.Globalization;
using Microsoft.EntityFrameworkCore;
using PS_Migration_Tool.Helpers;
using PS_Migration_Tool.Model.DatabaseV2;
using PS_Migration_Tool.Models;

namespace PS_Migration_Tool.Services;

internal sealed class MigrationService(PlotSystemContext v1Context, PlotSystemV2Context v2Context)
{
    private readonly Dictionary<int, string> _countryIdConversions = new();
    private readonly Dictionary<int, string> _cityProjectIdConversions = new();
    private readonly Dictionary<int, string> _difficultyConversions = new();

    public async Task MigrateAsync()
    {
        Console.WriteLine("Migrating database...");
        await CreateSystemInfo();
        await CreateDefaultToggleCriteria();

        await MigrateBuildTeams();
        await MigrateServers();
        await MigrateCountries();
        await MigrateCityProjects();
        await MigrateBuilders();
        await MigrateDifficulties();
        await MigratePlots();
        await MigrateTutorials();
        await MigrateReviews();


        await v1Context.SaveChangesAsync();
        Console.WriteLine("Done!");
    }

    private async Task CreateSystemInfo()
    {
        v2Context.SystemInfos.RemoveRange(v2Context.SystemInfos);
        v2Context.SystemInfos.Add(new SystemInfo
        {
            SystemId = 1,
            DbVersion = 2.0,
            CurrentPlotVersion = 4.0,
            Description = "Initial database schema for Plot-System v5.0"
        });
        await v2Context.SaveChangesAsync();
        Console.WriteLine("Created System info...");
    }

    private async Task CreateDefaultToggleCriteria()
    {
        v2Context.ReviewToggleCriteria.RemoveRange(v2Context.ReviewToggleCriteria);
        List<ReviewToggleCriterion> toggleCriteria =
        [
            new()
            {
                CriteriaName = "built_on_outlines",
                IsOptional = false
            },
            new()
            {
                CriteriaName = "correct_height",
                IsOptional = false
            },
            new()
            {
                CriteriaName = "correct_facade_colour",
                IsOptional = false
            },
            new()
            {
                CriteriaName = "correct_roof_colour",
                IsOptional = true
            },
            new()
            {
                CriteriaName = "correct_roof_shape",
                IsOptional = false
            },
            new()
            {
                CriteriaName = "correct_amount_windows_doors",
                IsOptional = false
            },
            new()
            {
                CriteriaName = "correct_window_type",
                IsOptional = true
            },
            new()
            {
                CriteriaName = "windows_blacked_out",
                IsOptional = false
            }
        ];
        v2Context.ReviewToggleCriteria.AddRange(toggleCriteria);
        await v2Context.SaveChangesAsync();
        Console.WriteLine($"Created {toggleCriteria.Count} default toggle criteria...");
    }

    private async Task MigrateBuildTeams()
    {
        v2Context.BuildTeams.RemoveRange(v2Context.BuildTeams.Include(static bt => bt.CriteriaNames));

        var newBuildTeams = new List<BuildTeam>();
        foreach (var bt in v1Context.PlotsystemBuildteams
                     .Include(static plotsystemBuildteam => plotsystemBuildteam.ApiKey))
        {
            var newBuildTeam = new BuildTeam
            {
                BuildTeamId = bt.Id,
                Name = bt.Name,
                ApiKey = bt.ApiKey?.ApiKey,
                ApiCreateDate = bt.ApiKey?.CreatedAt,
                CriteriaNames = await v2Context.ReviewToggleCriteria.ToListAsync()
            };
            newBuildTeams.Add(newBuildTeam);
        }

        v2Context.BuildTeams.AddRange(newBuildTeams);
        await v2Context.SaveChangesAsync();
        Console.WriteLine($"Migrated {newBuildTeams.Count} build teams...");
    }

    private async Task MigrateServers()
    {
        v2Context.RemoveRange(v2Context.CityProjects);
        v2Context.RemoveRange(v2Context.Servers);
        await v2Context.SaveChangesAsync();
        
        var newServers = await v1Context.PlotsystemServers
            .Include(static plotsystemServer => plotsystemServer.PlotsystemCountries)
            .ThenInclude(static plotsystemCountry => plotsystemCountry.PlotsystemBuildteamHasCountries)
            .Select(static server => new Server
            {
                ServerName = server.Name,
                BuildTeamId = server.PlotsystemCountries.First().PlotsystemBuildteamHasCountries.First().BuildteamId
            })
            .ToListAsync();

        v2Context.Servers.AddRange(newServers);
        await v2Context.SaveChangesAsync();
        Console.WriteLine($"Migrated {await v2Context.Servers.CountAsync()} servers...");
    }

    private async Task MigrateCountries()
    {
        v2Context.Countries.RemoveRange(v2Context.Countries);

        var newCountries = new List<Country>();
        foreach (var country in v1Context.PlotsystemCountries)
        {
            var cultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .FirstOrDefault(c => c.EnglishName.Contains(country.Name));
            
            if (cultureInfo == null)
            {
                Console.WriteLine($"Could not find ISO code for {country.Name} using id as code instead!");
            }
            var countryCode = cultureInfo == null ? country.Id.ToString() : new RegionInfo(cultureInfo.TextInfo.CultureName).TwoLetterISORegionName;;
            
            if (newCountries.Any(c => c.CountryCode == countryCode))
            {
                Console.WriteLine($"Country {countryCode} already exists!");
                continue;
            }

            var newContinent = country.Continent switch
            {
                "europe" => "EU",
                "asia" => "AS",
                "oceania" => "OC",
                "south america" => "SA",
                _ => "NA"
            };

            _countryIdConversions.Add(country.Id, countryCode);
            newCountries.Add(new Country
            {
                CountryCode = countryCode,
                Continent = newContinent,
                Material = $"head({country.HeadId})"
            });
        }

        v2Context.Countries.AddRange(newCountries);
        await v2Context.SaveChangesAsync();
        Console.WriteLine($"Migrated {newCountries.Count} countries...");
    }

    private async Task MigrateCityProjects()
    {
        v2Context.CityProjects.RemoveRange(v2Context.CityProjects);

        var newCityProjects = new List<CityProject>();
        foreach (var city in v1Context.PlotsystemCityProjects
                     .Include(static plotsystemCityProject => plotsystemCityProject.Country)
                     .ThenInclude(static plotsystemCountry => plotsystemCountry.Server)
                     .Include(static plotsystemCityProject => plotsystemCityProject.Country).ThenInclude(static plotsystemCountry =>
                         plotsystemCountry.PlotsystemBuildteamHasCountries))
        {
            var cityProjectId = city.Name.ToLower().Replace(" ", "-").RemoveDiacritics();
            _cityProjectIdConversions.Add(city.Id, cityProjectId);
            newCityProjects.Add(new CityProject
            {
                CityProjectId = cityProjectId,
                BuildTeamId = city.Country.PlotsystemBuildteamHasCountries.First().BuildteamId,
                CountryCode = _countryIdConversions[city.Country.Id],
                ServerName = city.Country.Server.Name,
                IsVisible = city.Visible == 1
            });
        }

        v2Context.CityProjects.AddRange(newCityProjects);
        await v2Context.SaveChangesAsync();
        Console.WriteLine($"Migrated {newCityProjects.Count} city projects...");
    }

    private async Task MigrateBuilders()
    {
        v2Context.Builders.RemoveRange(v2Context.Builders.Include(static b => b.BuildTeams));

        var newBuilders = new List<Builder>();
        foreach (var builder in v1Context.PlotsystemBuilders
                     .Include(static plotsystemBuilder => plotsystemBuilder.PlotsystemBuilderIsReviewers)
                     .ThenInclude(static plotsystemBuilderIsReviewer => plotsystemBuilderIsReviewer.Buildteam))
        {
            if (newBuilders.Any(b => b.Name.Equals(builder.Name)))
            {
                Console.WriteLine($"Builder {builder.Name} already exists!");
                continue;
            }

            List<BuildTeam> reviewerTeams = [];
            reviewerTeams.AddRange(builder.PlotsystemBuilderIsReviewers
                .Select(reviewer => v2Context.BuildTeams.First(bt => bt.BuildTeamId == reviewer.Buildteam.Id)));

            var newBuilder = new Builder
            {
                Uuid = builder.Uuid,
                Name = builder.Name,
                Score = builder.Score,
                FirstSlot = builder.FirstSlot,
                SecondSlot = builder.SecondSlot,
                ThirdSlot = builder.ThirdSlot,
                PlotType = builder.SettingPlotType ?? 1,
                BuildTeams = reviewerTeams
            };

            newBuilders.Add(newBuilder);
        }

        v2Context.Builders.AddRange(newBuilders);
        await v2Context.SaveChangesAsync();
        Console.WriteLine($"Migrated {await v2Context.Builders.CountAsync()} builders...");
    }

    private async Task MigrateDifficulties()
    {
        v2Context.PlotDifficulties.RemoveRange(v2Context.PlotDifficulties);
        var newDifficulties = new List<PlotDifficulty>();

        foreach (var difficulty in v1Context.PlotsystemDifficulties)
        {
            var difficultyId = difficulty.Id switch
            {
                1 => "EASY",
                2 => "MEDIUM",
                _ => "HARD"
            };

            _difficultyConversions.Add(difficulty.Id, difficultyId);
            newDifficulties.Add(new PlotDifficulty
            {
                DifficultyId = difficultyId,
                Multiplier = (decimal)difficulty.Multiplier,
                ScoreRequirement = difficulty.ScoreRequirment
            });
        }

        v2Context.PlotDifficulties.AddRange(newDifficulties);
        await v2Context.SaveChangesAsync();
        Console.WriteLine($"Migrated {newDifficulties.Count} difficulties...");
    }

    private async Task MigratePlots()
    {
        v2Context.RemoveRange(v2Context.Plots.Include(static p => p.Uus));
        await v2Context.SaveChangesAsync();
        
        var newPlots = new List<Plot>();

        foreach (var plot in v1Context.PlotsystemPlots)
        {
            if (plot.OwnerUuid != null && !v2Context.Builders.Any(b => b.Uuid.Equals(plot.OwnerUuid)))
            {
                Console.WriteLine("Could not find builder for uuid " + plot.OwnerUuid);
                continue;
            }
            var newPlot = new Plot
            {
                PlotId = plot.Id,
                CityProjectId = _cityProjectIdConversions[plot.CityProjectId],
                DifficultyId = _difficultyConversions[plot.DifficultyId],
                OwnerUuid = plot.OwnerUuid,
                Status = plot.Status,
                OutlineBounds = plot.Outline!,
                InitialSchematic = "OUTDATED_SCHEMATIC"u8.ToArray(),
                CompleteSchematic = "OUTDATED_SCHEMATIC"u8.ToArray(),
                LastActivityDate = plot.LastActivity,
                IsPasted = plot.Pasted != 0,
                PlotVersion = plot.Version ?? 1,
                PlotType = plot.Type,
                CreatedBy = plot.CreatePlayer,
                CreateDate = plot.CreateDate,
            };

            if (plot.MemberUuids != null)
            {
                List<Builder> members = [];
                foreach (var member in plot.MemberUuids?.Split(",")!)
                {
                    members.Add(await v2Context.Builders.SingleAsync(b => b.Uuid == member));
                }

                newPlot.Uus = members;
            }

            newPlots.Add(newPlot);
        }

        v2Context.Plots.AddRange(newPlots);
        await v2Context.SaveChangesAsync();
        Console.WriteLine($"Migrated {await v2Context.Plots.CountAsync()} plots...");
    }

    private async Task MigrateTutorials()
    {
        v2Context.Tutorials.RemoveRange(v2Context.Tutorials);
        var newTutorials = await v1Context.PlotsystemPlotsTutorials.Select(static tutorial => new Tutorial
            {
                TutorialId = tutorial.TutorialId,
                Uuid = tutorial.PlayerUuid,
                IsComplete = tutorial.IsCompleted == 1,
                StageId = tutorial.StageId,
                FirstStageStartDate = tutorial.CreateDate,
                LastStageCompleteDate = tutorial.LastStageCompleteDate
            })
            .ToListAsync();

        v2Context.Tutorials.AddRange(newTutorials);
        await v2Context.SaveChangesAsync();
        Console.WriteLine($"Migrated {await v2Context.Tutorials.CountAsync()} tutorials...");
    }

    private async Task MigrateReviews()
    {
        v2Context.PlotReviews.RemoveRange(v2Context.PlotReviews);
        var newReviews = new List<PlotReview>();

        foreach (var review in v1Context.PlotsystemReviews)
        {
            var plot = await v1Context.PlotsystemPlots.FirstOrDefaultAsync(p1 => p1.ReviewId == review.Id);
            if (plot == null || !await v2Context.Plots.AnyAsync(p => p.PlotId == plot.Id)) continue;

            newReviews.Add(new PlotReview
            {
                ReviewId = review.Id,
                PlotId = plot.Id,
                Rating = review.Rating,
                Score = (int)plot.Score!,
                Feedback = review.Feedback,
                ReviewedBy = review.ReviewerUuid,
                ReviewDate = review.ReviewDate,
            });
        }

        v2Context.PlotReviews.AddRange(newReviews);
        await v2Context.SaveChangesAsync();
        Console.WriteLine($"Migrated {await v2Context.PlotReviews.CountAsync()} reviews...");
    }
}