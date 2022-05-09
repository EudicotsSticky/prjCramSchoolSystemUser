using System;
using System.Collections.Generic;

#nullable disable

namespace prjCoreCramSchoolDB.Models
{
    public partial class TSubCommentPhoto
    {
        public string FSubCommentPhotoId { get; set; }
        public string FSubCommentId { get; set; }
        public string FSubCommentPhotoName { get; set; }
        public byte[] FSubCommentPhotoData { get; set; }

        public virtual TSubComment FSubCommentPhoto { get; set; }
    }
}
