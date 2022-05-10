using prjCramSchoolSystemUser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.ViewModel
{
    public class COrderReviewViewModel
    {
        [DisplayName("付款人姓名")]
        public string UserName { get; set; }
        public string OrderState { get; set; }
        public TOrder order { get; set; }
        public List<COrderDetailReviewViewModel> order_detail { get; set; }
        public decimal Price
        {
            get
            {
                decimal _price = 0;
                foreach (var item in order_detail)
                {
                    if (item.FMoney == null)
                        continue;
                    _price += Convert.ToDecimal(item.FMoney);
                }
                return _price;
            }
        }

    }

    public class COrderDetailReviewViewModel
    {
        public string FReceiverId { get; set; }
        public string FReceiverName { get; set; }
        public string FEchelonId { get; set; }
        public decimal? FMoney { get; set; }
        //圖片連結
        public string PhotoName { get; set; }

        //課程名稱
        public string Name { get; set; }

    }
}
