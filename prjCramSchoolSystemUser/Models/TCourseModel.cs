using System;
using System.Collections.Generic;

#nullable disable

namespace prjCramSchoolSystemUser.Models
{
    public partial class TCourseModel
    {
        public TCourseModel()
        {
            TCourseInformations = new HashSet<TCourseInformation>();
            TCourseModelDetails = new HashSet<TCourseModelDetail>();
        }

        public string FCourseId { get; set; }
        public int? FCategory { get; set; }
        public string FName { get; set; }
        public int? FTotalHours { get; set; }
        public int? FTotalNumber { get; set; }
        public string FGrade { get; set; }
        public int? FSchoolYear { get; set; }
        public string FSummary { get; set; }
        public int? FModleSate { get; set; }
        public string FTeachingMaterial { get; set; }
        public decimal? FOriginalPrice { get; set; }
        public decimal? FSpecialOffer { get; set; }
        public string FCreationUser { get; set; }
        public DateTime? FCreationDate { get; set; }
        public string FSaverUser { get; set; }
        public DateTime? FSaverDate { get; set; }

        public virtual ICollection<TCourseInformation> TCourseInformations { get; set; }
        public virtual ICollection<TCourseModelDetail> TCourseModelDetails { get; set; }
    }
}
