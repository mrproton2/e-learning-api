using System;
using System.Collections.Generic;

namespace e_learning.Models
{
    public class AddSubjectModel
    {


        public int subject_pk { get; set; }
        public string substream_pk { get; set; }
        //public string subjects  { get; set; }
        public string status { get; set; }
        public string createddate { get; set; }
        public string createdby { get; set; }
        
        public List<Subject> subjects { get; set; }

    }


    public class Subject
    {
        public string subjectname;
    }
 
}
