using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuineaPig.Tests.TestEntities
{
	public class EntityWithPrivateProperty
	{
		public string PrivateSetterProperty { get; private set; }

		private string noSetterProperty;
		public string NoSetterProperty
		{
			get { return noSetterProperty; }
		}
	}
}
