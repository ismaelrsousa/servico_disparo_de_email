using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using DBContext = ServicoEnvioDeEmail.Utils.DBContext;
using LogContext = ServicoEnvioDeEmail.Utils.LogContext;
using SmtpContext = ServicoEnvioDeEmail.Utils.SmtpContext;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Collections.Specialized;


namespace ServicoEnvioDeEmail
{
    public partial class ServicoEnvioDeEmail : ServiceBase
    {
        public ServicoEnvioDeEmail()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ThreadStart start = new ThreadStart(VerificarEmails);
            Thread thread = new Thread(start);

            LogContext.WriteLog(String.Format("[{0}] Serviço Iniciado", DateTime.Now));

            thread.Start();
        }

        protected override void OnStop()
        {
            LogContext.WriteLog(String.Format("[{0}] Serviço Parado", DateTime.Now));
        }

        public void VerificarEmails()
        {
            while(true)
            {
                try
                {
                    Thread.Sleep(5000);

                    DataTable data = DBContext.BuscarEnvioEmails();

                    foreach (DataRow item in data.Rows)
                    {
                        try
                        {
                            SmtpContext.EnviarEmail(item["ASSUNTO"].ToString(), item["MENSAGEM"].ToString(), item["DESTINATARIO"].ToString());

                            DBContext.AtualizarStatusEnvio(decimal.Parse(item["ROWID"].ToString()));

                            LogContext.WriteLog(String.Format("[{0}] Email disparado com sucesso! ID: {1}", DateTime.Now, item["ROWID"]));
                        }
                        catch (Exception ex)
                        {
                            LogContext.WriteLog(String.Format("[{0}] Erro ao disparar email! \n --{1}", DateTime.Now, ex.Message));
                        }
                    }
                }
                catch (Exception ex) {
                    LogContext.WriteLog(String.Format("[{0}] Erro ao disparar email! \n --{1}", DateTime.Now, ex.Message));
                }
            }
        }
    }
}
