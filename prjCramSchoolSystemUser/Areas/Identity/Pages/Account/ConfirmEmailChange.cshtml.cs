using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using prjCramSchoolSystemUser.Data;

namespace prjCramSchoolSystemUser.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ConfirmEmailChangeModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = "信箱更改發生錯誤，請稍後重試。";
                return Page();
            }

            // 此處不需要，因為已經將Username與Email分開
            //// In our UI email and user name are one and the same, so when we update the email
            //// we need to update the user name.
            //var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            //if (!setUserNameResult.Succeeded)
            //{
            //    StatusMessage = "Error changing user name.";
            //    return Page();
            //}

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "您的信箱已更改成功。";
            return Page();
        }
    }
}
