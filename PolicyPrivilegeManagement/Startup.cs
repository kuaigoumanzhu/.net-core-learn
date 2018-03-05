using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PolicyPrivilegeManagement
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
            services.AddMvc();
            //授权
            services.AddAuthorization(options=> {
                //基于角色组的策略
                options.AddPolicy("RequireClaim",policy=>policy.RequireRole("admin","system"));
                //基于用户名
                options.AddPolicy("RequireClaim",policy=>policy.RequireUserName("aaa"));
                //基于ClaimType
                options.AddPolicy("RequireClaim",policy=>policy.RequireClaim(ClaimTypes.Country,"中国"));
                //自定义
                options.AddPolicy("RequireClaim",policy=>policy.RequireClaim("customer","test"));
            });
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
