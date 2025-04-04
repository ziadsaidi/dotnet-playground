
using Sales.Domain.Common;

namespace Sales.Domain.Entities
{
  public class Sale : IEntity
  {
    private int _quantiy;

    private double _unitPrice;
    private double _totalPrice;
    public virtual Guid Id { get; } = Guid.NewGuid();

    public virtual required DateTime Date { get; init; }

    public virtual Guid? CustomerId { get; init; }  // Foreign key for Customer
    public virtual Customer? Customer { get; init; }

    public virtual Guid? EmployeeId { get; init; }  // Foreign key for Employee
    public virtual Employee? Employee { get; init; }

    public virtual Guid? ProductId { get; init; }  // Foreign key for Product
    public virtual Product? Product { get; init; }

    public virtual required double UnitPrice
    {
      get { return _unitPrice; }
      set
      {
        _unitPrice = value;
        UpdateTotalePrice();
      }
    }
    public virtual required int Quantity
    {
      get { return _quantiy; }
      set
      {
        _quantiy = value;
        UpdateTotalePrice();
      }
    }
    public virtual double TotalPrice { get; }

    private void UpdateTotalePrice()
    {
      _totalPrice = _quantiy * _unitPrice;
    }
  }
}
