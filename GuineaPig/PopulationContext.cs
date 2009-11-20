using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		public PopulationContext<T> Set(Expression<Func<T, object>> exp, object value)
		{
			var setter = CreateSetter(exp);
			setter.SetValue(Entity, value);
			return this;
		}

		public PopulationContext<T> Fill(Expression<Func<T, object>> exp)
		{
			var setter = CreateSetter(exp);

			try
			{
				var factoryFunction = GetValueObjectFactoryFunction(setter)
										?? GetEntityFactoryFunction(setter);

				var value = factoryFunction();
				if (value != null)
					setter.SetValue(entity, value);
			}
			catch (Exception)
			{
				throw new UnrecognisedTypeException("Unable to fill a value for type: " + setter.PropertyType);
			}

			return this;
		}

		private Func<object> GetEntityFactoryFunction(IPropertySetter setter)
		{
			var method = typeof(Factory)
				.GetMethod("Create", new Type[0])
				.MakeGenericMethod(setter.PropertyType);
			return () => method.Invoke(factory, null);
		}

		private Func<object> GetValueObjectFactoryFunction(IPropertySetter setter)
		{
			Func<object> factoryFunction;
			if (!factory.ValueObjects.TryGetValueProvider(setter, out factoryFunction))
			{
				factoryFunction = null;
			}
			return factoryFunction;
		}

		public PopulationContext<T> FillUninitialisedValueObjects()
		{
			foreach (var setter in GetSetters().Where(s => s.HasPublicSetter))
			{
				if(!IsPropertyDefaultValue(entity, setter))
					continue;

				var factoryFunction = GetValueObjectFactoryFunction(setter);
				if (factoryFunction != null)
					setter.SetValue(entity, factoryFunction());
			}
			return this;
		}

		private bool IsPropertyDefaultValue(T instance, IPropertySetter setter)
		{
			object defaultValue = null;
			DefaultValues.TryGetValue(setter.PropertyType, out defaultValue);
			return object.Equals(setter.GetValue(instance), defaultValue);
		}

		// TODO: Move this so it lives nearer the value object factory functions.
		protected Dictionary<Type, object> DefaultValues
		{
			get
			{
				return new Dictionary<Type, object>
				{
					{typeof(string), null},
					{typeof(int), 0},
					{typeof(long), 0L},
					{typeof(float) , 0f},
					{typeof(decimal), 0m},
					{typeof(double), 0d},
					{typeof(DateTime), new DateTime()}
				};
			}
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
			var propertyInfo = typeof(T).GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
			return new PropertyPropertySetter(propertyInfo);
		}

		private static string GetPropertyName<T_Value>(Expression<Func<T, T_Value>> expression)
		{
			return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
		}
	}
}