using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Auth.Services
{
    public class ConnectionFactory
    {
        IDbConnection conn = null;
        private string localSqliteConnectionString = "Data Source=" + Directory.GetCurrentDirectory() + "\\SimpleDb.sqlite";
        public IDbConnection Connection(string connection)
        {


            switch (connection.ToLower())
            {
                case "sqlite":
                    conn = new SQLiteConnection(localSqliteConnectionString);
                    break;
                default:
                    throw new Exception("정확한 connection String 명을 입력하세요.");
            }

            return conn;
        }
    }
}