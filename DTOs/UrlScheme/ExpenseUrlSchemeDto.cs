namespace MozeApi.DTOs.UrlScheme
{
    /// <summary>
    /// 新增支出 URL Scheme DTO (moze://expense)
    /// 必填參數: Amount, Account, Subcategory
    /// </summary>
    public class ExpenseUrlSchemeDto : TransactionUrlSchemeDto
    {
        // 繼承自 TransactionUrlSchemeDto 的所有參數
        // 無額外欄位
    }
}
