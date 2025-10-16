using Microsoft.Extensions.Configuration;

namespace MSSQLCrudExample.Config
{
    public static class DatabaseConfig
    {
        public static string ConnectionString
        {
            get
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                return configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string not found in appsettings.json");
            }
        }
    }
}

