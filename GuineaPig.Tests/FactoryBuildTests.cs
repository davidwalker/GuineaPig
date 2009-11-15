using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GuineaPig.Tests
{
	public class FactoryBuildTests
	{

		//[Fact]
		//public void Set_ShouldSetPropertyValue()
		//{
		//    var entity = new SimpleTestEntity();
		//    var factory = new Factory();
		//    factory.Entities.RegisterFactoryFunction(() => "Hello");

		//    factory.Populate(entity)
		//        .Set(e => e.StringProperty);

		//    Assert.Equal("Hello", entity.StringProperty);
		//}

		//[Fact]
		//public void Set_ShouldThrowUnrecognisedTypeException_WhenNoFactoryIsRegisteredForPropertyType()
		//{
		//    var entity = new SimpleTestEntity();
		//    var factory = new PrimativeFactory();

		//    var populator = factory.Populate(entity);

		//    Assert.Throws<UnrecognisedTypeException>(() => populator.Set(e => e.StringProperty));
		//}
	}
}
