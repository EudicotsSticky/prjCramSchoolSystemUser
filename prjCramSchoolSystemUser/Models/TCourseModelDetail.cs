using System;
using System.Collections.Generic;

#nullable disable

namespace prjCramSchoolSystemUser.Models
{
    public partial class TCourseModelDetail
    {
        public int FId { get; set; }
        public string FCourseId { get; set; }
        public int? FCourseNumber { get; set; }
        public string FSchedule { get; set; }
        public string FScheduleDetail { get; set; }
        public string FTeachingMethod { get; set; }
        public string FRemark { get; set; }
        public string FCreationUser { get; set; }
        public DateTime? FCreationDate { get; set; }
        public string FSaverUser { get; set; }
        public DateTime? FSaverDate { get; set; }

        public virtual TCourseModel FCourse { get; set; }
    }
}
