using System;
using System.Collections.Generic;

#nullable disable

namespace prjCoreCramSchoolDB.Models
{
    public partial class TPostThumbUp
    {
        public string FThumbUpId { get; set; }
        public string FPostId { get; set; }
        public string FAccount { get; set; }
        public DateTime? FThumbUpTime { get; set; }
        public byte? FThumbUpCheck { get; set; }

        public virtual TPost FThumbUp { get; set; }
    }
}
