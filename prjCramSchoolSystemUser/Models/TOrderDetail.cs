using System;
using System.Collections.Generic;

#nullable disable

namespace prjCramSchoolSystemUser.Models
{
    public partial class TOrderDetail
    {
        public int FId { get; set; }
        public string FOrderId { get; set; }
        public string FReceiverId { get; set; }
        public string FEchelonId { get; set; }
        public decimal? FMoney { get; set; }
        public string FCreationUser { get; set; }
        public DateTime? FCreationDate { get; set; }
        public string FSaverUser { get; set; }
        public DateTime? FSaverDate { get; set; }

        public virtual TCourseInformation FEchelon { get; set; }
        public virtual TOrder FOrder { get; set; }
    }
}
