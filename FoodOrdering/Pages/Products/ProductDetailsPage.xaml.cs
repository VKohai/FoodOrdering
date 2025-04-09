namespace FoodOrdering.Pages.Products;

[QueryProperty(nameof(Product), nameof(Product))]
public partial class ProductDetailsPage : ContentPage
{
    private readonly CartService _cartService;

    public Product Product { get; set; } = null!;

    public ProductDetailsPage(CartService cartService)
    {
        _cartService = cartService;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Set the page title
        TitlePage.Text = Product?.Name ?? "Продукт";
        AddToCartBtn.Text = $"В корзину за {Product?.Price ?? 0}₽";
        ProductImage.Source = Product?.Image;
        // Populate the size picker
        SizePicker.ItemsSource = Enum.GetValues(typeof(ProductSize));
        SizePicker.SelectedIndex = 0; // Default selection
    }

    private async void EditProduct_Clicked(object sender, EventArgs e)
    {
        // Navigate to the edit product page
        await Shell.Current.GoToAsync(
            nameof(EditProductPage),
            true,
            new Dictionary<string, object>
            {
                { nameof(EditProductPage.Product), Product }
            });
    }

    private async void AddToCart_Clicked(object sender, EventArgs e)
    {
        // Get the selected size from the picker
        if (SizePicker.SelectedItem is not ProductSize selectedSize) {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите размер.", "Ок");
            return;
        }

        // Create an order item
        var orderItem = new OrderItem
        {
            Size = selectedSize,
            Product = Product,
            ProductId = Product.Id
        };

        // Add the item to the cart
        _cartService.AddToCart(orderItem);

        // Notify the user
        await DisplayAlert("Успех!", "Товар добавлен в корзину.", "Ок");
        await AppShell.Current.GoToAsync("..", true);
    }
}