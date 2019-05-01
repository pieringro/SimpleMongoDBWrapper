using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleMongoDBWrapper {
    public class BaseCollection {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] //allow passing the parameter as type string instead of ObjectId. Mongo handles the conversion from string to ObjectId.
        public string Id { get; set; }
    }
}