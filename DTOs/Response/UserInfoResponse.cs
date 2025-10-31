namespace MozeApi.DTOs.Response
{
    /// <summary>
    /// 使用者資訊回應 DTO
    /// </summary>
    public class UserInfoResponse
    {
        public string? Id { get; set; }
        public required string UserName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Picture { get; set; }

        public List<string>? RoleNames { get; set; }
    }
}
