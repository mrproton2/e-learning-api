using e_learning.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace e_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddFacultyOrStaffController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public AddFacultyOrStaffController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
    
        // Add faculty
        [Route("addfaculty")]
        [HttpPost]
        public JsonResult Post([FromBody] addFacultyModel objfaculty)
        {
            string query = @"
                    insert into dbo.addfaculty 
                    (name,dob,address,contact,emailid,gender,qualification,experience,teaching_other_institute,
                    pan_no,aadhar_no,tsd_no,per_hour_amount,accout_no,ifsc_code,createddate,createdby,subject_pk,substream_pk)
                    values 
                    (
                    '" + objfaculty.fname + @"'
                    ,'" + objfaculty.fdob + @"'
                    ,'" + objfaculty.faddress + @"'
                     ,'" + objfaculty.fcontact + @"'
                    ,'" + objfaculty.femailid + @"'
                    ,'" + objfaculty.fgender + @"' 
                    ,'" + objfaculty.fqualification + @"'
                    ,'" + objfaculty.fexperience + @"'
                    ,'" + objfaculty.fteaching_other_institute + @"'
                     ,'" + objfaculty.fpan_no + @"'
                    ,'" + objfaculty.faadhar_no + @"'
                    ,'" + objfaculty.ftsd_no + @"'
                    ,'" + objfaculty.fper_hour_amount + @"'
                    ,'" + objfaculty.faccout_no + @"'
                    ,'" + objfaculty.fifsc_code + @"'
                    ,'" + objfaculty.createddate + @"'
                    ,'" + objfaculty.createdby + @"'
                    ,'" + objfaculty.fsubject + @"'
                     ,'" + objfaculty.fsubstream + @"'
                     
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

        // Add Staff
        [Route("addstaff")]
        [HttpPost]
        public JsonResult Post([FromBody] addStaffModel objstaff)
        {
            string query = @"
                    insert into dbo.addstaff 
                    (name,dob,address,contact,emailid,gender,qualification,experience,
                    pan_no,aadhar_no,tsd_no,salary,accout_no,ifsc_code,createddate,createdby)                   
                    values 
                    (
                    '" + objstaff.sname + @"'
                    ,'" + objstaff.sdob + @"'
                    ,'" + objstaff.saddress + @"'
                    ,'" + objstaff.scontact + @"'
                    ,'" + objstaff.semailid + @"'
                    ,'" + objstaff.sgender + @"'
                     ,'" + objstaff.squalification + @"'
                    ,'" + objstaff.sexperience + @"'
                    ,'" + objstaff.span_no + @"'
                    ,'" + objstaff.saadhar_no + @"'
                     ,'" + objstaff.stsd_no + @"'
                    ,'" + objstaff.ssalary + @"'
                     ,'" + objstaff.saccout_no + @"'
                    ,'" + objstaff.sifsc_code + @"'
                    ,'" + objstaff.createddate + @"'
                     ,'" + objstaff.createdby + @"'
                     
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

    }
}
