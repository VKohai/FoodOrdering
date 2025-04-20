namespace FoodOrdering.Pages.Authentication;

public partial class RegisterPage : ContentPage
{
    private readonly AuthenticationService _authService;

    public RegisterPage(AuthenticationService authService)
    {
        InitializeComponent();
        _authService =  authService;
    }

    private async void RegisterButton_Clicked(object sender, EventArgs e)
    {
        try {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            await _authService.RegisterAsync(email, password);
            await Shell.Current.GoToAsync($"//{nameof(CatalogPage)}", true);
        }
        catch (ArgumentException) {
            await DisplayAlert("������ �����������", "E-mail � ������ �� ����� ���� �������", "��");
        }
        catch (GotrueException ex) {
            string msg = ex.Reason switch
            {
                FailureHint.Reason.UserBadEmailAddress => "�������� ������ e-mail",
                FailureHint.Reason.UserBadPassword => "������ ������ ����� ������� 6 ��������",
                _ => "�������� ������ ������"
            };

            await DisplayAlert("������ �����������", msg, "��");
        }
    }

    private async void GoToLogin_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}", true);
    }

    private async void GoToStaffOnly_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(StaffOnlyPage), true);
    }
}