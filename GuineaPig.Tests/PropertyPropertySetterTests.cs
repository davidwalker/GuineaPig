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

		private PropertyInfo StringProperty
		{
			get { return typeof(SimpleTestEntity).GetProperty("StringProperty"); }
		}
	}
}
