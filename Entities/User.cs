using Microsoft.AspNetCore.Identity;

namespace MozeApi.Entities
{
    /// <summary>
    /// 使用者實體
    /// </summary>
    public class User: IdentityUser
    {
        /// <summary>
        /// Google 帳號唯一識別碼
        /// </summary>
        public string? GoogleId { get; set; } = string.Empty;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最後登入時間
        /// </summary>
        public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 大頭照 URL
        /// </summary>
        public string? Picture { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 使用者的交易記錄
        /// </summary>
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}