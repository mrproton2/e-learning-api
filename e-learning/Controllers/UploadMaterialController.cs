using e_learning.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace e_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadMaterialController : ControllerBase
    {

        private readonly string AppDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot2");
        private static List<UploadMaterial> fileDB = new List<UploadMaterial>();
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public UploadMaterialController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }







        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<HttpResponseMessage> PostAsync([FromForm] UploadMaterial model)
        {
            try
            {
                UploadMaterial file = await SaveFileAsync(model.MyFile);

                if (!string.IsNullOrEmpty(file.FilePath))
                {
                    file.substreamname = model.substreamname;
                    file.batchname = model.batchname;
                    file.subjectname = model.subjectname;
                    file.contenttitle = model.contenttitle;
                    file.facultyname = model.facultyname;
                    file.dateofupload = model.dateofupload;
                    file.uploadby = model.uploadby;
              


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

        private async Task<UploadMaterial> SaveFileAsync(IFormFile myFile)
        {
            UploadMaterial file = new UploadMaterial();
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


        private void SaveToDB(UploadMaterial record)
        {
            if (record == null)
                throw new ArgumentNullException($"{nameof(record)}");
            UploadMaterial objupload = new UploadMaterial();
            objupload.substreamname = record.substreamname;
            objupload.batchname = record.batchname;
            objupload.subjectname = record.subjectname;
            objupload.contenttitle = record.contenttitle;
            objupload.facultyname = record.facultyname;
            objupload.dateofupload = record.dateofupload;
            objupload.uploadby = record.uploadby;
       
            objupload.FilePath = record.FilePath;
            objupload.FileName = record.FileName;
            objupload.FileExtension = record.FileFormat;
            objupload.MimeType = record.ContentType;
            string query = @"
                    insert into dbo.upload_material 
                    (substreamname,batchname,subjectname,contenttitle,facultyname,
                    dateofupload,uploadby,FilePath,FileName,FileExtension,MimeType)
                    values 
                    (
                    
                    '" + objupload.substreamname + @"'
                    ,'" + objupload.batchname + @"'
                    ,'" + objupload.subjectname + @"'
                    ,'" + objupload.contenttitle + @"'
                    ,'" + objupload.facultyname + @"'
                    ,'" + objupload.dateofupload + @"'
                    ,'" + objupload.uploadby + @"'
                    ,'" + objupload.FilePath + @"'
                    ,'" + objupload.FileName + @"'
                    ,'" + objupload.FileExtension + @"'
                    ,'" + objupload.MimeType + @"'
                  
          
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


    }
}
