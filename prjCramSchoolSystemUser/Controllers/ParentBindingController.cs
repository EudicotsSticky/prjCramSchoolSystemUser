using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using prjCramSchoolSystemUser.Data;
using prjCramSchoolSystemUser.Models.ParentBindingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Controllers
{
    [Authorize(Roles = "Default, SuperAdmin")]
    public class ParentBindingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ParentBindingController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> ViewParent()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"無法載入使用者'{_userManager.GetUserName(User)}'.");
            
            ParentBindingModel parent = await (new ParentBindingModelFactory(_userManager, _signInManager)).LoadParentAsync(user);

            return await Task.Run(() => View(parent));
        }

        // 更改綁定
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewParent(ParentBindingModel parentBindingModel)
        {
            try
            {
                // 如果非從本頁post，重新導向
                if (parentBindingModel == null)
                    return RedirectToAction(nameof(ViewParent));
                // 建立使用者物件並將資料庫找到的使用者資料填入
                ApplicationUser user = await _userManager.FindByIdAsync(parentBindingModel.Id);
                // 如果找不到使用者，回傳
                if (user == null)
                    return NotFound("查無此使用者");
                // 找父親資料
                ApplicationUser fatherData =null;
                // text中有輸入資料，使用ParentBindingModelFactory中的方法找父親資料
                if(!String.IsNullOrEmpty(parentBindingModel.FatherEmailorUsername))
                fatherData = await (new ParentBindingModelFactory(_userManager, _signInManager)).FindParentAsync(parentBindingModel.FatherEmailorUsername);

                // 找母親資料
                ApplicationUser motherData =null;
                if (!String.IsNullOrEmpty(parentBindingModel.MotherEmailorUsername))
                    motherData = await (new ParentBindingModelFactory(_userManager, _signInManager)).FindParentAsync(parentBindingModel.MotherEmailorUsername);
                if (fatherData != null)
                {
                    if (fatherData.Id != user.FatherId)
                        user.FatherId = fatherData.Id;
                }
                if (motherData != null)
                {
                    if (motherData.Id != user.MotherId)
                        user.MotherId = motherData.Id;
                }
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction(nameof(ViewParent));
            }
            catch (Exception ex)
            {
                // 正式使用時註解此行
                return Content(ex.Message);
                // 正式使用時採重新導向
                //RedirectToAction(nameof(ViewParent));
            }
        }

        // Api:Ajax找，回傳是否有找到家長
        public async Task<IActionResult> FindUserParent(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Content("查無此資料");
            ApplicationUser parentData = new ApplicationUser();
            parentData = await (new ParentBindingModelFactory(_userManager, _signInManager)).FindParentAsync(id);
            if (parentData != null)
                return Content($"您的家長為：{parentData.LastName.ToString() + parentData.FirstName.ToString()}");
            else
                return Content("查無此資料");
        }
        // 點開連結會觸發
        public async Task<IActionResult> UnbindFather()
        {
            // 解除綁定判斷交給前端，此處直接解綁
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"無法載入使用者'{_userManager.GetUserName(User)}'.");
            if(String.IsNullOrEmpty(user.FatherId))
                return RedirectToAction(nameof(ViewParent));

            user.FatherId = null;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction(nameof(ViewParent));
        }

        // 點開連結會觸發
        public async Task<IActionResult> UnbindMother()
        {
            // 解除綁定判斷交給前端，此處直接解綁
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"無法載入使用者'{_userManager.GetUserName(User)}'.");
            if (String.IsNullOrEmpty(user.MotherId))
                return RedirectToAction(nameof(ViewParent));

            user.MotherId = null;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction(nameof(ViewParent));
        }

    }
}
