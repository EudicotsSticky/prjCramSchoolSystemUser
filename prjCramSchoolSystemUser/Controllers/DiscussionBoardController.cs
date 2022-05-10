using Microsoft.AspNetCore.Mvc;
using prjCoreCramSchoolDB.Models;
using prjCoreCramSchoolDB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCoreCramSchoolDB.Controllers
{
    public class DiscussionBoardController : Controller
    {
        public IActionResult Post()
        {
            dbCramSchoolContext db = new dbCramSchoolContext();
            IEnumerable<TPost> postdatas = null;
            postdatas = from t in (new dbCramSchoolContext()).TPosts
                        orderby t.FPostTime descending
                        select t;
            List<CPostViewModel> list = new List<CPostViewModel>();

            foreach (TPost t in postdatas)
            {
                list.Add(new CPostViewModel() { post = t });
            }
            return View("Post");
        }

        public IActionResult Create()
        {
            return View();
        }
        //參數只有一個的話,就叫ID
        //參數諾多個則須打網址上key一致(ex:key,key2)


        [HttpPost]
        public IActionResult Create(CPostViewModel newPost)//資料表類別
        {
            dbCramSchoolContext db = new dbCramSchoolContext();
            newPost.FPostId = Guid.NewGuid().ToString();
            newPost.FPostTime = DateTime.Now;
            newPost.FAccount = "YEE";
            db.TPosts.Add(newPost.post);
            db.SaveChanges();
            return RedirectToAction("Post");//重新導向
        }

        public IActionResult Delete(string id)
        {
            if (id != null)
            {
                dbCramSchoolContext db = new dbCramSchoolContext();
                TPost post = db.TPosts.FirstOrDefault(c => c.FPostId == (string)id);
                if (post != null)
                {
                    db.TPosts.Remove(post);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Post");
        }

        public IActionResult Edit(string id)
        {
            if (id != null)
            {
                dbCramSchoolContext db = new dbCramSchoolContext();
                TPost selectedPost = db.TPosts.FirstOrDefault(c => c.FPostId == (string)id);
                if (selectedPost != null)
                {
                    return View(new CPostViewModel() { post = selectedPost });
                }
            }
            return RedirectToAction("Post");
        }

        [HttpPost]
        public IActionResult Edit(CPostViewModel editedPost)
        {
            dbCramSchoolContext db = new dbCramSchoolContext();
            TPost selectedPost = db.TPosts.FirstOrDefault(c => c.FPostId ==editedPost.FPostId);
            if (selectedPost != null)
            {
                selectedPost.FPostTitle = editedPost.FPostTitle;
                selectedPost.FPostContent = editedPost.FPostContent;
                selectedPost.FPostUpdateTime = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("Post");
        }

        public IActionResult test3()
        {
            return View();
        }
        [HttpPost]
        public IActionResult test3(CPostViewModel newModel)
        {
            dbCramSchoolContext db = new dbCramSchoolContext();
            newModel.FPostId = Guid.NewGuid().ToString();
            newModel.FPostTime = DateTime.Now;
            newModel.FAccount = "YEE123";
            db.TPosts.Add(newModel.post);
            db.SaveChanges();
            return View("post");
        }

        public IActionResult edittest4(string id)
        {
            if(id!=null)
            {
                dbCramSchoolContext db = new dbCramSchoolContext();
                TPost selectP = db.TPosts.FirstOrDefault(p => p.FPostId == (string)id);
                if(selectP!=null)
                {
                    return View(new CPostViewModel() { post = selectP });
                }
            }
            return RedirectToAction("post");
        }
        [HttpPost]
        public IActionResult edittest4(CPostViewModel t)
        {
            dbCramSchoolContext db = new dbCramSchoolContext();
            TPost selectPost = db.TPosts.FirstOrDefault(c => c.FPostId == t.FPostId);
            if(selectPost != null)
            {
                selectPost.FPostTitle = t.FPostTitle;
                selectPost.FPostContent = t.FPostContent;
                selectPost.FPostUpdateTime = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("post");
        }

       
        
    }
}
