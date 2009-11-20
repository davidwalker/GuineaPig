using System;

namespace GuineaPig.Tests
{
	public class FakePropertySetter : IPropertySetter
	{
		public FakePropertySetter()
		{
			Name = "TestProperty";
		}

		public Type PropertyType
		{
			get; set;
		}

		public string Name
		{
			get; set;
		}

		public bool HasPublicSetter
		{
			get; set;
		}

		public void SetValue(object instance, object value)
		{
			
		}

		public object GetValue(object instance)
		{
			return null;
		}
	}
}