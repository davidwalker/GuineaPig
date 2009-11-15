using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GuineaPig.Tests
{
	public class FactoryTests_WhenPropertyValueHasBeenSet
	{
		private void AssertPropertyIsDefault<T>(Func<TestEntity, T> propertyEvaluationFunction)
		{
			var factory = new Factory();
			var entity = factory.CreateNew<TestEntity>();
			var defaultValue = propertyEvaluationFunction(new TestEntity());
			var actualValue = propertyEvaluationFunction(entity);
			Assert.Equal(defaultValue, actualValue);
		}

		[Fact]
		public void CreateNew_ShouldNotOverwriteStringProperties()
		{
			AssertPropertyIsDefault(e => e.StringProperty);
		}

		[Fact]
		public void CreateNew_ShouldNotOverwriteIntProperties()
		{
			AssertPropertyIsDefault(e => e.IntProperty);
		}

		[Fact]
		public void CreateNew_ShouldNotOverwriteFloatProperties()
		{
			AssertPropertyIsDefault(e => e.FloatProperty);
		}

		[Fact]
		public void CreateNew_ShouldNotOverwriteDoubleProperties()
		{
			AssertPropertyIsDefault(e => e.DoubleProperty);
		}

		[Fact]
		public void CreateNew_ShouldNotOverwriteDecimalProperties()
		{
			AssertPropertyIsDefault(e => e.DecimalProperty);
		}

		[Fact]
		public void CreateNew_ShouldNotOverwriteDateTimeProperties()
		{
			AssertPropertyIsDefault(e => e.DateTimeProperty);
		}

		[Fact]
		public void CreateNew_ShouldNotOverwriteLongProperties()
		{
			AssertPropertyIsDefault(e => e.LongProperty);
		}


		public class TestEntity
		{

			public TestEntity()
			{
				StringProperty = "Initial Value";
				IntProperty = 1;
				LongProperty = 1;
				FloatProperty = 1f;
				DecimalProperty = 1m;
				DoubleProperty = 1d;
				DateTimeProperty = new DateTime(2009, 10, 2);
			}

			public string StringProperty { get; set; }
			public int IntProperty { get; set; }
			public long LongProperty { get; set; }
			public float FloatProperty { get; set; }
			public decimal DecimalProperty { get; set; }
			public double DoubleProperty { get; set; }
			public DateTime DateTimeProperty { get; set; }
		}
	}
}
