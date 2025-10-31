using System.ComponentModel.DataAnnotations;
using MozeApi.Entities;

namespace MozeApi.DTOs.Request
{
    /// <summary>
    /// 新增交易請求 DTO
    /// </summary>
    public class CreateTransactionRequest
    {
        /// <summary>
        /// 交易類型 (支出/收入)
        /// </summary>
        [Required]
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// 貨幣名稱
        /// </summary>
        [MaxLength(10)]
        public string? Currency { get; set; }

        /// <summary>
        /// 帳戶名稱
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
        /// 子類別名稱
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
        /// 備註
        /// </summary>
        [MaxLength(500)]
        public string? Note { get; set; }

        /// <summary>
        /// 標籤 (用 "," 分隔)
        /// </summary>
        [MaxLength(500)]
        public string? Tags { get; set; }

        /// <summary>
        /// 日期 (格式: YYYY.MM.dd)
        /// </summary>
        [MaxLength(10)]
        public string? Date { get; set; }

        /// <summary>
        /// 時間 (格式: HH:mm)
        /// </summary>
        [MaxLength(5)]
        public string? Time { get; set; }

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

        /// <summary>
        /// 使用者 ID
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}
