using System;
using System.Collections.Generic;

#nullable disable

namespace prjCoreCramSchoolDB.Models
{
    public partial class TSubCommentThumbUp
    {
        public string FSubComThumbUpId { get; set; }
        public string FSubCommentId { get; set; }
        public string FAccount { get; set; }
        public DateTime? FSubComThumbUpTime { get; set; }
        public byte? FSubSubThumbCheck { get; set; }

        public virtual TSubComment FSubComThumbUp { get; set; }
    }
}
