using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Models
{
    public class CShoppingcartOperate
    {
        //加入商品到購物車Session(已加入過)
        public List<CShoppingCart> checkBought(string fEchelonId, decimal price, List<CShoppingCart> cart, string name,string photoname)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].EchelonId.Equals(fEchelonId, StringComparison.OrdinalIgnoreCase))
                {
                    cart[i].Count++;
                    cart[i].Price = price;
                    break;
                }
                else if (cart[i].EchelonId != fEchelonId && cart.Count == i + 1)
                {
                    CShoppingCart item = addBuy(fEchelonId, price, name,photoname);
                    cart.Add(item);
                    break;
                }
            }
            if (cart.Count == 0)
                cart.Add(addBuy(fEchelonId, price, name,photoname));
            return cart;
        }

        //加入商品到購物車Session(加入新商品)
        public CShoppingCart addBuy(string echelonId, decimal price, string name,string photoname)
        {
            return new CShoppingCart()
            {
                EchelonId = echelonId,
                Count = 1,
                Price = price,
                Name = name,
                PhotoName=photoname
            };
        }

        //購物車 確認價錢
        public decimal? checkPrice(decimal? fOriginalPrice, decimal? fSpecialOffer, DateTime? fDiscountDate)
        {
            if (fOriginalPrice == null)
                fOriginalPrice = 0;
            if (fSpecialOffer == null)
                fSpecialOffer = 0;
            if (fDiscountDate == null)
                return fOriginalPrice;
            DateTime now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (fDiscountDate >= now)
                return fSpecialOffer;
            else
                return fOriginalPrice;
        }
    }
}
