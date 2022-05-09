using System;
using System.Collections.Generic;

#nullable disable

namespace prjCoreCramSchoolDB.Models
{
    public partial class TPostComment
    {
        public string TCommentId { get; set; }
        public DateTime? FCommentTime { get; set; }
        public string FCommentContent { get; set; }
        public string FPostId { get; set; }
        public string FAccount { get; set; }
        public string FPhotoId { get; set; }

        public virtual TPost TComment { get; set; }
        public virtual TCommentPhoto TCommentPhoto { get; set; }
        public virtual TCommentThumbUp TCommentThumbUp { get; set; }
    }
}
