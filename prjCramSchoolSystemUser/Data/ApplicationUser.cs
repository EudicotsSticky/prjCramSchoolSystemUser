using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name="名字")]
        [PersonalData]
        public string FirstName { get; set; }

        [Display(Name ="姓")]
        [PersonalData]
        public string LastName { get; set; }

        [Display(Name ="性別")]
        [PersonalData]
        public string Gender { get; set; }

        [Display(Name ="生日")]
        [PersonalData]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "通訊地址")]
        [PersonalData]
        public string Address { get; set; }

        [Display(Name = "入學年度")]
        [PersonalData]
        public string Enrollment { get; set; }

        [Display(Name = "年級")]
        [PersonalData]
        public string Grade { get; set; }

        [Display(Name = "就學狀態")]
        [PersonalData]
        public string Status { get; set; }

        [PersonalData]
        public string ThumbnailName { get; set; }

        [Display(Name = "父親名稱")]
        [PersonalData]
        public string FatherId { get; set; }

        [Display(Name = "母親名稱")]
        [PersonalData]
        public string MotherId { get; set; }

        [Display(Name = "個人資料建立日期")]
        [PersonalData]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "最後更新日期")]
        [PersonalData]
        public DateTime? UpdateDate { get; set; }

        // 學生建立帳號時設立
        [ForeignKey("FatherId")]
        public virtual ApplicationUser Father { get; set; }

        [ForeignKey("MotherId")]
        public virtual ApplicationUser Mother { get; set; }



    }
}
