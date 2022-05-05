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
        public TOrder oder { get; set; }
        public List<TOrderDetail> order_detail { get; set; }
    }
}
