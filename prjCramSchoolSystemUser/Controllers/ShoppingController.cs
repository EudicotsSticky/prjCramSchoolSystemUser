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
    public class ShoppingController : Controller
    {
        private CramSchoolDBContext _context;
        public ShoppingController(CramSchoolDBContext context)
        {
            _context = context;
        }
        public IActionResult Shoppingcart()
        {
            CShoppingCartViewModel cart = new CShoppingCartViewModel() { ShoppingCart_List = null, TotalPrice = 0 };
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_COURSE_PURCHASED_LIST))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_COURSE_PURCHASED_LIST);
                cart.ShoppingCart_List = JsonSerializer.Deserialize<List<CShoppingCart>>(json);
                //cart.coursename_List = getCourseInformation(cart.ShoppingCart_List);
            }
            return View(cart);
        }

        //購物車指定課程數量加一
        public IActionResult AddShoppinCat(string fEchelonId)
        {
            string json = "";
            List<CShoppingCart> cart = null;
            CShoppingCartViewModel c_shoppingcart = new CShoppingCartViewModel();
            //string test = _context.TCourseInformations.FirstOrDefault(c => c.FEchelonId.Equals(fEchelonId)).FCourse.FCourseId;
            TCourseInformation course = _context.TCourseInformations.FirstOrDefault(c => c.FEchelonId.Equals(fEchelonId));

            if (course != null)
            {
                CShoppingcartOperate shopping_operate = new CShoppingcartOperate();//購物車操作class
                string echelonId = course.FEchelonId;//課程id
                string name = course.FCourse.FName;//課程名稱
                //DateTime discountDate = (DateTime)course.FDiscountDate;//打折期限
                decimal price = (decimal)shopping_operate.checkPrice(course.FCourse.FOriginalPrice, course.FCourse.FSpecialOffer, course.FDiscountDate);//課程價錢

                if (HttpContext.Session.Keys.Contains(CDictionary.SK_COURSE_PURCHASED_LIST))
                {
                    json = HttpContext.Session.GetString(CDictionary.SK_COURSE_PURCHASED_LIST);
                    cart = JsonSerializer.Deserialize<List<CShoppingCart>>(json);
                    cart = shopping_operate.checkBought(fEchelonId, price, cart, name, showImg(course.FEchelonId));//,course.FCoverImg
                }
                else
                {
                    cart = new List<CShoppingCart>();
                    CShoppingCart item = shopping_operate.addBuy(echelonId, price, name, showImg(course.FEchelonId));//,course.FCoverImg
                    cart.Add(item);
                }
                c_shoppingcart.ShoppingCart_List = cart;
                json = JsonSerializer.Serialize(cart);
                HttpContext.Session.SetString(CDictionary.SK_COURSE_PURCHASED_LIST, json);
            }
            //用json()轉過json檔 之前就不用傳為json檔了
            return Json(c_shoppingcart);//return Ok(); // Content("");
        }

        //購物車指定課程數量減一
        public IActionResult MinusShoppingCart(string fEchelonId)
        {
            CShoppingCartViewModel c_shoppingcart = new CShoppingCartViewModel();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_COURSE_PURCHASED_LIST))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_COURSE_PURCHASED_LIST);
                List<CShoppingCart> cart = JsonSerializer.Deserialize<List<CShoppingCart>>(json);
                for(int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].EchelonId == fEchelonId && cart[i].Count > 1)
                    {
                        cart[i].Count--;
                        break;
                    }
                    else if (cart[i].EchelonId == fEchelonId && cart[i].Count == 1)
                    {
                        cart.RemoveAt(i);
                        break;
                    }
                }
                c_shoppingcart.ShoppingCart_List = cart;
                json = JsonSerializer.Serialize(cart);
                HttpContext.Session.SetString(CDictionary.SK_COURSE_PURCHASED_LIST, json);
            }
            return Json(c_shoppingcart);
        }

        //購物車移除指定課程
        public IActionResult RemoveShoppingCart(string fEchelonId)
        {
            CShoppingCartViewModel c_shoppingcart = new CShoppingCartViewModel();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_COURSE_PURCHASED_LIST))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_COURSE_PURCHASED_LIST);
                List<CShoppingCart> cart = JsonSerializer.Deserialize<List<CShoppingCart>>(json);
                for(int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].EchelonId == fEchelonId)
                    {
                        cart.RemoveAt(i);
                        break;
                    }
                }
                c_shoppingcart.ShoppingCart_List = cart;
                json = JsonSerializer.Serialize(cart);
                HttpContext.Session.SetString(CDictionary.SK_COURSE_PURCHASED_LIST, json);
            }
            return Json(c_shoppingcart);
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

        //目前沒使用到
        //get CourseModel FName 課程名稱
        [NonAction]
        private List<string> getCourseInformation(List<CShoppingCart> shoppingCart_List)
        {
            List<string> coursename_List = new List<string>();
            for(int i = 0; i < shoppingCart_List.Count; i++)
            {
                string coursename = _context.TCourseInformations.FirstOrDefault(c => c.FEchelonId.Equals(shoppingCart_List[i].EchelonId)).FCourse.FName;
                coursename_List.Add(coursename);
            }
            return coursename_List;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
