using MongoDB.Bson.Serialization.Attributes;

namespace UmbracoMongoDbClient
{
	public class DatabaseNamePrefixDTO
	{
		[BsonId]
		public string ServerId { get; set; }
		public DateTimeOffset InitializedAtUtc { get; set; }
		public string DatabaseNamePrefix { get; set; }
	}
}