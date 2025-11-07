using System.Linq.Expressions;

namespace MozeApi.Helpers
{
    /// <summary>
    /// 搜尋表達式建構器，用於動態建立可由 EF Core 轉譯的搜尋條件
    /// </summary>
    public static class SearchExpressionBuilder
    {
        /// <summary>
        /// 為指定類型建立字串搜尋表達式
        /// </summary>
        /// <typeparam name="T">實體類型</typeparam>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <returns>可轉譯為 SQL 的搜尋表達式</returns>
        public static Expression<Func<T, bool>>? BuildSearchExpression<T>(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return null;

            // 建立參數表達式 (例如: t => ...)
            var parameter = Expression.Parameter(typeof(T), "x");

            // 取得 string.Contains 方法
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;

            // 取得所有 string 類型的屬性
            var stringProperties = typeof(T)
                .GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanRead);

            Expression? combinedExpression = null;

            foreach (var property in stringProperties)
            {
                // 建立屬性存取表達式 (例如: x.Name)
                var propertyAccess = Expression.Property(parameter, property);

                // 建立 null 檢查 (例如: x.Name != null)
                var nullCheck = Expression.NotEqual(
                    propertyAccess,
                    Expression.Constant(null, typeof(string))
                );

                // 建立 Contains 呼叫 (例如: x.Name.Contains(searchTerm))
                var containsCall = Expression.Call(
                    propertyAccess,
                    containsMethod,
                    Expression.Constant(searchTerm)
                );

                // 組合成: x.Name != null && x.Name.Contains(searchTerm)
                var condition = Expression.AndAlso(nullCheck, containsCall);

                // 使用 OR 連接多個條件
                combinedExpression = combinedExpression == null
                    ? condition
                    : Expression.OrElse(combinedExpression, condition);
            }

            // 如果沒有任何 string 屬性，返回 null
            if (combinedExpression == null)
                return null;

            // 建立完整的 Lambda 表達式
            return Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
        }
    }
}
