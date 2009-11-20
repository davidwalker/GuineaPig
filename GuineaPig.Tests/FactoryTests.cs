using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuineaPig.Tests.TestEntities;
using Rhino.Mocks;
using Xunit;

namespace GuineaPig.Tests
{
	public class FactoryTests
	{
		[Fact]
		public void CreateNew_ShouldCreateInstanceOfTypeParameter()
		{
			var factory = new Factory();
			var entity = factory.Create<SimpleTestEntity>();

			Assert.NotNull(entity);
			Assert.IsAssignableFrom<SimpleTestEntity>(entity);
		}

		[Fact]
		public void CreateNew_ShouldUseRegisteredFactoryFunction()
		{
			var expectedEntity = new SimpleTestEntity();
			var factory = new Factory();
			factory.Entities.RegisterFactoryFunction(() => expectedEntity);

			var entity = factory.Create<SimpleTestEntity>();

			Assert.Same(expectedEntity, entity);
		}

		[Fact]
		public void CreateNew_ShouldStillUseRegisteredFactoryFunction_WhenTypeDoesNotHaveADefaultCtor()
		{
			var factory = new Factory();
			factory.Entities.RegisterFactoryFunction(() => new SimpleTestEntityNoDefaultCtor("a"));

			var entity = factory.Create<SimpleTestEntityNoDefaultCtor>();
			Assert.NotNull(entity);
		}

		[Fact]
		public void CreateNew_ShouldInvokePassedCallbackWithNewEntity()
		{
			var factory = new Factory();
			var entity = factory.Create<TestEntity>(e => e.Name = "Explicit Value");

			Assert.Equal("Explicit Value", entity.Name);
		}

		[Fact]
		public void CreateNew_ShouldInvokePassedCallbackWithNewEntity_WhenEntityIsCreatedWithRegisteredFunction()
		{
			var factory = new Factory();
			factory.Entities.RegisterFactoryFunction(() => new TestEntity { Name = "Initial" });

			var entity = factory.Create<TestEntity>(e => e.Name = "Explicit Value");

			Assert.Equal("Explicit Value", entity.Name);
		}

		[Fact]
		public void CreateNew_ShouldPopulatePrimatives_WhenNotUsingFactoryFunction()
		{
			var factory = new Factory();
			factory.ValueObjects.RegisterFactoryFunction(() => "Expected");

			var entity = factory.Create<TestEntity>();

			Assert.Equal("Expected", entity.Name);
		}

		[Fact]
		public void CreateNew_ShouldNotPopulatePrimatives_WhenUsingFactoryFunction()
		{
			var factory = new Factory();
			factory.Entities.RegisterFactoryFunction(() => new TestEntity { Name = "Expected" });
			factory.ValueObjects.RegisterFactoryFunction(() => "Not Expected");

			var entity = factory.Create<TestEntity>();

			Assert.Equal("Expected", entity.Name);
		}

		[Fact]
		public void CreateNew_ShouldPassItselfToRegisteredFactoryMethod_WhenFactoryMethodWantsIt()
		{
			Factory actual = null;
			var factory = new Factory();
			factory.Entities.RegisterFactoryFunction<TestEntity>((f) => { actual = f; return new TestEntity(); });

			factory.Create<TestEntity>();
			Assert.Same(factory, actual);
		}

		[Fact]
		public void CreateNew_ShouldThrowMissingMethodException_WhenTypeHasNoDefaultCtorAndNoCustomCreateMethod()
		{
			var factory = new Factory();

			var exception = Assert.Throws<MissingMethodException>(
							() => factory.Create<SimpleTestEntityNoDefaultCtor>());

			var expectedMessage = string.Format(
				"Unable to create instance of {0} because the requested type does not define a parameterless constructor and no custom factory method was registered.", 
				typeof(SimpleTestEntityNoDefaultCtor).FullName);

			Assert.Equal(expectedMessage, exception.Message);
		}

		public class TestEntity
		{
			public string Name { get; set; }
		}
	}
}
