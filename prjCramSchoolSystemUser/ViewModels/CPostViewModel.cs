using prjCoreCramSchoolDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjCoreCramSchoolDB.ViewModels
{
    public class CPostViewModel
    {
        
        private TPost _post = null;
        public CPostViewModel()
        {
            _post = new TPost();
        }
        public TPost post
        {
            get { return _post; }
            set { _post = value; }
        }
        public string FPostId
        {
            get { return this.post.FPostId; }
            set { this.post.FPostId = value; }
        }
        [DisplayName("貼文標題")]
        public string FPostTitle
        {
            get { return this.post.FPostTitle; }
            set { this.post.FPostTitle = value; }
        }
        [DisplayName("貼文內容")]
        public string FPostContent
        {
            get { return this.post.FPostContent; }
            set { this.post.FPostContent = value; }
        }
        [DisplayName("發文時間")]
        public DateTime? FPostTime
        {
            get { return this.post.FPostTime; }
            set { this.post.FPostTime = value; }
        }
        [DisplayName("發文者")]
        public string FAccount
        {
            get { return this.post.FAccount; }
            set { this.post.FAccount = value; }
        }
        [DisplayName("更新時間")]
        public DateTime? FPostUpdateTime
        {
            get; set;
        }
        public string FPhotoId
        {
            get { return this.post.FPhotoId; }
            set { this.post.FPhotoId = value; }
        }
    }
}
