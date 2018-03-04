using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrivilegeManagement.PermissionMiddleware;

namespace PrivilegeManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加认证 cookie 信息
            //options是CookieAuthenticationOptions，关于这个类型提供如下属性，可参考：https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?tabs=aspnetcore2x
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    //默认地址，在这里可以设置未登陆默认跳转登陆页面
                    options.LoginPath = new PathString("/login");
                    //没有权限时跳转地址
                    options.AccessDeniedPath = new PathString("/home/denied");
                });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            //验证中间件
            app.UseAuthentication();
            //添加权限中间件, 一定要放在app.UseAuthentication后
            //因为UseAuthentication要从Cookie中加载通过验证的用户信息到Context.User中
            //所以一定要在加载完成后才能去验证用户信息（也可以自己读取cookie）
            app.UserPermission(new PermissionMiddlewareOption()
            {
                LoginAction = @"/login",
                NoPermissinoAction = @"/denied",
                //这个集合从数据库中查出用户的所有权限
                UserPermissions = new List<UserPermission>()
                {
                    new UserPermission { Url="/", UserName="aaa"},
                    new UserPermission { Url="/home/contact", UserName="aaa"},
                    new UserPermission { Url="/home/about", UserName="bbb"},
                    new UserPermission { Url="/", UserName="bbb"}
                }
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
