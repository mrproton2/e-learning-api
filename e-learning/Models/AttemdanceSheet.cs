using System;
using System.Collections.Generic;

namespace e_learning.Models
{
    public class AttemdanceSheet
    {

        public string batchname { get; set; }
        public string dateoflecture { get; set; }
        public string facultyname { get; set; }
        public string schedule_id { get; set; }
        public string subjectname { get; set; }
        public string Timing { get; set; }

        public List<Studentdata> tableData { get; set; }

        public class Studentdata
        {
            
            public string user_id;
            public string studentname;
            public string presentflag;

        }


    }
}
