using Microsoft.Extensions.Configuration;
using System.IO;

namespace ICSB.DataContext
{
    public class AppSettings
    {       
        protected const string MasterDatabaseName = "DBConnectionString";    
        public readonly string _appSetting = string.Empty;
        
        public AppSettings(string settingName = null)
        {            
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();
            _appSetting = root.GetSection("ApplicationSettings").GetSection(settingName).Value;            
        }

        public string GetAppSettings
        {
            get => _appSetting;
        }

    }
}
