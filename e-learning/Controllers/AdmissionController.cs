using e_learning.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System;
using System.Collections.Generic;
using e_learning.Services;
using System.Threading.Tasks;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using System.IO;
using MimeKit;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Text.Unicode;

namespace e_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmissionController : ControllerBase
    {

        private readonly IMailService mailService;
      
       

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public AdmissionController(IConfiguration configuration, IWebHostEnvironment env, IMailService mailService)
        {
            _configuration = configuration;
            _env = env;
            this.mailService = mailService;
        }
   


        [Route("postAdmissionForm")]
        [HttpPost]
        public JsonResult Post([FromBody] admissionForm objaadmissionForm)
        {
            //string firstNamePrefix = objaadmissionForm.studentname.Length >= 3 ? objaadmissionForm.studentname.Substring(0, 4) : objaadmissionForm.studentname;
            //string username = $"{firstNamePrefix.ToLower()}{objaadmissionForm.studentcontactno.ToLower()}";
            //string password = GenerateRandomPassword();
              
                    


            string query = @"
                    insert into dbo.admission_details 
                    (studentname,dob,address,studentcontactno,studentemail,gender,
                    previousQualification,school_college_name,studentaadharno,parentName,occupation,income,parentemail,parentcontactNo,
                    stream_name,substream_name,batchName,dateofadmission,totalFees,discount,gst,feeswithoutgst,feeswithgst,payableamount,
                    paymentType,numberOfInstallments,installmentamount,installmentdate,paymentvia,paymentMode,offlineCollectedBy,createddate,createdby,Payingamount)
                    values 
                    (
                    
                    '" + objaadmissionForm.studentname + @"'
                    ,'" + objaadmissionForm.dob + @"'
                    ,'" + objaadmissionForm.address + @"'
                    ,'" + objaadmissionForm.studentcontactno + @"'
                    ,'" + objaadmissionForm.studentemail + @"'
                    ,'" + objaadmissionForm.gender + @"'
                    ,'" + objaadmissionForm.previousQualification + @"'
                    ,'" + objaadmissionForm.school_college_name + @"'
                    ,'" + objaadmissionForm.studentaadharno + @"'
                    ,'" + objaadmissionForm.parentName + @"'
                    ,'" + objaadmissionForm.occupation + @"'
                    ,'" + objaadmissionForm.income + @"'
                    ,'" + objaadmissionForm.parentemail + @"'   
                    ,'" + objaadmissionForm.parentcontactNo + @"'
                    ,'" + objaadmissionForm.stream_name + @"'
                    ,'" + objaadmissionForm.substream_name + @"'
                    ,'" + objaadmissionForm.batchName + @"'
                    ,'" + objaadmissionForm.dateofadmission + @"'
                    ,'" + objaadmissionForm.totalFees + @"'
                    ,'" + objaadmissionForm.discount + @"'
                    ,'" + objaadmissionForm.gst + @"'
                    ,'" + objaadmissionForm.feeswithoutgst + @"'
                    ,'" + objaadmissionForm.feeswithgst + @"'
                    ,'" + objaadmissionForm.payableamount + @"'
                    ,'" + objaadmissionForm.paymentType + @"'
                    ,'" + objaadmissionForm.numberOfInstallments + @"'
                    ,'" + objaadmissionForm.installmentamount + @"'
                    ,'" + objaadmissionForm.installmentdate + @"'
                    ,'" + objaadmissionForm.paymentvia + @"'
                    ,'" + objaadmissionForm.paymentMode + @"'
                    ,'" + objaadmissionForm.offlineCollectedBy + @"'
                    ,'" + objaadmissionForm.createddate + @"'
                    ,'" + objaadmissionForm.createdby + @"'
                     ,'" + objaadmissionForm.Payingamount + @"'
                     
                    )";

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


       [HttpPost("generateIDpassword")]
        public async Task<IActionResult> SendMall(StudentIDPasswordModel objaadmissionForm)
            {

            string firstNamePrefix = objaadmissionForm.studentname.Length >= 3 ? objaadmissionForm.studentname.Substring(0, 2) : objaadmissionForm.studentname;
            var user_name = $"{firstNamePrefix.ToLower()}{objaadmissionForm.studentcontactno.ToLower()}";
            string user_password = GenerateRandomPassword();
            string query = @"
                    insert into dbo.users 
                    (user_name,user_password,admissionid_pk,batchName,idpassflag,RoleID,contact)
                    values  
                    (
                    '" + user_name + @"'
                    ,'" + user_password + @"'
                    ,'" + objaadmissionForm.admissionid_pk + @"' 
                    ,'" + objaadmissionForm.batchName + @"'
                    ,'" + objaadmissionForm.idpassflag + @"' 
                    ,'" + objaadmissionForm.RoleID + @"'
                    ,'" + objaadmissionForm.studentcontactno + @"'                   
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



           
            MailRequest mailRequest= new MailRequest();
            mailRequest.ToEmail = objaadmissionForm.studentemail;
            mailRequest.Subject = "Your ID password";
            mailRequest.Body = $"Your User ID is {user_name} And Your Password is {user_password}.";
           
            await mailService.SendEmailAsync(mailRequest);
            return Ok();

        }

        private string GenerateRandomPassword(int length = 8)
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






        [HttpPost("generateparentsIDpassword")]
        public async Task<IActionResult> ParentsIDpass(StudentIDPasswordModel objaadmissionForm)
        {

            string firstNamePrefix = objaadmissionForm.parentName.Length >= 3 ? objaadmissionForm.parentName.Substring(0, 2) : objaadmissionForm.parentName;
            var user_name = $"{firstNamePrefix.ToLower()}{objaadmissionForm.parentcontactNo.ToLower()}";
            string user_password = GenerateRandomPassword();
            string query = @"
                    insert into dbo.users 
                    (user_name,user_password,admissionid_pk,batchName,idpassflag,RoleID,contact)
                    values  
                    (
                    '" + user_name + @"'
                    ,'" + user_password + @"'
                    ,'" + objaadmissionForm.admissionid_pk + @"' 
                    ,'" + objaadmissionForm.batchName + @"'
                    ,'" + objaadmissionForm.idpassflag + @"' 
                     ,'" + objaadmissionForm.RoleIDParents + @"'
                    ,'" + objaadmissionForm.parentcontactNo + @"'                   
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
            MailRequest mailRequest = new MailRequest();
            mailRequest.ToEmail = objaadmissionForm.parentemail;
            mailRequest.Subject = "Your ID password";
            mailRequest.Body = $"Your User ID is {user_name} And Your Password is {user_password}.";

            await mailService.SendEmailAsync(mailRequest);
            return Ok();

        }

    






























        //[Route("generatepdf")]
        //[HttpGet]
        //public async Task<IActionResult> generatepdf(StudentIDPasswordModel objaadmissionForm)
        //{
        //    var document = new PdfDocument();
        //    string htmlcontent = "<html";
        //    htmlcontent += "<head>";
        //    htmlcontent += " <style>";
        //    htmlcontent += "    body {font-family: Arial, sans-serif;}";
        //    htmlcontent += "   .container {width: 100%;margin: 2px ;border: 2px solid #000;padding: 10px;border-radius: 5px;}";
        //    htmlcontent += " .header {text-align: center;font-size: 12px;font-weight: bold;}";
        //    htmlcontent += "    .box {border: 1px solid #000;padding: 2px;}";
        //    htmlcontent += "   .box-header {font-size: 8px;font-weight: bold;}";
        //    htmlcontent += " </style>";
        //    htmlcontent += "<title>Invoice</title>";
        //    htmlcontent += "</head>";
        //    htmlcontent += "<body>";
        //    htmlcontent += "  <div class='container'> ";
        //    htmlcontent += "    <div class='header'>Invoice</div>";
        //    htmlcontent += " <div class='box'>";

        //    htmlcontent += "      <div class=content'>";
        //    htmlcontent += "        <p><strong>Class Name:</strong> National classes</p>";
        //    htmlcontent += "        <p><strong>Address:</strong> Ghatkopar east</p>";
        //    htmlcontent += "        <p><strong>Contact Number:</strong>9326196417</p>";
        //    htmlcontent += "      </div>";
        //    htmlcontent += "    </div>";
        //    htmlcontent += "  <div class='box'>";

        //    htmlcontent += " <div class='content'>";
        //    htmlcontent += "<p><strong>Student Name:</strong>" + objaadmissionForm.studentname + "</p>";
        //    htmlcontent += "    <p><strong>Date of Birth:</strong> " + objaadmissionForm.dob + "</p>";
        //    htmlcontent += "   <p><strong>Address:</strong>" + objaadmissionForm.address + "</p>";
        //    htmlcontent += "  <p><strong>Contact Number:</strong> " + objaadmissionForm.studentcontactno + "</p>";
        //    htmlcontent += "  <p><strong>Email ID:</strong>" + objaadmissionForm.studentemail + "</p>";
        //    htmlcontent += "     <p><strong>Gender:</strong> " + objaadmissionForm.gender + "</p>";
        //    htmlcontent += "  <p><strong>Aadhar Card Number:</strong> " + objaadmissionForm.studentaadharno + "</p>";
        //    htmlcontent += "  <p><strong>Parents Name:</strong> " + objaadmissionForm.parentName + "</p>";
        //    htmlcontent += "  <p><strong>Parents Contact Number:</strong> " + objaadmissionForm.parentcontactNo + "</p>";
        //    htmlcontent += "    <p><strong>Substream Name:</strong> " + objaadmissionForm.substream_name + "</p>";
        //    htmlcontent += "    <p><strong>Batch Name:</strong> " + objaadmissionForm.batchName + "</p>";
        //    htmlcontent += "   <p><strong>Date of Admission:</strong> " + objaadmissionForm.dateofadmission + "</p>";
        //    htmlcontent += "   </div>";
        //    htmlcontent += "  </div>";
        //    htmlcontent += "    <div class='box'>";
        //    htmlcontent += "    <div class='content'>";
        //    htmlcontent += "      <p><strong>Total Fees of Course:</strong>" + objaadmissionForm.totalFees + "</p>";
        //    htmlcontent += "      <p><strong>Discount %:</strong> " + objaadmissionForm.discount + "%</p>";
        //    htmlcontent += "       <p><strong>GST:</strong> 18%</p>";
        //    htmlcontent += "       <p><strong>Fees under GST:</strong> " + objaadmissionForm.feeswithgst + "</p>";
        //    htmlcontent += "       <p><strong>Total Payable Amount:</strong> " + objaadmissionForm.payableamount + "</p>";
        //    htmlcontent += "        <p><strong>Type of Payment:</strong> " + objaadmissionForm.paymentType + "</p>";
        //    htmlcontent += "       <p><strong>Installment Amount and Date:</strong> " + objaadmissionForm.address + "</p>";
        //    htmlcontent += "       <p><strong>Payment Mode:</strong> " + objaadmissionForm.paymentMode + "</p>";
        //    htmlcontent += "        <p><strong>Paying Amount:</strong>" + objaadmissionForm.Payingamount + "</p>";
        //    htmlcontent += "      </div>";
        //    htmlcontent += "   </div>";
        //    htmlcontent += "  </div>";
        //    htmlcontent += "</body>";
        //    htmlcontent += "</html>";

        //    PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);
        //    byte[]? response = null;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        document.Save(ms);
        //        response = ms.ToArray();
        //    }
        //    string FileName = "invoice" + ".pdf";

        //    return File(response, "application/pdf", FileName);


        //}



        [Route("getAdmissionformdetails")]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from admission_details a Left join users b on a.admissionid_pk=b.admissionid_pk";

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
