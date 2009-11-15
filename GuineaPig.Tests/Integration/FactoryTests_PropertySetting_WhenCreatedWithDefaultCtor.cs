using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuineaPig.Tests.TestEntities;
using Xunit;

namespace GuineaPig.Tests.Integration
{
	public class FactoryTests_PropertySetting_WhenCreatedWithDefaultCtor
	{
		// These are sanity tests to make sure a factory created with the default constructor sets properties.

		private Factory factory;
		private SimpleTestEntity entity;
		
		public FactoryTests_PropertySetting_WhenCreatedWithDefaultCtor()
		{
			factory = new Factory();
			entity = factory.CreateNew<SimpleTestEntity>();
		}

		[Fact]
		public void CreateNew_ShouldSetStringPropertyToNotBeNullOrEmpty()
		{
			Assert.NotEqual(null, entity.StringProperty);
			Assert.NotEqual("", entity.StringProperty);
		}

		[Fact]
		public void CreateNew_ShouldSetIntPropertyToNotBeZero()
		{
			Assert.NotEqual(0, entity.IntProperty);
		}

		[Fact]
		public void CreateNew_ShouldSetLongPropertyToNotBeZero()
		{
			Assert.NotEqual(0, entity.LongProperty);
		}

		[Fact]
		public void CreateNew_ShouldSetFloatPropertyToNotBeZero()
		{
			Assert.NotEqual(0, entity.FloatProperty);
		}

		[Fact]
		public void CreateNew_ShouldSetDoublePropertyToNotBeZero()
		{
			Assert.NotEqual(0, entity.DoubleProperty);
		}

		[Fact]
		public void CreateNew_ShouldSetDecimalPropertyToNotBeZero()
		{
			Assert.NotEqual(0, entity.DecimalProperty);
		}

		[Fact]
		public void CreateNew_ShouldSetDateTimePropertyToNotBeZero()
		{
			Assert.NotEqual(new DateTime(), entity.DateProperty);
		}

	}
}