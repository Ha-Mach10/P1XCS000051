using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace P1XCS000051
{
    class MySQLBase
    {
        /// <summary>
        /// MySQLへ接続するための接続文字列生成メソッド
        /// This method is generate of connection string for conection to MySQL server.
        /// </summary>
        /// <returns>接続文字列を返す。/return the connection string.</returns>
        private string MySQLConnectionString()
        {
            var defaultVariant = Properties.Settings.Default;
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = defaultVariant.mySQLServerName;
            builder.UserID = defaultVariant.mySQLUserName;
            builder.Database = defaultVariant.mySQLDatabaseName;
            builder.PersistSecurityInfo = true;
            builder.Password = defaultVariant.mySQLPassword;

            string connentionString = builder.ToString();

            return connentionString;
        }

        /// <summary>
        /// 「SELECT」用MySQLメソッド
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal DataTable MySQLDataRequest(string command)
        {
            DataTable dt = new DataTable();

            string conStr = MySQLConnectionString();
            using (MySqlConnection conn = new MySqlConnection(conStr))
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, conn))
            {
                adapter.Fill(dt);
            }

            return dt;
        }

        /// <summary>
        /// 「INSERT」「DELETE」「UPDATE」用MySQLメソッド
        /// </summary>
        /// <param name="command"></param>
        internal void MySQLDataProcessing(string command)
        {
            string conStr = MySQLConnectionString();

            using (MySqlConnection conn = new MySqlConnection(conStr))
            using (MySqlCommand sqlCommand = new MySqlCommand())
            {
                MySqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    // トランザクション使用時未設定の場合、例外が発生する
                    sqlCommand.Connection = conn;
                    transaction = conn.BeginTransaction();
                    sqlCommand.CommandText = command;
                    var result = sqlCommand.ExecuteNonQuery();
                    if (result != 1)
                    {
                        return;
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // トランザクション有効時、ロールバック処理を行う
                    if (transaction != null) transaction.Rollback();
                }
            }
        }
    }
}
