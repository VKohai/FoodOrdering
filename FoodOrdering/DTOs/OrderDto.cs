namespace FoodOrdering.DTOs;

[Table("Order")]
public class OrderDto : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("total")]
    public decimal Total { get; set; }

    [Column("status")]
    public string Status { get; set; } = Enum.GetName(OrderStatus.New)!;

    [Column("user_id")]
    public string UserId { get; set; } = null!;
}
