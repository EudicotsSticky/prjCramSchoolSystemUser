using prjCramSchoolSystemUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.ViewModel
{
    public class COrderListViewModel
    {
        //付款人姓名
        public string UserName { get; set; }
        public string OrderState { get; set; }
        public TOrder order { get; set; }
        public List<COrderDetailList> order_detail { get; set; }
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
        public List<CShowOrderDetail> order_detail_count
        {
            get
            {
                List<CShowOrderDetail> List = new List<CShowOrderDetail>();
                if (List.Count == 0)
                    List.Add(new CShowOrderDetail() { Name = order_detail[0].Name, Count = 1 });
                for (int x = 1, y = 0; x < order_detail.Count; x++)
                {
                    if (List[y].Name == order_detail[x].Name)
                    {
                        int _count = List[y].Count + 1;
                        List[y].Count = _count;
                    }
                    else
                    {
                        List.Add(new CShowOrderDetail() { Name = order_detail[x].Name, Count = 1 });

                    }
                }
                return List;
            }
        }
    }
    public class COrderDetailList
    {
        public string FEchelonId { get; set; }
        public decimal? FMoney { get; set; }
        //課程名稱
        public string Name { get; set; }
        //public decimal money
        //{
        //    get
        //    {
        //        if (FMoney == null)
        //            return 0;
        //        return Convert.ToDecimal(FMoney);
        //    }
        //}
    }

    public class CShowOrderDetail
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
