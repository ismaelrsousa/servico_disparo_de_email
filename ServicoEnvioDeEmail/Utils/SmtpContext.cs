using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServicoEnvioDeEmail.Utils
{
    internal class SmtpContext
    {
        public static SmtpClient smtp = new SmtpClient();
        public static void InitSmtp()
        {
            NameValueCollection smtpConfig = (NameValueCollection)ConfigurationManager.GetSection("smtpConfig");

            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Host = smtpConfig["host"];
            smtp.Port = int.Parse(smtpConfig["port"]);
            smtp.Credentials = new NetworkCredential(smtpConfig["mail"], smtpConfig["mailPass"]);
        }

        public static void EnviarEmail(string assunto, string mensagem, string destinatario)
        {
            InitSmtp();

            NameValueCollection smtpConfig = (NameValueCollection)ConfigurationManager.GetSection("smtpConfig");

            MailMessage email = new MailMessage();

            email.From = new MailAddress(smtpConfig["mail"]);

            email.To.Add(destinatario);

            email.Subject = assunto;

            email.Body = mensagem;

            email.IsBodyHtml = false;

            smtp.Send(email);
        }
    }
}
