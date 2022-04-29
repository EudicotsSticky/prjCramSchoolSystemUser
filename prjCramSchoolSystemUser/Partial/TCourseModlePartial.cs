using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static prjCramSchoolSystemUser.Models.TCourseModel;

namespace prjCramSchoolSystemUser.Models
{
    //MetadataType
    //ModelMetadataTypeAttribute
    //MetadataTypeAttribute
    [ModelMetadataTypeAttribute(typeof(TCourseModelMD))]
    public partial class TCourseModel
    {
    }
    public class TCourseModelMD
    {
        //[DisplayName("test")]
        public string FCourseId { get; set; }
        [DisplayName("課程類別")]
        public int? FCategory { get; set; }
        [DisplayName("課程名稱")]
        public string FName { get; set; }
        [DisplayName("課程總時數")]
        public int? FTotalHours { get; set; }
        [DisplayName("課程堂數")]
        public int? FTotalNumber { get; set; }
        [DisplayName("授課年級")]
        public string FGrade { get; set; }
        [DisplayName("學期")]
        public int? FSchoolYear { get; set; }
        [DisplayName("課程大綱")]
        public string FSummary { get; set; }
        public int? FModleSate { get; set; }
        public string FTeachingMaterial { get; set; }
        [DisplayName("課程費用_原價")]
        public decimal? FOriginalPrice { get; set; }
        [DisplayName("課程費用_特價")]
        public decimal? FSpecialOffer { get; set; }
        public string FCreationUser { get; set; }
        public DateTime? FCreationDate { get; set; }
        public string FSaverUser { get; set; }
        public DateTime? FSaverDate { get; set; }

    }
}
