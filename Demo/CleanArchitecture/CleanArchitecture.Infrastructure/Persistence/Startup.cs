using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Core.CustomExceptions;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Infrastructure.Persistence;

public static class Startup
{
    public static IServiceCollection AddDbContextAndUnitOfWork(this IServiceCollection services,
     IConfiguration configuration)
    {
        const string dataSettingKey = "ConnectionStrings";

        var configSetion = configuration.GetSection(dataSettingKey);

        var dataProviderOption = configSetion.Get<ConnectionString>(); ;

        if (dataProviderOption is null
        || string.IsNullOrWhiteSpace(dataProviderOption.Provider.ToString()))
        {
            throw new NullConfigurationException(dataSettingKey);
        }

        if (string.IsNullOrWhiteSpace(configSetion[dataProviderOption!.Provider.ToString()]))
        {
            throw new NullConfigurationException($"dataSettingKey:{dataProviderOption.Provider}");
        }

        var enableSensitiveDataLogging =
         configuration["EnableSensitiveDataLogging"]?.ToString().ToLower() == "true";
        switch (dataProviderOption.Provider)
        {
            case DataProvider.MariaDb:

                services.AddScoped<MariaDbContext>(x =>
                {
                    return new MariaDbContext(UseMariaDb(dataProviderOption.MariaDb, enableSensitiveDataLogging));
                });

                AddUnitOfWork<MariaDbContext>(services);
                break;
            case DataProvider.Postgresql:
                services.AddScoped<PostgresqlContext>(options =>
                         new PostgresqlContext(
                            UsePostgresql(dataProviderOption.Postgresql, enableSensitiveDataLogging)));

                AddUnitOfWork<PostgresqlContext>(services);

                break;
            case DataProvider.MySql:
                services.AddScoped<MySqlContext>(options =>
                          new MySqlContext(UseMySql(dataProviderOption.MySql, enableSensitiveDataLogging)));

                AddUnitOfWork<MySqlContext>(services);
                break;
            case DataProvider.Oracle:

                services.AddScoped<OracleContext>(options =>
                          new OracleContext(UseOracle(dataProviderOption.Oracle, enableSensitiveDataLogging)));

                AddUnitOfWork<OracleContext>(services);
                break;
            default:

                services.AddScoped<SqlServerContext>(options =>
                           new SqlServerContext(UseSqlServer(dataProviderOption.SQlServer, enableSensitiveDataLogging)));

                AddUnitOfWork<SqlServerContext>(services);

                break;
        }

        return services;
    }

    private static DbContextOptionsBuilder CreateDbContextOptionBuilder(bool enableSensitiveDataLogging)
    {
        var builder = new DbContextOptionsBuilder();
        if (enableSensitiveDataLogging)
        {
            builder.EnableSensitiveDataLogging(true);
            builder.LogTo(Console.WriteLine);
        }

        return builder;
    }

    private static DbContextOptionsBuilder UseOracle(string connection, bool enableSensitiveDataLogging)
    {
        DbContextOptionsBuilder dbOptionsBuilder = CreateDbContextOptionBuilder(enableSensitiveDataLogging);

        dbOptionsBuilder.UseOracle(connection);

        return dbOptionsBuilder;
    }

    private static DbContextOptionsBuilder UseMySql(string connection, bool enableSensitiveDataLogging)
    {
        var dbOptionsBuilder = CreateDbContextOptionBuilder(enableSensitiveDataLogging);

        dbOptionsBuilder.UseMySql(MySqlServerVersion.AutoDetect(connection));

        return dbOptionsBuilder;
    }

    private static DbContextOptionsBuilder UsePostgresql(string connection, bool enableSensitiveDataLogging)
    {
        var dbOptionsBuilder = CreateDbContextOptionBuilder(enableSensitiveDataLogging);

        dbOptionsBuilder.UseNpgsql(connection);

        return dbOptionsBuilder;
    }

    private static DbContextOptionsBuilder UseSqlServer(string connection, bool enableSensitiveDataLogging)
    {
        var dbOptionsBuilder = CreateDbContextOptionBuilder(enableSensitiveDataLogging);
        dbOptionsBuilder.UseSqlServer(connection);

        return dbOptionsBuilder;
    }
    private static DbContextOptionsBuilder UseMariaDb(string connection, bool enableSensitiveDataLogging)
    {
        var dbOptionsBuilder = CreateDbContextOptionBuilder(enableSensitiveDataLogging);

        dbOptionsBuilder.UseMySql(MariaDbServerVersion.AutoDetect(connection));

        return dbOptionsBuilder;
    }

    private static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : ApplicationDbContext
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
        services.AddScoped<IDataMigrationService, DataMigrationService<TContext>>();

        return services;
    }
}
