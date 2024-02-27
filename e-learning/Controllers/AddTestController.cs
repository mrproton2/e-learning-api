using e_learning.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using System.Data.SqlClient;
using System.Data;
using System;


namespace e_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddTestController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public AddTestController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [Route("AddTest")]
        [HttpPost]
        public JsonResult TestSchedule([FromBody] TestModel objtest)
        {
            foreach (var testdetail in objtest.testdetails)
            {
                string query = @"
                    insert into dbo.test 
                    (substreamname,batchname,subjectname,facultyname,dateoftest,topicname,testtype,totalmark,startTime,endTime,notice)
                    values 
                    (
                    '" + objtest.substreamname + @"'
                    ,'" + objtest.batchname + @"'
                    ,'" + testdetail.subjectname + @"'
                    ,'" + testdetail.facultyname + @"'
                    ,'" + testdetail.dateoftest + @"'
                    ,'" + testdetail.topicname + @"'
                    ,'" + testdetail.testtype + @"'
                    ,'" + testdetail.totalmark + @"'
                    ,'" + testdetail.startTime + @"'
                    ,'" + testdetail.endTime + @"'
                    ,'" + testdetail.notice + @"'
                    )";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("ElearningAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            return new JsonResult("Successfully Done");

        }

        [Route("gettestschedulebystudent")]
        [HttpGet]
        public JsonResult gettestschedule()
        {
            string query = @"select us.user_id,us.batchName,ad.studentname,ad.studentcontactno 
                            from users as us
                            inner join admission_details as ad 
                            on us.admissionid_pk=ad.admissionid_pk where us.RoleID='2'";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ElearningAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [Route("gettestschedulebystudent/{userid}")]
        [HttpGet]
        public JsonResult gettestschedule(string userid)
        {
            string query = @"select tes.test_id,CONVERT(varchar, tes.dateoftest, 105) dateoftest,tes.testtype,CONCAT(tes.startTime,'To ',tes.endTime)Timing,tes.notice,tes.batchname,ab.batch_name,addsub.subjects
                            ,af.name
                            from 
                            test as tes 
                            left join addbatch as ab
                            on tes.batchname=ab.addbatch_pk
                            left join addsubject as addsub
                            on tes.subjectname=addsub.subject_pk
                            left join addfaculty as af
                            on tes.facultyname=af.add_faculty_pk
                            left join users as us
                            on tes.batchname=us.batchName
                            where tes.dateoftest>= GETDATE() and us.user_id='" + userid + "'  order by tes.dateoftest ASC";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ElearningAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }




    }
}
