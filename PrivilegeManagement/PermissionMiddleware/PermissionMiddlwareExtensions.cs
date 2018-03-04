using Microsoft.AspNetCore.Builder;

namespace PrivilegeManagement.PermissionMiddleware
{
    /// <summary>
    /// 扩展权限中间件
    /// </summary>
    public static class PermissionMiddlwareExtensions
    {
        /// <summary>
        /// 引入权限中间件
        /// </summary>
        /// <param name="builder">扩展类型</param>
        /// <param name="option">权限中间件配置选项</param>
        /// <returns></returns>
        public static IApplicationBuilder UserPermission(
            this IApplicationBuilder builder, PermissionMiddlewareOption option)
        {
            return builder.UseMiddleware<PermissionMiddleware>(option);
        }
    }
}
