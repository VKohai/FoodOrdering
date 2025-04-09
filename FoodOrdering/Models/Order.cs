namespace FoodOrdering.Models;

public class Order : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("total")]
    public decimal Total
    {
        get
        {
            if (OrderItems.Count == 0) return 0M;
            return OrderItems.Sum(item => item.Quantity * item.Product.Price);
        }
    }

    [Column("status")]
    public OrderStatus Status { get; set; }

    [Column("user_id")]
    public string UserId { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = [];
}

public enum OrderStatus
{
    New, Cooking, Delivering, Delivered
}