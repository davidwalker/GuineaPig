using System;

namespace GuineaPig
{
	public class FixedValueFactoryContainer : IValueObjectFactoryContainer
	{
		public void RegisterWith(ValueObjectFactoryFunctionCollection factory)
		{
			factory.RegisterFactoryFunction(() => "Test Value");
			factory.RegisterFactoryFunction(() => 10);
			factory.RegisterFactoryFunction(() => 10L);
			factory.RegisterFactoryFunction(() => 10f);
			factory.RegisterFactoryFunction(() => 10d);
			factory.RegisterFactoryFunction(() => 10m);
			factory.RegisterFactoryFunction(() => new DateTime(2009, 10, 01));
		}
	}
}