namespace MozeApi.DTOs.Response
{
    /// <summary>
    /// 分頁回應 DTO
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class PagedResponse<T>
    {
        /// <summary>
        /// 資料列表
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotalCount { get; set; }

    }
}
