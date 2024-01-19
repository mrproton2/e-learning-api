using Microsoft.AspNetCore.Http;

namespace e_learning.Models
{
    public class ClassProfileModel
    {

        public string class_pk { get; set; }
        public string classname { get; set; }
        public string date_of_creation { get; set; }
        public string slogan { get; set; }

        public IFormFile MyFile { get; set; }

        public string branchname { get; set; }
        public string address { get; set; }
        public string contact { get; set; }
        public string Alternative_contact { get; set; }
        public string email { get; set; }
        public string panno { get; set; }
        public string aadharno { get; set; }
        public string gstno { get; set; }
        public string createddate { get; set; }
        public string createdby { get; set; }

        //public class fileupload
        //{
        //    public IFormFile logo { get; set; }
        //}

        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string MimeType { get; set; }
        public string FilePath { get; set; }

    }
}
