namespace MozeApi.DTOs.UrlScheme
{
    /// <summary>
    /// 開啟新記錄頁面 URL Scheme DTO (moze://new)
    /// 所有參數都是可選的，用於預填記錄頁面
    /// </summary>
    public class NewRecordUrlSchemeDto : UrlSchemeBaseDto
    {
        // 繼承自 UrlSchemeBaseDto 的可選參數: Date, Time, Note
        // 無額外欄位
    }
}
