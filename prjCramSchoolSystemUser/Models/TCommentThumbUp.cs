using System;
using System.Collections.Generic;

#nullable disable

namespace prjCoreCramSchoolDB.Models
{
    public partial class TCommentThumbUp
    {
        public string FComThumbUpId { get; set; }
        public string FCommentId { get; set; }
        public string FAccount { get; set; }
        public DateTime? FThumbUpTime { get; set; }
        public byte? FThumbUpCheck { get; set; }

        public virtual TPostComment FComThumbUp { get; set; }
    }
}
