namespace FoodOrdering.Models;

public class Product : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = null!;
    [Column("image")]
    public string Image { get; set; } = null!;
    [Column("price")]
    public decimal Price { get; set; }
}