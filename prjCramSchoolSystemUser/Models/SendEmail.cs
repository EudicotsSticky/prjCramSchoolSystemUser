using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjCramSchoolSystemUser.Models
{
    public class SendEmail : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // 建立郵件物件
            MimeMessage emailMessage = new MimeMessage();

            // 寄件人
            emailMessage.From.Add(new MailboxAddress("艾登登補習班", "gbn.tienju@gmail.com"));

            // 收件人
            emailMessage.To.Add(new MailboxAddress(email, email));

            // 郵件標題
            emailMessage.Subject = subject;

            // BodyBuilder建立郵件內容
            BodyBuilder bodyBuilder = new BodyBuilder();

            // 設定HTML內容
            bodyBuilder.HtmlBody = htmlMessage;

            emailMessage.Body = bodyBuilder.ToMessageBody();
         

            using (SmtpClient smtpClient = new SmtpClient())
            {
                // SMTP主機：smtp.gmail.com
                var hostUrl = "smtp.gmail.com";
                // SMTP 埠號：465
                var port = 465;
                // SMTP 安全模式：SSL/TLS
                var useSsl = true;
                // 連接 Mail Server (郵件伺服器網址, 連接埠, 是否使用 SSL)
                await smtpClient.ConnectAsync(hostUrl, port, useSsl);

                // 帳號登入驗證
                await smtpClient.AuthenticateAsync("gbn.tienju@gmail.com", "mofbzaylydmnmxqx");

                await smtpClient.SendAsync(emailMessage);
                await smtpClient.DisconnectAsync(true);

            }
        }
    }
}
