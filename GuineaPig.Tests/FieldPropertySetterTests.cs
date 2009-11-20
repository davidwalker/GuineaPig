using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace GuineaPig.Tests
{
	public class FieldPropertySetterTests
	{
		[Fact]
		public void HasPublicSetter_ShouldBeFalse_WhenFieldIsPrivate()
		{
			var setter = new FieldPropertySetter(typeof(TestEntity).GetField("PrivateField", BindingFlags.NonPublic | BindingFlags.Instance));

			Assert.False(setter.HasPublicSetter);
		}

		[Fact]
		public void HasPublicSetter_ShouldBeTrue_WhenFieldIsPublic()
		{
			var setter = new FieldPropertySetter(typeof(TestEntity).GetField("PublicField"));

			Assert.True(setter.HasPublicSetter);
		}

		[Fact]
		public void GetValue_ShouldReturnFieldValue()
		{
			var expected = "expected";
			var entity = new TestEntity() { PublicField = expected };

			var setter = new FieldPropertySetter(typeof (TestEntity).GetField("PublicField"));
			Assert.Equal(expected, setter.GetValue(entity));
		}


		class TestEntity
		{
			private string PrivateField;
			public string PublicField;
		}
	}
}
