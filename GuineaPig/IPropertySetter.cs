using System;

namespace GuineaPig
{
	public interface IPropertySetter
	{
		Type PropertyType { get; }
		string Name { get; }
		bool HasPublicSetter { get; }
		void SetValue(object instance, object value);
		object GetValue(object instance);
	}
}