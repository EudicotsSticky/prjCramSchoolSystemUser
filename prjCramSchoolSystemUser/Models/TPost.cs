using System;
using System.Collections.Generic;

#nullable disable

namespace prjCoreCramSchoolDB.Models
{
    public partial class TPost
    {
        public string FPostId { get; set; }
        public string FPostTitle { get; set; }
        public string FPostContent { get; set; }
        public DateTime? FPostTime { get; set; }
        public string FAccount { get; set; }
        public DateTime? FPostUpdateTime { get; set; }
        public string FPhotoId { get; set; }
        public string FPostSort { get; set; }

        public virtual TPostComment TPostComment { get; set; }
        public virtual TPostThumbUp TPostThumbUp { get; set; }
        public virtual TSubComment TSubComment { get; set; }
    }
}
