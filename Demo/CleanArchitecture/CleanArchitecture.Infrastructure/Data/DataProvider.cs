using System.Text.Json.Serialization;

namespace CleanArchitecture.Infrastructure.Data;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DataProvider
{
    SqlServer,
    MariaDb,
    MySql,
    Postgresql,
    Oracle
}