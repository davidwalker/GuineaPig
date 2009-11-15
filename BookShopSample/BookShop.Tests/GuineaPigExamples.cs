using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookShop.Domain;
using GuineaPig;
using Xunit;

namespace BookShop.Tests
{
public class GunieaPigExamples
{
	public void SimplestUsage()
	{
		// Create entity and auto populates primitive properties
		var factory = new GuineaPig.Factory();
		var book = factory.CreateNew<Book>();
	}

	public void CustomiseInstance()
	{
		// Customise the instance using a lambda
		var factory = new GuineaPig.Factory();
		var book = factory.CreateNew<Book>(b => b.Title = "Test Title");
	}

	public void ChangeHowPrimitivesAreGenerated()
	{
		// Change the strategy for populating primitives.
		var factory = new GuineaPig.Factory();
		factory.ValueObjects.RegisterFactories(new RandomPrimativeGenerator());
		var book = factory.CreateNew<Book>();
	}

	public void CustomValueType()
	{
		// Define your own primitive factory methods for things like money, 
		// rectangle, etc.
		var factory = new GuineaPig.Factory();
		factory.ValueObjects.RegisterFactoryFunction(
			() => new Money(21, "USD"));

		var book = factory.CreateNew<Book>();
	}

	public void CustomEntity()
	{
		// Define custom entity factory methods when you want more control 
		// over the creation or for types that require special construction.
		var factory = new GuineaPig.Factory();
		factory.Entities.RegisterFactoryFunction(
			() => new Customer("Some Required Ctor Param"));
		var customer = factory.CreateNew<Customer>();
	}

	public void CustomEntityUsingFactory()
	{
		// Take an instance of the factory in your factory method and use it
		// to populate primitives
		var factory = new GuineaPig.Factory();
		factory.Entities.RegisterFactoryFunction(
			f => f.Build(new Customer("Some Required Ctor Param"))
				.FillUninitialisedValueObjects()
				.Entity);
		var customer = factory.CreateNew<Customer>();
	}

	public void CustomEntityWithFactoryFillSelectProperties()
	{
		// Populate only desired properties, both entity and value types
		var factory = new GuineaPig.Factory();
		factory.Entities.RegisterFactoryFunction(
			f => f.Build(new Book())
				.Fill(b => b.Publisher) // Populate entity using factory
				.Fill(b => b.PublishedOn) // Populate value object using factory
				.Set(b => b.ISBN = "12345678") // Explicitly set property
				.Entity);
		var book = factory.CreateNew<Book>();
	}

	public void FactoryMethodsContainedInAFactoryClass()
	{
		// Register many factory methods by grouping them in a class.
		var factory = new GuineaPig.Factory();
		factory.Entities.RegisterFactories(new EntityFactoryClass());
		var book = factory.CreateNew<Book>();
	}
	public class EntityFactoryClass
	{
		public Customer CreateCustomer()
		{
			return new Customer("Some Required Ctor Param");
		}

		public Book CreateBook(Factory factory)
		{
			return factory.Build(new Book())
				.FillUninitialisedValueObjects()
				.Fill(b => b.Publisher)
				.Set(b => b.ISBN = "12345678")
				.Entity;
		}
	}
}
}
