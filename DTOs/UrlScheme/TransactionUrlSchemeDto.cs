using System.ComponentModel.DataAnnotations;

namespace MozeApi.DTOs.UrlScheme
{
    /// <summary>
    /// 交易 URL Scheme 基底 DTO - 包含支出和收入共用的參數
    /// </summary>
    public class TransactionUrlSchemeDto : UrlSchemeBaseDto
    {
        /// <summary>
        /// 金額 (必填)
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// 貨幣名稱
        /// </summary>
        [MaxLength(10)]
        public string? Currency { get; set; }

        /// <summary>
        /// 帳戶名稱 (必填)
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 專案名稱
        /// </summary>
        [MaxLength(100)]
        public string? Project { get; set; }

        /// <summary>
        /// 類別名稱
        /// </summary>
        [MaxLength(100)]
        public string? Category { get; set; }

        /// <summary>
        /// 子類別名稱 (必填)
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Subcategory { get; set; } = string.Empty;

        /// <summary>
        /// 記錄名稱
        /// </summary>
        [MaxLength(200)]
        public string? Name { get; set; }

        /// <summary>
        /// 商家
        /// </summary>
        [MaxLength(200)]
        public string? Store { get; set; }

        /// <summary>
        /// 標籤 (用 "," 分隔)
        /// </summary>
        [MaxLength(500)]
        public string? Tags { get; set; }

        /// <summary>
        /// 手續費
        /// </summary>
        public decimal? Fee { get; set; }

        /// <summary>
        /// 手續費名稱
        /// </summary>
        [MaxLength(100)]
        public string? FeeName { get; set; }

        /// <summary>
        /// 回饋金
        /// </summary>
        public decimal? Bonus { get; set; }

        /// <summary>
        /// 回饋名稱
        /// </summary>
        [MaxLength(100)]
        public string? BonusName { get; set; }
    }
}
