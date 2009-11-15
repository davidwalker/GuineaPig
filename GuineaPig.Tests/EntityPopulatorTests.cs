using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Mocks;
using Xunit;

namespace GuineaPig.Tests
{
	public class EntityPopulatorTests
	{
		//[Fact(Skip="Not sure of the best way to test this")]
		//public void PopulateInstance_ShouldSetPropertiesOfInstanceToValuesProvidedByPrimativeFactory()
		//{
		//    //var mockPrimativeFactory = new MockPrimativeFactory();
		//    var populator = new EntityPopulator();
		//    var entity = new SimpleTestEntity();
		//    populator.PopulateInstance(entity);


		//}


		//[Fact(Skip = "Not ready for this yet")]
		//public void SetProprety_ShouldUseLambdaParamterToAccessPropertyAndSetValue()
		//{
		//    var expected = 12345m;
		//    var provider = new EntityPopulator();
		//    //provider.RegisterFactoryFunction(() => expected);

		//    var entity = new SimpleTestEntity();
		//    //provider.SetValue(entity, e => e.DecimalProperty);

		//    Assert.Equal(expected, entity.DecimalProperty);
		//}
	}

	//public class MockPrimativeFactory : PrimativeFactory
	//{
	//    private Func<object> ProviderFunction;


	//    public override Func<object> GetValueProvider(System.Reflection.PropertyInfo propertyInfo)
	//    {
	//        ProviderFunction = new Func<object>();
	//        return ProviderFunction;
	//    }
	//}
}
