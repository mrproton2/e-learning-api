using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace e_learning.Models
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string studentname { get; set; }
        public string studentcontactno { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
