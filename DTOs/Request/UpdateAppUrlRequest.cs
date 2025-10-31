using System.ComponentModel.DataAnnotations;

namespace MozeApi.DTOs.Request
{
    /// <summary>
    /// 更新 App URL 請求 DTO
    /// </summary>
    public class UpdateAppUrlRequest
    {
        /// <summary>
        /// URL 網址
        /// </summary>
        [MaxLength(2000)]
        public string? Url { get; set; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool? IsFinished { get; set; }

        /// <summary>
        /// 交易記錄 ID
        /// </summary>
        public int? TransactionId { get; set; }
    }
}
