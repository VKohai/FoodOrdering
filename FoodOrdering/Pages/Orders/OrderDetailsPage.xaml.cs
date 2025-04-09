namespace FoodOrdering.Pages.Orders;

[QueryProperty(nameof(Order), nameof(Order))]
public partial class OrderDetailsPage : ContentPage
{
    public Order Order { get; set; } = null!;

    public OrderDetailsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Order == null) {
            DisplayAlert("Ошибка", "Заказ не найден.", "Ок");
            return;
        }

        Title = $"Заказ #{Order.Id}";

        StatusPicker.ItemsSource = Enum.GetValues<OrderStatus>();
        StatusPicker.SelectedItem = Order.Status;

        OrderItemsCollection.ItemsSource = Order.OrderItems;
    }
}