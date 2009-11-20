using System;
using System.Collections.Generic;
using System.Reflection;

namespace GuineaPig
{
	public class EntityFactoryFunctionCollection
	{
		// TODO: Would like to make this generic
		public delegate object EntityFactoryFunction(Factory factory);

		private Dictionary<Type, MulticastDelegate> entityObjectConstructors = new Dictionary<Type, MulticastDelegate>();

		public void RegisterFactories(object factoryContainer)
		{
			var methods = factoryContainer.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
			foreach (var method in methods)
			{
				var methodInfo = method;
				var parameters = methodInfo.GetParameters();
				if (parameters.Length == 0 && methodInfo.ReturnType != null)
				{
					EntityFactoryFunction func = ignored => methodInfo.Invoke(factoryContainer, null);
					entityObjectConstructors[methodInfo.ReturnType] = func;
				}
				else if (parameters.Length == 1 &&
					parameters[0].ParameterType == typeof(Factory) &&
					methodInfo.ReturnType != null)
				{
					EntityFactoryFunction func = i => methodInfo.Invoke(factoryContainer, new object[] { i });
					entityObjectConstructors[methodInfo.ReturnType] = func;
				}
			}
		}

		public void RegisterFactoryFunction<T>(Func<T> factoryFunction)
		{
			RegisterFactoryFunction<T>((f) => factoryFunction());
		}

		public void RegisterFactoryFunction<T>(Func<Factory, T> factoryFunction)
		{
			entityObjectConstructors.Add(typeof(T), factoryFunction);
		}

		public Func<Factory, T> GetFactoryFunction<T>()
		{
			Func<Factory, T> factoryFunction;
			if (!TryGetFactoryFunction<T>(out factoryFunction))
				throw new ArgumentException("No factory function registered for type " + typeof(T).FullName);
			return factoryFunction;
		}

		public bool TryGetFactoryFunction<T>(out Func<Factory, T> factoryFunction)
		{
			MulticastDelegate func;
			if (entityObjectConstructors.TryGetValue(typeof(T), out func))
			{
				factoryFunction = (f) => (T)func.DynamicInvoke(f);
				return true;
			}
			factoryFunction = null;
			return false;
		}
	}
}