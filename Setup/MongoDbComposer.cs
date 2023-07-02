using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace UmbracoMongoDbClient.Setup
{

	public sealed class MongoDbComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.AddComponent<MongoDbComponent>();
		}
	}
}
