using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MozeApi.Data;
using MozeApi.DTOs.Request;
using MozeApi.DTOs.Response;
using MozeApi.Entities;

namespace MozeApi.Services
{
    /// <summary>
    /// 記錄服務介面 (Transaction 與 AppUrl CRUD)
    /// </summary>
    public interface IRecordService
    {
        // Generic Find Method
        Task<PagedResponse<TResponse>> FindAsync<TEntity, TResponse>(
            int page,
            int pageSize,
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool orderByDescending = true,
            Expression<Func<TEntity, bool>>? searchPredicate = null,
            params Expression<Func<TEntity, object>>[]? includes
        ) where TEntity : class where TResponse : class;

        // Transaction CRUD
        Task<TransactionResponse?> CreateTransactionAsync(CreateTransactionRequest request);
        Task<TransactionResponse?> GetTransactionAsync(int id);
        Task<TransactionResponse?> UpdateTransactionAsync(int id, UpdateTransactionRequest request);
        Task<bool> DeleteTransactionAsync(int id);

        // AppUrl CRUD
        Task<AppUrlResponse?> CreateAppUrlAsync(CreateAppUrlRequest request);
        Task<AppUrlResponse?> GetAppUrlAsync(int id);
        Task<AppUrlResponse?> UpdateAppUrlAsync(int id, UpdateAppUrlRequest request);
        Task<bool> DeleteAppUrlAsync(int id);
    }

    /// <summary>
    /// 記錄服務實作 (Transaction 與 AppUrl CRUD)
    /// </summary>
    public class RecordService(MozeContext context, IMapper mapper) : IRecordService
    {
        private readonly MozeContext _context = context;
        private readonly IMapper _mapper = mapper;

        #region Generic Find Method

        public async Task<PagedResponse<TResponse>> FindAsync<TEntity, TResponse>(
            int page,
            int pageSize,
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool orderByDescending = true,
            Expression<Func<TEntity, bool>>? searchPredicate = null,
            params Expression<Func<TEntity, object>>[]? includes
        ) where TEntity : class where TResponse : class
        {
            var query = _context.Set<TEntity>().AsQueryable();

            // 套用 Include
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // 套用基本篩選
            if (filter != null)
                query = query.Where(filter);

            // 套用搜尋條件
            if (searchPredicate != null)
                query = query.Where(searchPredicate);

            // 套用排序
            if (orderBy != null)
            {
                query = orderByDescending
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var entities = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<TResponse>
            {
                Data = _mapper.Map<List<TResponse>>(entities),
                TotalCount = totalCount,
            };
        }

        #endregion

        #region Transaction CRUD

        public async Task<TransactionResponse?> CreateTransactionAsync(CreateTransactionRequest request)
        {
            var transaction = _mapper.Map<Transaction>(request);
            transaction.CreatedAt = DateTime.UtcNow;
            transaction.IsActive = true;

            _context.Transaction.Add(transaction);
            await _context.SaveChangesAsync();

            return _mapper.Map<TransactionResponse>(transaction);
        }

        public async Task<TransactionResponse?> GetTransactionAsync(int id)
        {
            var transaction = await _context.Transaction
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TransactionId == id && t.IsActive);

            return transaction == null ? null : _mapper.Map<TransactionResponse>(transaction);
        }

        public async Task<TransactionResponse?> UpdateTransactionAsync(int id, UpdateTransactionRequest request)
        {
            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(t => t.TransactionId == id && t.IsActive);

            if (transaction == null)
                return null;

            // 只更新有提供的欄位
            if (request.TransactionType.HasValue)
                transaction.TransactionType = request.TransactionType.Value;
            if (request.Amount.HasValue)
                transaction.Amount = request.Amount.Value;
            if (request.Currency != null)
                transaction.Currency = request.Currency;
            if (request.Account != null)
                transaction.Account = request.Account;
            if (request.Project != null)
                transaction.Project = request.Project;
            if (request.Category != null)
                transaction.Category = request.Category;
            if (request.Subcategory != null)
                transaction.Subcategory = request.Subcategory;
            if (request.Name != null)
                transaction.Name = request.Name;
            if (request.Store != null)
                transaction.Store = request.Store;
            if (request.Note != null)
                transaction.Note = request.Note;
            if (request.Tags != null)
                transaction.Tags = request.Tags;
            if (request.Date != null)
                transaction.Date = request.Date;
            if (request.Time != null)
                transaction.Time = request.Time;
            if (request.Fee.HasValue)
                transaction.Fee = request.Fee.Value;
            if (request.FeeName != null)
                transaction.FeeName = request.FeeName;
            if (request.Bonus.HasValue)
                transaction.Bonus = request.Bonus.Value;
            if (request.BonusName != null)
                transaction.BonusName = request.BonusName;
            if (request.IsActive.HasValue)
                transaction.IsActive = request.IsActive.Value;

            transaction.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<TransactionResponse>(transaction);
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(t => t.TransactionId == id && t.IsActive);

            if (transaction == null)
                return false;

            // 軟刪除
            transaction.IsActive = false;
            transaction.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region AppUrl CRUD

        public async Task<AppUrlResponse?> CreateAppUrlAsync(CreateAppUrlRequest request)
        {
            var appUrl = _mapper.Map<AppUrl>(request);
            appUrl.CreatedAt = DateTime.UtcNow;

            _context.AppUrl.Add(appUrl);
            await _context.SaveChangesAsync();

            return _mapper.Map<AppUrlResponse>(appUrl);
        }

        public async Task<AppUrlResponse?> GetAppUrlAsync(int id)
        {
            var appUrl = await _context.AppUrl
                .Include(a => a.Transaction)
                .FirstOrDefaultAsync(a => a.AppUrlId == id);

            return appUrl == null ? null : _mapper.Map<AppUrlResponse>(appUrl);
        }

        public async Task<AppUrlResponse?> UpdateAppUrlAsync(int id, UpdateAppUrlRequest request)
        {
            var appUrl = await _context.AppUrl
                .FirstOrDefaultAsync(a => a.AppUrlId == id);

            if (appUrl == null)
                return null;

            // 只更新有提供的欄位
            if (request.Url != null)
                appUrl.Url = request.Url;
            if (request.IsFinished.HasValue)
                appUrl.IsFinished = request.IsFinished.Value;
            if (request.TransactionId.HasValue)
                appUrl.TransactionId = request.TransactionId.Value;

            appUrl.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<AppUrlResponse>(appUrl);
        }

        public async Task<bool> DeleteAppUrlAsync(int id)
        {
            var appUrl = await _context.AppUrl
                .FirstOrDefaultAsync(a => a.AppUrlId == id);

            if (appUrl == null)
                return false;

            // 硬刪除
            _context.AppUrl.Remove(appUrl);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion
    }
}
