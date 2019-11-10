using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFPTClient
{
    public class DataAccess
    {
        public DBResult ExecuteNonQuery(FileContent xmlData)
        {
            DBResult result = new DBResult();

            using (SqlConnection conn = new SqlConnection($"Server={ApplicationSettings.DBHost};DataBase={ApplicationSettings.DBName};User ID={ApplicationSettings.DBUsername};Password={ApplicationSettings.DBUserPassword};"))
            {
                conn.Open();                
                SqlCommand cmd = new SqlCommand("spAddNexusAuditOrder", conn);                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@XmlData", xmlData.XmlData));
                cmd.Parameters.Add(new SqlParameter("@PdfObject", xmlData.PdfString));
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        result.VendorId = Convert.ToInt32(dr["VendorId"]);
                        result.ControlId = Convert.ToInt32(dr["ControlId"]);
                    }
                }
                conn.Close();
            }

            return result;
        }
    }
}
