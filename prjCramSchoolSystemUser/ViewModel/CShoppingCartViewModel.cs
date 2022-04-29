using prjCramSchoolSystemUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.ViewModel
{
    public class CShoppingCartViewModel
    {
        public List<CShoppingCart> ShoppingCart_List { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return calculatePrice();
            }
            set
            {
                value = calculatePrice();
            }
        }
        //public List<string> coursename_List { get; set; }

        private decimal calculatePrice()
        {
            decimal price = 0;
            if (ShoppingCart_List != null && ShoppingCart_List.Count > 0)
                for (int i = 0; i < this.ShoppingCart_List.Count; i++)
                    price += this.ShoppingCart_List[i].Course_TotalPrice;
            return price;
        }
    }
}
