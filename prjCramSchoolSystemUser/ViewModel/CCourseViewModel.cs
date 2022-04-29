using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using prjCramSchoolSystemUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.ViewModel
{
    public class CCourseViewModel
    {
        //課程
        public TCourseInformation course { get; set; }
        public TCourseInformationImg[] courseimg_arr { get; set; }

        //讀取舊課程或新增課程
        public int? casestatus { get; set; }

        //上傳圖片
        //public List<IFormFile> photo { get; set; }
        public IFormFile photo1 { get; set; }
        public IFormFile photo2 { get; set; }
        public IFormFile photo3 { get; set; }
        public IFormFile photo4 { get; set; }
        public IFormFile photo5 { get; set; }
        public IFormFile photo6 { get; set; }

        //下拉選單
        public List<SelectListItem> CourseDDL;
        public List<SelectListItem> ClassDDL = new CourseMenu().ClassStateDDL;



        //要放在函式裡 不能使用變數
        //(屬性:{get; set;})
        //放在這 可以new 但不能被使用
        //CramSchoolDBContext db = new CramSchoolDBContext();
        //public List<SelectListItem> CourseDDL1 = new CourseMenu1(db).CourseModelDDL;

        //public List<SelectListItem> CourseDDL1 { get; set; }
        //public CCourseViewModel()
        //{
        //    CramSchoolDBContext db = new CramSchoolDBContext();
        //    CourseDDL1 = new CourseMenu1(db).CourseModelDDL;
        //}
    }
}
