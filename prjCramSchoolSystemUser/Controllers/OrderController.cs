//using ECPay.Payment.Integration;
using FluentEcpay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjCramSchoolSystemUser.Models;
using prjCramSchoolSystemUser.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;//

namespace prjCramSchoolSystemUser.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        
        private CramSchoolDBContext _context;
        public OrderController(CramSchoolDBContext context)
        {
            _context = context;
        }
        public IActionResult Create()
        {
            //取得購物車session
            List<CShoppingCart> List = getShoppingCart();
            if (List == null || List.Count == 0)
                return RedirectToAction("List", "Course");
            //{//測試
            //    List = new List<CShoppingCart>();

            //    List.Add(new CShoppingCart()
            //    {
            //        Count = 1,
            //        Course_TotalPrice = 100,
            //        EchelonId = "CI202205030440306",
            //        Name = "英文文法",
            //        PhotoName = @"https://i.imgur.com/pRmqy56.jpg",
            //        Price = 100
            //    });

            //}//

            COrderCreateViewModel c = new COrderCreateViewModel() { coursedata = new CShoppingCartViewModel() };
            //付款人資料
            string UserId = "", UserName = "";
            DateTime now;
            readUserData(out UserId, out UserName, out now);
            //姓名
            c.UserName = UserName;
            c.oder = new TOrder() { FUserId = UserId };
            //購買課程
            c.coursedata.ShoppingCart_List = List;
            c.order_detail = getOderDetail(List);//.Count
            return View(c);
        }
        [HttpPost]
        public IActionResult Create(COrderCreateViewModel c)
        {
            //讀取登入帳號、現在時間
            string UserId = "", UserName = "";
            DateTime now;
            readUserData(out UserId, out UserName, out now);
            //訂單編號
            string orderid = getOrderID();
            //將使用者帳號加入List
            List<string> ReceiverId_List = new List<string>();
            foreach (var item in c.order_detail)
                ReceiverId_List.Add(item.FReceiverId);
            //建立訂單
            TOrder order = new TOrder()
            {
                FOrderId = orderid,
                FUserId = UserId,
                FPayment = 1,//1 : 線上刷卡
                FOrderState = 0,//0 : 待付款
                FCreationDate = now,
                FCreationUser = UserId,
                FSaverDaate = now,
                FSaverUser = UserId
            };
            _context.TOrders.Add(order);
            _context.SaveChanges();
            //建立訂單詳情
            List<CShoppingCart> List = getShoppingCart();
            List<TOrderDetail> orderdetail_List = new List<TOrderDetail>();
            int x = 0;
            foreach (var item in List)
            {
                for(int i = 0; i < item.Count; i++)
                {
                    orderdetail_List.Add(new TOrderDetail()
                    {
                        FOrderId = orderid,
                        FReceiverId = ReceiverId_List[x],
                        FEchelonId = item.EchelonId,
                        FMoney = item.Price,
                        FCreationDate = now,
                        FCreationUser=UserId,
                        FSaverDate=now,
                        FSaverUser=UserId
                    });
                    x++;
                }
            }
            _context.TOrderDetails.AddRange(orderdetail_List);
            _context.SaveChanges();

            //return View();
            return RedirectToAction("New");
        }

        // POST api/payment
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult New()
        {
            return RedirectToAction("checkout");
        }

        [HttpGet("checkout")]
        public IActionResult CheckOut()
        {
            var service = new
            {
                Url = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                MerchantId = "2000132",
                HashKey = "5294y06JbISpM5x9",
                HashIV = "v77hoKGq4kWxNNIS",
                //ServerUrl = "https://test.com/api/payment/callback",
                ServerUrl = "https://localhost:44376/order/callback",
                //ClientUrl = "https://test.com/payment/success"//交易成功
                ClientUrl = "https://localhost:44376/order/revieworder"//交易成功
            };
            var transaction = new
            {
                No = "test00003",
                Description = "測試購物系統",
                Date = DateTime.Now,
                Method = EPaymentMethod.Credit,
                Items = new List<Item>{
                    new Item{
                        Name = "手機",
                        Price = 14000,
                        Quantity = 2
                    },
                    new Item{
                        Name = "隨身碟",
                        Price = 900,
                        Quantity = 10
                    }
                }
            };
            IPayment payment = new PaymentConfiguration()
                .Send.ToApi(
                    url: service.Url)
                .Send.ToMerchant(
                    service.MerchantId)
                .Send.UsingHash(
                    key: service.HashKey,
                    iv: service.HashIV)
                .Return.ToServer(
                    url: service.ServerUrl)
                .Return.ToClient(
                    url: service.ClientUrl)
                .Transaction.New(
                    no: transaction.No,
                    description: transaction.Description,
                    date: transaction.Date)
                .Transaction.UseMethod(
                    method: transaction.Method)
                .Transaction.WithItems(
                    items: transaction.Items)
                .Generate();

            return View(payment);
        }

        [HttpPost("callback")]
        public IActionResult Callback(PaymentResult result)
        {
            var hashKey = "5294y06JbISpM5x9";
            var hashIV = "v77hoKGq4kWxNNIS";

            // 務必判斷檢查碼是否正確。
            if (!CheckMac.PaymentResultIsValid(result, hashKey, hashIV)) return BadRequest();

            // 處理後續訂單狀態的更動等等...。

            return Ok("1|OK");
        }

        public IActionResult ReviewOrder()
        {
            return View();
        }

        //確認使用者帳號是否存在
        public IActionResult checkReceiverId(string account)
        {
            CCourseModelShowState c = new CCourseModelShowState();
            User buycourse_user = null;
            buycourse_user = _context.Users.FirstOrDefault(t => t.UserName.Equals(account));
            if(buycourse_user==null)
            buycourse_user = _context.Users.FirstOrDefault(t => t.Email.Equals(account));
            CShowStudentData student = new CShowStudentData() { UserState= c.showCourse("N"), UserName ="", FirstName = "", LastName = "" };
            if (buycourse_user != null)
            {
                student.UserState = c.showCourse("Y");
                student.UserName = buycourse_user.UserName;
                student.FirstName = buycourse_user.FirstName;
                student.LastName = buycourse_user.LastName;
            }
            return Json(student);
            //return Content(buycourse_user.ToString());//, "text/plain"
        }

        //建立訂單詳情List 先預設使用者id為空字串
        [NonAction]
        public List<TOrderDetail> getOderDetail(List<CShoppingCart>List)
        {
            List<TOrderDetail> oder_detail_list = new List<TOrderDetail>();
            foreach (var item in List)
            {
                for (int i = 0; i < item.Count; i++)
                    oder_detail_list.Add(new TOrderDetail() { FReceiverId = "" });
            }
            return oder_detail_list;
        }

        //new案件編號
        [NonAction]
        public string getOrderID()
        {
            string now = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") 
                + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
            Random r = new Random();
            return "OR" + now + r.Next(0, 9);
        }

        //取出購買課程
        [NonAction]
        public List<CShoppingCart> getShoppingCart()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_COURSE_PURCHASED_LIST))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_COURSE_PURCHASED_LIST);
                return JsonSerializer.Deserialize<List<CShoppingCart>>(json);
            }
            return null;
        }

        //取出userID、現在時間
        [NonAction]
        public void readUserData(out string userID, out string username, out DateTime now)
        {
            userID = "";
            username = "";
            string _userid = "";
            string json = "";
            now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //讀Session-userID
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LONGUNED_ID))
            {
                
                json = HttpContext.Session.GetString(CDictionary.SK_LONGUNED_ID);
                userID = JsonSerializer.Deserialize<string>(json);
                _userid = userID;
            }
            //讀UserName
            var user = _context.Users.FirstOrDefault(t => t.UserName.Equals(_userid));
            if (user != null)
                username = user.FirstName + user.LastName;
            //if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            //{
            //    json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            //    username = JsonSerializer.Deserialize<string>(json);
            //}
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
