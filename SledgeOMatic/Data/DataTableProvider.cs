using Microsoft.Extensions.Configuration;
using System; 
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog;
namespace SOM
{
    public class DataTableProvider
    {
        private IConfiguration config;
        private Serilog.ILogger logger;

        public DataTableProvider(IConfiguration config)
        {
            this.config = config;
        }

        public DataTableProvider(IConfiguration _config
            , Serilog.ILogger _logger)
        {
            this.config = _config;
            this.logger = _logger;
        }
        public DataTable Provide(string sql)
        {
            DataTable dt = new DataTable();
            string connstr = null;
            try
            {
                connstr = config.GetSection("ConnectionStrings")["default"];
            }
            catch (Exception ex)
            {
                logger.Warning(ex, "Connection string does not exist");
            } 
            if (string.IsNullOrWhiteSpace(connstr)){
                logger.Warning( "Connection string does not exist");
                return dt;
            } 
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            try
            { 
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                logger.Warning(ex, "SqlConnection exception");
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
    }
}
