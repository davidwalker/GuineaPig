using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace GuineaPig
{
	public class PopulationContext<T>
	{
		private readonly T entity;
		private readonly Factory factory;

		public PopulationContext(T entity, Factory factory)
		{
			this.entity = entity;
			this.factory = factory;
		}

		public T Entity
		{
			get { return entity; }
		}

		public PopulationContext<T> Set(Action<T> callback)
		{
			callback(entity);
			return this;
		}

		public PopulationContext<T> Fill(Expression<Func<T, object>> exp)
		{
			var setter = CreateSetter(exp);

			Func<object> factoryFunction;
			if (factory.ValueObjects.TryGetValueProvider(setter, out factoryFunction))
			{
				setter.SetValue(entity, factoryFunction());
				return this;
			}

			var method = typeof(Factory)
								.GetMethod("CreateNew", new Type[0])
								.MakeGenericMethod(setter.PropertyType);
			try
			{
				var value = method.Invoke(factory, null);
				if (value != null)
				{
					setter.SetValue(entity, value);
					return this;
				}
			}
			catch (Exception)
			{
				throw new UnrecognisedTypeException("Unable to fill a valuefor type: " + setter.PropertyType);
			}

			return this;
		}

		public PopulationContext<T> FillUninitialisedValueObjects()
		{
			foreach (var setter in GetSetters())
			{
				Func<object> factoryFunction;
				if (factory.ValueObjects.TryGetValueProvider(setter, out factoryFunction))
					setter.SetValue(entity, factoryFunction());
			}
			return this;
		}

		private IEnumerable<IPropertySetter> GetSetters()
		{
			foreach (var propertyInfo in typeof(T).GetProperties())
				yield return new PropertyPropertySetter(propertyInfo);

			foreach (var fieldInfo in typeof(T).GetFields())
				yield return new FieldPropertySetter(fieldInfo);
		}

		private IPropertySetter CreateSetter(Expression<Func<T, object>> expression)
		{
			var name = GetPropertyName(expression);
			var propertyInfo = typeof (T).GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
			return new PropertyPropertySetter(propertyInfo);
		}

		private static string GetPropertyName<T_Value>(Expression<Func<T, T_Value>> expression)
		{
			return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
		}
	}
}