namespace MyProject.Contracts.Settings;

public class AppSettings
{
    public string Secret { get; set; } = string.Empty;
    public double TokenExpirationInMinutes { get; set; } = 15; // Access token expiration
    public double RefreshTokenExpirationInDays { get; set; } = 7; // Refresh token expiration
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string GoogleClientId { get; set; } = string.Empty;
    public string GoogleClientSecret { get; set; } = string.Empty;
}