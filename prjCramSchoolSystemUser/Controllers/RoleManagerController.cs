using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class RoleManagerController : Controller
    {
        // 注入使用IdentityRole的RoleManager
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleManagerController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: RoleManagerController 顯示所有的身分
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // POST: RoleManagerController/AddRole 新增身分
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string roleName)
        {
            try
            {
                if (roleName != null)
                {
                    // 移除目前roleName開頭和結尾的所有空白字元(整理格式)，加入身分
                    await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));

                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                if (id == null)
                    return RedirectToAction(nameof(Index));
                if (id != null)
                {
                    var role =await _roleManager.FindByIdAsync(id);
                    if(role==null)
                        return RedirectToAction(nameof(Index));
                    await _roleManager.DeleteAsync(role);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }

        }
    }
}