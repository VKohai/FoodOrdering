namespace FoodOrdering.Models;

public class OrderItem : BaseModel
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public ProductSize Size { get; set; }

    private int _quantity;
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 0) value = 0;
            _quantity = value;
        }
    }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}

public enum ProductSize
{
    S, M, X, XL
}