using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjCramSchoolSystemUser.Models;
using prjCramSchoolSystemUser.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Controllers
{
    public class CourseModelDetailController : Controller
    {
        public IActionResult addData(string coursenumber,string schedule,string scheduledetail,string teachingmethod,string remark)
        {
            string json = "";
            List<CCourseModelDetailViewModel> list = null;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_COURSE_MODELDETAIL))
            {
                json = HttpContext.Session.GetString(CDictionary.SK_COURSE_MODELDETAIL);
                list = JsonSerializer.Deserialize<List<CCourseModelDetailViewModel>>(json);
            }
            else
            {
                list = new List<CCourseModelDetailViewModel>();
            }
            list.Add(new CCourseModelDetailViewModel()
            {
                FCourseNumber = Convert.ToInt32(coursenumber),
                FSchedule = schedule,
                FScheduleDetail = scheduledetail,
                FTeachingMethod = teachingmethod,
                FRemark = remark
            });  
            json = JsonSerializer.Serialize(list);
            HttpContext.Session.SetString(CDictionary.SK_COURSE_MODELDETAIL, json);

            return Json(list);
        }
    }
}
