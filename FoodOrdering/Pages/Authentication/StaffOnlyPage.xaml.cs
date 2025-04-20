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
            await DisplayAlert("Ошибка входа", "E-mail и пароль не могут быть пустыми", "Ок");
        }
        catch (GotrueException ex) {
            string msg = ex.Reason switch
            {
                FailureHint.Reason.UserBadEmailAddress => "Неверный формат e-mail",
                FailureHint.Reason.UserBadPassword => "Пароль должен иметь минимум 6 символов",
                FailureHint.Reason.UserBadLogin => "Пользователь не найден",
                _ => "Неверный формат данных"
            };

            await DisplayAlert("Ошибка входа", msg, "Ок");
        }
    }
}