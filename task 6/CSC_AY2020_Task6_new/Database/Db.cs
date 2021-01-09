using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace CSC_AY2020_Task6_new.Database
{
    public class Db
    {

        public SqlConnection conn { get; set; }
        public Db()
        {


            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "";
            builder.UserID = "";
            builder.Password = "";
            builder.InitialCatalog = "";
            conn = new SqlConnection(builder.ConnectionString);

            if (IsSQLOnline() != true)
            {
                conn = null;
            }
        }


        public bool IsSQLOnline()
        {

            String sql = "SELECT 1";
            bool res = false;
            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        res = true;
                    }
                    else
                    {
                        res = false;
                    }
                }
            }
            conn.Close();
            return res;
        }

        


        }
}
