namespace GuineaPig
{
	public interface IEntityPopulator
	{
		void PopulateInstance<T>(T instance);
	}
}