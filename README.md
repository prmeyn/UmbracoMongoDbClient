# Setup procedure

The `appsettings.json` file should be given a connection string to your MongoDB database
```json
{
	"ConnectionStrings": {
		"mongoDBConnectionStringWithPassword": "mongodb+srv://some:password@somecluster.bulb.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"
	}
}
```
