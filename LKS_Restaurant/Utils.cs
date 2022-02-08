using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LKS_Restaurant
{
    class Utils
    {
        public static string conn = @"Data Source=desktop-00eposj;Initial Catalog=LKS_Restaurant;Integrated Security=True";
    }

    class Model
    {
        public static int id { set; get; }
        public static string name { set; get; }
        public static string position { set; get; }
        public static string password { set; get; }
        public static string hp { set; get; }
        public static string email { set; get; }
    }

    class Encrypt
    {
        public static string enc(string data)
        {
            using(SHA256Managed managed = new SHA256Managed())
            {
                return Convert.ToBase64String(managed.ComputeHash(Encoding.UTF8.GetBytes(data)));
            }
        }
    }

    class Command
    {
        public static DataTable GetData(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlDataAdapter adapter = new SqlDataAdapter(com, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public static void exec(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlCommand command = new SqlCommand(com, connection);
            connection.Open();
            command.ExecuteNonQuery();

        }
    }
}
