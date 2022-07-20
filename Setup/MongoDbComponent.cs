using Microsoft.Extensions.Configuration;
using Umbraco.Cms.Core.Composing;
using Microsoft.AspNetCore.Hosting;
using Umbraco.Extensions;

namespace UmbracoMongoDbClient.Setup
{
	public class MongoDbComponent : IComponent
	{
		private readonly IHostingEnvironment _env;
		private readonly IConfiguration _config;

		public MongoDbComponent(IHostingEnvironment env, IConfiguration config)
		{
			_env = env;
			_config = config;
		}

		public void Initialize()
		{
			MongoDBClientConnection.Initialize(
				connectionStringWithPassword: _config.GetUmbracoConnectionString("mongoDBConnectionStringWithPassword"),
				databaseNamePrefix: _env.EnvironmentName);
		}

		public void Terminate()
		{
		}
	}
}