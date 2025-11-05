using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MozeApi.Controllers
{
    /// <summary>
    /// 基礎控制器，提供共用方法
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// 取得當前登入使用者的 ID
        /// </summary>
        /// <returns>使用者 ID，若未登入則返回 null</returns>
        protected string? GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        /// <summary>
        /// 取得當前登入使用者的 Email
        /// </summary>
        /// <returns>使用者 Email，若未登入則返回 null</returns>
        protected string? GetCurrentUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        /// <summary>
        /// 取得當前登入使用者的名稱
        /// </summary>
        /// <returns>使用者名稱，若未登入則返回 null</returns>
        protected string? GetCurrentUserName()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }

        /// <summary>
        /// 驗證使用者是否已登入
        /// </summary>
        /// <returns>ActionResult 包含錯誤訊息，若已登入則返回 null</returns>
        protected ActionResult? ValidateUserAuthentication()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User not authenticated" });
            }
            return null;
        }
    }
}
