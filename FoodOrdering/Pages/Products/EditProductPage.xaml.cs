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
            Name = "������",
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
                PickerTitle = "����������� ��� ��������"
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
            await AppShell.Current.DisplayAlert("������ ������ �����.", ex.Message, "�������");
        }
    }

    private async void SaveProduct_Clicked(object sender, EventArgs e)
    {
        try {
            if (string.IsNullOrWhiteSpace(ProductNameEntry.Text))
                throw new Exception("������� �������� ��������.");

            if (string.IsNullOrWhiteSpace(ProductPriceEntry.Text) ||
                !decimal.TryParse(ProductPriceEntry.Text, out var price))
                throw new Exception("������� ���������� ����.");

            Product.Name = ProductNameEntry.Text.Trim();
            Product.Price = price;
            Product.Image = _imagePath;

            var res = await SupabaseService.SB.From<Product>().Update(Product);
            if (res.Model == null)
                throw new Exception("������� �� ������");

            await AppShell.Current.DisplayAlert("�����!", $"{Product.Name} ��������!", "��");
            await AppShell.Current.GoToAsync("..", true);
        }
        catch (Exception ex) {
            await AppShell.Current.DisplayAlert("������ ����������.", ex.Message, "��");
        }
    }

    private async void DeleteProduct_Clicked(object sender, EventArgs e)
    {
        try {
            var answer = await AppShell.Current.DisplayActionSheet(
                title: "�� ������������� ������ ������� �������?",
                cancel: "���",
                destruction: null,
                buttons: ["��"]
            );

            if (answer == null || answer.Equals("���")) return;

            await SupabaseService.SB.From<Product>()
                .Where(p => p.Id == Product.Id)
                .Delete(Product);

            await AppShell.Current.DisplayAlert("�����!", $"{Product.Name} ������.", "��");
            await AppShell.Current.GoToAsync("..", true);
        }
        catch (Exception ex) {
            await AppShell.Current.DisplayAlert("������ ��������.", ex.Message, "��");
        }
    }
}