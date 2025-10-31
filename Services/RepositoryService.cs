using Microsoft.EntityFrameworkCore;
using  MozeApi.Data;

namespace MozeApi.Services
{
    /// <summary>
    /// 泛型 Repository 介面，約束提升到介面級別
    /// </summary>
    /// <typeparam name="T">實體類型</typeparam>
    public interface IRepositoryService<T> where T : class
    {
        /// <summary>
        /// 取得單筆資料
        /// </summary>
        Task<T?> GetDataWithIdAsync(object[] id);

        /// <summary>
        /// 取得整個Table資料
        /// </summary>
        Task<List<T>> GetAllDataAsync();

    }

    /// <summary>
    /// 泛型 Repository 實作
    /// </summary>
    public class RepositoryService<T>(MozeContext context) : IRepositoryService<T> where T : class
    {
        private readonly MozeContext _context = context;

        public async Task<T?> GetDataWithIdAsync(object[] id)
        {
            try
            {
                var result = await _context.Set<T>().FindAsync(id);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<T>> GetAllDataAsync()
        {
            try
            {
                var items = await _context.Set<T>().ToListAsync();
                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object?[] GetPrimaryKeyValues(T entity)
        {
            try
            {
                return _context.Entry(entity).Metadata.FindPrimaryKey()!.Properties
                               .Select(p => entity.GetType().GetProperty(p.Name)?.GetValue(entity))
                               .ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
