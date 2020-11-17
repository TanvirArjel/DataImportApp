using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataImportApp.Infrastructure.Data.Extensions
{
    public static class DbContextStartupExtensions
    {
        public static readonly ILoggerFactory EFCoreLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information)
                    .AddConsole();
            });

        public static void AddDataImportDbContext(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            string connectionName = hostingEnvironment.IsDevelopment() ? "DataImportDbConnection" : "DataImportDbConnection";
            string connectionString = configuration.GetConnectionString(connectionName);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string is either null or empty.");
            }

            services.AddDbContext<DataImportDbContext>(options =>
            {
                options.UseLoggerFactory(EFCoreLoggerFactory);
                options.EnableSensitiveDataLogging(true);
                options.UseSqlServer(connectionString, builder =>
                {
                    ////builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
                    builder.MigrationsAssembly("DataImportApp.Infrastructure");
                    builder.MigrationsHistoryTable("__EFCoreMigrationsHistory", schema: "_Migration");
                }).UseEnumCheckConstraints();
            });
        }
    }
}
