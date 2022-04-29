//using ECPay.Payment.Integration;
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
    public class OrderController : Controller
    {
        private CramSchoolDBContext _context;
        public OrderController(CramSchoolDBContext context)
        {
            _context = context;
        }
        public IActionResult Create()
        {
            List<CShoppingCart> List = getShoppingCart();
            //if (List == null || List.Count == 0)
            //    return RedirectToAction("", "");
            COrderCreateViewModel c = new COrderCreateViewModel() { coursedata = new CShoppingCartViewModel() };
            string UserId = "", UserName = "";
            DateTime now;
            readUserData(out UserId, out UserName, out now);
            //付款人資料
            c.UserName = UserName;
            c.oder = new TOrder() { FUserId = UserId };
            //購買課程
            c.coursedata.ShoppingCart_List = List;
            c.order_detail = getOderDetail(List.Count);
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
            return View(c);
        }

        //todo



        public IActionResult CheckOutFeedback(string RtnMsg)
        {
            ViewBag.RtnMsg = RtnMsg;
            return View();
        }
        public string Index()
        {
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

            //需填入 你的網址
            var website = $"XXXX";

            var order = new Dictionary<string, string>
        {
            //特店交易編號
            { "MerchantTradeNo",  orderId},

            //特店交易時間 yyyy/MM/dd HH:mm:ss
            { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},

            //交易金額
            { "TotalAmount",  "100"},

            //交易描述
            { "TradeDesc",  "無"},

            //商品名稱
            { "ItemName",  "測試商品"},

            //允許繳費有效天數(付款方式為 ATM 時，需設定此值)
            { "ExpireDate",  "3"},

            //自訂名稱欄位1
            { "CustomField1",  ""},

            //自訂名稱欄位2
            { "CustomField2",  ""},

            //自訂名稱欄位3
            { "CustomField3",  ""},

            //自訂名稱欄位4
            { "CustomField4",  ""},

            //綠界回傳付款資訊的至 此URL
            { "ReturnURL",  $"{website}/api/Ecpay/AddPayInfo"},

            //使用者於綠界 付款完成後，綠界將會轉址至 此URL
            { "OrderResultURL", $"{website}/Ecpay/PayInfo/{orderId}"},

            //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界回傳 虛擬帳號資訊至 此URL
            { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},

            //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界會轉址至 此URL。
            { "ClientRedirectURL",  $"{website}/Ecpay/AccountInfo/{orderId}"},

            //特店編號， 2000132 測試綠界編號
            { "MerchantID",  "2000132"},

            //忽略付款方式
            { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},

            //交易類型 固定填入 aio
            { "PaymentType",  "aio"},

            //選擇預設付款方式 固定填入 ALL
            { "ChoosePayment",  "ALL"},

            //CheckMacValue 加密類型 固定填入 1 (SHA256)
            { "EncryptType",  "1"},
        };

            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);

            return order["CheckMacValue"];
        }

        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();

            var checkValue = string.Join("&", param);

            //測試用的 HashKey
            var hashKey = "5294y06JbISpM5x9";

            //測試用的 HashIV
            var HashIV = "v77hoKGq4kWxNNIS";

            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";

            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();

            checkValue = GetSHA256(checkValue);

            return checkValue.ToUpper();
        }

        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256Managed.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);

            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }

            return result.ToString();
        }

        public IActionResult fail() { return View(); }

        //確認使用者帳號是否存在
        public IActionResult checkReceiverId(string account)
        {
            var buycourse_user = _context.Users.Any(t => t.UserName.Equals(account) || t.Email.Equals(account));
            return Content(buycourse_user.ToString());//, "text/plain"
        }

        //建立訂單詳情List 先預設使用者id為空字串
        [NonAction]
        public List<TOrderDetail> getOderDetail(int count)
        {
            List<TOrderDetail> oder_detail_list = new List<TOrderDetail>();
            for(int i = 0; i < count; i++)
                oder_detail_list.Add(new TOrderDetail() { FReceiverId = "" });
            return oder_detail_list;
        }

        //new案件編號
        [NonAction]
        public string getOrderID()
        {
            string now = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
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
            string json = "";
            now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LONGUNED_ID))
            {
                //讀Session-userID
                json = HttpContext.Session.GetString(CDictionary.SK_LONGUNED_ID);
                userID = JsonSerializer.Deserialize<string>(json);
            }
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                username = JsonSerializer.Deserialize<string>(json);
            }
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
