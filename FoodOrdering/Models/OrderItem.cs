using System.ComponentModel;

namespace FoodOrdering.Models;

public class OrderItem : BaseModel, INotifyPropertyChanged
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("size")]
    public ProductSize Size { get; set; }

    private int _quantity;

    public event PropertyChangedEventHandler? PropertyChanged;

    [Column("quantity")]
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 0) value = 0;
            _quantity = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
        }
    }

    [Column("product_id")]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public decimal CountTotal() => _quantity * Product.Price;
}

public enum ProductSize
{
    S, M, X, XL
}