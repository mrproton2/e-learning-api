﻿
using System.Collections.Generic;

namespace e_learning.Models
{
    public class TestModel
    {


        public string substreamname { get; set; }
        public string batchname { get; set; }

        public List<testdetail> testdetails { get; set; }

        public class testdetail
        {
            public string subjectname;
            public string facultyname;
            public string dateoftest;
            public string topicname;
            public string totalmark;
            public string notice;
            public string startTime;
            public string endTime;
            public string testtype;
        }

    }
}
