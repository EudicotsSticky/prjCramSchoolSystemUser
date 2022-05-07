using prjCramSchoolSystemUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.ViewModel
{
    public class COrderListViewModel
    {
        public TOrder order { get; set; }
        public List<COrderDetailListViewModel> order_detail { get; set; }
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
    public class COrderDetailListViewModel
    {
        public string FEchelonId { get; set; }
        public decimal? FMoney { get; set; }
        //課程名稱
        public string Name { get; set; }

    }
}
