# SimpleMongoDBWrapper
.Net Core Class library. Simple wrapper for MongoDB

**Get started**

 * Set configuration file (appsettings.json in root of your project)
```c
"ConnectionStrings": {
  "DatabaseName": "MyDb",
  "MyDb": "mongodb://localhost:27017/database"
},
"ItemsPerPage" : 10
```

 * Initialize DbContext
```c
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");
var config = builder.Build();
DBContext.GetInstance(config);
```

* Create a model class that extends BaseCollection
```c
using SimpleMongoDBWrapper;

public class TestModel : BaseCollection {
    public string data { get; set; }
}
```
BaseCollection contains a string field "Id", declared for mongo as BsonId

 * Read
```c
var rep = new Repository<TestModel>();
var list = await rep.GetAll();
```

* Insert
```c
var rep = new Repository<TestModel>();
rep.InsertOne(new TestModel() {
    Id = "5ccedb8ae69af8035cab128d",
    data = "Hello world!"
}).Wait();
```

* Update
```c
TestModel model = new TestModel() {
    data = "Hello world! Edited!!!"
};
var rep = new Repository<TestModel>();
var result = rep.UpdateOne("5ccedb8ae69af8035cab128d", model).Result;
```

* Delete
```c
var rep = new Repository<TestModel>();
var result = rep.DeleteOne("5ccedb8ae69af8035cab128d").Result;
```


