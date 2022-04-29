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
    public class CourseModelController : Controller
    {
        //private readonly CramSchoolDBContext _context;
        //public CourseModelController(CramSchoolDBContext context)
        //{
        //    _context = context;
        //}

        ////列表
        //public IActionResult List()
        //{
        //    //課程狀態 是否顯示
        //    CCourseModelShowState c = new CCourseModelShowState();
        //    int show = c.showCourse("Y");

        //    var datas = from t in _context.TCourseModels.Where(c => c.FModleSate.Equals(show))
        //    //var datas = from t in _context.TCourseModels.Where(c => c.FModleSate == show)
        //                select t;
        //    return View(datas);
        //}

        ////新增、修改
        //public IActionResult Edit(string id)
        //{
        //    CCourseModelAllViewModel cmodel_all = new CCourseModelAllViewModel();
        //    //判斷新增或修改(有沒有id)
        //    if (string.IsNullOrEmpty(id))
        //        return View(cmodel_all);

        //    //判斷有沒有課程模板
        //    TCourseModel c_model = _context.TCourseModels.FirstOrDefault(m => m.FCourseId == id);
        //    if (c_model == null)
        //        return View("List");
        //    cmodel_all.c_model = c_model;

        //    //判斷有沒有課程細項
        //    var data = from t in _context.TCourseModelDetails.Where(c => c.FCourseId == id)
        //               select t;
        //    if (data != null)
        //        cmodel_all.c_model_detail_list = data.ToList();
        //    return View(cmodel_all);
        //}

        //[HttpPost]
        //public IActionResult Edit(CCourseModelAllViewModel c)
        //{
        //    string courseID = "", userID = ""; DateTime now;
        //    readUserData(out userID, out now);

        //    if (string.IsNullOrEmpty(c.c_model.FCourseId))
        //        //新增課程模板
        //        courseID = newCaseModel(c, userID, now);

        //    else
        //        //編輯課程模板
        //        courseID = editCaseModel(c, userID, now);

        //    if (c.c_model_detail_list != null && c.c_model_detail_list.Count > 0)
        //        saveDetail(courseID, c.c_model_detail_list, userID, now);
        //    return RedirectToAction("List");
        //}

        ////新增課程模板
        //[NonAction]
        //private string newCaseModel(CCourseModelAllViewModel c, string userID, DateTime now)
        //{
        //    string courseID = getCourseModelID(c.c_model.FCategory.ToString());
        //    c.c_model.FCourseId = courseID;
        //    c.c_model.FCreationUser = userID;
        //    c.c_model.FSaverUser = userID;
        //    c.c_model.FCreationDate = now;
        //    c.c_model.FSaverDate = now;
        //    c.c_model.FModleSate = new CCourseModelShowState().showCourse("Y");

        //    _context.TCourseModels.Add(c.c_model);
        //    _context.SaveChanges();
        //    return courseID;
        //}

        ////編輯課程模板
        //[NonAction]
        //private string editCaseModel(CCourseModelAllViewModel c, string userID, DateTime now)
        //{
        //    string courseID = c.c_model.FCourseId;
        //    TCourseModel course = _context.TCourseModels.FirstOrDefault(m => m.FCourseId == c.c_model.FCourseId);
        //    course.FCategory = c.c_model.FCategory;
        //    course.FName = c.c_model.FName;
        //    course.FTotalHours = c.c_model.FTotalHours;
        //    course.FTotalNumber = c.c_model.FTotalNumber;
        //    course.FGrade = c.c_model.FGrade;
        //    course.FSchoolYear = c.c_model.FSchoolYear;
        //    course.FSummary = c.c_model.FSummary;
        //    course.FModleSate = c.c_model.FModleSate;
        //    course.FTeachingMaterial = c.c_model.FTeachingMaterial;
        //    course.FOriginalPrice = c.c_model.FOriginalPrice;
        //    course.FSpecialOffer = c.c_model.FSpecialOffer;

        //    course.FCreationUser = userID;
        //    course.FSaverUser = userID;
        //    course.FCreationDate = now;
        //    course.FSaverDate = now;
        //    _context.SaveChanges();
        //    return courseID;
        //}

        ////儲存課程細項
        //[NonAction]
        //public void saveDetail(string courseID, List<TCourseModelDetail> detail_List, string userID, DateTime now)
        //{
        //    foreach (var item in detail_List)
        //    {
        //        item.FCourseId = courseID;
        //        item.FCreationUser = userID;
        //        item.FCreationDate = now;
        //        item.FSaverUser = userID;
        //        item.FSaverDate = now;
        //    }
        //    clearModelDetail(courseID);
        //    _context.TCourseModelDetails.AddRange(detail_List);
        //    _context.SaveChanges();
        //}

        ////new案件編號
        //[NonAction]
        //public string getCourseModelID(string category)
        //{
        //    string now = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
        //    Random r = new Random();
        //    return category.getCourseEName() + now + r.Next(0, 9);
        //}

        ////取出userID、現在時間
        //[NonAction]
        //public void readUserData(out string userID,out DateTime now)
        //{
        //    userID = "";
        //    now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //    if (HttpContext.Session.Keys.Contains(CDictionary.SK_LONGUNED_ID))
        //    {
        //        //讀Session-userID
        //        string json = HttpContext.Session.GetString(CDictionary.SK_LONGUNED_ID);
        //        userID = JsonSerializer.Deserialize<string>(json);
        //    }
        //}

        ////清除Detail舊資料
        //[NonAction]
        //private void clearModelDetail(string fCourseId)
        //{
        //    var data = from t in _context.TCourseModelDetails.Where(c => c.FCourseId == fCourseId)
        //               select t;
        //    if (data.ToList().Count == 0)
        //        return;
        //    _context.TCourseModelDetails.RemoveRange(data);
        //    _context.SaveChanges();
        //}

        ////目前沒使用
        ////刪除課程模板
        //public IActionResult Delete(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return RedirectToAction("List");
        //    TCourseModel modle = _context.TCourseModels.FirstOrDefault(m => m.FCourseId == id);
        //    if (modle != null)
        //    {
        //        _context.Remove(modle);
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("List");
        //}

        ////目前沒使用到
        ////整理拿到的json成detail_list
        //[NonAction]
        //private List<TCourseModelDetail> tidyDetail(string courseID, string arrDetail)
        //{
        //    //移除_Count屬性
        //    while (arrDetail.Contains("_Count"))
        //    {
        //        int p_start = arrDetail.IndexOf("_");
        //        int p_end = arrDetail.IndexOf(",", p_start);
        //        arrDetail = arrDetail.Remove(p_start - 1, p_end - (p_start - 1) + 1);
        //    }

        //    //DateTime now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //    //string userID = readUserID();
        //    string userID = "";
        //    DateTime now;
        //    readUserData(out userID, out now);

        //    List<TCourseModelDetail> list = JsonSerializer.Deserialize<List<TCourseModelDetail>>(arrDetail);
        //    foreach (var item in list)
        //    {
        //        item.FCourseId = courseID;
        //        item.FCreationUser = userID;
        //        item.FCreationDate = now;
        //        item.FSaverUser = userID;
        //        item.FSaverDate = now;
        //    }
        //    return list;
        //}

        ////目前沒使用到
        ////儲存課程細項
        //public IActionResult saveDetail(string courseID, string suject, string arrDetail)
        //{
        //    //new CourseModel 建立新資料
        //    if (string.IsNullOrEmpty(courseID))
        //    {
        //        courseID = getCourseModelID(suject);
        //        TCourseModel model = new TCourseModel();
        //        model.FCourseId = courseID;
        //        _context.TCourseModels.Add(model);
        //        _context.SaveChanges();
        //    }
        //    //子項目是否有資料
        //    if (string.IsNullOrEmpty(arrDetail))
        //        return Content(courseID);

        //    clearModelDetail(courseID);
        //    List<TCourseModelDetail> detail_List = tidyDetail(courseID, arrDetail);
        //    _context.TCourseModelDetails.AddRange(detail_List);
        //    _context.SaveChanges();
        //    return Content(courseID);
        //}

        //#region
        ////新增、修改
        ////public IActionResult Detail(string id)
        ////{
        ////    CCourseModelAllViewModel cmodel_all = new CCourseModelAllViewModel();
        ////    //判斷新增或修改(有沒有id)
        ////    if (string.IsNullOrEmpty(id))
        ////        return View(cmodel_all);

        ////    //判斷有沒有課程模板
        ////    TCourseModel c_model = _context.TCourseModels.FirstOrDefault(m => m.FCourseId == id);
        ////    if (c_model == null)
        ////        return View("List");
        ////    cmodel_all.c_model = c_model;

        ////    //判斷有沒有課程細項
        ////    var data = from t in _context.TCourseModelDetails.Where(c => c.FCourseId == id)
        ////               select t;
        ////    if (data != null)
        ////        cmodel_all.c_model_detail_list = data.ToList();
        ////    return View(cmodel_all);
        ////}

        ////[HttpPost]
        ////public IActionResult Detail(CCourseModelAllViewModel c)
        ////{
        ////    string courseID = "", userID = ""; DateTime now;
        ////    readUserData(out userID, out now);

        ////    if (string.IsNullOrEmpty(c.c_model.FCourseId))
        ////        //新增課程模板
        ////        courseID = newCaseModel(c, userID, now);

        ////    else
        ////        //編輯課程模板
        ////        courseID = editCaseModel(c, userID, now);

        ////    if (c.c_model_detail_list != null && c.c_model_detail_list.Count > 0)
        ////        saveDetail(courseID, c.c_model_detail_list, userID, now);
        ////    return RedirectToAction("List");
        ////}
        //#endregion

        //public IActionResult Index(CCourseModelAllViewModel cmodel_all)
        //{
        //    return View();
        //}
    }
}
