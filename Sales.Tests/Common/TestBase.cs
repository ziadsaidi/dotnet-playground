
using FluentValidation;
using Moq;
using Sales.Application.Customers;
using Sales.Application.Customers.Commands.Create;
using Sales.Application.Employees;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Tests.Common
{
  public abstract class TestBase
  {
    protected readonly Mock<IUnitOfWork> MockUnitOfWork;
    protected readonly Mock<ICustomerRepository> MockCustomerRepository;
    protected readonly Mock<IEmployeeRepository> MockEmployeeRepository;
    protected readonly Mock<IValidator<CreateCustomerCommand>> MockValidator;

    protected TestBase()
    {
      // Initialize mocks
      MockUnitOfWork = new Mock<IUnitOfWork>();
      MockCustomerRepository = new Mock<ICustomerRepository>();
      MockEmployeeRepository = new Mock<IEmployeeRepository>();
      MockValidator = new Mock<IValidator<CreateCustomerCommand>>();

      // Set up default mock behavior
      _ = MockUnitOfWork.Setup(u => u.Customers).Returns(MockCustomerRepository.Object);
      _ = MockUnitOfWork.Setup(u => u.Employees).Returns(MockEmployeeRepository.Object);
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
