using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GuineaPig
{
	public class RandomPrimativeGenerator : IValueObjectFactoryContainer
	{
		private readonly Random rnd;

		public RandomPrimativeGenerator()
		{
			rnd = new Random();
		}

		public RandomPrimativeGenerator(int seed)
		{
			rnd = new Random(seed);
		}

		public void RegisterWith(ValueObjectFactoryFunctionCollection factoryFunctionCollection)
		{
			factoryFunctionCollection.RegisterFactoryFunction((info) => info.Name + " " + rnd.Next());
			factoryFunctionCollection.RegisterFactoryFunction(() => rnd.Next());
			factoryFunctionCollection.RegisterFactoryFunction(() => (long)rnd.Next());
			factoryFunctionCollection.RegisterFactoryFunction(() => rnd.NextDouble());
			factoryFunctionCollection.RegisterFactoryFunction(() => (float)rnd.NextDouble());
			factoryFunctionCollection.RegisterFactoryFunction(() => (decimal)rnd.NextDouble());
			factoryFunctionCollection.RegisterFactoryFunction(() =>
														{
															const long minTicks = 599266080000000000;
															const long maxTicks = 662380416000000000;
															const long range = maxTicks - minTicks;
															const long multiplier = range / int.MaxValue;
															return new DateTime((rnd.Next(int.MaxValue) * multiplier) + minTicks);
														});
		}
	}
}
