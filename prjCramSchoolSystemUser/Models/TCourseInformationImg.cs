using System;
using System.Collections.Generic;

#nullable disable

namespace prjCramSchoolSystemUser.Models
{
    public partial class TCourseInformationImg
    {
        public int FId { get; set; }
        public string FEchelonId { get; set; }
        public string FCourseImageName { get; set; }

        public virtual TCourseInformation FEchelon { get; set; }
    }
}
