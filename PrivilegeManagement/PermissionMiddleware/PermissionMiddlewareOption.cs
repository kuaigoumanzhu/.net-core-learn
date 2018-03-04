using System.Collections.Generic;

namespace PrivilegeManagement.PermissionMiddleware
{
    /// <summary>
    /// 权限中间件选项
    /// </summary>
    public class PermissionMiddlewareOption
    {
        /// <summary>
        /// 登陆action
        /// </summary>
        public string LoginAction { get; set; }

        /// <summary>
        /// 无权限action
        /// </summary>
        public string NoPermissinoAction { get; set; }
        /// <summary>
        /// 用户权限集合
        /// </summary>
        public List<UserPermission> UserPermissions { get; set; }=new List<UserPermission>();
    }
}
