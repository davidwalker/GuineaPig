using BookShop.Domain;

namespace GuineaPig.Tests.Integration.BookShopSample.Tests
{
	public class ValueObjectFactories
	{
		public Money CreateMoney()
		{
			return new Money(56, "USD");
		}
	}
}