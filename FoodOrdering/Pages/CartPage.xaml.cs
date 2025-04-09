using Supabase.Postgrest;

namespace FoodOrdering.Pages;

public partial class CartPage : ContentPage
{
    private decimal _total;
    private readonly CartService _cartService;

    public CartPage(CartService cartService)
    {
        _cartService = cartService;
        InitializeComponent();
        OrderItemsCollection.ItemsSource = _cartService.OrderItems;
    }

    protected override void OnAppearing()
    {
        UpdateTotal();
    }

    private void Increase_Click(object sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not OrderItem item) return;

        // Increase the quantity of the selected item
        ++item.Quantity;
        UpdateTotal();
    }

    private void Decrease_Click(object sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not OrderItem item) return;

        --item.Quantity;
        if (item.Quantity < 1) {
            _cartService.RemoveFromCart(item);
        }
        UpdateTotal();
    }

    private async void CreateOrder_Clicked(object sender, EventArgs e)
    {
        OrderDto? insertedOrder = null;
        try {
            // Create a new order object
            var order = new OrderDto
            {
                UserId = SupabaseService.Session!.User!.Id!,
                Total = _cartService.OrderItems.Sum(x => x.CountTotal())
            };

            var response = await SB.From<OrderDto>().Insert(order);
            if (response.Model is null) return;
            insertedOrder = response.Model;

            // Insert each OrderItem into the database
            await Parallel.ForEachAsync(
                _cartService.OrderItems,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                async (orderItem, cancelationToken) =>
            {
                try {
                    var orderItemDto = new OrderItemDto
                    {
                        Size = Enum.GetName(orderItem.Size)!,
                        OrderId = insertedOrder.Id,
                        Quantity = orderItem.Quantity,
                        ProductId = orderItem.ProductId,
                    };
                    await SB.From<OrderItemDto>().Insert(orderItemDto, cancellationToken: cancelationToken);
                }
                catch (Exception ex) {
                    await SB.From<OrderDto>().Where(o => o.Id == insertedOrder.Id).Delete();
                    await DisplayAlert("Ошибка оформления заказа. Заказ отменен", ex.Message, "Ок");
                }
            });

            // Clear the cart after successful order creation
            _cartService.ClearCart();
            UpdateTotal();

            // Notify the user
            await DisplayAlert("Успех!", "Заказ успешно оформлен.", "Ок");
        }
        catch (Exception ex) {
            // Handle errors during order creation
            if (insertedOrder != null)
                await SB.From<OrderDto>().Where(o => o.Id == insertedOrder.Id).Delete();
            await DisplayAlert("Ошибка оформления заказа", ex.Message, "Ок");
        }
    }

    private void UpdateTotal()
    {
        // Calculate the total price of all items in the cart
        _total = _cartService.OrderItems.Sum(item => item.Quantity * item.Product.Price);

        // Update the total label
        TotalLabel.Text = $"Итог: {_total}₽";
    }
}