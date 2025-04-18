﻿namespace FoodOrdering.Models;

public class OrderItem : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("size")]
    public ProductSize Size { get; set; }

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
    public Product Product { get; set; } = null!;

    public decimal CountTotal() => _quantity * Product.Price;
}

public enum ProductSize
{
    S, M, X, XL
}