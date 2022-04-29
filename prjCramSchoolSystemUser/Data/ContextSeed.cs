using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Data
{
    public static class ContextSeed
    {
        // 使用SeedRolesAsync來對帳號加入身分，參數：UserManager使用自定義ApplicationUser(繼承自IdentityUser)，RoleManager則採用預設的IdentityRole
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Teacher.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Parent.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Default.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "eudicots626@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            // 如果所有User找不到Id與defaultUser相同
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                // 找Email，有無defaultUser的Email
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                // 找不到(即無此筆資料，尚未創建超級帳號)
                if (user == null)
                {
                    // 建立defaultUser，密碼為第二個參數
                    await userManager.CreateAsync(defaultUser, "Yee123!");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString()); await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Teacher.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Parent.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Default.ToString());
                }
            }
        }
    }
}
