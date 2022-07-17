# Setup procedure

The `appsettings.json` file should be given a connection string to your MongoDB database
```json
{
	"MongoDBClient": {
		"ConnectionStringWithPassword": "mongodb+srv://some:password@somecluster.bulb.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"
	}
}
```

And your `Startup` class can call the `MongoDBClient.MongoDBClientConnection.Setup` method from within the `Configure` method

```csharp
var mongoDBCientConfigPath = "MongoDBClient";
			MongoDBClient.MongoDBClientConnection.Setup(
				connectionStringWithPassword: _config.GetValue<string>($"{mongoDBCientConfigPath}:ConnectionStringWithPassword"),
				databaseNamePrefix: env.EnvironmentName);
```

