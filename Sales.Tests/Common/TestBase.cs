
using FluentValidation;
using Moq;
using Sales.Application.Common.Mapping;
using Sales.Application.Customers.Commands.Create;
using Sales.Application.Customers.Common.Responses;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Tests.Common
{
  public abstract class TestBase
  {
    internal readonly Mock<IUnitOfWork> MockUnitOfWork;
    internal readonly Mock<ICustomerRepository> MockCustomerRepository;
    internal readonly Mock<IEmployeeRepository> MockEmployeeRepository;
    internal readonly Mock<IValidator<CreateCustomerCommand>> MockValidator;

    internal readonly Mock<IMapper<Customer, CustomerResponse>> MockMapper;

    internal readonly Mock<IUserContext> MockUserContext;

    protected TestBase()
    {
      // Initialize mocks
      MockUnitOfWork = new Mock<IUnitOfWork>();
      MockCustomerRepository = new Mock<ICustomerRepository>();
      MockEmployeeRepository = new Mock<IEmployeeRepository>();
      MockValidator = new Mock<IValidator<CreateCustomerCommand>>();
      MockMapper = new Mock<IMapper<Customer, CustomerResponse>>();
      MockUserContext = new Mock<IUserContext>();

      // Set up default mock behavior
      _ = MockUnitOfWork.Setup(static u => u.Customers).Returns(MockCustomerRepository.Object);
      _ = MockUnitOfWork.Setup(static u => u.Employees).Returns(MockEmployeeRepository.Object);
    }
  }
}
