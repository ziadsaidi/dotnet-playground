
using FluentValidation;
using Moq;
using Sales.Application.Customers;
using Sales.Application.Customers.Commands.CreateCustomer;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Tests.Common
{
  public abstract class TestBase
  {
    protected readonly Mock<IUnitOfWork> MockUnitOfWork;
    protected readonly Mock<ICustomerRepository> MockCustomerRepository;
    protected readonly Mock<IValidator<CreateCustomerModel>> MockValidator;

    protected TestBase()
    {
      // Initialize mocks
      MockUnitOfWork = new Mock<IUnitOfWork>();
      MockCustomerRepository = new Mock<ICustomerRepository>();
      MockValidator = new Mock<IValidator<CreateCustomerModel>>();

      // Set up default mock behavior
      _ = MockUnitOfWork.Setup(u => u.Customers).Returns(MockCustomerRepository.Object);
    }

    /// <summary>
    /// Generates a fake customer entity for testing.
    /// </summary>
    protected static Customer CreateFakeCustomer(string name = "Test Customer")
    {
      return new Customer
      {
        Name = name
      };
    }
  }
}
