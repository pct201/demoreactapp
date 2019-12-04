using Microsoft.Extensions.Configuration;
using System.IO;

namespace ICSB.DataContext
{
    public class AppConfiguration
    {       
        protected const string MasterDatabaseName = "DBConnectionString";    
        public readonly string _connectionString = string.Empty;
       
        public AppConfiguration(string productUid = null)
        {            
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();

            if (!string.IsNullOrEmpty(productUid))          
                _connectionString = root.GetSection("connectionStrings").GetSection(productUid).Value;
            else
                _connectionString = root.GetSection("connectionStrings").GetSection(MasterDatabaseName).Value;
            //var appSetting = root.GetSection("ApplicationSettings");
        }

        public string GetConnectionString
        {
            get => _connectionString;
        }

    }
}
