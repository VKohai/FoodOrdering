using System.ComponentModel;

namespace FoodOrdering.Services;

public partial class AuthenticationService : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task LoginAsync(string email, string password, bool isStaffOnly) {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        SupabaseService.Session = await SB.Auth.SignIn(email, password);
        if (isStaffOnly)
            await CheckIfStaff();
    }

    public async Task RegisterAsync(string email, string password) {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        SupabaseService.Session = await SB.Auth.SignUp(email, password);
    }

    private async Task CheckIfStaff() {
        var profile = await SB
            .From<Profile>()
            .Select("*")
            .Where(p => p.Id == SupabaseService.Session!.User!.Id)
            .Single() ?? throw new GotrueException("Профиль не найден", FailureHint.Reason.UserBadLogin);

        if (profile.Group == ProfileGroup.ADMIN) {
            IsAdmin = AdminVisibility = true;
            return;
        }

        throw new GotrueException("Вы не являетесь сотрудником", FailureHint.Reason.UserBadLogin);
    }

    private bool _adminVisibility = IsAdmin;

    public bool AdminVisibility
    {
        get => _adminVisibility;
        set {
            _adminVisibility = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdminVisibility)));
        }
    }
}