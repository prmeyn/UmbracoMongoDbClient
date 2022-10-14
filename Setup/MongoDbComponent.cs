using Microsoft.Extensions.Configuration;
using Umbraco.Cms.Core.Composing;
using Microsoft.AspNetCore.Hosting;
using Umbraco.Extensions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace UmbracoMongoDbClient.Setup
{
	public class MongoDbComponent : IComponent
	{
		private readonly IHostingEnvironment _env;
		private readonly IConfiguration _config;
		private readonly ILogger<MongoDbComponent> _logger;

		public MongoDbComponent(IHostingEnvironment env, IConfiguration config, ILogger<MongoDbComponent> logger)
		{
			_env = env;
			_config = config;
			_logger = logger;
		}

		public void Initialize()
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			MongoDBClientConnection.Initialize(
				connectionStringWithPassword: _config.GetUmbracoConnectionString("mongoDBConnectionStringWithPassword"),
				databaseNamePrefix: _env.IsProduction() ? _env.EnvironmentName : $"{_env.EnvironmentName}_{Environment.MachineName}");
			stopWatch.Stop();
			// Get the elapsed time as a TimeSpan value.
			TimeSpan ts = stopWatch.Elapsed;

			// Format and display the TimeSpan value.
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
				ts.Hours, ts.Minutes, ts.Seconds,
				ts.Milliseconds / 10);
			_logger.LogInformation("Time taken to establish MongoDB initial connection {elapsedTime}", elapsedTime);
		}

		public void Terminate()
		{
		}
	}
}