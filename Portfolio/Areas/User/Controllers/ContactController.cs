using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.DataAccess.Data;
using Portfolio.Models.Models.ViewModels;
using System.Diagnostics;
using System.Net.Mail;
using System.Web.Helpers;

namespace Portfolio.Areas.User.Controllers
{
    [Area("User")]
    public class ContactController : Controller
    {
        private readonly IConfiguration _conf;
        private readonly ILogger<ContactController> _logger;
        private readonly ApplicationDbContext _db;

        public ContactController(ILogger<ContactController> logger, ApplicationDbContext db, IConfiguration conf)
        {
            _logger = logger;
            _db = db;
            _conf = conf;
        }

        [HttpPost]
        public IActionResult Submit([FromForm] ContactViewModel formData)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                string body = $"<p>Wiadomość od użytkownika <b>{formData.FormUserName}</b> z adresu <i>{formData.FormUserEmail}</i>:</p><br><hr>";
                body+=formData.FormBodyText;

                mail.From = new MailAddress(_conf["Smtp:Sender"]);
                mail.To.Add(_conf["Smtp:Receiver"]);
                mail.Subject = _conf["Smtp:Title"];
                mail.IsBodyHtml = true;
                mail.Body = body;

                smtp.Host = _conf["Smtp:Host"];
                smtp.Port = int.Parse(_conf["Smtp:Port"]);
                smtp.Credentials = new System.Net.NetworkCredential(_conf["Smtp:User"], _conf["Smtp:Password"]);
                smtp.EnableSsl = true;

                smtp.Send(mail);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "ERROR" });
            }
            return Ok("OK");
        }
    }
}