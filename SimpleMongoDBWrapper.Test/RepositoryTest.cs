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

        [Fact]
        public void CreateModelTest() {
            init();
            var rep = new Repository<TestModel>();
            rep.InsertOne(new TestModel() {
                Id = "5ccedb8ae69af8035cab128d",
                    data = "Hello world!"
            }).Wait();
        }

        [Fact]
        public void UpdateModelTest() {
            init();
            TestModel model = new TestModel() {
                data = "Hello world! Edited!!!"
            };
            var rep = new Repository<TestModel>();
            var result = rep.UpdateOne("5ccedb8ae69af8035cab128d", model).Result;
            Assert.True(result);
        }

        [Fact]
        public void DeleteModelTest() {
            init();
            var rep = new Repository<TestModel>();
            var result = rep.DeleteOne("5ccedb8ae69af8035cab128d").Result;
            Assert.True(result);
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