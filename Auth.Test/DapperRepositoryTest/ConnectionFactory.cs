using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using MySql.Data.MySqlClient;

namespace DapperRepository.Test
{
    public class ConnectionFactory
    {
        IDbConnection conn = null;
        private string localSqliteConnectionString = "Data Source=" + Directory.GetCurrentDirectory() + "\\SimpleDb.sqlite";
        private string mysqlConnectionString = "---";

        public IDbConnection Connection(string connection)
        {


            switch (connection.ToLower())
            {
                case "sqlite":
                    conn = new SQLiteConnection(localSqliteConnectionString);
                    break;
                case "mysql":
                    conn = new MySqlConnection(mysqlConnectionString);
                    break;
                default:
                    throw new Exception("정확한 connection String 명을 입력하세요.");
            }

            return conn;
        }
    }
}