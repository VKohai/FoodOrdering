namespace FoodOrdering.Services;

public class AuthenticationService
{
    public async Task LoginAsync(string email, string password, bool isStaffOnly)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        SupabaseService.Session = await SB.Auth.SignIn(email, password);

        if (isStaffOnly) {
            await CheckIfStaff();
        }
    }

    public async Task RegisterAsync(string email, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        SupabaseService.Session = await SB.Auth.SignUp(email, password);
    }

    private static async Task CheckIfStaff()
    {
        var profile = await SB
            .From<Profile>()
            .Select("*")
            .Where(p => p.Id == SupabaseService.Session!.User!.Id)
            .Single();

        if (profile == null) {
            throw new GotrueException("Профиль не найден", FailureHint.Reason.UserBadLogin);
        }

        if (profile.Group == ProfileGroup.ADMIN) {
            SupabaseService.IsAdmin = true;
            return;
        }

        throw new GotrueException("Вы не являетесь сотрудником", FailureHint.Reason.UserBadLogin);
    }
}