using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Models
{
    public class CDictionary
    {
        //為了不用每次使用都要new ->static
        //ID
        public static readonly string SK_LONGUNED_ID = "SK_LONGUNED_ID";
        //Name
        public static readonly string SK_LOGINED_USER = "SK_LOGINED_USER";
        //
        public static readonly string SK_COURSE_PURCHASED_LIST = "SK_COURSE_PURCHASED_LIST";

        //改寫 CourseModel_Detail (目前沒使用到)
        public static readonly string SK_COURSE_MODELDETAIL = "SK_COURSE_MODELDETAIL";
    }
}
