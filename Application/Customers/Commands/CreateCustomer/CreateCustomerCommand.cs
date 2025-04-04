using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Common.Errors;
using ErrorOr;
using Application.Interfaces;
using Application.Customers.Common.Responses;

namespace Application.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommand : ICreateCustomerCommand
{
  private readonly IApplicationDbContext _context;
  private readonly IValidator<CreateCustomerModel> _validator;

  public CreateCustomerCommand(
      IApplicationDbContext context,
      IValidator<CreateCustomerModel> validator)
  {
    _context = context;
    _validator = validator;
  }

  public async Task<ErrorOr<CustomerResponse?>> ExecuteAsync(CreateCustomerModel model, CancellationToken cancellationToken = default)
  {
    // Validate using FluentValidation
    var validationResult = await _validator.ValidateAsync(model, cancellationToken);

    if (!validationResult.IsValid)
    {
      return validationResult.Errors
          .ConvertAll(error => Error.Validation(
              error.PropertyName,
              error.ErrorMessage));
    }

    // Check for duplicates
    var customerExists = await _context.Customers
    .AsNoTracking()
    .AnyAsync(c => EF.Functions.Like(c.Name, model.Name), cancellationToken);

    if (customerExists)
    {
      return Errors.Customer.DuplicateName;
    }

    // Create and persist the customer
    var customer = new Customer
    {
      Name = model.Name
    };

    _context.Customers.Add(customer);
    await _context.SaveChangesAsync(cancellationToken);

    // Return result
    return new CustomerResponse(
        customer.Id,
        customer.Name);
  }
}