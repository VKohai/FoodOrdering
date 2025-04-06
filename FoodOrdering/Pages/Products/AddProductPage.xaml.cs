namespace FoodOrdering.Pages.Products;

public partial class AddProductPage : ContentPage
{
    private string _imagePath = "";

    public AddProductPage()
    {
        InitializeComponent();
    }

    private async void LoadImage_Click(object sender, EventArgs e)
    {
        try {
            var pickOptions = new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Изображение для продукта"
            };

            var result = await FilePicker.PickAsync(pickOptions);
            if (result is null) return;

            using var stream = await result.OpenReadAsync();
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            ProductImage.Source = ImageSource.FromStream(() => memoryStream);
            _imagePath = result.FullPath;
        }
        catch (Exception ex) {
            await AppShell.Current.DisplayAlert("Ошибка выбора файла.", ex.Message, "Понятно");
        }
    }

    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        try {
            if (!SupabaseService.IsAdmin)
                throw new Exception("Вы не являетесь админом, чтобы добавлять продукты");

            if (string.IsNullOrWhiteSpace(ProductNameEntry.Text))
                throw new Exception("Введите название продукта.");
            if (string.IsNullOrWhiteSpace(ProductPriceEntry.Text) ||
                !decimal.TryParse(ProductPriceEntry.Text, out var price))
                throw new Exception("Введите корректную цену.");

            var product = new Product
            {
                Name = ProductNameEntry.Text.Trim(),
                Image = _imagePath,
                Price = price
            };

            await SupabaseService.SB.From<Product>().Insert(product);

            await AppShell.Current.DisplayAlert("Успех!", "Продукт добавлен", "Ок");
            await AppShell.Current.GoToAsync("..");
        }
        catch (Exception ex) {
            await AppShell.Current.DisplayAlert("Ошибка добавления продукта", ex.Message, "Ок");
        }
    }
}