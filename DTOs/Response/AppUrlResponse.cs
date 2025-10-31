namespace MozeApi.DTOs.Response
{
    /// <summary>
    /// App URL 記錄回應 DTO
    /// </summary>
    public class AppUrlResponse
    {
        public int? AppUrlId { get; set; }
        public string? Url { get; set; }
        public bool? IsFinished { get; set; }
        public int? TransactionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
