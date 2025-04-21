namespace FoodOrdering.Pages.Products;

public partial class AddProductPage : ContentPage {
    private FileResult? _fileResult;

    public AddProductPage() {
        InitializeComponent();
    }

    private async void LoadImage_Click(object sender, EventArgs e) {
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

            ProductImage.Source = ImageSource.FromStream(() => memoryStream);
        } catch (Exception ex) {
            await DisplayAlert("������ ������ �����.", ex.Message, "�������");
        }
    }

    private async void AddProduct_Clicked(object sender, EventArgs e) {
        try {
            if (!IsAdmin)
                throw new Exception("�� �� ��������� �������, ����� ��������� ��������");

            if (_fileResult == null) {
                await DisplayAlert("���� �� �������", "�������� ���������� ��������!", "��");
                return;
            }

            if (string.IsNullOrWhiteSpace(ProductNameEntry.Text))
                throw new Exception("������� �������� ��������.");

            if (string.IsNullOrWhiteSpace(ProductPriceEntry.Text) ||
                !decimal.TryParse(ProductPriceEntry.Text, out var price))
                throw new Exception("������� ���������� ����.");

            var supabasePath = $"Products/{Guid.NewGuid()}";
            supabasePath = await SB.Storage
                .From("product-images")
                .Upload(
                    localFilePath: _fileResult.FullPath,
                    supabasePath: supabasePath,
                    options: new Supabase.Storage.FileOptions { ContentType = _fileResult.ContentType });

            await SB.From<ProductDto>().Insert(new ProductDto {
                Name = ProductNameEntry.Text.Trim(),
                Image = supabasePath,
                Price = price
            });

            await DisplayAlert("�����!", "������� ��������", "��");
            await AppShell.Current.GoToAsync("..");
        } catch (Exception ex) {
            await DisplayAlert("������ ���������� ��������", ex.Message, "��");
        }
    }
}