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
    public class addSubStreamController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public addSubStreamController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [Route("getsubstream")]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from dbo.addsubstream ORDER BY addsubstream_pk DESC";

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
        [HttpDelete("{addsubstream_pk}")]
        public JsonResult Delete(int addsubstream_pk)
        {
            string query = @"
                    delete from dbo.addsubstream
                    where addsubstream_pk = " + addsubstream_pk + @" 
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

        [Route("addsubstream")]
        [HttpPost]
        public JsonResult Post([FromBody] addSubStream objaddSubStream)
        {
            string query = @"
                    insert into dbo.addsubstream 
                    (sub_stream_name,fees,creation_date,status,createddate,createdby,stream_pk)
                    values 
                    (
                    '" + objaddSubStream.sub_stream_name + @"'
                    ,'" + objaddSubStream.fees + @"'
                    ,'" + objaddSubStream.creation_date + @"'
                    ,'" + objaddSubStream.status + @"'
                    
                    ,'" + objaddSubStream.createddate + @"'
                     ,'" + objaddSubStream.createdby + @"'
                    ,'" + objaddSubStream.stream_pk + @"'


                     
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

        [Route("updatesubstream")]
        [HttpPut]
        public JsonResult Put(addSubStream objaddSubStream)
        {
            string query = @"
                    update dbo.addsubstream set 
                    
                    ,sub_stream_name = '" + objaddSubStream.sub_stream_name + @"'
                    ,fees = '" + objaddSubStream.fees + @"'
                    ,creation_date = '" + objaddSubStream.creation_date + @"'
                    ,status = '" + objaddSubStream.status + @"'
                    
                    where addsubstream_pk = '" + objaddSubStream.addsubstream_pk + @"'
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




        [Route("getstream")]
        [HttpGet]
        public JsonResult getstream()
        {
            string query = @"select addstream_pk,stream_name from addstream";

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




        [Route("addsubject")]
        [HttpPost]
        public JsonResult Post([FromBody] AddSubjectModel objaddsubject)
        {
            string query = @"
                    insert into dbo.addsubject 
                    (substream_pk,subjects,status,createddate,createdby)
                    values 
                    (
                    (
                    '" + objaddsubject.substream_pk + @"'
               
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
            return new JsonResult("added successfully");

        }

    }
    
}
