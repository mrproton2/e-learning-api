using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using e_learning.Models;

namespace e_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentProfileController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public StudentProfileController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [Route("studentprofile/{user_id}")]
        [HttpGet]
        public JsonResult StudentProfile (string user_id)
        {
            string query = @"select us.user_id,us.user_name,ad.studentname,ad.dob,ad.studentcontactno,ad.studentemail,ad.gender,
                            ab.batch_name,addsub.sub_stream_name
                            from users as us 
                            right join admission_details as ad 
                            on us.admissionid_pk=ad.admissionid_pk 
                            left join addbatch as ab 
                            on ad.batchName=ab.addbatch_pk
                            left join addsubstream as addsub on
                            addsub.addsubstream_pk=ad.substream_name
                            where us.user_id='" + user_id+"' ";

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





        [HttpPost("reset")]
        public IActionResult ResetPassword([FromBody] PasswordResetModel model)
        {
            string sqlDataSource = _configuration.GetConnectionString("ElearningAppCon");
            using (var connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                // Validate the existing password
               var query = "SELECT user_password FROM users WHERE (user_id = @userid)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userid", model.userid);
                    var existingPassword = (string)command.ExecuteScalar();
                    if (existingPassword != model.existingPassword)
                    {
                        return new JsonResult("Invalid existing password");
                    }
                }

                // Update the password
                query = "UPDATE users SET user_password = @newPassword WHERE (user_id = @userid)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userid", model.userid);
                    command.Parameters.AddWithValue("@newPassword", model.newPassword);
                    command.ExecuteNonQuery();
                }

                return new JsonResult("Password reset successfully");
            }
        }



    }

    }


