using CleanArchTemplate.Shared.Requests.Mail;

using System.Threading.Tasks;

namespace CleanArchTemplate.Application.Common.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}