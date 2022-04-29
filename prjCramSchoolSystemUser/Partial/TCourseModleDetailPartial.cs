using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Models
{
    [ModelMetadataTypeAttribute(typeof(TCourseModelDetailMD))]
    public partial class TCourseModelDetail 
    {
    }
    
    public class TCourseModelDetailMD
    {
        public int FId { get; set; }
        public string FCourseId { get; set; }
        [DisplayName("課堂數")]
        public int? FCourseNumber { get; set; }
        [DisplayName("課堂進度(頁數)")]
        public string FSchedule { get; set; }
        [DisplayName("課堂進度說明")]
        public string FScheduleDetail { get; set; }
        [DisplayName("授課方式")]
        public string FTeachingMethod { get; set; }
        [DisplayName("備註")]
        public string FRemark { get; set; }
        public string FCreationUser { get; set; }
        public DateTime? FCreationDate { get; set; }
        public string FSaverUser { get; set; }
        public DateTime? FSaverDate { get; set; }
    }
}
