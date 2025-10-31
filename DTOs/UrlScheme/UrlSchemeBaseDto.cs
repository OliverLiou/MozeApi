using System.ComponentModel.DataAnnotations;

namespace MozeApi.DTOs.UrlScheme
{
    /// <summary>
    /// URL Scheme 基底 DTO - 包含所有 Actions 共用的基礎參數
    /// </summary>
    public class UrlSchemeBaseDto
    {
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
    }
}
