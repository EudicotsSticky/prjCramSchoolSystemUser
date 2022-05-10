using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Models
{
    [ModelMetadataTypeAttribute(typeof(TOrderMD))]
    public partial class TOrder
    {
        public class TOrderMD
        {
            public string FOrderId { get; set; }
            [DisplayName("付款人帳號")]
            public string FUserId { get; set; }
            //
            public int? FPayment { get; set; }
            public int? FOrderState { get; set; }
            public string FCreationUser { get; set; }
            public DateTime? FCreationDate { get; set; }
            public string FSaverUser { get; set; }
            //[DataType(DataType.Date)]
            public DateTime? FSaverDaate { get; set; }
        }
    }
}
