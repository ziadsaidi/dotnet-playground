
using Moq;
using Sales.Application.Customers.Commands.CreateCustomer;
using Sales.Domain.Entities;
using Sales.Tests.Common;
using Sales.Domain.Common;

namespace Sales.Tests.Application.Customers.Commands
{
  public class CreateCustomerCommandTests : TestBase
  {
    private readonly CreateCustomerCommand _command;

    public CreateCustomerCommandTests()
    {
      _command = new CreateCustomerCommand(MockUnitOfWork.Object, MockValidator.Object);
    }

    [Fact]
    public async Task ExecuteAsyncShouldReturnValidationErrorsWhenModelIsInvalid()
    {
      // Arrange
      CreateCustomerModel model = new(Name: string.Empty);

      _ = MockValidator.Setup(v => v.ValidateAsync(model, default))
          .ReturnsAsync(new FluentValidation.Results.ValidationResult(
              new List<FluentValidation.Results.ValidationFailure>
              {
                      new("Name", "Name is required")
              }));

      // Act
     var result = await _command.ExecuteAsync(model);

      // Assert
      Assert.True(result.IsError);
      Assert.NotEmpty(result.Errors);

      // Verify repository methods
      MockUnitOfWork.Verify(u => u.Customers.ExistsAsync(model.Name, CancellationToken.None), Times.Never);
      MockUnitOfWork.Verify(u => u.Customers.AddAsync(It.IsAny<Customer>(), CancellationToken.None), Times.Never);
      MockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    [Fact]
    public async Task ExecuteAsyncShouldReturnDuplicateNameErrorWhenCustomerAlreadyExists()
    {
      // Arrange
      CreateCustomerModel model = new("ExistingCustomer");

      _ = MockValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
         .ReturnsAsync(new FluentValidation.Results.ValidationResult());

      _ = MockUnitOfWork.Setup(u => u.Customers.ExistsAsync(model.Name, CancellationToken.None)).ReturnsAsync(true);

      // Act
      ErrorOr.ErrorOr<Sales.Application.Customers.Common.Responses.CustomerResponse?> result = await _command.ExecuteAsync(model);

      // Assert
      Assert.True(result.IsError);
      Assert.Contains(Errors.CustomerErrors.DuplicateName, result.Errors);

      // Verify repository methods
      MockUnitOfWork.Verify(u => u.Customers.ExistsAsync(model.Name, CancellationToken.None), Times.Once);
      MockUnitOfWork.Verify(u => u.Customers.AddAsync(It.IsAny<Customer>(), CancellationToken.None), Times.Never);
      MockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsyncShouldCreateCustomerWhenValidModel()
    {
      // Arrange
      CreateCustomerModel model = new(Name: "NewCustomer");
      _ = MockValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
          .ReturnsAsync(new FluentValidation.Results.ValidationResult());

      _ = MockUnitOfWork.Setup(u => u.Customers.ExistsAsync(model.Name, CancellationToken.None)).ReturnsAsync(false);
      _ = MockUnitOfWork.Setup(u => u.Customers.AddAsync(It.IsAny<Customer>(), CancellationToken.None)).Returns(Task.CompletedTask);
      _ = MockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

      // Act
      ErrorOr.ErrorOr<Sales.Application.Customers.Common.Responses.CustomerResponse?> result = await _command.ExecuteAsync(model);

      // Assert
      Assert.False(result.IsError);
      Assert.Equal(model.Name, result.Value?.Name);

      // Verify repository methods
      MockUnitOfWork.Verify(u => u.Customers.ExistsAsync(model.Name, CancellationToken.None), Times.Once);
      MockUnitOfWork.Verify(u => u.Customers.AddAsync(It.IsAny<Customer>(), CancellationToken.None), Times.Once);
      MockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
  }
}
