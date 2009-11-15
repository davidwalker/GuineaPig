using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace GuineaPig.Tests
{
	public class RandomPrimativeGeneratorTests
	{
		private const int seed = 123456;
		private const int firstRandomInt = 570869451; // The first random int produced with this seed
		private const long firstRandomLong = firstRandomInt;
		private const double firstRandomDouble = 0.26583180356110997;
		private const float firstRandomFloat = (float)firstRandomDouble;
		private const decimal firstRandomDeciaml = (decimal)firstRandomDouble;

		[Fact]
		public void StringGenerator_ShouldUsePropertyNamePlusRandomNumber()
		{
			var expected = "MyProperty 570869451"; // property name + first random number from provided seed
			var factory = CreateFactoryWithRandomFunctions();
			var fakeSetter = new FakePropertySetter { Name = "MyProperty", PropertyType = typeof(string) };
			var func = factory.GetValueProvider(fakeSetter);
			Assert.Equal(expected, func());
		}

		[Fact]
		public void IntGenerator_ShouldUseRandomNumber()
		{
			var factory = CreateFactoryWithRandomFunctions();
			var fakeSetter = new FakePropertySetter { PropertyType = typeof(int) };
			var func = factory.GetValueProvider(fakeSetter);
			Assert.Equal(firstRandomInt, func());
		}

		[Fact]
		public void LongGenerator_ShouldUseRandomNumber()
		{
			var factory = CreateFactoryWithRandomFunctions();
			var fakeSetter = new FakePropertySetter { PropertyType = typeof(long) };
			var func = factory.GetValueProvider(fakeSetter);
			Assert.Equal(firstRandomLong, func());
		}

		[Fact]
		public void DoubleGenerator_ShouldUseRandomNumber()
		{
			var factory = CreateFactoryWithRandomFunctions();
			var fakeSetter = new FakePropertySetter { PropertyType = typeof(double) };
			var func = factory.GetValueProvider(fakeSetter);
			Assert.Equal(firstRandomDouble, func());
		}

		[Fact]
		public void FloatGenerator_ShouldUseRandomNumber()
		{
			var factory = CreateFactoryWithRandomFunctions();
			var fakeSetter = new FakePropertySetter { PropertyType = typeof(float) };
			var func = factory.GetValueProvider(fakeSetter);
			Assert.Equal(firstRandomFloat, func());
		}

		[Fact]
		public void DecimalGenerator_ShouldUseRandomNumber()
		{
			var factory = CreateFactoryWithRandomFunctions();
			var fakeSetter = new FakePropertySetter { PropertyType = typeof(decimal) };
			var func = factory.GetValueProvider(fakeSetter);
			Assert.Equal(firstRandomDeciaml, func());
		}

		[Fact]
		public void DateTimeGenerator_ShouldUseRandomNumbers()
		{
			var factory = CreateFactoryWithRandomFunctions();
			var fakeSetter = new FakePropertySetter { PropertyType = typeof(DateTime) };
			var func = factory.GetValueProvider(fakeSetter);
			var firstDate = func();
			var secondDate = func();
			Assert.NotEqual(new DateTime(), firstDate);
			Assert.NotEqual(firstDate, secondDate);
		}

		[Fact]
		public void DateTimeGenerator_ShouldOnlyGenerateDatesBetween1900and2100()
		{
			var factory = CreateFactoryWithRandomFunctions();
			var fakeSetter = new FakePropertySetter { PropertyType = typeof(DateTime) };
			var func = factory.GetValueProvider(fakeSetter);

			var min = new DateTime(1900, 01, 01);
			var max = new DateTime(2100, 01, 01);

			// TODO: Find a better way to test this
			for (int i = 0; i < 100000; i++)
				Assert.InRange(func(), min, max);
		}

		private ValueObjectFactoryFunctionCollection CreateFactoryWithRandomFunctions()
		{
			var factory = new ValueObjectFactoryFunctionCollection();
			factory.RegisterFactories(new RandomPrimativeGenerator(seed));
			return factory;
		}
	}
}
