using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ServicoEnvioDeEmail.Utils
{
    internal class LogContext
    {
        static string path = ConfigurationManager.AppSettings["logsPath"];
        static public StreamWriter writer;
        public static void WriterInit()
        {
            writer = new StreamWriter(path, true);
        }

        public static void WriteLog(string message)
        {
            WriterInit();

            FileInfo file = new FileInfo(path);

            if (!file.Exists)
                file.Create();

            writer.WriteLine(message);

            writer.Close();
        }
    }
}
