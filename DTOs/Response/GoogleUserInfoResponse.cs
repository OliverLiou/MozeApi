namespace MozeApi.DTOs.Response
{
    /// <summary>
    /// Google 使用者資訊回應 DTO
    /// </summary>
    public class GoogleUserInfoResponse
    {
        public string GoogleId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Picture { get; set; }
        public bool EmailVerified { get; set; }
    }
}
