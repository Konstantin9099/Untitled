using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Untitled
{
    internal class DBMySQLUtils
    {
        public static MySqlConnection
     GetDBConnection(string host, string database, string user, string password)
        {
            String connString = "Server=" + host + "; database=" + database + "; user=" + user + "; password=" + password + ";";
            // Создаем объект MySqlConnection с именем conn и передаем ему строку для подключения connString:
            MySqlConnection conn = new MySqlConnection(connString);
            return conn;
        }

    }
}
