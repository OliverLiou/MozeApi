using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MozeApi.Entities
{
    /// <summary>
    /// 角色實體
    /// </summary>
    public class Role :IdentityRole
    {
        /// <summary>
        /// 角色名稱
        /// </summary>
        [MaxLength(50)]
        public string RoleDesc { get; set; } = null!;
    }
}