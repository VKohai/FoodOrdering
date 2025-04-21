namespace FoodOrdering.Pages;

public partial class CatalogPage : ContentPage {
    private readonly ObservableCollection<Product> _products = [];

    public CatalogPage(AuthenticationService authService) {
        InitializeComponent();
        BindingContext = authService;
        ProductsCollectionView.ItemsSource = _products;
        TitlePage.Text = Title;
    }

    protected override async void OnAppearing() {
        try {
            IsBusy = true;
            await GetProductsAsync();
        } catch (Exception ex) {
            await DisplayAlert("Ошибка получения продуктов", ex.Message, "Ок");
            _products.Clear();
        } finally {
            IsBusy = false;
        }
    }

    private async Task GetProductsAsync() {
        var products = await SB.From<Product>().Get();
        if (products.Models.Count == 0)
            throw new Exception("Products are not found");

        _products.Clear();
        foreach (var product in products.Models) {
            product.ImageSource = await product.Image.DownloadImageAsync();
            _products.Add(product);
        }
    }

    private async void Catalog_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (e.CurrentSelection.Count == 0) return;
        if (e.CurrentSelection[0] is not Product selectedProduct) return;

        await Shell.Current.GoToAsync(
            nameof(ProductDetailsPage),
            animate: true,
            new Dictionary<string, object>
            {
                { nameof(ProductDetailsPage.Product), selectedProduct }
            });

        ((CollectionView)sender).SelectedItem = null;
    }

    private async void GoToAddProduct(object sender, EventArgs e) {
        await Shell.Current.GoToAsync(nameof(AddProductPage), true);
    }
}