
// using Moq;
// using Sales.Domain.Entities;
// using Sales.Tests.Common;
// using Sales.Application.Customers.Commands.Create;
// using ErrorOr;
// using Sales.Application.Customers.Common.Responses;

// namespace Sales.Tests.Application.Customers.Commands
// {
//   public class CreateCustomerCommandTests : TestBase
//   {
//     private readonly CreateCustomerCommandHandler _command;

//     public CreateCustomerCommandTests()
//     {
//       _command = new CreateCustomerCommandHandler(MockUnitOfWork.Object, MockMapper.Object, MockUserContext.Object);
//     }

//    [Fact]
//         public async Task HandleAsync_ShouldReturnUserErrors_WhenUserContextFails()
//         {
//             // Arrange
//             var model = new CreateCustomerCommand("Test Address");
//             var expectedErrors = new List<Error> { Error.Validation("User.Authentication", "User not authenticated") };

//             MockUserContext
//                 .Setup(c => c.GetAuthenticatedUserId())
//                 .Returns(ErrorOr<Guid>.From(expectedErrors));

//             // Act
//             var result = await _command.HandleAsync(model);

//             // Assert
//             Assert.True(result.IsError);
//             Assert.Equal(expectedErrors, result.Errors);

//             // Verify repository methods were never called
//             MockUnitOfWork.Verify(u => u.Customers.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
//             MockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
//         }

//         [Fact]
//         public async Task HandleAsync_ShouldCreateCustomer_WhenModelIsValid()
//         {
//             // Arrange
//             var model = new CreateCustomerCommand("28 quai");
//             var userId = Guid.NewGuid();
//             var customer = Customer.Create(userId, model.Address);
//             var expectedResponse = new CustomerResponse(customer.Id, "Test User",);

//             MockUserContext
//                 .Setup(c => c.GetAuthenticatedUserId())
//                 .Returns(ErrorOr<Guid>.From(userId));

//             MockUnitOfWork
//                 .Setup(u => u.Customers.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
//                 .Returns(Task.CompletedTask);

//             MockUnitOfWork
//                 .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
//                 .ReturnsAsync(1);

//             _mockCustomerMapper
//                 .Setup(m => m.Map(It.IsAny<Customer>()))
//                 .Returns(expectedResponse);

//             // Act
//             var result = await _handler.HandleAsync(model);

//             // Assert
//             Assert.False(result.IsError);
//             Assert.Equal(expectedResponse, result.Value);

//             // Verify repository methods were called
//             MockUnitOfWork.Verify(u => u.Customers.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
//             MockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
//             _mockCustomerMapper.Verify(m => m.Map(It.IsAny<Customer>()), Times.Once);
//         }

//         [Fact]
//         public async Task HandleAsync_ShouldUseTheMapper_ToTransformCustomerToResponse()
//         {
//             // Arrange
//             var model = new CreateCustomerCommand("123 Test Street");
//             var userId = Guid.NewGuid();
//             var expectedCustomerId = Guid.NewGuid();
//             var expectedName = "John Doe";
//             var expectedResponse = new CustomerResponse(expectedCustomerId, expectedName);

//             MockUserContext
//                 .Setup(c => c.GetAuthenticatedUserId())
//                 .Returns(ErrorOr<Guid>.From(userId));

//             MockUnitOfWork
//                 .Setup(u => u.Customers.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
//                 .Returns(Task.CompletedTask)
//                 .Callback<Customer, CancellationToken>((c, _) => 
//                 {
//                     // Simulate setting the Id property that would happen in a real repository
//                     // This is a bit of a hack since Customer.Id is probably read-only
//                     typeof(Customer).GetProperty("Id")?.SetValue(c, expectedCustomerId);
//                 });

//             MockUnitOfWork
//                 .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
//                 .ReturnsAsync(1);

//             _mockMapper
//                 .Setup(m => m.Map(It.IsAny<Customer>()))
//                 .Returns(expectedResponse);

//             // Act
//             var result = await _handler.HandleAsync(model);

//             // Assert
//             Assert.False(result.IsError);
//             Assert.Equal(expectedCustomerId, result.Value.Id);
//             Assert.Equal(expectedName, result.Value.Name);

//             _mockCustomerMapper.Verify(m => m.Map(It.Is<Customer>(c => 
//                 c.Id == expectedCustomerId && 
//                 c.Address == model.Address)), 
//                 Times.Once);
//         }
//     }
// }