using System;
using System.Collections.Generic;

namespace e_learning.Models
{
    public class ScheduleModel
    {

        public string substreamname { get; set; }
        public string batchname { get; set; }

        public List<Subjesubjectdetailsct> subjectdetails { get; set; }

        public class Subjesubjectdetailsct
        {
            public string subjectname;
            public string facultyname;
            public string dateoflecture;
            public string startTime;
            public string endTime;
            public string notice;

        }



    }
}
