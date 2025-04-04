using Domain.Common;

namespace Domain.Entities;

public class Sale : IEntity
{
  private int _quantiy;

  private double _unitPrice;
  private double _totalPrice;
  public Guid Id { get; } = Guid.CreateVersion7();

  public required DateTime Date { get; init; }

  public Guid? CustomerId { get; init; }  // Foreign key for Customer
  public Customer? Customer { get; init; }

  public Guid? EmployeeId { get; init; }  // Foreign key for Employee
  public Employee? Employee { get; init; }

  public Guid? ProductId { get; init; }  // Foreign key for Product
  public Product? Product { get; init; }

  public required double UnitPrice
  {
    get { return _unitPrice; }
    set
    {
      _unitPrice = value;
      UpdateTotalePrice();
    }
  }
  public required int Quantity
  {
    get { return _quantiy; }
    set
    {
      _quantiy = value;
      UpdateTotalePrice();
    }
  }
  public double TotalPrice { get; }

  private void UpdateTotalePrice()
  {
    _totalPrice = _quantiy * _unitPrice;
  }
}
