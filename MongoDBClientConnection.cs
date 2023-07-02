using MongoDB.Driver;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace UmbracoMongoDbClient
{
	public static class MongoDBClientConnection
	{
		private static MongoClient MongoClient;
		private static string _databaseNamePrefix;

		public static void Initialize(X509Certificate2 cert, string connectionString, string environmentName, bool isProduction)
		{
			var settings = MongoClientSettings.FromConnectionString(connectionString);

			settings.SslSettings = new SslSettings
			{
				ClientCertificates = new List<X509Certificate>() { cert }
			};

			MongoClient = new MongoClient(settings);
			_databaseNamePrefix = isProduction ? "" : getDatabaseNamePrefix(environmentName, Environment.MachineName);
			RegisterInitialization();
		}

		public static void Initialize(string connectionString, string environmentName, bool isProduction)
		{
			var settings = MongoClientSettings.FromConnectionString(connectionString);
			settings.ServerApi = new ServerApi(ServerApiVersion.V1);
			MongoClient = new MongoClient(settings);
			_databaseNamePrefix = isProduction ? "" : getDatabaseNamePrefix(environmentName, Environment.MachineName);
			RegisterInitialization();
		}

		private static string getDatabaseNamePrefix(string environmentName, string machineName)
		{
			var database = MongoClient.GetDatabase("DatabaseNamePrefixes");
			var collection = database.GetCollection<DatabaseNamePrefixDTO>("Servers");
			var id = $"{environmentName}-{machineName}";
			var databaseNamePrefixDTO = collection.Find(
						filter: Builders<DatabaseNamePrefixDTO>.Filter.Eq("_id", id)
					)?.ToListAsync()?.Result?.FirstOrDefault();
			if (databaseNamePrefixDTO == null)
			{
				var databaseNamePrefix = $"{environmentName}{collection.CountDocuments(filter: _ => true)}";
				collection.InsertOne(new DatabaseNamePrefixDTO()
				{
					ServerId = id,
					DatabaseNamePrefix = databaseNamePrefix,
					InitializedAtUtc = DateTime.UtcNow
				});
				return databaseNamePrefix;
			}
			return databaseNamePrefixDTO.DatabaseNamePrefix;
		}

		private static void RegisterInitialization()
		{
			var database = GetDatabase(MethodBase.GetCurrentMethod().DeclaringType.Name);
			var collection = database.GetCollection<InitializationDTO>("Initializations");
			collection.InsertOne(new InitializationDTO()
			{
				Id = Guid.NewGuid().ToString(),
				InitializedAtUtc = DateTime.UtcNow
			});
		}

		public static IMongoDatabase GetDatabase(string dBName) => MongoClient.GetDatabase(string.IsNullOrWhiteSpace(_databaseNamePrefix) ? dBName :$"{_databaseNamePrefix}-{dBName}");
	}
}