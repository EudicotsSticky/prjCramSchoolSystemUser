using prjCramSchoolSystemUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.ViewModel
{
    public class CCourseModelViewModel
    {
        //判斷有沒有放值進去 有就回傳 沒有件建立一個空的新的
        private TCourseModel _coursemodel = null;
        public TCourseModel coursemodel 
        {
            get { return _coursemodel; }
            set{ _coursemodel = value; }
        }
        public CCourseModelViewModel()
        {
            _coursemodel = new TCourseModel();
            TCourseInformations = new HashSet<TCourseInformation>();
            CCourseModleDetailsViewModel = new HashSet<CCourseModelDetailViewModel>();
            TCourseModelImgs = new HashSet<TCourseInformationImg>();
        }

        public string FCourseId
        {
            get { return this.coursemodel.FCourseId; }
            set { this.coursemodel.FCourseId = value; }
        }
        public int? FCategory
        {
            get { return this.coursemodel.FCategory; }
            set { this.coursemodel.FCategory = value; }
        }
        public string FName
        {
            get { return this.coursemodel.FName; }
            set { this.coursemodel.FName = value; }
        }
        public int? FTotalHours 
        { 
            get { return this.coursemodel.FTotalHours; }
            set { this.coursemodel.FTotalHours = value; } 
        }
        public int? FTotalNumber
        {
            get { return this.coursemodel.FTotalHours; }
            set { this.coursemodel.FTotalHours = value; }
        }
        public string FGrade
        {
            get { return this.coursemodel.FGrade; }
            set { this.coursemodel.FGrade = value; }
        }
        public int? FSchoolYear 
        {
            get { return this.coursemodel.FSchoolYear; }
            set { this.coursemodel.FSchoolYear = value; } 
        }
        public string FSummary 
        {
            get { return this.coursemodel.FSummary; }
            set { this.coursemodel.FSummary = value; }
        }
        public int? FModleSate 
        { 
            get { return this.coursemodel.FModleSate; }
            set { this.coursemodel.FModleSate = value; }
        }
        public decimal? FOriginalPrice 
        {
            get { return this.coursemodel.FOriginalPrice; }
            set { this.coursemodel.FOriginalPrice = value; }
        }
        public decimal? FSpecialOffer 
        {
            get { return this.coursemodel.FSpecialOffer; }
            set{ this.coursemodel.FSpecialOffer = value; }
        }
        public string FCreationUser
        {
            get { return this.coursemodel.FCreationUser; }
            set { this.coursemodel.FCreationUser = value; }
        }
        public DateTime? FCreationDate 
        {
            get { return this.coursemodel.FCreationDate; }
            set { this.coursemodel.FCreationDate = value; }
        }
        public string FSaverUser 
        {
            get { return this.coursemodel.FSaverUser; }
            set { this.coursemodel.FSaverUser = value; }
        }
        public DateTime? FSaverDate 
        {
            get { return this.coursemodel.FSaverDate; }
            set { this.coursemodel.FSaverDate = value; }
        }

        public virtual ICollection<TCourseInformation> TCourseInformations { get; set; }
        public virtual ICollection<CCourseModelDetailViewModel> CCourseModleDetailsViewModel { get; set; }
        public virtual ICollection<TCourseInformationImg> TCourseModelImgs { get; set; }

    }
}
