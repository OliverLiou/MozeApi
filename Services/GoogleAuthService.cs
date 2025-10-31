using MozeApi.DTOs.Response;
using Google.Apis.Auth;

namespace MozeApi.Services
{
    public interface IGoogleAuthService
    {
        Task<GoogleUserInfoResponse?> VerifyGoogleTokenAsync(string idToken);
    }

    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IConfiguration _configuration;

        public GoogleAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GoogleUserInfoResponse?> VerifyGoogleTokenAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string> { _configuration["GoogleAuth:ClientId"]! }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                return new GoogleUserInfoResponse
                {
                    GoogleId = payload.Subject,
                    Email = payload.Email,
                    Name = payload.Name,
                    Picture = payload.Picture,
                    EmailVerified = payload.EmailVerified
                };
            }
            catch (Exception ex)
            {
                // 記錄錯誤
                Console.WriteLine($"Google token validation failed: {ex.Message}");
                return null;
            }
        }
    }
}
