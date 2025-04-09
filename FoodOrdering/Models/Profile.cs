namespace FoodOrdering.Models;

public class Profile : BaseModel
{
    [Column("id")]
    public string Id { get; set; } = null!;
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Website { get; set; }

    [Column("group")]
    public string Group { get; set; } = ProfileGroup.USER;
    public DateTime? UpdatedAt { get; set; }
}

public static class ProfileGroup
{
    public const string USER = nameof(USER);
    public const string ADMIN = nameof(ADMIN);
}