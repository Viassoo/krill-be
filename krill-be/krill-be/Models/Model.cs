using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace krill_be.Models
{
	public class Model
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public string? Id { get; set; }
	}
}
