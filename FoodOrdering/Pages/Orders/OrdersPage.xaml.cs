namespace FoodOrdering.Pages.Orders;

public partial class OrdersPage : ContentPage
{
    private List<Order> _orders = null!;

    public OrdersPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        _orders = (await SB.From<Order>()
            .Where(order => order.UserId == SupabaseService.Session!.User!.Id)
            .Get()).Models;
        OrdersCollection.ItemsSource = _orders;
    }

    private void OrderFilterPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (OrderFilterPicker.SelectedIndex == -1) return;

        var filteredOrders = OrderFilterPicker.SelectedIndex switch
        {
            0 => _orders,
            1 => _orders.Where(order =>
            order.Status == OrderStatus.New ||
            order.Status == OrderStatus.Delivering).ToList(),
            2 => _orders.Where(order => order.Status == OrderStatus.Delivered).ToList(),
            _ => _orders
        };

        OrdersCollection.ItemsSource = filteredOrders;
    }

    private async void OrdersCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;
        if (e.CurrentSelection[0] is not Order selectedOrder) return;

        await Shell.Current.GoToAsync(
            nameof(OrderDetailsPage),
            animate: true,
            new Dictionary<string, object>
            {
                { nameof(OrderDetailsPage.Order), selectedOrder }
            });
        ((CollectionView)sender).SelectedItem = null;
    }
}