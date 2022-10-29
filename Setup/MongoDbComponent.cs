using Microsoft.Extensions.Configuration;
using Umbraco.Cms.Core.Composing;
using Microsoft.AspNetCore.Hosting;
using Umbraco.Extensions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

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
			var mongoDBConnectionStringWithPassword = _config.GetUmbracoConnectionString("mongoDBConnectionStringWithPassword");
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			if (!string.IsNullOrWhiteSpace(mongoDBConnectionStringWithPassword))
			{
				MongoDBClientConnection.Initialize(
				connectionString: mongoDBConnectionStringWithPassword,
				environmentName: _env.EnvironmentName,
				isProduction: _env.IsProduction());
			}
			else 
			{
				var mongoDbCredentials = "MongoDbCredentials";
				var certificateFilePathWithName = _config.GetValue<string>($"{mongoDbCredentials}:CertificateFilePathWithName");
				var connectionString = _config.GetValue<string>($"{mongoDbCredentials}:ConnectionString");

				if (!string.IsNullOrWhiteSpace(certificateFilePathWithName))
				{
					var certificatePassword = _config.GetValue<string>($"{mongoDbCredentials}:CertificatePassword");
					MongoDBClientConnection.Initialize(
						cert: string.IsNullOrWhiteSpace(certificatePassword) ? new X509Certificate2(certificateFilePathWithName) : new X509Certificate2(certificateFilePathWithName, certificatePassword),
						connectionString: connectionString,
						environmentName: _env.EnvironmentName,
						isProduction: _env.IsProduction());
				}
				else if (!string.IsNullOrWhiteSpace(connectionString))
				{
					MongoDBClientConnection.Initialize(
						connectionString: connectionString,
						environmentName: _env.EnvironmentName,
						isProduction: _env.IsProduction());
				}
				else
				{
					throw new Exception("Unable to figure out MongoDB connection configurations!!");
				}
			}
			
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