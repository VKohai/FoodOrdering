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
            DisplayAlert("������", "����� �� ������.", "��");
            return;
        }

        Title = $"����� #{Order.Id}";

        StatusPicker.ItemsSource = Enum.GetValues<OrderStatus>();
        StatusPicker.SelectedItem = Order.Status;

        OrderItemsCollection.ItemsSource = Order.OrderItems;
    }
}