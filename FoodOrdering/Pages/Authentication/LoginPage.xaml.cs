namespace FoodOrdering.Pages.Authentication;

public partial class LoginPage : ContentPage
{
    private readonly AuthenticationService _authService;

    public LoginPage()
    {
        InitializeComponent();
        _authService = new AuthenticationService();
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        try {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            await _authService.LoginAsync(email, password, isStaffOnly: false);
            await Shell.Current.GoToAsync($"//{nameof(CatalogPage)}", true);
        }
        catch (ArgumentException) {
            await DisplayAlert("������ �����", "E-mail � ������ �� ����� ���� �������", "��");
        }
        catch (GotrueException ex) {
            string msg = ex.Reason switch
            {
                FailureHint.Reason.UserBadEmailAddress => "�������� ������ e-mail",
                FailureHint.Reason.UserBadPassword => "������ ������ ����� ������� 6 ��������",
                FailureHint.Reason.UserBadLogin => "������������ �� ������",
                _ => "�������� ������ ������"
            };

            await DisplayAlert("������ �����", msg, "��");
        }
    }

    private async void GoToRegister_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}", true);
    }

    private async void GoToStaffOnly_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(StaffOnlyPage), true);
    }
}