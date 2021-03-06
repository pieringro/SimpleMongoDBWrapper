using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace SimpleMongoDBWrapper {
    public class DBContext {

        private Dictionary<string, Repository<BaseCollection>> collections = new Dictionary<string, Repository<BaseCollection>>();

        public readonly IMongoDatabase Database;

        public static DBContext Instance {
            get;
            private set;
        }

        public static DBContext GetInstance(IConfiguration config) {
            if (Instance == null) {
                Instance = new DBContext(config);
            }
            return Instance;
        }

        private DBContext(IConfiguration config) {
            // mongo use string for id
            BsonSerializer.RegisterIdGenerator(typeof(string), new StringObjectIdGenerator());

            Settings.customConfiguration = config;
            string databaseName = Settings.Instance.DatabaseName;
            if (databaseName == null) {
                throw new AppException("Configuration error DatabaseName");
            }

            string connectionString = Settings.Instance.ConnectionString(databaseName);
            if (connectionString == null) {
                throw new AppException(string.Format("Configuration error connectionString for databaseName={0}", databaseName));
            }
            var client = new MongoClient(connectionString);
            Database = client.GetDatabase(databaseName);
        }
    }
}