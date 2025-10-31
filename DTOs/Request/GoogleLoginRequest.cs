using System.ComponentModel.DataAnnotations;

namespace MozeApi.DTOs.Request
{
    /// <summary>
    /// Google 登入請求 DTO
    /// </summary>
    public class GoogleLoginRequest
    {
        [Required]
        public string IdToken { get; set; } = string.Empty;
    }
}
