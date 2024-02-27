using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using e_learning.Models;
using System.IO;
using System.Threading.Tasks;

namespace e_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public AttendanceController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [Route("getstudenddata")]
        [HttpGet]
        public JsonResult Get()
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



        //[Route("attendancedata")]
        //[HttpPost]
        //public JsonResult Attendancesheet([FromBody] AttemdanceSheet objlecturedeatils)
        //{
        //    foreach (var objstudent in objlecturedeatils.tableData)
        //    {
        //        string query = @"
        //            insert into dbo.attendance 
        //            (batchname,dateoflecture,facultyname,schedule_id,subjectname,Timing,user_id,studentname,presentflag)
        //            values 
        //            (
        //            '" + objlecturedeatils.batchname + @"'
        //            ,'" + objlecturedeatils.dateoflecture + @"'
        //            ,'" + objlecturedeatils.facultyname + @"'
        //            ,'" + objlecturedeatils.schedule_id + @"'
        //            ,'" + objlecturedeatils.subjectname + @"'
        //            ,'" + objlecturedeatils.Timing + @"'
        //            ,'" + objstudent.user_id + @"'
        //            ,'" + objstudent.studentname + @"'
        //            ,'" + objstudent.presentflag + @"'

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
        //    return new JsonResult("Successfully Done");

        //}



        [Route("getfile")]
        [HttpGet]
        public JsonResult Getfile()
        {
            string query = @"select FilePath from upload_material where batchname='1029' and FileExtension='.pdf'";

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

            //var memory = new MemoryStream();
            //using (var stream = new FileStream(path, FileMode.Open))
            //{
            //    await stream.CopyToAsync(memory);
            //}
            //memory.Position = 0;
            //var contentType = "APPLICATION/octet-stream";
            //var fileName = Path.GetFileName(path);

            //return File(memory, contentType, fileName);





        }

        //public  Task<IActionResult> DownloadFile(string paFith)

        //{


        //    var path = Path.Combine();
        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(path, FileMode.Open))
        //    {
        //         stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;
        //    var contentType = "APPLICATION/octet-stream";
        //    var fileName = Path.GetFileName(path);

        //    return File(memory, contentType, fileName);

        //}


        //[HttpGet("{id}")]
        //public async Task<IActionResult> DownloadFile(int id)
        //{
        //    string filePath = null;

        //    // Assuming dBContext is a DbContext instance
        //    using (var connection = .Database.GetDbConnection())
        //    {
        //        await connection.OpenAsync();

        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandText = "SELECT FilePath FROM FileData WHERE Id = @Id";
        //            command.Parameters.Add(new SqlParameter("@Id", id));

        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                if (await reader.ReadAsync())
        //                {
        //                    filePath = reader.GetString(0); // Assuming FilePath is stored as a string in the database
        //                }
        //            }
        //        }
        //    }

        //    // Check if the file path is found
        //    if (string.IsNullOrEmpty(filePath))
        //    {
        //        return NotFound(); // Or handle the case when the file is not found
        //    }

        //    // Assuming filePath contains the full file path (including file name and extension)
        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(filePath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    var contentType = "APPLICATION/octet-stream";
        //    var fileName = Path.GetFileName(filePath);

        //    return File(memory, contentType, fileName);   
        //}


    }
}
