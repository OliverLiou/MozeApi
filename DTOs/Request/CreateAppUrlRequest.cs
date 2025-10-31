using System.ComponentModel.DataAnnotations;

namespace MozeApi.DTOs.Request
{
    /// <summary>
    /// 新增 App URL 請求 DTO
    /// </summary>
    public class CreateAppUrlRequest
    {
        /// <summary>
        /// URL 網址
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// 是否已完成
        /// </summary>
        [Required]
        public bool IsFinished { get; set; }

        /// <summary>
        /// 交易記錄 ID
        /// </summary>
        [Required]
        public int TransactionId { get; set; }
    }
}
