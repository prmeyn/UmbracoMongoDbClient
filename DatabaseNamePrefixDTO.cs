using MongoDB.Bson.Serialization.Attributes;

namespace UmbracoMongoDbClient
{
	public class DatabaseNamePrefixDTO
	{
		[BsonId]
		public required string ServerId { get; set; }
		public required DateTimeOffset InitializedAtUtc { get; set; }
		public required string DatabaseNamePrefix { get; set; }
	}
}