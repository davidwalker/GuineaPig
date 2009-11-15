namespace GuineaPig.Tests.Integration.BookShopSample.Tests
{
	public class UnitTestsBase
	{
		public Factory Factory;

		public UnitTestsBase()
		{
			Factory = new Factory();
			Factory.Entities.RegisterFactories(new EntityFactories());
			Factory.ValueObjects.RegisterFactories(new RandomPrimativeGenerator());
			Factory.ValueObjects.RegisterFactories(new ValueObjectFactories());
		}
	}
}