using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MozeApi.Entities
{
    /// <summary>
    /// 交易類型枚舉
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// 支出
        /// </summary>
        Expense = 1,

        /// <summary>
        /// 收入
        /// </summary>
        Income = 2
    }

    /// <summary>
    /// 交易記錄實體 (支出和收入)
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// 交易記錄 ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        /// <summary>
        /// 交易類型 (支出/收入)
        /// </summary>
        [Required]
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Fee { get; set; }

        /// <summary>
        /// 手續費名稱
        /// </summary>
        [MaxLength(100)]
        public string? FeeName { get; set; }

        /// <summary>
        /// 回饋金
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Bonus { get; set; }

        /// <summary>
        /// 回饋名稱
        /// </summary>
        [MaxLength(100)]
        public string? BonusName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 使用者 ID
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 關聯的使用者
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
