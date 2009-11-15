using System;

namespace GuineaPig
{
	public interface IPropertySetter
	{
		Type PropertyType { get; }
		string Name { get; }
		void SetValue(object instance, object value);
	}
}