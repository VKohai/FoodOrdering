namespace FoodOrdering.Pages.Authentication;

public partial class StaffOnlyPage : ContentPage
{
    private readonly AuthenticationService _authService;

    public StaffOnlyPage(AuthenticationService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        try {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            await _authService.LoginAsync(email, password, isStaffOnly: true);
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
}