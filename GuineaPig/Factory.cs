using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GuineaPig
{
	public class Factory
	{
		private const string NoParameterlessCtorMessage = "Unable to create instance of {0} because the requested type does not define a parameterless constructor and no custom factory method was registered.";

		private IEntityPopulator populator;
		public EntityFactoryFunctionCollection Entities { get; private set; }
		public ValueObjectFactoryFunctionCollection ValueObjects { get; private set; }

		public Factory()
		{
			Entities = new EntityFactoryFunctionCollection();
			ValueObjects = ValueObjectFactoryFunctionCollection.CreateWithBasicTypeSupport();
			populator = new EntityPopulator(ValueObjects);
		}

		public IEntityPopulator Populator
		{
			get { return populator; }
		}

		public T CreateNew<T>() where T : class
		{
			return CreateWithRegisteredCtor<T>() ?? CreateDefault<T>();
		}

		public T CreateNew<T>(Action<T> callback) where T : class, new()
		{
			var instance = CreateNew<T>();
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
				PopulateInstance(instance);
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

		private void PopulateInstance<T>(T instance)
		{
			populator.PopulateInstance(instance);
		}

		public PopulationContext<T> Build<T>(T entity)
		{
			return new PopulationContext<T>(entity, this);
		}
	}
}
