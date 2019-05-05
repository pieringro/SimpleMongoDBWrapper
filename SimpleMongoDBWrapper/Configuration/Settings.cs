using System.IO;
using Microsoft.Extensions.Configuration;

namespace SimpleMongoDBWrapper {
    public class Settings {
        protected readonly string appSettingsJsonNameFile = "appsettings.json";
        protected IConfigurationRoot Configuration { get; set; }
        public bool refreshInstance = false;

        private static Settings _instance;
        public static Settings Instance {
            get {
                if (_instance == null || _instance.refreshInstance) {
                    _instance = new Settings();
                    _instance.buildConfigurations();
                }
                return _instance;
            }
        }
        protected void buildConfigurations() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appSettingsJsonNameFile);

            Configuration = builder.Build();
        }

        private int? _ItemsPerPage;
        public int? ItemsPerPage {
            get {
                if (_ItemsPerPage == null) {
                    string itemsPerPageString = Configuration["ItemsPerPage"];
                    if (itemsPerPageString != null) {
                        _ItemsPerPage = int.Parse(Configuration["ItemsPerPage"]);
                    }
                }
                return _ItemsPerPage;
            }
        }

        private string _DatabaseName;
        public string DatabaseName {
            get {
                if (_DatabaseName == null) {
                    _DatabaseName = Configuration.GetConnectionString("DatabaseName");
                }
                return _DatabaseName;
            }
        }

        public string ConnectionString(string databaseName) {
            return Configuration.GetConnectionString(databaseName);
        }
    }
}