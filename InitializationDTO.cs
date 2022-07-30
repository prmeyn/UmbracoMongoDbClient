using MongoDB.Bson.Serialization.Attributes;

namespace UmbracoMongoDbClient
{
	public class InitializationDTO
	{
		[BsonId]
		public string Id { get; set; }
		public DateTimeOffset ValidUntilUtc { get; set; }
	}
}