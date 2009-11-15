using System;
using BookShop.Domain;

namespace GuineaPig.Tests.Integration.BookShopSample.Tests
{
	public class EntityFactories
	{
		public Book CreateNewBook(Factory f)
		{
			var book = new Book();
			f.Build(book)
				.FillUninitialisedValueObjects()
				.Fill(b => b.Publisher)
				.Set(b => b.ISBN = GenerateIsbn());
			return book;
		}

		private string GenerateIsbn()
		{
			return "978123456";
		}
	}
}