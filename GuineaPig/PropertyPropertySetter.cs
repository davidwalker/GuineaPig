using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GuineaPig
{
	public class PropertyPropertySetter : IPropertySetter
	{
		private readonly PropertyInfo property;

		public PropertyPropertySetter(PropertyInfo property)
		{
			this.property = property;
		}

		public Type PropertyType
		{
			get { return property.PropertyType; }
		}

		public string Name
		{
			get { return property.Name; }
		}

		public bool HasPublicSetter
		{
			get
			{
				return property.GetSetMethod() != null;
			}
		}

		public void SetValue(object instance, object value)
		{
			property.SetValue(instance, value, null);
		}

		public object GetValue(object instance)
		{
			return property.GetValue(instance, null);
		}
	}

	public class FieldPropertySetter : IPropertySetter
	{
		private readonly FieldInfo field;

		public FieldPropertySetter(FieldInfo property)
		{
			this.field = property;
		}

		public Type PropertyType
		{
			get { return field.FieldType; }
		}

		public string Name
		{
			get { return field.Name; }
		}

		public bool HasPublicSetter
		{
			get { return field.IsPublic; }
		}

		public void SetValue(object instance, object value)
		{
			field.SetValue(instance, value);
		}

		public object GetValue(object instance)
		{
			return field.GetValue(instance);
		}
	}
}
