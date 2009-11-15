using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuineaPig.Tests.TestEntities;
using Xunit;

namespace GuineaPig.Tests
{
	public class Factory_BuildTests
	{
		[Fact]
		public void Fill_ShouldSetPropertyValueOfValueObjectType()
		{
			var entity = new SimpleTestEntity();
			var factory = new Factory();
			factory.ValueObjects.RegisterFactoryFunction(() => "Hello");

			factory.Build(entity)
				.Fill(e => e.StringProperty);

			Assert.Equal("Hello", entity.StringProperty);
		}

		[Fact]
		public void Fill_ShouldThrowUnrecognisedTypeException_WhenNoFactoryIsRegisteredForPropertyType()
		{
			var entity = new SimpleTestEntity();
			var factory = new Factory();
			factory.ValueObjects.Clear();

			var builder = factory.Build(entity);

			Assert.Throws<UnrecognisedTypeException>(() => builder.Fill(e => e.StringProperty));
		}

		[Fact]
		public void Fill_ShouldSetInstanceOfEntity_WhenFactoryIsAbleToConstruct()
		{
			var entity = new EntityWithSimpleTestEntityProperty();
			var factory = new Factory();
			factory.Build(entity)
				.Fill(e => e.SimpleTestEntity);

			Assert.NotNull(entity.SimpleTestEntity);
			Assert.NotEqual(0, entity.SimpleTestEntity.IntProperty);
		}

		[Fact]
		public void SetAllValueObjects_ShouldSetAllValueObjectProperties()
		{
			var entity = new SimpleTestEntity();
			var factory = new Factory();

			factory.Build(entity).FillUninitialisedValueObjects();

			Assert.NotEqual(0, entity.IntProperty);
			Assert.NotEqual(0, entity.LongProperty);
			Assert.NotEqual(0, entity.DecimalProperty);
			Assert.NotEqual(0, entity.FloatProperty);
			Assert.NotEqual(0, entity.DoubleProperty);
			Assert.NotEqual(new DateTime(), entity.DateProperty);
			Assert.NotEqual("", entity.StringProperty);
			Assert.NotNull(entity.StringProperty);
		}

		[Fact]
		public void SetAllValueObjects_ShouldSetAllValueObjectFields()
		{
			var entity = new SimpleTestEntityWithFields();
			var factory = new Factory();

			factory.Build(entity).FillUninitialisedValueObjects();

			Assert.NotEqual(0, entity.IntField);
			Assert.NotEqual(0, entity.LongField);
			Assert.NotEqual(0, entity.DecimalField);
			Assert.NotEqual(0, entity.FloatField);
			Assert.NotEqual(0, entity.DoubleField);
			Assert.NotEqual(new DateTime(), entity.DateField);
			Assert.NotEqual("", entity.StringField);
			Assert.NotNull(entity.StringField);
		}


	}
}
