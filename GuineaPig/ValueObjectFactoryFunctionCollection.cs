using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GuineaPig
{
	public class ValueObjectFactoryFunctionCollection
	{
		private delegate object FactoryFunction(IPropertySetter info);
		private readonly Dictionary<Type, MulticastDelegate> factoryFunctions = new Dictionary<Type, MulticastDelegate>();

		public static ValueObjectFactoryFunctionCollection CreateWithBasicTypeSupport()
		{
			var factory = new ValueObjectFactoryFunctionCollection();
			factory.RegisterFactories(new FixedValueFactoryContainer());
			return factory;
		}

		public void RegisterFactories(IValueObjectFactoryContainer factoryContainer)
		{
			factoryContainer.RegisterWith(this);
		}

		public void RegisterFactories(object factoryContainer)
		{
			var methods = factoryContainer.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
			foreach (var method in methods)
			{
				var methodInfo = method;
				var parameters = methodInfo.GetParameters();
				if (parameters.Length == 0 && methodInfo.ReturnType != null)
				{
					FactoryFunction func = i => methodInfo.Invoke(factoryContainer, null);
					factoryFunctions[methodInfo.ReturnType] = func;
				}
				else if (parameters.Length == 1 &&
					parameters[0].ParameterType == typeof(IPropertySetter) &&
					methodInfo.ReturnType != null)
				{
					FactoryFunction func = i => methodInfo.Invoke(factoryContainer, new object[] { i });
					factoryFunctions[methodInfo.ReturnType] = func;
				}
			}
		}

		public void RegisterFactoryFunction<T>(Func<T> constructorFunction)
		{
			RegisterFactoryFunction(i => constructorFunction());
		}

		public void RegisterFactoryFunction<T>(Func<IPropertySetter, T> constructorFunction)
		{
			factoryFunctions[typeof(T)] = constructorFunction;
		}

		public Func<object> GetValueProvider(IPropertySetter setter)
		{
			Func<object> func;
			if(!TryGetValueProvider(setter, out func))
				throw new UnrecognisedTypeException("No factory function registered for type " + setter.PropertyType.FullName);
			return func;
		}

		public bool TryGetValueProvider(IPropertySetter setter, out Func<object> factoryFunction)
		{
			MulticastDelegate ctor;
			if (factoryFunctions.TryGetValue(setter.PropertyType, out ctor))
			{
				factoryFunction = () => ctor.DynamicInvoke(setter);
				return true;
			}
			factoryFunction = null;
			return false;
		}

		public void Clear()
		{
			factoryFunctions.Clear();
		}
	}
}
