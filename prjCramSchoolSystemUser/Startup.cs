using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using prjCramSchoolSystemUser.Data;
using prjCramSchoolSystemUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser
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
            #region Identity Database configuration
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            #endregion
            // 此處為用razor pages生成驗證頁面
            services.AddRazorPages();

            services.AddDbContext<CramSchoolDBContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(
                    Configuration.GetConnectionString("CramSchoolDB")));


            services.AddDatabaseDeveloperPageExceptionFilter();

            // 加入session服務
            services.AddSession();

            // 此處加入身分驗證服務
            services.Configure<IdentityOptions>(options =>
            {
                // 密碼設定
                // 需要數字
                options.Password.RequireDigit = true;
                // 需要小寫
                options.Password.RequireLowercase = true;
                // 需要非英數字
                options.Password.RequireNonAlphanumeric = true;
                // 需要大寫
                options.Password.RequireUppercase = true;
                // 需要密碼長度大於6
                options.Password.RequiredLength = 6;
                // 需要特殊字元
                options.Password.RequiredUniqueChars = 1;

                // 鎖定設定
                // 預設密碼鎖定：五分鐘
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                // 最大可嘗試次數
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // 允許使用者名稱為：
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            // 加入信箱驗證服務
            services.AddTransient<IEmailSender, SendEmail>();

            // 加入cookie服務
            services.ConfigureApplicationCookie(options =>
            {
                //  設定httpOnly屬性，禁止JavaScript直接存取cookie，避免他人盜用帳號。
                options.Cookie.HttpOnly = true;
                // 設定Cookie有效期限：五分鐘
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                // 登入路徑
                options.LoginPath = "/Identity/Account/Login";
                // 設定到拒絕存取路徑
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                // 設定Cookie是否會過期
                options.SlidingExpiration = true;
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //加入session
            app.UseSession();


            // 啟動驗證服務
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
