using e_learning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace e_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public ScheduleController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }




        [Route("Addschedule")]
        [HttpPost]
        public JsonResult Post([FromBody] ScheduleModel objschedule)
        {
            foreach (var lecturesdetail in objschedule.subjectdetails)
            {
                string query = @"
                    insert into dbo.schedule 
                    (substreamname,batchname,subjectname,facultyname,dateoflecture,startTime,endTime,notice)
                    values 
                    (
                    '" + objschedule.substreamname + @"'
                    ,'" + objschedule.batchname + @"'
                    ,'" + lecturesdetail.subjectname + @"'
                    ,'" + lecturesdetail.facultyname + @"'
                    ,'" + lecturesdetail.dateoflecture + @"'
                    ,'" + lecturesdetail.startTime + @"'
                    ,'" + lecturesdetail.endTime + @"'
                    ,'" + lecturesdetail.notice + @"'
                    )
                    ";

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
            return new JsonResult("added successfully");

        }



        //[Route("test")]
        //[HttpPost]
        //public JsonResult PostTest([FromBody] TestModel objtest)
        //{
        //    foreach (var testdetail in objtest.testdetails)
        //    {
        //        string query = @"
        //            insert into dbo.test 
        //            (substreamname,batchname,subjectname,facultyname,dateoftest,topicname,testtype,totalmark,startTime,endTime,notice)
        //            values 
        //            (
        //            '" + objtest.substreamname + @"'
        //            ,'" + objtest.batchname + @"'
        //            ,'" + testdetail.subjectname + @"'
        //            ,'" + testdetail.facultyname + @"'
        //            ,'" + testdetail.dateoftest + @"'
        //             ,'" + testdetail.topicname + @"'
        //            ,'" + testdetail.testtype + @"'
        //            ,'" + testdetail.totalmark + @"'
        //            ,'" + testdetail.startTime + @"'
        //            ,'" + testdetail.endTime + @"'
        //            ,'" + testdetail.notice + @"'
        //            )
        //            ";

        //        DataTable table = new DataTable();
        //        string sqlDataSource = _configuration.GetConnectionString("ElearningAppCon");
        //        SqlDataReader myReader;
        //        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //        {
        //            myCon.Open();
        //            using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //            {
        //                myReader = myCommand.ExecuteReader();
        //                table.Load(myReader);
        //                myReader.Close();
        //                myCon.Close();
        //            }
        //        }
        //    }
        //    return new JsonResult("added successfully");

        //}



        [Route("getschedule")]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select sch.dateoflecture,sch.schedule_id,sch.batchname,sch.subjectname,sch.facultyname,sch.startTime,sch.endTime,
                                addsu.subjects,addfa.name
                                from schedule as sch 
                                join addsubject as addsu
                                on sch.subjectname=addsu.subject_pk 
                                join addfaculty as addfa 
                                on sch.facultyname=addfa.add_faculty_pk;";

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



        [Route("chooseddate/{selecteddate}")]
        [HttpGet]
        public JsonResult LoginGet(string selecteddate)
        {
         
            string query = @"select sch.schedule_id,sch.substreamname,sch.batchname,sch.subjectname,
                            sch.facultyname,CONVERT(varchar,sch.dateoflecture, 105)dateoflecture,
                            concat(sch.startTime,' ', sch.endTime)Timing,sch.notice,addsub.subjects,af.name from 
                            schedule as sch
                            left join addsubject as addsub
                            on sch.subjectname=addsub.subject_pk
                            left join addfaculty as af 
                            on sch.facultyname=af.add_faculty_pk where sch.dateoflecture='" + selecteddate + "' ";

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


        [Route("studentid/{userid}")]
        [HttpGet]
        public JsonResult studnetscheduledara(string userid)
        {

            string query = @"select sch.schedule_id,CONVERT(varchar, sch.dateoflecture, 105)dateoflecture,sch.startTime,sch.endTime,sch.notice,addsub.subjects,
                            addfac.name from schedule as sch 
                            inner join users as us 
                            on sch.batchname=us.batchName 
                            left join addsubject as addsub
                            on sch.subjectname=addsub.subject_pk
                            left join addfaculty as addfac
                            on sch.facultyname=addfac.add_faculty_pk
                            where us.user_id='" + userid + "' and sch.dateoflecture >= GETDATE() ORDER BY sch.dateoflecture ASC";

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
