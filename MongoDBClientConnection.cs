using MongoDB.Driver;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace UmbracoMongoDbClient
{
	public static class MongoDBClientConnection
	{
		private static MongoClient MongoClient;
		private static string _databaseNamePrefix;

		public static void Initialize(X509Certificate2 cert, string connectionString, string databaseNamePrefix)
		{
			var settings = MongoClientSettings.FromConnectionString(connectionString);

			settings.SslSettings = new SslSettings
			{
				ClientCertificates = new List<X509Certificate>() { cert }
			};

			MongoClient = new MongoClient(settings);
			_databaseNamePrefix = databaseNamePrefix;
			RegisterInitialization();
		}

		public static void Initialize(string connectionStringWithPassword, string databaseNamePrefix)
		{
			var settings = MongoClientSettings.FromConnectionString(connectionStringWithPassword);
			settings.ServerApi = new ServerApi(ServerApiVersion.V1);
			MongoClient = new MongoClient(settings);
			_databaseNamePrefix = databaseNamePrefix;
			RegisterInitialization();
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

		public static IMongoDatabase GetDatabase(string dBName) => MongoClient.GetDatabase($"{_databaseNamePrefix}-{dBName}");
	}
}