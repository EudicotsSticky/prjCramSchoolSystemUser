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
        //public TCourseInformationImg[] courseimg_arr { get; set; }
        public string courseimg { get; set; }
        public string Price { get; set; }
        public string DiscountDate
        {
            get
            {
                if (course.FDiscountDate == null)
                    return "";
                else
                {
                    DateTime discountdate = Convert.ToDateTime(course.FDiscountDate);
                    return discountdate.ToString("yyyy") + "年" + discountdate.ToString("MM") + "月" + discountdate.ToString("dd") + "日";
                }
            }
        }

        public string ClassState
        {
            get
            {
                if (course.FClassState == null)
                    return "";
                CourseData c = new CourseData();
                int p = Array.IndexOf(c.classstate_number, course.FClassState.ToString());
                if (p == -1)
                    return "";
                return c.classstate_name[p];
            }
        }

        public string SchoolYear
        {
            get
            {
                if (course.FCourse.FSchoolYear == null)
                    return "";
                CourseData c = new CourseData();
                int p = Array.IndexOf(c.schoolyear_number, course.FCourse.FSchoolYear.ToString());
                if (p == -1)
                    return "";
                return c.schoolyear_name[p];
            }
        }

        public List<CCourseModelDetail_List> Detail_List { get; set; }

        //讀取舊課程或新增課程
        //public int? casestatus { get; set; }

        //上傳圖片
        //public List<IFormFile> photo { get; set; }
        //public IFormFile photo1 { get; set; }
        //public IFormFile photo2 { get; set; }
        //public IFormFile photo3 { get; set; }
        //public IFormFile photo4 { get; set; }
        //public IFormFile photo5 { get; set; }
        //public IFormFile photo6 { get; set; }

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

    public class CCourseModelDetail_List
    {
        public int? FCourseNumber { get; set; }
        public string FSchedule { get; set; }
        public string FScheduleDetail { get; set; }
        public string FTeachingMethod { get; set; }
        public string FRemark { get; set; }
    }
}
