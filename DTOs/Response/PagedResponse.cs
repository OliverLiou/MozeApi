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
        /// 當前頁碼
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否有上一頁
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// 是否有下一頁
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
