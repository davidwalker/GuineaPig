using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GuineaPig.Tests.TestEntities;
using Rhino.Mocks;
using Xunit;

namespace GuineaPig.Tests
{
	public class ValueObjectFactoryFunctionCollectionTests
	{
		[Fact]
		public void ShouldUseRegisteredLambda_ForPropertiesOfMatchingType()
		{
			var factory = new ValueObjectFactoryFunctionCollection();

			factory.RegisterFactoryFunction(() => 12345);

			var providerFunc = factory.GetValueProvider(IntProperty);

			Assert.NotNull(providerFunc);
			Assert.Equal(12345, (int)providerFunc());
		}

		[Fact]
		public void ShouldPassIPropertySetterToCustomValueObjectCtor_IfRequested()
		{
			var expected = IntProperty;
			IPropertySetter actual = null;

			var factory = new ValueObjectFactoryFunctionCollection();
			factory.RegisterFactoryFunction(info => { actual = info; return 12345; });

			factory.GetValueProvider(expected)();

			Assert.Same(expected, actual);
		}

		[Fact]
		public void RegisterFactoryFunction_ShouldOverwriteExistingFunctionForSameType()
		{
			var expected = 67890;
			var factory = new ValueObjectFactoryFunctionCollection();

			factory.RegisterFactoryFunction(() => 12345);
			factory.RegisterFactoryFunction(() => expected);
			var func = factory.GetValueProvider(IntProperty);

			Assert.Equal(expected, func());
		}

		[Fact]
		public void GetValueProvider_ShouldThrowException_WhenNoFactoryFunctionHasBeenRegisteredWithReturnType()
		{
			var factory = new ValueObjectFactoryFunctionCollection();

			Assert.Throws<UnrecognisedTypeException>(
						() => factory.GetValueProvider(IntProperty));
		}

		[Fact]
		public void RegisterFactories_ShouldRegisterMethodsOfObjectThatTakeNoParam()
		{
			var factory = new ValueObjectFactoryFunctionCollection();

			factory.RegisterFactories(new FactoryClass());

			var providerFunc = factory.GetValueProvider(IntProperty);

			Assert.NotNull(providerFunc);
			Assert.Equal(123456, (int)providerFunc());
		}

		[Fact]
		public void RegisterFactories_ShouldRegisterMethodsOfObjectThatTakeMemberInfoParam()
		{
			var factory = new ValueObjectFactoryFunctionCollection();

			factory.RegisterFactories(new FactoryClass());

			var providerFunc = factory.GetValueProvider(StringProperty);

			Assert.NotNull(providerFunc);
			Assert.Equal("Injected Value", (string)providerFunc());
		}

		[Fact]
		public void GetValueProvider_ShouldCurryFunctionWithIPropertySetter_WhenRegisteredViaFactoryFunctionClass()
		{
			var expectedInfo = StringProperty;

			var factory = new ValueObjectFactoryFunctionCollection();
			var factoryClass = new FactoryClass();
			factory.RegisterFactories(factoryClass);

			var providerFunc = factory.GetValueProvider(expectedInfo);
			providerFunc();

			Assert.Equal(expectedInfo, factoryClass.PassedSetter);
		}

		[Fact]
		public void RegisterFactories_ShouldCallRegisterOn_IfPassedObjectImplementsIValueObjectFactoryContainer()
		{
			var mockFunctionGroup = MockRepository.GenerateStub<IValueObjectFactoryContainer>();

			var factory = new ValueObjectFactoryFunctionCollection();
			factory.RegisterFactories(mockFunctionGroup);

			mockFunctionGroup.AssertWasCalled(g => g.RegisterWith(factory));
		}

		private IPropertySetter IntProperty
		{
			get { return new PropertyPropertySetter(typeof(SimpleTestEntity).GetProperty("IntProperty")); }
		}

		private IPropertySetter StringProperty
		{
			get { return new PropertyPropertySetter(typeof(SimpleTestEntity).GetProperty("StringProperty")); }
		}

		private class FactoryClass
		{
			public IPropertySetter PassedSetter;

			public string CreateString(IPropertySetter info)
			{
				PassedSetter = info;
				return "Injected Value";
			}

			public int CreateInt()
			{
				return 123456;
			}
		}
	}
}
