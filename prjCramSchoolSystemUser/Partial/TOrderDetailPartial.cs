using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Models
{
    [ModelMetadataTypeAttribute(typeof(TOrderDetailMD))]
    public partial class TOrderDetail
    {
        public class TOrderDetailMD
        {
            public int FId { get; set; }
            public string FOrderId { get; set; }
            [DisplayName("上課學生")]
            public string FReceiverId { get; set; }
            public string FEchelonId { get; set; }
            public decimal? FMoney { get; set; }
            public string FCreationUser { get; set; }
            public DateTime? FCreationDate { get; set; }
            public string FSaverUser { get; set; }
            public DateTime? FSaverDate { get; set; }
        }
    }
}
