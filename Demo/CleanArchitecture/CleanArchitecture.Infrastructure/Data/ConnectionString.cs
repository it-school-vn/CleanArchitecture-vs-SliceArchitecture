#nullable disable
namespace CleanArchitecture.Infrastructure.Data
{
    public sealed record ConnectionString
    {
        public DataProvider Provider { get; set; }
        public string SqlLite { get; set; }

        public string Oracle { get; set; }

        public string MySql { get; set; }

        public string MariaDb { get; set; }

        public string Postgresql { get; set; }

        public string SQlServer { get; set; }
    }
}