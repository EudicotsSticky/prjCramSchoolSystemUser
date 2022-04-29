using prjCramSchoolSystemUser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.ViewModel
{
    public class CCourseModelDetailViewModel
    {  
        private TCourseModelDetail _coursemodeldetail = null;
       public CCourseModelDetailViewModel()
        {
            _coursemodeldetail = new TCourseModelDetail();
        }

        public TCourseModelDetail coursemodeldetail
        {
            get { return _coursemodeldetail; }
            set { _coursemodeldetail = value; }
        }
       
        public int FId
        {
            get { return this.coursemodeldetail.FId; }
            set { this.coursemodeldetail.FId = value; }
        }
        [DisplayName("test")]
        public string FCourseId
        {
            get { return this.coursemodeldetail.FCourseId; }
            set { this.coursemodeldetail.FCourseId = value; }
        }

        public int? FCourseNumber
        {
            get { return this.coursemodeldetail.FCourseNumber; }
            set { this.coursemodeldetail.FCourseNumber = value; }
        }

        public string FSchedule
        {
            get { return this.coursemodeldetail.FSchedule; }
            set { this.coursemodeldetail.FSchedule = value; }
        }

        public string FScheduleDetail
        {
            get { return this.coursemodeldetail.FScheduleDetail; }
            set { this.coursemodeldetail.FScheduleDetail = value; }
        }

        public string FTeachingMethod
        {
            get { return this.coursemodeldetail.FTeachingMethod; }
            set { this.coursemodeldetail.FTeachingMethod = value; }
        }

        public string FRemark
        {
            get { return this.coursemodeldetail.FRemark; }
            set { this.coursemodeldetail.FRemark = value; }
        }

        public string FCreationUser
        {
            get { return this.coursemodeldetail.FCreationUser; }
            set { this.coursemodeldetail.FCreationUser = value; }
        }

        public DateTime? FCreationDate
        {
            get { return this.coursemodeldetail.FCreationDate; }
            set { this.coursemodeldetail.FCreationDate = value; }
        }

        public string FSaverUser
        {
            get { return this.coursemodeldetail.FSaverUser; }
            set { this.coursemodeldetail.FSaverUser = value; }
        }

        public DateTime? FSaverDate
        {
            get { return this.coursemodeldetail.FSaverDate; }
            set { this.coursemodeldetail.FSaverDate = value; }
        }


        public virtual CCourseModelViewModel FCourse { get; set; }

    }
}
