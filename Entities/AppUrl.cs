using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MozeApi.Entities
{
    /// <summary>
    /// App URL 記錄實體
    /// </summary>
    public class AppUrl
    {
        /// <summary>
        /// App URL ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppUrlId { get; set; }

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

        /// <summary>
        /// 關聯的交易記錄
        /// </summary>
        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; } = null!;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
