namespace FoodOrdering.Services;
public static class SupabaseService {
    private const string SB_URL = "https://jvdjlcbsxuefpenvcljw.supabase.co";
    private const string SB_ANON_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Imp2ZGpsY2JzeHVlZnBlbnZjbGp3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDI5MzQwNDIsImV4cCI6MjA1ODUxMDA0Mn0.NYTlfJwiWBZitBIu62g3IU3Nik_qgoGbj_8TCxGNy3M";

    public static readonly Supabase.Client SB = new(SB_URL, SB_ANON_KEY);
    public static Session? Session { get; set; }

    public static bool IsAdmin { get; set; } = false;
}