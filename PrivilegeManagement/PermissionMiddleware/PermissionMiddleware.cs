using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PrivilegeManagement.PermissionMiddleware
{
    /// <summary>
    /// 在Invoke中用了Context.User
    /// 首先调用app.UseAuthentication加载用户信息后才能在这里使用
    /// 没有验证的不做处理，如果登陆成功，就查看本次请求的url和用户权限是否匹配
    /// 不匹配就跳转到拒绝页（在startup添加中间件时，用noPermissionAction=@"/denied" 设置）
    /// </summary>
    public class PermissionMiddleware
    {
        /// <summary>
        /// 管道代理
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 权限中间件配置选项
        /// </summary>
        private readonly PermissionMiddlewareOption _option;
        /// <summary>
        /// 用户权限集合
        /// </summary>
        private static List<UserPermission> _userPermissions;

        /// <summary>
        /// 权限中间件构造
        /// </summary>
        /// <param name="next">管道代理对象</param>
        /// <param name="option">权限中间件配置选项</param>
        public PermissionMiddleware(RequestDelegate next, PermissionMiddlewareOption option)
        {
            _option = option;
            _next = next;
            _userPermissions = option.UserPermissions;
        }
       /// <summary>
       /// 调用管道
       /// </summary>
       /// <param name="context">请求上下文</param>
       /// <returns></returns>
        public Task Invoke(HttpContext context)
        {
            //请求url
            var questUrl = context.Request.Path.Value.ToLower();
            //是否验证通过
            var isAuthenticated = context.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                if (_userPermissions.GroupBy(g => g.Url).Any(w => w.Key.ToLower() == questUrl))
                {
                    //用户名
                    var userName = context.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid)?.Value;
                    if (_userPermissions.Any(w => w.UserName == userName && w.Url.ToLower() == questUrl))
                    {
                        return this._next(context);
                    }
                    else
                    {
                        //无权限跳转到拒绝页面
                        context.Response.Redirect(_option.NoPermissinoAction);
                    }
                }
            }
            return this._next(context);
        }
    }
}
