namespace FoodOrdering.Pages.Orders;

[QueryProperty(nameof(Order), nameof(Order))]
public partial class OrderDetailsPage : ContentPage
{
    public Order Order { get; set; } = null!;

    public OrderDetailsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        if (Order == null) {
            await DisplayAlert("Ошибка", "Заказ не найден.", "Ок");
            return;
        }
        // TODO: Get order by id
        #region Set values
        IdLabel.Text = $"Заказ #{Order.Id} на сумму {Order.Total}₽";
        CreatedAtLabel.Text = $"Создан: {Order.CreatedAt.ToString("HH:mm")}";
        StatusLabel.Text = Enum.GetName(Order.Status);
        StatusPicker.ItemsSource = Enum.GetValues<OrderStatus>();
        StatusPicker.SelectedItem = Order.Status;
        #endregion

        try {
            await GetOrderItemsAsync();
        }
        catch (Exception ex) {
            await DisplayAlert("Ошибка получения продуктов в заказе.", ex.Message, "Назад");
            await AppShell.Current.GoToAsync("..");
        }
    }

    private async Task GetOrderItemsAsync()
    {
        var orderItems = (await SB.From<OrderItem>()
            .Select("*,Product!inner(*)")
            .Where(oi => oi.OrderId == Order.Id)
            .Get()).Models;
        foreach (var oi in orderItems)
            oi.Product.ImageSource = await oi.Product.Image.DownloadImageAsync();
        OrderItemsCollection.ItemsSource = Order.OrderItems = orderItems;
    }

    private bool isAppearing = true;
    private async void StatusPicker_SelectedChanged(object sender, EventArgs e)
    {
        if (isAppearing) {
            isAppearing = false;
            return;
        }
        OrderStatus prevStatus = Order.Status;
        try {
            if (!IsAdmin)
                throw new GotrueException("Это функция доступна только администратору!", FailureHint.Reason.AdminTokenRequired);

            var picker = (Picker)sender;
            Order.Status = (OrderStatus)picker.SelectedIndex;

            var update = await SB.From<OrderDto>()
                .Where(x => x.Id == Order.Id)
                .Set(x => x.Status, Enum.GetName(Order.Status))
                .Update();
        }
        catch (GotrueException ex) {
            await DisplayAlert("Отказано в доступе.", ex.Message, "Отменить");
            RollbackStatus(prevStatus);
        }
        catch (Exception ex) {
            await DisplayAlert("Ошибка изменения статуса заказа.", ex.Message, "Отменить");
            RollbackStatus(prevStatus);
        }

        void RollbackStatus(OrderStatus prevStatus) =>
            (Order.Status, StatusPicker.SelectedIndex) = (prevStatus, (int)prevStatus);
    }
}