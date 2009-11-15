using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GuineaPig
{
	public class EntityPopulator : IEntityPopulator
	{
		private readonly ValueObjectFactoryFunctionCollection factoryFunctionCollection;

		private Dictionary<Type, object> defaults = new Dictionary<Type, object>
				{
					{typeof(string), null},
					{typeof(int), 0},
					{typeof(long), 0L},
					{typeof(float) , 0f},
					{typeof(decimal), 0m},
					{typeof(double), 0d},
					{typeof(DateTime), new DateTime()}
				};

		public EntityPopulator(ValueObjectFactoryFunctionCollection factoryFunctionCollection)
		{
			this.factoryFunctionCollection = factoryFunctionCollection;
		}

		public void PopulateInstance<T>(T instance)
		{
			var properties = typeof(T).GetProperties();
			foreach (var propertyInfo in properties)
				SetPropertyValue(instance, propertyInfo);
		}

		private void SetPropertyValue<T>(T instance, PropertyInfo propertyInfo)
		{
			if (!IsPropertyDefaultValue(instance, propertyInfo))
				return;

			// TODO: Fix me
			var valueProvider = factoryFunctionCollection.GetValueProvider(new PropertyPropertySetter(propertyInfo));
			if (valueProvider != null)
				propertyInfo.SetValue(instance, valueProvider(), null);
		}

		private bool IsPropertyDefaultValue<T>(T instance, PropertyInfo propertyInfo)
		{
			object value = null;
			defaults.TryGetValue(propertyInfo.PropertyType, out value);
			return object.Equals(propertyInfo.GetValue(instance, null), value);
		}
	}
}
