using System;
using System.Collections.Generic;

#nullable disable

namespace prjCoreCramSchoolDB.Models
{
    public partial class TCommentPhoto
    {
        public string FCommentPhotoId { get; set; }
        public string FCommentId { get; set; }
        public string FCommentPhotoName { get; set; }
        public byte[] FCommentPhotoData { get; set; }

        public virtual TPostComment FCommentPhoto { get; set; }
    }
}
