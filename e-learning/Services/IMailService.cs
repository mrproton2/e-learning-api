using e_learning.Models;
using System.Threading.Tasks;

namespace e_learning.Services
{
    public interface IMailService
    {
      

        Task SendEmailAsync(MailRequest mailRequest);

    }
}
