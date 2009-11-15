using System;

namespace GuineaPig.Tests.TestEntities
{
	public class SimpleTestEntityNoDefaultCtor
	{
		public SimpleTestEntityNoDefaultCtor(string parameter)
		{
			
		}

		public string StringProperty { get; set; }
		public int IntProperty { get; set; }
		public long LongProperty { get; set; }
		public float FloatProperty { get; set; }
		public decimal DecimalProperty { get; set; }
		public double DoubleProperty { get; set; }
		public DateTime DateProperty { get; set; }
	}
}