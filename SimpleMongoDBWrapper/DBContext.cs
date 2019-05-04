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

            string databaseName = Settings.Instance.DatabaseName;
            string connectionString = Settings.Instance.ConnectionString(databaseName);
            var client = new MongoClient(connectionString);
            Database = client.GetDatabase(databaseName);
        }
    }
}