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
            builder.DataSource = "csc-ay2020.database.windows.net";
            builder.UserID = "csc_db_admin";
            builder.Password = "Password1!";
            builder.InitialCatalog = "csc_db";
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
