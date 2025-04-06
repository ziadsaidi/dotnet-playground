
using Moq;
using Sales.Domain.Entities;
using Sales.Tests.Common;
using Sales.Domain.Common;
using Sales.Application.Customers.Commands.Create;

namespace Sales.Tests.Application.Customers.Commands
{
  public class CreateCustomerCommandTests : TestBase
  {
    private readonly CreateCustomerCommandHandler _command;

    public CreateCustomerCommandTests()
    {
      _command = new CreateCustomerCommandHandler(MockUnitOfWork.Object, MockValidator.Object);
    }

    [Fact]
    public async Task ExecuteAsyncShouldReturnValidationErrorsWhenModelIsInvalid()
    {
      // Arrange
      CreateCustomerCommand model = new(Name: string.Empty);

      _ = MockValidator.Setup(v => v.ValidateAsync(model, default))
          .ReturnsAsync(new FluentValidation.Results.ValidationResult(
              new List<FluentValidation.Results.ValidationFailure>
              {
                      new("Name", "Name is required")
              }));

      // Act
      var result = await _command.HandleAsync(model);

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
      CreateCustomerCommand model = new("ExistingCustomer");

      _ = MockValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
         .ReturnsAsync(new FluentValidation.Results.ValidationResult());

      _ = MockUnitOfWork.Setup(u => u.Customers.ExistsAsync(model.Name, CancellationToken.None)).ReturnsAsync(true);

      // Act
       var result = await _command.HandleAsync(model);

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
      CreateCustomerCommand model = new(Name: "NewCustomer");
      _ = MockValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
          .ReturnsAsync(new FluentValidation.Results.ValidationResult());

      _ = MockUnitOfWork.Setup(u => u.Customers.ExistsAsync(model.Name, CancellationToken.None)).ReturnsAsync(false);
      _ = MockUnitOfWork.Setup(u => u.Customers.AddAsync(It.IsAny<Customer>(), CancellationToken.None)).Returns(Task.CompletedTask);
      _ = MockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

      // Act
      var result = await _command.HandleAsync(model);

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
