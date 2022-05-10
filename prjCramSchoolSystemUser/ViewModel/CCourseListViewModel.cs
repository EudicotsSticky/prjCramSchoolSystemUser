using prjCramSchoolSystemUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.ViewModel
{
    public class CCourseListViewModel
    {
        public List<CCourseList> course { get; set; }
        public string[] CategoryDDL = CourseData.c_name;
        public int page { get; set; }
    }
    public class CCourseList
    {
        CShoppingcartOperate c = new CShoppingcartOperate();
        CCourseDataChange data_change = new CCourseDataChange();

        public string FEchelonId { get; set; }
        public string Name { get; set; }
        public string ClassState { get; set; }
        public string CourseState
        {
            get { return data_change.getCourseState(this.ClassState); }
            set { value= data_change.getCourseState(this.ClassState); }
        }

        //原價
        public decimal? OriginalPrice { get; set; }
        //優惠價
        public decimal? SpecialOffer { get; set; }
        //優惠價期限
        public DateTime? DiscountDate { get; set; }
        public decimal Price
        {
            get { return (decimal)c.checkPrice(OriginalPrice, SpecialOffer, DiscountDate); }
            set { value = (decimal)c.checkPrice(OriginalPrice, SpecialOffer, DiscountDate); }
        }

        public string Price_Format
        {
            get { return String.Format("{0:0,0}", Price); }
        }
        public string PhotoName { get; set; }
    }
}
