namespace FoodOrdering.DTOs;

[Table("OrderItem")]
public class OrderItemDto : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("size")]
    public string Size { get; set; } = Enum.GetName(ProductSize.M)!;

    private int _quantity;

    [Column("quantity")]
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 0) value = 0;
            _quantity = value;
        }
    }

    [Column("product_id")]
    public int ProductId { get; set; }
}
