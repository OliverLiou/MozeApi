namespace MozeApi.DTOs.Response
{
    /// <summary>
    /// Token 回應 DTO
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// access token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// refresh token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}
