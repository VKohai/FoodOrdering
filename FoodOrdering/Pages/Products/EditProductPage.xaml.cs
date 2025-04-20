namespace FoodOrdering.Pages.Products;

[QueryProperty(nameof(Product), nameof(Product))]
public partial class EditProductPage : ContentPage {
    private FileResult? _fileResult;

    public Product Product { get; set; } = null!;

    public EditProductPage() {
        InitializeComponent();
    }

    protected override async void OnAppearing() {
        Title = Product.Name;
        ProductNameEntry.Text = Product.Name;
        ProductPriceEntry.Text = Product.Price.ToString();
        ProductImage.Source = await Product.Image.DownloadImageAsync();
    }

    private async void LoadImage_Clicked(object sender, EventArgs e) {
        try {
            var pickOptions = new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "����������� ��� ��������"
            };

            _fileResult = await FilePicker.PickAsync(pickOptions);
            if (_fileResult is null) return;

            using var stream = await _fileResult.OpenReadAsync();
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            ProductImage.Source = Product.ImageSource = ImageSource.FromStream(() => memoryStream);
        } catch (Exception ex) {
            await DisplayAlert("������ ������ �����.", ex.Message, "��");
        }
    }

    private async void SaveProduct_Clicked(object sender, EventArgs e) {
        try {
            if (string.IsNullOrWhiteSpace(ProductNameEntry.Text))
                throw new Exception("������� �������� ��������.");

            if (Product.ImageSource == null) {
                await DisplayAlert("���� �� �������", "�������� ���������� ��������!", "��");
                return;
            }

            if (string.IsNullOrWhiteSpace(ProductPriceEntry.Text) ||
                !decimal.TryParse(ProductPriceEntry.Text, out var price))
                throw new Exception("������� ���������� ����.");

            if (_fileResult != null) {
                var supabasePath = $"Products/{DateTime.UtcNow.ToShortTimeString()}-{Guid.NewGuid()}";
                supabasePath = await SB.Storage
                    .From("product-images")
                    .Upload(
                        localFilePath: _fileResult.FullPath,
                        supabasePath: supabasePath,
                        options: new Supabase.Storage.FileOptions { ContentType = _fileResult.ContentType });
                Product.Image = supabasePath;
            }

            Product.Name = ProductNameEntry.Text.Trim();
            Product.Price = price;

            var res = await SB.From<ProductDto>().Update(new ProductDto {
                Id = Product.Id,
                Name = Product.Name,
                Price = Product.Price,
                Image = Product.Image,
            });

            if (res.Model == null)
                throw new Exception("������� �� ������");

            await DisplayAlert("�����!", $"{Product.Name} ��������!", "��");
            await AppShell.Current.GoToAsync("..", true);
        } catch (Exception ex) {
            await DisplayAlert("������ ����������.", ex.Message, "��");
        }
    }

    private async void DeleteProduct_Clicked(object sender, EventArgs e) {
        try {
            var answer = await AppShell.Current.DisplayActionSheet(
                title: "�� ������������� ������ ������� �������?",
                cancel: "���",
                destruction: null,
                buttons: ["��"]
            );

            if (answer == null || answer.Equals("���")) return;

            await SB.From<Product>()
                .Where(p => p.Id == Product.Id)
                .Delete(Product);
            var supabasePath = Product.Image[(Product.Image.IndexOf('/') + 1)..];
            await SB.Storage.From("product-images").Remove(supabasePath);
            await DisplayAlert("�����!", $"{Product.Name} ������.", "��");
            await AppShell.Current.GoToAsync("..", true);
        } catch (Exception ex) {
            await DisplayAlert("������ ��������.", ex.Message, "��");
        }
    }
}