namespace FoodOrdering.Pages.Products;

[QueryProperty(nameof(Product), nameof(Product))]
public partial class EditProductPage : ContentPage
{
    public Product Product { get; private set; } = null!;
    private string _imagePath = "";

    public EditProductPage()
    {
        InitializeComponent();
        Product = new Product
        {
            Name = "Яблоко",
            Price = 799M,
            Id = 1
        };
        LoadProductDetails();
    }

    private void LoadProductDetails()
    {
        ProductNameEntry.Text = Product.Name;
        ProductPriceEntry.Text = Product.Price.ToString();
        ProductImage.Source = ImageSource.FromFile(Product.Image);
        _imagePath = Product.Image;
        Title = Product.Name;
    }

    private async void LoadImage_Clicked(object sender, EventArgs e)
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

    private async void SaveProduct_Clicked(object sender, EventArgs e)
    {
        try {
            if (string.IsNullOrWhiteSpace(ProductNameEntry.Text))
                throw new Exception("Введите название продукта.");

            if (string.IsNullOrWhiteSpace(ProductPriceEntry.Text) ||
                !decimal.TryParse(ProductPriceEntry.Text, out var price))
                throw new Exception("Введите корректную цену.");

            Product.Name = ProductNameEntry.Text.Trim();
            Product.Price = price;
            Product.Image = _imagePath;

            var res = await SupabaseService.SB.From<Product>().Update(Product);
            if (res.Model == null)
                throw new Exception("Продукт не найден");

            await AppShell.Current.DisplayAlert("Успех!", $"{Product.Name} обновлен!", "Ок");
            await AppShell.Current.GoToAsync("..", true);
        }
        catch (Exception ex) {
            await AppShell.Current.DisplayAlert("Ошибка сохранения.", ex.Message, "Ок");
        }
    }

    private async void DeleteProduct_Clicked(object sender, EventArgs e)
    {
        try {
            var answer = await AppShell.Current.DisplayActionSheet(
                title: "Вы действительно хотите удалить продукт?",
                cancel: "Нет",
                destruction: null,
                buttons: ["Да"]
            );

            if (answer == null || answer.Equals("Нет")) return;

            await SupabaseService.SB.From<Product>()
                .Where(p => p.Id == Product.Id)
                .Delete(Product);

            await AppShell.Current.DisplayAlert("Успех!", $"{Product.Name} удален.", "Ок");
            await AppShell.Current.GoToAsync("..", true);
        }
        catch (Exception ex) {
            await AppShell.Current.DisplayAlert("Ошибка удаления.", ex.Message, "Ок");
        }
    }
}