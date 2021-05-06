using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Managers
{
    public class User
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string _id { get; }
        public int Id { get; }
        
        public string Login { get; }
        
        public string Password { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UserType UserType { get; }
        
        public User(int id, string login, string password, UserType userType, string _id)
        {
            this._id = _id;
            this.Login = login;
            this.Password = password;
            Id = id;
            UserType = userType;
        }
    }
}