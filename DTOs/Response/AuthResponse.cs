namespace MozeApi.DTOs.Response
{
    /// <summary>
    /// 認證回應 DTO
    /// </summary>
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public UserInfoResponse? UserInfo { get; set; }
    }
}
