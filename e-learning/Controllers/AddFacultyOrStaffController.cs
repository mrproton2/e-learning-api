using e_learning.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System;

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
                    pan_no,aadhar_no,tsd_no,per_hour_amount,accout_no,ifsc_code,createddate,createdby,subject_pk,substream_pk,employee)
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
                      ,'" + objfaculty.employee + @"'
                     
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









        [HttpPost("generateIDpasswordfaculty")]
        public JsonResult Post([FromBody] FacultyIDPasswordModel objidpassfaculty)
        {

            string firstNamePrefix = objidpassfaculty.name.Length >= 3 ? objidpassfaculty.name.Substring(0, 4) : objidpassfaculty.name;
            var username = $"{firstNamePrefix.ToLower()}{objidpassfaculty.contact.ToLower()}";
            string password = GenerateRandomPassword();
            string query = @"
                    insert into dbo.users 
                    (user_name,user_password,admissionid_pk,idpassflag,RoleID,contact)
                    values 
                    (
                    '" + username + @"'
                    ,'" + password + @"'
                    ,'" + objidpassfaculty.add_faculty_pk + @"'
                     ,'" + objidpassfaculty.idpassflag + @"' 
                      ,'" + objidpassfaculty.RoleID + @"' 
                    ,'" + objidpassfaculty.contact + @"'  

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

        private string GenerateRandomPassword(int length = 10)
        {
            const string validChars = "123456abcdef789ghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0@#$&+";

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(validChars.Length);
                password.Append(validChars[index]);
            }
            return password.ToString();
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

        [Route("getfacultydata")]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from dbo.addfaculty ";

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


        [Route("getfacultywithidpass")]
        [HttpGet]
        public JsonResult  idpassfacultydata()
        {
            string query = @"select* from addfaculty a Left join facultyIDPass_details b on a.add_faculty_pk=b.add_faculty_pk ";

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