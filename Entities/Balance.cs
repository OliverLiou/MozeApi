using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MozeApi.Entities
{
    /// <summary>
    /// 餘額調整記錄實體
    /// </summary>
    public class Balance
    {
        /// <summary>
        /// 餘額調整記錄 ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BalanceId { get; set; }

        /// <summary>
        /// 帳戶名稱
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 調整金額
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

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
        /// 備註
        /// </summary>
        [MaxLength(500)]
        public string? Note { get; set; }

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
    }
}
