using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjCoreCramSchoolDB.Models;
using prjCoreCramSchoolDB.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace prjCoreCramSchoolDB.Controllers
{
    [Authorize]
    public class MessageBordController : Controller
    {
        dbCramSchoolContext db = new dbCramSchoolContext();
        private readonly IWebHostEnvironment _hostingEnvironment;
        public MessageBordController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public ActionResult List(KeywordViewModel vModel)
        {
            IEnumerable<TPost> postdata = null;
            string inputKeyword = vModel.keyword;
            // 如果關鍵字搜尋欄位為空，keyword會沒有值，執行下方判斷式
            if (String.IsNullOrEmpty(inputKeyword))
            {
                postdata = from t in db.TPosts
                           orderby t.FPostTime descending
                           select t;
            }
            // 如果有輸入關鍵字，submit到方法內，則KeywordViewModel的string keyword不為空，執行else內容
            else
            {
                postdata = db.TPosts.Where(t => t.FPostTitle.Contains(inputKeyword)
                            || t.FPostContent.Contains(inputKeyword));
            }
            return View(postdata);
        }

        public IActionResult ArticleA()
        {
            return View();
        }

        public IActionResult Cre()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Cre(TPost tpost, IFormFile photo)
        {

            //將檔案存到board資料夾中
            string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "board", photo.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }


            //準備寫進資料庫的資料
            tpost.FAccount = "李偉誠";
            tpost.FPostId = Guid.NewGuid().ToString();
            tpost.FPostTime = DateTime.Now;
            tpost.FPhotoId = photo.FileName;
            tpost.FPostUpdateTime = DateTime.Now;

            //寫進資料庫
            dbCramSchoolContext db = new dbCramSchoolContext();
            db.TPosts.Add(tpost);
            db.SaveChanges();

            //轉到 list action
            return RedirectToAction("List");
        }

        //編輯
        public ActionResult Edit(string id)
        {
            //抓取TPosts.ID等於輸入id的資料
            var result = (from s in db.TPosts where s.FPostId == id select s).FirstOrDefault();
            if (result != default(Models.TPost))//判斷此id是否有資料
            {
                return View(result);//如果有回傳編輯頁面
            }
            else
            {
                //如果沒有則回傳顯示錯誤訊息並導回List頁面
                TempData["resultMessage"] = "資料有誤,請重新操作";
                return RedirectToAction("List");
            }
        }
        //編輯--資料傳回處理
        [HttpPost]
        public ActionResult Edit(Models.TPost postdata, IFormFile postPicture)
        {
            if (this.ModelState.IsValid)//判斷使用者輸入資料是否正確
            {
                //抓取TPosts.ID等於回傳postdata.ID的資料
                var result = (from s in db.TPosts where s.FPostId == postdata.FPostId select s).FirstOrDefault();

                //儲存變更者資料
                result.FPostTitle = postdata.FPostTitle;
                result.FPostContent = postdata.FPostContent;
                result.FPostTime = postdata.FPostTime;
                result.FAccount = "李偉誠";
                result.FPostUpdateTime = DateTime.Now;
                if (postPicture != null)
                {
                    // 新上傳的圖片的名稱覆蓋資料庫原有的圖片名稱
                    result.FPhotoId =postPicture.FileName;

                    //將檔案存到board資料夾中
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "board", postPicture.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        postPicture.CopyTo(fileStream);
                    }

                }

                result.FPostSort = postdata.FPostSort;

                //儲存所有變更
                db.SaveChanges();

                //設定成功訊息並導回List頁面
                TempData["ResultMessage"] = String.Format("成功編輯!", postdata.FPostTitle);
                return RedirectToAction("List");
            }
            else//如果資料不正確導回自己(Edit頁面)
            {
                return View(postdata);
            }
        }

        public IActionResult Delete(string id)
        {
            if (id != null)
            {
                TPost post = db.TPosts.FirstOrDefault(c => c.FPostId == (string)id);
                if (post != null)
                {
                    db.TPosts.Remove(post);
                    post.FAccount = "YEE";
                    post.FPostUpdateTime = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }

        public ActionResult Detail(string id, Models.TPostThumbUp thumbUp)
        {
            var query = (from p in db.TPosts where p.FPostId == id select p).FirstOrDefault();


            thumbUp.FThumbUpId = Guid.NewGuid().ToString();
            thumbUp.FThumbUpTime = DateTime.Now;
            thumbUp.FThumbUpCheck = 1;

            db.SaveChanges();


            if (query == null)
                return Content("null");
            else
                return View(query);


        }

        public ActionResult ChinesePage()
        {
            var postdata = from t in db.TPosts
                           where t.FPostSort == "國    文"
                           orderby t.FPostTime descending
                           select t;
            return View(postdata);
        }

        public ActionResult MathPage()
        {
            var postdata = from t in db.TPosts
                           where t.FPostSort == "數    學"
                           orderby t.FPostTime descending
                           select t;
            return View(postdata);
        }

        public ActionResult EnglishPage()
        {
            var postdata = from t in db.TPosts
                           where t.FPostSort == "英    文"
                           orderby t.FPostTime descending
                           select t;
            return View(postdata);
        }

        public ActionResult ChemicalPage()
        {
            var postdata = from t in db.TPosts
                           where t.FPostSort == "化    學"
                           orderby t.FPostTime descending
                           select t;
            return View(postdata);
        }

        public ActionResult BiologyPage()
        {
            var postdata = from t in db.TPosts
                           where t.FPostSort == "生    物"
                           orderby t.FPostTime descending
                           select t;
            return View(postdata);
        }

        public ActionResult CivicsPage()
        {
            var postdata = from t in db.TPosts
                           where t.FPostSort == "公    民"
                           orderby t.FPostTime descending
                           select t;
            return View(postdata);
        }

        public ActionResult HistoryPage()
        {
            var postdata = from t in db.TPosts
                           where t.FPostSort == "歷    史"
                           orderby t.FPostTime descending
                           select t;
            return View(postdata);
        }

        public ActionResult GeographyPage()
        {
            var postdata = from t in db.TPosts
                           where t.FPostSort == "地    理"
                           orderby t.FPostTime descending
                           select t;
            return View(postdata);
        }

        //public ActionResult Select()
        //{
        //    var postdata = from t in db.TPosts
        //                   where t.FPostTitle == 
        //}








    }
}

