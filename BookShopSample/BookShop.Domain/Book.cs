using System;

namespace BookShop.Domain
{
	public class Book
	{
		public string ISBN { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public Money Price { get; set; }
		public DateTime PublishedOn { get; set; }
		public Publisher Publisher { get; set; }
	}
}