using MyProject.Contracts.Google;
using System.Threading.Tasks;

namespace MyProject.Application.Abstractions;

public interface IGoogleOAuthService
{
    string GenerateGoogleAuthUrl(string redirectUri, string state);
    Task<GoogleOAuthResult> ProcessOAuthCallbackAsync(string code, string redirectUri);
}

public class GoogleOAuthResult
{
    public bool IsSuccess { get; set; }
    public string Error { get; set; }
    public GoogleUserInfo? UserInfo { get; set; }
    public GoogleTokenResponse? TokenResponse { get; set; }
}