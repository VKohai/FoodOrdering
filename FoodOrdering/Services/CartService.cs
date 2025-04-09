namespace FoodOrdering.Services;

public class CartService
{
    public ObservableCollection<OrderItem> OrderItems { get; private set; }

    public CartService()
    {
        OrderItems = new ObservableCollection<OrderItem>();
    }

    public void AddToCart(OrderItem orderItem)
    {
        // Check if the item already exists in the cart
        var existingItem = OrderItems.FirstOrDefault(item =>
            item.ProductId == orderItem.ProductId && item.Size == orderItem.Size);

        if (existingItem != null) {
            // If the item exists, increase its quantity
            existingItem.Quantity++;
        }
        else {
            // Otherwise, add the new item to the cart
            OrderItems.Add(orderItem);
            orderItem.Quantity = 1;
        }
    }

    public void RemoveFromCart(OrderItem item) => OrderItems.Remove(item);

    public void ClearCart()
    {
        // Clear all items from the cart
        OrderItems.Clear();
    }
}