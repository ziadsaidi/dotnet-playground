
namespace Sales.Application.Customers.Common.Responses;

public record CustomerResponse(Guid Id, string Name, string Address, string Phone, string Email);
