using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Umbraco.Cms.Core.Composing;
using Umbraco.Extensions;

namespace UmbracoMongoDbClient.Setup
{
	public sealed class MongoDbComponent : IComponent
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
			
            var mongoDbCredentials = "MongoDbCredentials";
            var certificateFilePathWithName = _config.GetValue<string>($"{mongoDbCredentials}:CertificateFilePathWithName");
            string appsettingsJsonConnectionStringKey = $"{mongoDbCredentials}:ConnectionString";
            string? connectionString = _config.GetValue<string>(appsettingsJsonConnectionStringKey);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception($"Unable to figure out MongoDB connection configurations from {appsettingsJsonConnectionStringKey}");
            }
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            if (!string.IsNullOrWhiteSpace(certificateFilePathWithName))
            {
                var certificatePassword = _config.GetValue<string>($"{mongoDbCredentials}:CertificatePassword");
                MongoDBClientConnection.Initialize(
                    cert: string.IsNullOrWhiteSpace(certificatePassword) ? new X509Certificate2(certificateFilePathWithName) : new X509Certificate2(certificateFilePathWithName, certificatePassword),
                    connectionString: connectionString,
                    environmentName: _env.EnvironmentName,
                    isProduction: _env.IsProduction());
            }
            else
            {
                MongoDBClientConnection.Initialize(
                    connectionString: connectionString,
                    environmentName: _env.EnvironmentName,
                    isProduction: _env.IsProduction());
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