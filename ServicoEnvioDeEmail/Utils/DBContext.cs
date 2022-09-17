using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Configuration;

namespace ServicoEnvioDeEmail.Utils
{
    internal class DBContext
    {
        public static SQLiteConnection DBConnection()
        {
            string dbPath = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            SQLiteConnection con = new SQLiteConnection(dbPath);

            con.Open();
            return con;
        }

        public static DataTable BuscarEnvioEmails()
        {
            try
            {
                using(var cmd = DBConnection().CreateCommand())
                {
                    DataTable data = new DataTable();

                    cmd.CommandText = "SELECT * FROM ENVIO_EMAIL WHERE ENVIADO = 0";
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(data);

                    return data;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static void AtualizarStatusEnvio(decimal id)
        {
            try
            {
                using (var cmd = DBConnection().CreateCommand())
                {
                    DataTable data = new DataTable();

                    cmd.CommandText = String.Format("UPDATE ENVIO_EMAIL SET ENVIADO = 1 WHERE ROWID = {0}", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
