using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace SimpleMongoDBWrapper {
    public class RepositoryTest {
        [Fact]
        public void GetPageTest() {
            init();
            var rep = new Repository<TestModel>();
            rep.GetPage(1).Wait();
        }

        private void init() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            DBContext.GetInstance(config);
        }
    }
}