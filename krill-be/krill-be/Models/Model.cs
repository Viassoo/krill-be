using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ThirdParty.Json.LitJson;

namespace krill_be.Models
{
	public class Model
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
	}
}
