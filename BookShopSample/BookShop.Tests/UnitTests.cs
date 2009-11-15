using System;
using BookShop.Domain;
using Xunit;

namespace GuineaPig.Tests.Integration.BookShopSample.Tests
{
	public class UnitTests : UnitTestsBase
	{
		[Fact]
		public void ExcerciseProductFactory()
		{
			var book = Factory.CreateNew<Book>(b => b.Title = "Test Title");

			Assert.NotNull(book);
			Assert.Equal("Test Title", book.Title);
			Assert.True(book.ISBN.StartsWith("978"));
		}
	}
}