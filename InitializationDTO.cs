﻿using MongoDB.Bson.Serialization.Attributes;

namespace UmbracoMongoDbClient
{
	public class InitializationDTO
	{
		[BsonId]
		public required string Id { get; set; }
		public required DateTimeOffset InitializedAtUtc { get; set; }
	}
}