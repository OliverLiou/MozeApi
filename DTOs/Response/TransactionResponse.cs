namespace MozeApi.DTOs.Response
{
    /// <summary>
    /// 交易記錄回應 DTO
    /// </summary>
    public class TransactionResponse
    {
        public int? TransactionId { get; set; }
        public string? TransactionType { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public string? Account { get; set; }
        public string? Project { get; set; }
        public string? Category { get; set; }
        public string? Subcategory { get; set; }
        public string? Name { get; set; }
        public string? Store { get; set; }
        public string? Note { get; set; }
        public string? Tags { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public decimal? Fee { get; set; }
        public string? FeeName { get; set; }
        public decimal? Bonus { get; set; }
        public string? BonusName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
