using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using prjCramSchoolSystemUser.Models;
using prjCramSchoolSystemUser.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Controllers
{
    public class CourseController : Controller
    {
        private CramSchoolDBContext _context;
        private IWebHostEnvironment _enviroment;
        public CourseController(CramSchoolDBContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _enviroment = environment;
        }
        
        public IActionResult List()
        {
            CCourseListViewModel c = new CCourseListViewModel();
            //補圖片檔名
            IQueryable<CCourseList> data = from t in _context.TCourseInformations
                       select new CCourseList()
                       {
                           FEchelonId = t.FEchelonId,
                           Name = t.FCourse.FName,
                           ClassState = t.FClassState.ToString(),
                           OriginalPrice = t.FCourse.FOriginalPrice,
                           SpecialOffer = t.FCourse.FSpecialOffer,
                           DiscountDate=t.FDiscountDate,
                           PhotoName=t.FCoverImg
                       };
            int count = data.Count();
            c.page = (count < 10) ? 1 : (int)Math.Ceiling(Math.Round((decimal)count / 10, 1));
            c.course = data.Take(10).ToList();
            return View(c);
        }

        //瀏覽
        public IActionResult Detail(string id)
        {
            CCourseViewModel c = courseDDL("");
            //c.courseimg_arr = new TCourseInformationImg[6];
            CCourseShowState cShow = new CCourseShowState();
            
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("List");

            TCourseInformation course = _context.TCourseInformations.FirstOrDefault(c => c.FEchelonId.Equals(id));
            if (course == null)
                return RedirectToAction("List");

            c.course = course;
            c.courseimg = showImg(c.course.FEchelonId);
            c.Price = string.Format("{0:0,0}",(decimal)checkPrice(course.FCourse.FOriginalPrice, course.FCourse.FSpecialOffer, course.FDiscountDate));
            c.Detail_List = getDetailList(course.FCourse.FCourseId);

            //c.casestatus = cShow.showCourse("old");
            //string[] photoarr = showCourseImg(c.course.FEchelonId);
            //for (int i = 0; i < c.courseimg_arr.Length; i++)
            //    c.courseimg_arr[i] = new TCourseInformationImg() { FEchelonId = c.course.FEchelonId, FCourseImageName = photoarr[i] };
            return View(c);
        }
        #region
        //瀏覽不用submit
        //[HttpPost]
        //public IActionResult Detail(CCourseViewModel c)
        //{
        //    CCourseShowState cShow = new CCourseShowState();
        //    string userID = "";
        //    DateTime now;
        //    readUserData(out userID, out now);
        //    if (c.casestatus == cShow.showCourse("old"))//編輯
        //    {
        //        TCourseInformation course = _context.TCourseInformations.FirstOrDefault(t => t.FEchelonId.Equals(c.course.FEchelonId));
        //        if (course != null)
        //        {
        //            c.course.FSaverUser = userID;
        //            c.course.FSaverDate = now;
        //            _context.SaveChanges();
        //        }

        //    }
        //    else if (c.casestatus == cShow.showCourse("new"))//新增
        //    {
        //        c.course.FCreationUser = userID;
        //        c.course.FCreationDate = now;
        //        c.course.FSaverUser = userID;
        //        c.course.FSaverDate = now;
        //        _context.TCourseInformations.Add(c.course);
        //        _context.SaveChanges();
        //    }
        //    SavePhoto(c);
        //    return RedirectToAction("List");
        //}
        #endregion

        //加入購物車
        public IActionResult AddShoppinCat(string fEchelonId)
        {
            string json = "";
            //string test = _context.TCourseInformations.FirstOrDefault(c => c.FEchelonId.Equals(fEchelonId)).FCourse.FCourseId;
            TCourseInformation course = _context.TCourseInformations.FirstOrDefault(c => c.FEchelonId.Equals(fEchelonId));

            if (course != null)
            {
                CShoppingcartOperate shopping_operate = new CShoppingcartOperate();//購物車操作class
                string echelonId = course.FEchelonId;//課程id
                string name = course.FCourse.FName;//課程名稱
                DateTime discountDate = (DateTime)course.FDiscountDate;//打折期限
                decimal price = (decimal)shopping_operate.checkPrice(course.FCourse.FOriginalPrice, course.FCourse.FSpecialOffer,course.FDiscountDate);//課程價錢
                List<CShoppingCart> cart = null;

                if (HttpContext.Session.Keys.Contains(CDictionary.SK_COURSE_PURCHASED_LIST))
                {
                    json = HttpContext.Session.GetString(CDictionary.SK_COURSE_PURCHASED_LIST);
                    cart = JsonSerializer.Deserialize<List<CShoppingCart>>(json);
                    cart = shopping_operate.checkBought(fEchelonId, price, cart, name,course.FCoverImg);
                }
                else
                {
                    cart = new List<CShoppingCart>();
                    CShoppingCart item = shopping_operate.addBuy(echelonId, price, name,course.FCoverImg);
                    cart.Add(item);
                }

                json = JsonSerializer.Serialize(cart);
                HttpContext.Session.SetString(CDictionary.SK_COURSE_PURCHASED_LIST,json);
            }
            return Content("");
        }

        //確認價錢
        public decimal? checkPrice(decimal? fOriginalPrice, decimal? fSpecialOffer, DateTime? fDiscountDate)
        {
            if (fOriginalPrice == null)
                fOriginalPrice = 0;
            if (fSpecialOffer == null)
                fSpecialOffer = 0;
            DateTime now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (fDiscountDate >= now)
                return fSpecialOffer;
            else
                return fOriginalPrice;
        }

        public List<CCourseModelDetail_List> getDetailList(string fCourseId)
        {
            var data = from t in _context.TCourseModelDetails
                       where t.FCourseId.Equals(fCourseId)
                       select new CCourseModelDetail_List()
                       {
                           FCourseNumber = t.FCourseNumber,
                           FSchedule = t.FSchedule,
                           FScheduleDetail = t.FScheduleDetail,
                           FTeachingMethod = t.FTeachingMethod,
                           FRemark = t.FRemark
                       };
            List<CCourseModelDetail_List> List = data.ToList();
            if (List.Count == 0)
                return null;
            return List;
        }

        //顯示圖片
        private string showImg(string fEchelonId)
        {
            string photoarr = "NullImg.jpg";
            TCourseInformationImg data = _context.TCourseInformationImgs.FirstOrDefault(c => c.FEchelonId.Equals(fEchelonId));
            if (data == null)
                return photoarr;
            return data.FCourseImageName;
        }

        //下拉(課程模板)
        [NonAction]
        private CCourseViewModel courseDDL(string state)
        {
            //CourseModelDDL 課程模板->顯示(Y)
            CCourseViewModel c = new CCourseViewModel();
            //判斷DDL是否顯示存在的CourseModel
            #region
            //IQueryable<TCourseModel> data = null;
            //switch (state)
            //{
            //    case "new":
            //        data = from t in _context.TCourseModels.Where(c => c.FModleSate.Equals(new CCourseModelShowState().showCourse("Y")))
            //                   select t;
            //        break;
            //    default:
            //        data = from t in _context.TCourseModels
            //               select t;
            //        break;
            //}
            #endregion
            var data= from t in _context.TCourseModels
                      select t;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in data.ToList())
                list.Add(new SelectListItem() { Text = item.FName, Value = item.FCourseId });
            c.CourseDDL = list;
            return c;
        }

        //取出userID、現在時間
        [NonAction]
        public void readUserData(out string userID, out DateTime now)
        {
            userID = "";
            now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LONGUNED_ID))
            {
                //讀Session-userID
                string json = HttpContext.Session.GetString(CDictionary.SK_LONGUNED_ID);
                userID = JsonSerializer.Deserialize<string>(json);
            }
        }

        //目前沒使用
        //顯示圖片
        [NonAction]
        private string[] showCourseImg(string fEchelonId)
        {
            string[] photoarr = new string[6] { "NullImg.jpg", "NullImg.jpg", "NullImg.jpg", "NullImg.jpg", "NullImg.jpg", "NullImg.jpg" };
            var data = from t in _context.TCourseInformationImgs.Where(c => c.FEchelonId.Equals(fEchelonId))
                       select t;
            List<TCourseInformationImg> data_List = data.ToList();

            if (data_List.Count > 0)
            {
                for (int i = 0; i < data_List.Count; i++)
                {
                    string imgName = data_List[i].FCourseImageName;
                    int p = imgName.IndexOf("_");
                    int num = Convert.ToInt32(imgName.Substring(p + 1, 1));
                    photoarr[num - 1] = imgName;
                }
            }
            return photoarr;
        }

        #region
        //新增、修改
        //public IActionResult Edit(string id)
        //{
        //    CCourseViewModel c = courseDDL("new");
        //    c.courseimg_arr = new TCourseInformationImg[6];
        //    CCourseShowState cShow = new CCourseShowState();
        //    //編輯
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        TCourseInformation course = _context.TCourseInformations.FirstOrDefault(c => c.FEchelonId.Equals(id));
        //        if (course == null)
        //            return RedirectToAction("List");

        //        c.casestatus = cShow.showCourse("old");
        //        c.course = course;

        //        string[] photoarr = showCourseImg(c.course.FEchelonId);
        //        for (int i = 0; i < c.courseimg_arr.Length; i++)
        //            c.courseimg_arr[i] = new TCourseInformationImg() { FEchelonId = c.course.FEchelonId, FCourseImageName = photoarr[i] };
        //    }
        //    else//新增
        //    {
        //        c.casestatus = cShow.showCourse("new");
        //        for (int i = 0; i < c.courseimg_arr.Length; i++)
        //            c.courseimg_arr[i] = new TCourseInformationImg() { FCourseImageName = "NullImg.jpg" };
        //        c.course = new TCourseInformation();
        //    }
        //    return View(c);
        //}

        //[HttpPost]
        //public IActionResult Edit(CCourseViewModel c)
        //{
        //    CCourseShowState cShow = new CCourseShowState();
        //    string userID = "";
        //    DateTime now;
        //    readUserData(out userID, out now);
        //    if (c.casestatus == cShow.showCourse("old"))//編輯
        //    {
        //        TCourseInformation course = _context.TCourseInformations.FirstOrDefault(t => t.FEchelonId.Equals(c.course.FEchelonId));
        //        if (course != null)
        //        {
        //            c.course.FSaverUser = userID;
        //            c.course.FSaverDate = now;
        //            _context.SaveChanges();
        //        }

        //    }
        //    else if (c.casestatus == cShow.showCourse("new"))//新增
        //    {
        //        c.course.FCreationUser = userID;
        //        c.course.FCreationDate = now;
        //        c.course.FSaverUser = userID;
        //        c.course.FSaverDate = now;
        //        _context.TCourseInformations.Add(c.course);
        //        _context.SaveChanges();
        //    }
        //    SavePhoto(c);
        //    return RedirectToAction("List");
        //}

        ////儲存tCourseInformationImg圖片資料
        //[NonAction]
        //private void SavePhoto(CCourseViewModel c)
        //{
        //    clearCourseImg(c.course.FEchelonId);
        //    IFormFile[] photoarr = new IFormFile[6] { c.photo1, c.photo2, c.photo3, c.photo4, c.photo5, c.photo6 };
        //    List<TCourseInformationImg> saveImg_List = new List<TCourseInformationImg>();
        //    for (int i = 0; i < photoarr.Length; i++)
        //    {
        //        if (photoarr[i] == null)
        //            continue;
        //        //string photoName = Guid.NewGuid().ToString() + ".jpg";
        //        string photoName = $"{c.course.FEchelonId}_{i + 1}" + ".jpg";
        //        photoarr[i].CopyTo(new FileStream(_enviroment.WebRootPath + @"\images\" + photoName, FileMode.Create));
        //        saveImg_List.Add(new TCourseInformationImg() { FEchelonId = c.course.FEchelonId, FCourseImageName = photoName });
        //    }
        //    if (saveImg_List.Count == 0)
        //        return;
        //    _context.TCourseInformationImgs.AddRange(saveImg_List);
        //    _context.SaveChanges();
        //}

        ////清除tCourseInformationImg資料
        //[NonAction]
        //private void clearCourseImg(string fEchelonId)
        //{
        //    var data = from t in _context.TCourseInformationImgs.Where(m => m.FEchelonId == fEchelonId)
        //               select t;
        //    if (data.ToList().Count == 0)
        //        return;
        //    _context.TCourseInformationImgs.RemoveRange(data.ToList());
        //    _context.SaveChanges();
        //}
        #endregion

        //test db 要放在函式裡
        public IActionResult Test()
        {
            //CramSchoolDBContext db = new CramSchoolDBContext();
            CourseMenu1 vm = new CourseMenu1(_context);
            vm.Menu();

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
