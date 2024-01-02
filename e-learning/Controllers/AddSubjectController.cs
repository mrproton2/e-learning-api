using e_learning.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System;

namespace e_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddSubjectController : ControllerBase

    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public AddSubjectController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [Route("addsubject")]
        [HttpPost]
        public JsonResult Post([FromBody] AddSubjectModel objaddsubject)
        {
            foreach (var subname in objaddsubject.subjects)
            {
            string query = @"
                    insert into dbo.addsubject 
                    (substream_pk,subjects,status,createddate,createdby)
                    values 
                    
                    (
                    '" + objaddsubject.substream_pk + @"'
                    ,'" + subname.subjectname + @"'
                    ,'" + objaddsubject.status + @"'
                    ,'" + objaddsubject.createddate + @"'
                    ,'" + objaddsubject.createdby + @"'  
                
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

        [Route("Getsubjects")]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select abatch.batch_name,
                asubject.subjects,
                asubstream.sub_stream_name
                from addbatch as abatch 
                left join addsubject as asubject on abatch.substream_pk=asubject.substream_pk
                join addsubstream as asubstream on asubstream.addsubstream_pk=abatch.substream_pk";

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



        [Route("subjecttableget")]
        [HttpGet]
        public JsonResult GetJson()
        {
            string query = @"select * from dbo.addsubject ORDER BY subject_pk DESC";

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
