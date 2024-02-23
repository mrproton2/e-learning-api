using Microsoft.AspNetCore.Http;

namespace e_learning.Models
{
    public class UploadMaterial
    {


        public string substreamname { get; set; }
        public string batchname { get; set; }
        public string subjectname { get; set; }
        public string contenttitle { get; set; }
        public IFormFile MyFile { get; set; }
        public string facultyname { get; set; }
        public string dateofupload { get; set; }
        public string uploadby { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string MimeType { get; set; }
        public string FilePath { get; set; }
        public int Id { get; set; }
        public string FileFormat { get; set; }
        public string ContentType { get; set; }
        public string AltText { get; set; }
        
        public string Description { get; set; }
    }
}
