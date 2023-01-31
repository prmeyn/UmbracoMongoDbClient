# Setup procedure

The `appsettings.json` file should be given a connection string to your MongoDB databases
```json
{
	"MongoDbCredentials": {
	    "CertificateFilePathWithName": "<optional Certificate file path with name [pfx file]>",
	    "CertificatePassword": "<optional Certificate password>",
	    "ConnectionString": "<mandatory> mongodb+srv://cluster0.fyu.mongodb.net/?authSource=%24external&authMechanism=MONGODB-X509&retryWrites=true&w=majority"
	  }
}
```

Sample code
```csharp
var database = MongoDBClientConnection.GetDatabase("YouDatabaseName");
var collection = database.GetCollection<T>("YourCollectionName"); // of type T
```
For more examples on how to Insert, Modify your data, check the MongoDB official documentation: https://www.mongodb.com/docs/drivers/csharp/current/quick-reference/
