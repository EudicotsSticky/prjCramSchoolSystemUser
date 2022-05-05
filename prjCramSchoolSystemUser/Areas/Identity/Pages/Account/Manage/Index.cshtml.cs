using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using prjCramSchoolSystemUser.Data;

namespace prjCramSchoolSystemUser.Areas.Identity.Pages.Account.Manage
{
    // 首頁的模組繼承自PageModel
    public partial class IndexModel : PageModel
    {
        // 相依性注入
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _folder;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        // 資料模型
        [Display(Name = "帳號")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [Display(Name = "父親名稱")]
        [DataType(DataType.Text)]
        public string FatherName { get; set; }

        [Display(Name = "母親名稱")]
        [DataType(DataType.Text)]
        public string MotherName { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "手機號碼")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "請輸入名字")]
            [DataType(DataType.Text)]
            [Display(Name = "名字")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "請輸入姓")]
            [DataType(DataType.Text)]
            [Display(Name = "姓")]
            public string LastName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "性別")]
            public string Gender { get; set; }

            public List<SelectListItem> Genders { get; } = new List<SelectListItem>
            {
                new SelectListItem{Value=Enums.Gender.male.ToString(), Text="男"},
                new SelectListItem{Value=Enums.Gender.female.ToString(), Text="女"},
                new SelectListItem{Value=Enums.Gender.other.ToString(), Text="其他"},
            };

            [DataType(DataType.Date)]
            [Display(Name = "生日")]
            public DateTime? BirthDate { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "通訊地址")]
            public string Address { get; set; }

            [Display(Name = "入學年度")]
            public string Enrollment { get; set; }

            [Display(Name = "年級")]
            public string Grade { get; set; }

            [Display(Name = "就學狀態")]
            public string Status { get; set; }

            [Display(Name = "大頭貼照")]
            public string ThumbnailName { get; set; }

            [Display(Name = "最後更新日期")]
            [DataType(DataType.Date)]
            public DateTime? UpdateDate { get; set; }
        }


        // 載入方法，被後續方法呼叫
        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            if (!String.IsNullOrEmpty(user.FatherId))
            {
                ApplicationUser fatherData = await _userManager.FindByIdAsync(user.FatherId);
                if (fatherData != null)
                    FatherName = fatherData.LastName + fatherData.FirstName;
            }
            if (!String.IsNullOrEmpty(user.MotherId))
            {
                ApplicationUser motherData = await _userManager.FindByIdAsync(user.MotherId);
                if (motherData != null)
                    FatherName = motherData.LastName + motherData.FirstName;
            }
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                BirthDate = user.BirthDate,
                Enrollment = user.Enrollment,
                Gender = user.Gender,
                Grade = user.Grade,
                Status = user.Status,
                ThumbnailName = user.ThumbnailName,
                UpdateDate = user.UpdateDate
            };
            // Url.Content呼叫處，如果未來有要改資料夾儲存路徑，修改此處
            FolderPath = "~/Files/thumbnail/";

            // 如果完全無存入照片，使用預設照片
            if (!String.IsNullOrEmpty(user.ThumbnailName))
                ThumbnailPath = user.ThumbnailName;
            else
                ThumbnailPath = "noThumbnail.png";

        }
        // HttpGet方式取得資料
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            // 發生無預期狀況導致無法以ID載入資料
            if (user == null)
            {
                return NotFound($"無法載入使用者'{_userManager.GetUserId(User)}'。");
            }
            // 一般情況就採用載入方法加入user資料
            await LoadAsync(user);
            return Page();
        }

        // HttpPost方法(修改)
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"無法載入使用者'{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "嘗試設定電話號碼時發生未預期錯誤";
                    return RedirectToPage();
                }
            }
            ApplicationUser originData = user;
            // 如果資料有更新，更新欄位
            if (Input.Address != user.Address)
                user.Address = Input.Address;
            if (Input.BirthDate != user.BirthDate)
                user.BirthDate = Input.BirthDate;
            if (Input.Enrollment != user.Enrollment)
                user.Enrollment = Input.Enrollment;
            if (Input.FirstName != user.FirstName)
                user.FirstName = Input.FirstName;
            if (Input.Gender != user.Gender)
                user.Gender = Input.Gender;
            if (Input.Grade != user.Grade)
                user.Grade = Input.Grade;
            if (Input.LastName != user.LastName)
                user.LastName = Input.LastName;
            if (Input.Status != user.Status)
                user.Status = Input.Status;
            if (originData != user)
                user.UpdateDate = DateTime.Now;

            // 最初沒有照片時的上傳
            // IFormFile有抓到thumbnail且user.ThumbnailName沒有值，建立新圖片
            if (thumbnail != null && String.IsNullOrEmpty(user.ThumbnailName))
            {
                // 取得附檔名
                string thumbnailExt = Path.GetExtension(thumbnail.FileName);
                // 建立新檔案名稱，後面加上附檔名
                string newThumbnailName = Guid.NewGuid().ToString() + thumbnailExt;
                // 更新資料庫抓到的檔案名稱
                user.ThumbnailName = newThumbnailName;
                // 建立完整的檔案上傳路徑
                string newThumbNailSavePath = _folder + newThumbnailName;
                await SavePhotoToFileAsync(newThumbNailSavePath);
            }
            // 當資料庫有存檔案位置，但有更新頭貼資料
            else if (thumbnail != null && !String.IsNullOrEmpty(user.ThumbnailName))
            {
                // 建立完整的檔案上傳路徑
                string newThumbNailSavePath = _folder + user.ThumbnailName;
                // 使用Using，FileStream結束後釋放資源
                await SavePhotoToFileAsync(newThumbNailSavePath);
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "您的個人資料已更新成功";
            return RedirectToPage();
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

