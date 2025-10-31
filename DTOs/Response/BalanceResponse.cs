namespace MozeApi.DTOs.Response
{
    /// <summary>
    /// 餘額調整記錄回應 DTO
    /// </summary>
    public class BalanceResponse
    {
        public int? BalanceId { get; set; }
        public string? Account { get; set; }
        public decimal? Amount { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Note { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
