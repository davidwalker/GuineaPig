using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuineaPig
{
	public interface IValueObjectFactoryContainer
	{
		void RegisterWith(ValueObjectFactoryFunctionCollection factoryFunctionCollection);
	}
}
