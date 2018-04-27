using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace BackEnd.BackService
{
    public class EmailService
    {
        public async Task SendEmailAsync(Models.Booking Client, string Email)
        {

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Новый клиент", "nikiti498@gmail.com"));//TODO: Change address
            emailMessage.To.Add(new MailboxAddress("", Email));
            emailMessage.Subject = Client.Name;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = Client.Phone
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("nikiti498@gmail.com", "pidorsksisa");//TODO: Add mail
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
