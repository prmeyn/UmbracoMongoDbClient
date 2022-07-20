# Setup procedure

The `appsettings.json` file should be given a connection string to your MongoDB databases
```json
{
	"ConnectionStrings": {
		"mongoDBConnectionStringWithPassword": "mongodb+srv://some:password@somecluster.bulb.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"
	}
}
```

Sample code
```csharp
var database = MongoDBClientConnection.GetDatabase("YouDatabaseName");
var collection = database.GetCollection<T>("YourCollectionName"); // of type T
```