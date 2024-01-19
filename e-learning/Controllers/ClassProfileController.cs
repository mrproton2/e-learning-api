using e_learning.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Generic;
    using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;


using Microsoft.AspNetCore.Cors;
using System.Linq;




namespace e_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassProfileController : ControllerBase
    {
        private readonly string AppDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        private static List<FileRecord> fileDB = new List<FileRecord>();

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public ClassProfileController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<HttpResponseMessage> PostAsync([FromForm] ClassProfileModel model)
        {
            try
            {
                FileRecord file = await SaveFileAsync(model.MyFile);

                if (!string.IsNullOrEmpty(file.FilePath))
                {
                     file.classname = model.classname;
                    file.date_of_creation = model.date_of_creation;
                    file.slogan = model.slogan;
                    file.branchname = model.branchname;
                    file.address = model.address;
                    file.contact = model.contact;
                    file.Alternative_contact = model.Alternative_contact;
                    file.email = model.email;
                    file.panno = model.panno;
                    file.aadharno = model.aadharno;
                    file.createddate = model.createddate;
                    file.gstno = model.gstno;
                    file.createdby = model.createdby;


                    fileDB.Add(file);  
                    SaveToDB(file);
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message),
                };
            }
        }

        private async Task<FileRecord> SaveFileAsync(IFormFile myFile)
        {
            FileRecord file = new FileRecord();
            if (myFile != null)
            {
                if (!Directory.Exists(AppDirectory))
                    Directory.CreateDirectory(AppDirectory);

                var fileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(myFile.FileName);
                var path = Path.Combine(AppDirectory, fileName);
                file.Id = fileDB.Count() + 1;
                file.FilePath = path;
                file.FileName = fileName;
                file.FileFormat = Path.GetExtension(myFile.FileName);
                file.ContentType = myFile.ContentType;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await myFile.CopyToAsync(stream);
                }

                return file;
            }
            return file;
        }


        private void SaveToDB(FileRecord record)
        {
            if (record == null)
                throw new ArgumentNullException($"{nameof(record)}");
            ClassProfileModel objclassForm = new ClassProfileModel();
            objclassForm.classname = record.classname;
            objclassForm.date_of_creation = record.date_of_creation;
            objclassForm.slogan = record.slogan;
            objclassForm.branchname = record.branchname;
            objclassForm.address = record.address;
            objclassForm.contact = record.contact;
            objclassForm.Alternative_contact = record.Alternative_contact;
            objclassForm.email = record.email;
            objclassForm.panno = record.panno;
            objclassForm.aadharno = record.aadharno;
            objclassForm.createddate = record.createddate;
            objclassForm.gstno = record.gstno;
            objclassForm.createdby = record.createdby;
            objclassForm.FilePath = record.FilePath;
            objclassForm.FileName = record.FileName;
            objclassForm.FileExtension = record.FileFormat;
            objclassForm.MimeType = record.ContentType;

            
            string query = @"
                    insert into dbo.class_profile 
                    (classname,date_of_creation,slogan,branchname,address,
                    contact,email,
                    panno,aadharno,gstno,createddate,createdby,FilePath,FileName,FileExtension,MimeType)
                    values 
                    (
                    
                    '" + objclassForm.classname + @"'
                    ,'" + objclassForm.date_of_creation + @"'
                    ,'" + objclassForm.slogan + @"'
                    ,'" + objclassForm.branchname + @"'
                    ,'" + objclassForm.address + @"'
                    ,'" + objclassForm.contact + @"'
                    ,'" + objclassForm.email + @"'
                    ,'" + objclassForm.panno + @"'
                    ,'" + objclassForm.aadharno + @"'
                    ,'" + objclassForm.gstno + @"'
                    ,'" + objclassForm.createddate + @"'
                    ,'" + objclassForm.createdby + @"'
                    ,'" + objclassForm.FilePath + @"'
                    ,'" + objclassForm.FileName + @"'
                    ,'" + objclassForm.FileExtension + @"'
                    ,'" + objclassForm.MimeType + @"'
                  
          
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

            //return new JsonResult("added successfully");

        }



        //[HttpGet]
        //public List<FileRecord> GetAllFiles()
        //{
        //    //getting data from inmemory obj
        //    //return fileDB;
        //    //getting data from SQL DB
        //    return dBContext.FileData.Select(n => new FileRecord
        //    {
        //        Id = n.Id,
        //        ContentType = n.MimeType,
        //        FileFormat = n.FileExtension,
        //        FileName = n.FileName,
        //        FilePath = n.FilePath
        //    }).ToList();
        //}



        [Route("profiledata")]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from dbo.class_profile  ORDER BY class_pk DESC";
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

            return new JsonResult(table);

        }



    }
}
