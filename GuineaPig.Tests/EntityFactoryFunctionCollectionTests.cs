using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuineaPig.Tests.TestEntities;
using Xunit;

namespace GuineaPig.Tests
{
	public class EntityFactoryFunctionCollectionTests
	{
		[Fact]
		public void RegisterFactoryFunction_ShouldAddMethodThatTakesNoParameters()
		{
			var expected = new SimpleTestEntity();
			var col = new EntityFactoryFunctionCollection();
			col.RegisterFactoryFunction(() => expected);

			var factoryFunction = col.GetFactoryFunction<SimpleTestEntity>();

			Assert.Same(expected, factoryFunction(null));
		}

		[Fact]
		public void RegisterFactoryFunction_ShouldAddMethodThatTakesFactory()
		{
			var expectedFactory = new Factory();
			var col = new EntityFactoryFunctionCollection();
			Factory passed = null;
			col.RegisterFactoryFunction(f =>
											{
												passed = f;
												return new SimpleTestEntity();
											});

			var factoryFunction = col.GetFactoryFunction<SimpleTestEntity>();
			factoryFunction(expectedFactory);

			Assert.Same(expectedFactory, passed);
		}

		[Fact]
		public void RegisterFactoryFunction_ShouldThrowWhenFactoryFunctionAlreadyExistsForType()
		{
			var col = new EntityFactoryFunctionCollection();
			col.RegisterFactoryFunction(() => new SimpleTestEntity());

			Assert.Throws<ArgumentException>(() =>
				col.RegisterFactoryFunction(() => new SimpleTestEntity()));
		}

		[Fact]
		public void RegisterFactories_ShouldRegisterMethodsOfObjectThatTakeNoParam()
		{
			var collection = new EntityFactoryFunctionCollection();

			var factoryClass = new FactoryClassWithNoParam();
			collection.RegisterFactories(factoryClass);

			var factoryFunction = collection.GetFactoryFunction<SimpleTestEntity>();

			Assert.Equal(factoryClass.SimpleTestEntity, factoryFunction(new Factory()));
		}

		[Fact]
		public void RegisterFactories_ShouldRegisterMethodsOfObjectThatTakeFactoryParam()
		{
			var collection = new EntityFactoryFunctionCollection();

			var factoryClass = new FactoryClassWithParam();
			collection.RegisterFactories(factoryClass);

			var factoryFunction = collection.GetFactoryFunction<EntityWithMoneyProperty>();

			var expectedFactory = new Factory();
			Assert.Equal(factoryClass.EntityWithMoneyProperty, factoryFunction(expectedFactory));
			Assert.Equal(factoryClass.PassedFactory, expectedFactory);
		}

		public class FactoryClassWithNoParam
		{
			public SimpleTestEntity SimpleTestEntity = new SimpleTestEntity();
			
			public SimpleTestEntity CreateSimpleTestEntity()
			{
				return SimpleTestEntity;
			}
		}

		public class FactoryClassWithParam
		{
			public EntityWithMoneyProperty EntityWithMoneyProperty = new EntityWithMoneyProperty();
			public Factory PassedFactory;

			public EntityWithMoneyProperty CreateEntityWithMoneyProperty(Factory f)
			{
				PassedFactory = f;
				return EntityWithMoneyProperty;
			}
		}


	}
}
