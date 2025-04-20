namespace FoodOrdering.DTOs;
[Table("Product")]
public class ProductDto : BaseModel {
    [PrimaryKey("id", false)]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = null!;
    [Column("image")]
    public string Image { get; set; } = null!;
    [Column("price")]
    public decimal Price { get; set; }
}
