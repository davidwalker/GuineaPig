using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GuineaPig.Tests.TestEntities;
using Xunit;

namespace GuineaPig.Tests
{
	public class PropertyPropertySetterTests
	{
		[Fact]
		public void PropertyType_ShouldReturnPropertyTypeOfProperty()
		{
			var setter = new PropertyPropertySetter(StringProperty);
			Assert.Equal(typeof(string), setter.PropertyType);
		}

		[Fact]
		public void Name_ShouldReturnNameOfProperty()
		{
			var setter = new PropertyPropertySetter(StringProperty);
			Assert.Equal("StringProperty", setter.Name);
		}

		[Fact]
		public void SetValue_ShouldApplyValueToInstance()
		{
			var entity = new SimpleTestEntity();

			var setter = new PropertyPropertySetter(StringProperty);

			setter.SetValue(entity, "Test Value");

			Assert.Equal("Test Value", entity.StringProperty);
		}

		[Fact]
		public void HasPublicSetter_ShouldBeFalse_WhenSetterIsPrivate()
		{
			var setter = new PropertyPropertySetter(PrivateSetterProperty);

			Assert.False(setter.HasPublicSetter);
		}

		[Fact]
		public void HasPublicSetter_ShouldBeFalse_WhenThereIsNoSetter()
		{
			var setter = new PropertyPropertySetter(NoSetterProperty);

			Assert.False(setter.HasPublicSetter);
		}

		[Fact]
		public void HasPublicSetter_ShouldBeTrue_WhenSetterIsPublic()
		{
			var setter = new PropertyPropertySetter(StringProperty);

			Assert.True(setter.HasPublicSetter);
		}

		[Fact]
		public void GetValue_ShouldReturnValueOfProperty()
		{
			var expected = "Expected";
			var entity = new SimpleTestEntity(){ StringProperty = expected};

			var setter = new PropertyPropertySetter(StringProperty);

			Assert.Equal(expected, setter.GetValue(entity));
		}

		private PropertyInfo StringProperty
		{
			get { return typeof(SimpleTestEntity).GetProperty("StringProperty"); }
		}

		private PropertyInfo PrivateSetterProperty
		{
			get { return typeof(EntityWithPrivateProperty).GetProperty("PrivateSetterProperty"); }
		}

		private PropertyInfo NoSetterProperty
		{
			get { return typeof(EntityWithPrivateProperty).GetProperty("NoSetterProperty"); }
		}
	}
}
