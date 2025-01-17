﻿using e_learning.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static PdfSharpCore.Pdf.PdfDictionary;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace e_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class dataController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public dataController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [Route("getstream")]
        [HttpGet]
        public JsonResult Get()
        {
            //string query = @"select * from dbo.addstream ORDER BY addstream_pk DESC";

            string query = @"select addstream_pk, stream_name, CONVERT(varchar, creation_date, 105) creation_date,status,createddate,createdby from dbo.addstream ORDER BY addstream_pk DESC";

            ;



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

        [Route("addstream")]
        [HttpPost]
        public JsonResult Post([FromBody] addStream objfaculty )
            {
            string query = @"
                    insert into dbo.addstream 
                    (stream_name,creation_date,status,createddate,createdby)
                    values 
                    (
                    '" + objfaculty.stream_name + @"'
                    ,'" + objfaculty.creation_date + @"'
                    ,'" + objfaculty.status + @"'
                    ,'" + objfaculty.createddate + @"'
                     ,'" + objfaculty.createdby + @"'
                     
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
        //[Route("xyz")]

        //[HttpPost]
        //public JsonResult create(addStream objfaculty)
        //{
        //    string query = @"
        //            insert into dbo.addstream 
        //            (stream_name,creation_date,status,createddate,createdby)
        //            values 
        //            (
        //            '" + objfaculty.addsubstream + @"'
        //            ,'" + objfaculty.doc + @"'
        //            ,'" + objfaculty.status + @"'
        //            ,'" + objfaculty.createddate + @"'
        //             ,'" + objfaculty.createdby + @"'

        //            )
        //            ";
        //    DataTable table = new DataTable();
        //    string sqlDataSource = _configuration.GetConnectionString("ElearningAppCon");
        //    SqlDataReader myReader;
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myReader = myCommand.ExecuteReader();
        //            table.Load(myReader);
        //            myReader.Close();
        //            myCon.Close();
        //        }
        //    }
        //    return new JsonResult("added successfully");

        //}

        [HttpDelete("{addstream_pk}")]
        public JsonResult Delete(int addstream_pk)
        {
            string query = @"
                    delete from dbo.addstream
                    where addstream_pk = " + addstream_pk + @" 
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


        [Route("update")]
        [HttpPut]
        public JsonResult Put(addStream objupdateStream)
        {
            string query = @"
                    update dbo.addstream set 
                    stream_name = '" + objupdateStream.stream_name + @"'
                    ,creation_date = '" + objupdateStream.creation_date + @"'
                    ,status = '" + objupdateStream.status + @"'

                    where addstream_pk = '" + objupdateStream.addstream_pk + @"'
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



    }

}

