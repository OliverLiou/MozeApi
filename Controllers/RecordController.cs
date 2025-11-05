using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MozeApi.DTOs.Request;
using MozeApi.DTOs.Response;
using MozeApi.Entities;
using MozeApi.Services;

namespace MozeApi.Controllers
{
    /// <summary>
    /// 記錄控制器 (Transaction 與 AppUrl CRUD)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController(IRecordService recordService) : BaseController
    {
        private readonly IRecordService _recordService = recordService;

        #region Transaction Endpoints

        /// <summary>
        /// 新增交易記錄
        /// </summary>
        [HttpPost("CreateTransaction")]
        public async Task<ActionResult<TransactionResponse>> CreateTransaction([FromBody] CreateTransactionRequest request)
        {
            try
            {
                var result = await _recordService.CreateTransactionAsync(request);
                return result == null ? BadRequest("Failed to create transaction") : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        /// <summary>
        /// 取得單筆交易記錄
        /// </summary>
        [HttpGet("GetTransaction/{id}")]
        public async Task<ActionResult<TransactionResponse>> GetTransaction(int id)
        {
            try
            {
                var result = await _recordService.GetTransactionAsync(id);
                return result == null ? NotFound(new { Message = $"Transaction with ID {id} not found" }) : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        /// <summary>
        /// 取得交易記錄列表 (支援分頁、排序、搜尋)
        /// </summary>
        /// <param name="page">頁碼 (預設: 1)</param>
        /// <param name="pageSize">每頁筆數 (預設: 10)</param>
        /// <param name="sortBy">排序欄位 (可選: amount, date, createdAt)</param>
        /// <param name="sortOrder">排序方向 (asc/desc, 預設: desc)</param>
        /// <param name="search">搜尋關鍵字</param>
        [Authorize]
        [HttpGet("GetTransactions")]
        public async Task<ActionResult<PagedResponse<TransactionResponse>>> GetTransactions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? search = null)
        {
            try
            {
                // 取得當前登入使用者的 ID
                var userId = GetCurrentUserId()!;

                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // 限制最大筆數

                // 建立搜尋條件
                Expression<Func<Transaction, bool>>? searchPredicate = null;
                if (!string.IsNullOrWhiteSpace(search))
                {
                    searchPredicate = t => typeof(Transaction)
                        .GetProperties()
                        .Where(p => p.PropertyType == typeof(string) && p.GetValue(t) != null)
                        .Any(p => ((string)p.GetValue(t)!).Contains(search));
                }

                // 建立排序欄位
                Expression<Func<Transaction, object>> orderBy = sortBy?.ToLower() switch
                {
                    "amount" => t => t.Amount,
                    "date" => t => t.Date!,
                    _ => t => t.TransactionId
                };

                var result = await _recordService.FindAsync<Transaction, TransactionResponse>(
                    page: page,
                    pageSize: pageSize,
                    filter: t => t.IsActive && t.UserId == userId, // 只顯示當前使用者的資料
                    orderBy: orderBy,
                    orderByDescending: sortOrder?.ToLower() != "asc",
                    searchPredicate: searchPredicate,
                    includes: t => t.User
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        /// <summary>
        /// 更新交易記錄
        /// </summary>
        [HttpPut("UpdateTransaction/{id}")]
        public async Task<ActionResult<TransactionResponse>> UpdateTransaction(int id, [FromBody] UpdateTransactionRequest request)
        {
            try
            {
                var result = await _recordService.UpdateTransactionAsync(id, request);
                return result == null ? NotFound(new { Message = $"Transaction with ID {id} not found" }) : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        /// <summary>
        /// 刪除交易記錄 (軟刪除)
        /// </summary>
        [HttpDelete("DeleteTransaction/{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            try
            {
                var result = await _recordService.DeleteTransactionAsync(id);
                return result
                    ? Ok(new { Message = $"Transaction with ID {id} deleted successfully" })
                    : NotFound(new { Message = $"Transaction with ID {id} not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        #endregion

        #region AppUrl Endpoints

        /// <summary>
        /// 新增 App URL 記錄
        /// </summary>
        [HttpPost("CreateAppUrl")]
        public async Task<ActionResult<AppUrlResponse>> CreateAppUrl([FromBody] CreateAppUrlRequest request)
        {
            try
            {
                var result = await _recordService.CreateAppUrlAsync(request);
                return result == null ? BadRequest("Failed to create app URL") : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        /// <summary>
        /// 取得單筆 App URL 記錄
        /// </summary>
        [HttpGet("GetAppUrl/{id}")]
        public async Task<ActionResult<AppUrlResponse>> GetAppUrl(int id)
        {
            try
            {
                var result = await _recordService.GetAppUrlAsync(id);
                return result == null ? NotFound(new { Message = $"AppUrl with ID {id} not found" }) : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        /// <summary>
        /// 取得 App URL 記錄列表 (支援分頁、排序、搜尋)
        /// </summary>
        /// <param name="page">頁碼 (預設: 1)</param>
        /// <param name="pageSize">每頁筆數 (預設: 10)</param>
        /// <param name="sortBy">排序欄位 (可選: isFinished, createdAt)</param>
        /// <param name="sortOrder">排序方向 (asc/desc, 預設: desc)</param>
        /// <param name="search">搜尋關鍵字</param>
        [HttpGet("GetAppUrls")]
        public async Task<ActionResult<PagedResponse<AppUrlResponse>>> GetAppUrls(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] string? search = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // 限制最大筆數

                // 建立搜尋條件
                Expression<Func<AppUrl, bool>>? searchPredicate = null;
                if (!string.IsNullOrWhiteSpace(search))
                {
                    searchPredicate = a => typeof(AppUrl)
                        .GetProperties()
                        .Where(p => p.PropertyType == typeof(string) && p.GetValue(a) != null)
                        .Any(p => ((string)p.GetValue(a)!).Contains(search));
                }

                // 建立排序欄位
                Expression<Func<AppUrl, object>> orderBy = sortBy?.ToLower() switch
                {
                    "isfinished" => a => a.IsFinished,
                    _ => a => a.CreatedAt
                };

                var result = await _recordService.FindAsync<AppUrl, AppUrlResponse>(
                    page: page,
                    pageSize: pageSize,
                    filter: null,
                    orderBy: orderBy,
                    orderByDescending: sortOrder?.ToLower() != "asc",
                    searchPredicate: searchPredicate,
                    includes: a => a.Transaction
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        /// <summary>
        /// 更新 App URL 記錄
        /// </summary>
        [HttpPut("UpdateAppUrl/{id}")]
        public async Task<ActionResult<AppUrlResponse>> UpdateAppUrl(int id, [FromBody] UpdateAppUrlRequest request)
        {
            try
            {
                var result = await _recordService.UpdateAppUrlAsync(id, request);
                return result == null ? NotFound(new { Message = $"AppUrl with ID {id} not found" }) : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        /// <summary>
        /// 刪除 App URL 記錄 (硬刪除)
        /// </summary>
        [HttpDelete("DeleteAppUrl/{id}")]
        public async Task<ActionResult> DeleteAppUrl(int id)
        {
            try
            {
                var result = await _recordService.DeleteAppUrlAsync(id);
                return result
                    ? Ok(new { Message = $"AppUrl with ID {id} deleted successfully" })
                    : NotFound(new { Message = $"AppUrl with ID {id} not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        #endregion
    }
}
