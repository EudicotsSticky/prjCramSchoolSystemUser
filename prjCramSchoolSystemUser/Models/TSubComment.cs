using System;
using System.Collections.Generic;

#nullable disable

namespace prjCoreCramSchoolDB.Models
{
    public partial class TSubComment
    {
        public string FSubCommentId { get; set; }
        public DateTime? FSubCommentTime { get; set; }
        public string FSubCommentContent { get; set; }
        public string FCommentId { get; set; }
        public string FPostId { get; set; }
        public string FAccount { get; set; }
        public string FPhotoId { get; set; }

        public virtual TPost FSubComment { get; set; }
        public virtual TSubCommentPhoto TSubCommentPhoto { get; set; }
        public virtual TSubCommentThumbUp TSubCommentThumbUp { get; set; }
    }
}
