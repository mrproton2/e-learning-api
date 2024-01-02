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
    public class InquiryFormController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public InquiryFormController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [Route("InquiryFormData")]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from dbo.addinquiry  ORDER BY addinquiry_pk DESC";

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


        [Route("addInquiryForm")]
        [HttpPost]
        public JsonResult Post([FromBody] addInquiry objaddInquiryForm)
            {
            string query = @"
                    insert into dbo.addinquiry 
                    (name,date,address,contact_no,email_id,gender,
                    date_of_birth,previous_qualification,
                    school_college_name,stream_name,substream_name,createddate,createdby)
                    values 
                    (
                    
                    '" + objaddInquiryForm.name + @"'
                    ,'" + objaddInquiryForm.date + @"'
                    ,'" + objaddInquiryForm.address + @"'
                    ,'" + objaddInquiryForm.contact_no + @"'
                    ,'"+ objaddInquiryForm.email_id+ @"'
                    ,'"+ objaddInquiryForm.gender+ @"'
                    ,'"+ objaddInquiryForm.date_of_birth+ @"'
                    ,'"+ objaddInquiryForm.previous_qualification+ @"'
                    ,'"+ objaddInquiryForm.school_college_name+ @"'
                    ,'"+ objaddInquiryForm.stream_name+ @"'
                    ,'"+ objaddInquiryForm.substream_name+ @"'
                    ,'"+ objaddInquiryForm.createddate+ @"'
                    ,'"+ objaddInquiryForm.createdby+ @"'
                  
                     
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
            return new JsonResult("added successfully");
        }

        [Route("updateinquiryform")]
        [HttpPut]
        public JsonResult Put(addInquiry objaddInquiryForm)
        {
            string query = @"
                    update dbo.addinquiry set 
                    name = '" + objaddInquiryForm.name + @"'
                    ,date = '" + objaddInquiryForm.date + @"'
                    ,address = '" + objaddInquiryForm.address + @"'
                     ,contact_no = '" + objaddInquiryForm.contact_no + @"'
                    ,email_id = '" + objaddInquiryForm.email_id + @"'
                     ,gender = '" + objaddInquiryForm.gender + @"'
                    ,date_of_birth = '" + objaddInquiryForm.date_of_birth + @"'
                     ,previous_qualification = '" + objaddInquiryForm.previous_qualification + @"'
                    
                    ,school_college_name = '" + objaddInquiryForm.school_college_name + @"'
                    ,stream_name = '" + objaddInquiryForm.stream_name + @"'
                    ,substream_name = '" + objaddInquiryForm.substream_name + @"'
    
                    where addinquiry_pk = '" + objaddInquiryForm.addinquiry_pk + @"'
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
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{addinquiry_pk}")]
        public JsonResult Delete(int addinquiry_pk)
        {
            string query = @"
                    delete from dbo.addinquiry
                    where addinquiry_pk = " + addinquiry_pk + @" 
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
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

    }
}
