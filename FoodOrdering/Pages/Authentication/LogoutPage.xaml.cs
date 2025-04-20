namespace FoodOrdering.Pages.Authentication;

public partial class LogoutPage : ContentPage
{
	private readonly AuthenticationService _authService;
	public LogoutPage(AuthenticationService authService)
	{
		_authService = authService;
		InitializeComponent();
		EmailLabel.Text = SB.Auth.CurrentUser!.Email;
	}

    private async void Logout_Clicked(object sender, EventArgs e) {
		await SB.Auth.SignOut();
		await AppShell.Current.GoToAsync($"//{nameof(LoginPage)}", true);
		IsAdmin = _authService.AdminVisibility = false;
    }
}