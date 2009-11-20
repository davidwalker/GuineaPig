using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GuineaPig
{
	public class Factory
	{
		private const string NoParameterlessCtorMessage = "Unable to create instance of {0} because the requested type does not define a parameterless constructor and no custom factory method was registered.";

		public EntityFactoryFunctionCollection Entities { get; private set; }
		public ValueObjectFactoryFunctionCollection ValueObjects { get; private set; }

		public Factory()
		{
			Entities = new EntityFactoryFunctionCollection();
			ValueObjects = ValueObjectFactoryFunctionCollection.CreateWithBasicTypeSupport();
		}

		public T Create<T>() where T : class
		{
			return CreateWithRegisteredCtor<T>() ?? CreateDefault<T>();
		}

		public T Create<T>(Action<T> callback) where T : class, new()
		{
			var instance = Create<T>();
			if (callback != null)
				callback(instance);
			return instance;
		}

		private T CreateWithRegisteredCtor<T>() where T : class
		{
			Func<Factory, T> ctor;
			if (Entities.TryGetFactoryFunction(out ctor))
				return ctor(this);
			return null;
		}

		private T CreateDefault<T>()
		{
			try
			{
				var instance = (T)Activator.CreateInstance(typeof(T));
				Build(instance).FillUninitialisedValueObjects();
				return instance;
			}
			catch (MissingMethodException e)
			{
				var message = string.Format(
								NoParameterlessCtorMessage,
								typeof(T).FullName);
				throw new MissingMethodException(message, e);
			}
		}

		public PopulationContext<T> Build<T>(T entity)
		{
			return new PopulationContext<T>(entity, this);
		}
	}
}
