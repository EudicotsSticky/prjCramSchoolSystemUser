using System;
using System.Collections.Generic;

#nullable disable

namespace prjCramSchoolSystemUser.Models
{
    public partial class TOrder
    {
        public TOrder()
        {
            TOrderDetails = new HashSet<TOrderDetail>();
        }

        public string FOrderId { get; set; }
        public string FUserId { get; set; }
        public int? FPayment { get; set; }
        public int? FOrderState { get; set; }
        public string FCreationUser { get; set; }
        public DateTime? FCreationDate { get; set; }
        public string FSaverUser { get; set; }
        public DateTime? FSaverDaate { get; set; }

        public virtual ICollection<TOrderDetail> TOrderDetails { get; set; }
    }
}
