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
    }

    public class COrderDetailReviewViewModel
    {
        public string FReceiverName { get; set; }
        public string FReceiverId { get; set; }
        public string FEchelonId { get; set; }
        public decimal? FMoney { get; set; }

    }
}
