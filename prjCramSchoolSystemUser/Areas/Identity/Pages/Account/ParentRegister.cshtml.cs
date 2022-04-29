using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using prjCramSchoolSystemUser.Data;

namespace prjCramSchoolSystemUser.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ParentRegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ParentRegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _folder;

        public ParentRegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ParentRegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            // 把上傳目錄設為：wwwroot\Files\thumbnail
            _folder = Path.Combine(_webHostEnvironment.WebRootPath, @"Files\thumbnail\");
        }

        // 大頭貼資料夾存取路徑
        public string FolderPath { get; set; }

        // 大頭貼路徑組成
        public string ThumbnailPath { get; set; }

        // 從input file裡取name為thumbnail的值
        public IFormFile thumbnail { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            // 更改預設註冊Username
            [Required]
            [Display(Name = "帳號")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0}必須至少包含{2}字且最多不超過{1}字。", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "密碼")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "密碼確認")]
            [Compare("Password", ErrorMessage = "兩次輸入密碼並不相符。")]
            public string ConfirmPassword { get; set; }

            // 下方為自訂欄位

            [Display(Name = "大頭貼照")]
            public string ThumbnailName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "名字")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "姓")]
            public string LastName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "性別")]
            public string Gender { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "生日")]
            public DateTime? BirthDate { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "通訊地址")]
            public string Address { get; set; }

            [Display(Name = "子女名稱")]
            public ICollection<ApplicationUser> ChildNames { get; set; }

            [Display(Name = "個人資料建立日期")]
            [DataType(DataType.Date)]
            public DateTime? CreateDate { get; set; }

            [Display(Name = "最後更新日期")]
            [DataType(DataType.Date)]
            public DateTime? UpdateDate { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            // Url.Content呼叫處，如果未來有要改資料夾儲存路徑，修改此處
            FolderPath = "~/Files/thumbnail/";

            ThumbnailPath = "noThumbnail.png";

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                if (thumbnail != null)
                {
                    string thumbnailExt = Path.GetExtension(thumbnail.FileName);
                    string newThumbnailName = Guid.NewGuid().ToString() + thumbnailExt;
                    Input.ThumbnailName = newThumbnailName;
                    string newThumbNailSavePath = _folder + newThumbnailName;
                    await SavePhotoToFileAsync(newThumbNailSavePath);

                }

                var user = new ApplicationUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    ThumbnailName = Input.ThumbnailName,
                    Address = Input.Address,
                    BirthDate = Input.BirthDate,
                    CreateDate = DateTime.Now,
                    FirstName = Input.FirstName,
                    Gender = Input.Gender,
                    LastName = Input.LastName
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    // 註冊時給予預設的身分
                    await _userManager.AddToRoleAsync(user, Enums.Roles.Default.ToString());
                    // 產生確認信token
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "確認您的信箱",
                        $"請<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>點擊此處</a>確認您的帳號。");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task SavePhotoToFileAsync(string newThumbNailSavePath)
        {
            using (FileStream fs = new FileStream(newThumbNailSavePath, FileMode.Create))
            {
                // 直接覆蓋檔案名稱，覆蓋原檔案
                await thumbnail.CopyToAsync(fs);
            }
        }
    }
}
