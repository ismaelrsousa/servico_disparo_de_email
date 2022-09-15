using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.IO;
using System.Text;
using System.Threading;
using BDContext = ServicoEnvioDeEmail.Utils.BDContext;
using System.Net.Mail;
using System.Net;


namespace ServicoEnvioDeEmail
{
    public partial class ServioEnvioDeEmail : ServiceBase
    {
        public ServioEnvioDeEmail()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ThreadStart start = new ThreadStart(VerificarEmails);
            Thread thread = new Thread(start);

            thread.Start();
        }

        protected override void OnStop()
        {

        }

        public void VerificarEmails()
        {
            while(true)
            {
                Thread.Sleep(5000);

                DataTable data = BDContext.BuscarEnvioEmails();

                string path = "D:/www/logs/log.txt";

                FileInfo file = new FileInfo(path);

                if (!file.Exists)
                    file.Create();

                StreamWriter writer = new StreamWriter(path, true);

                foreach (DataRow item in data.Rows)
                {
                    try
                    {
                        MailMessage email = new MailMessage();
                        
                        email.From = new MailAddress(item["REMETENTE"].ToString());

                        email.To.Add(item["DESTINATARIO"].ToString());

                        email.Subject = item["ASSUNTO"].ToString();

                        email.Body = item["MENSAGEM"].ToString();

                        email.IsBodyHtml = false;

                        SmtpClient smtp = new SmtpClient();
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = true;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.Credentials = new NetworkCredential("ismaelrsousa18@gmail.com", "sroylzewbobxwsko");

                        smtp.Send(email);


                        BDContext.AtualizarStatusEnvio(decimal.Parse(item["ROWID"].ToString()));
                        writer.WriteLine(String.Format("[{0}] Email disparado com sucesso! ID: {1}", DateTime.Now, item["ROWID"]));
                    }
                    catch(Exception ex)
                    {
                        writer.WriteLine(String.Format("[{0}] Erro ao disparar email! ID: {1} \n --{2}", DateTime.Now, item["ROWID"], ex.Message));
                    }                    
                }

                writer.Close();
            }
        }
    }
}
