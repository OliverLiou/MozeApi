namespace MozeApi.DTOs.UrlScheme
{
    /// <summary>
    /// 新增收入 URL Scheme DTO (moze://income)
    /// 必填參數: Amount, Account, Subcategory
    /// </summary>
    public class IncomeUrlSchemeDto : TransactionUrlSchemeDto
    {
        // 繼承自 TransactionUrlSchemeDto 的所有參數
        // 無額外欄位
    }
}
