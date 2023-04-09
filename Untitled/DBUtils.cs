using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Untitled
{
    internal class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "194.67.74.139";  // Имя хоста.
            string database = "Untitled"; // Вводим название базы данных, имеющейся в программе MySQL.
            string user = "Konstantin"; // Логин в MySQL.
            string password = "0000XXXX"; // Пароль в MySQL.
            return DBMySQLUtils.GetDBConnection(host, database, user, password);
        }
    }
}
