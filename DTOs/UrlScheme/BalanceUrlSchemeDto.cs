using System.ComponentModel.DataAnnotations;

namespace MozeApi.DTOs.UrlScheme
{
    /// <summary>
    /// 餘額調整 URL Scheme DTO (moze://balance)
    /// 必填參數: Account, Amount
    /// </summary>
    public class BalanceUrlSchemeDto : UrlSchemeBaseDto
    {
        /// <summary>
        /// 帳戶名稱 (必填)
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 調整金額 (必填)
        /// </summary>
        [Required]
        public decimal Amount { get; set; }
    }
}
