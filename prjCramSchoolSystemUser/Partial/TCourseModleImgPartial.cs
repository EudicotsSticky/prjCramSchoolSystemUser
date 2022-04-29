using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Models
{
    [ModelMetadataTypeAttribute(typeof (TCourseInformationImgMD))]
    public partial class TCourseInformationImg
    {
    }
    public class TCourseInformationImgMD
    {
        public int FId { get; set; }
        public string FEchelonId { get; set; }
        public string FCourseImageName { get; set; }
    }
}
