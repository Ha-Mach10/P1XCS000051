using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace P1XCS000051
{
    class Transrator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal string TranslateTableName(string tableName)
        {
            string command = $@"SELECT `japanese`
                                FROM `table_translator`
                                WHERE `type` = 'table' AND `column_name` = '{tableName}';";

            MySQLBase mySQLBase = new MySQLBase();
            DataTable dt = mySQLBase.MySQLDataRequest(command);

            return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// テーブル・カラム名を日本語名に変換し、返すメソッド
        /// </summary>
        /// <param name="columnName">テーブル名</param>
        /// <returns></returns>
        internal string TranslateColumnName(string tableName, string columnName)
        {
            string command = MySQLCommand(tableName, columnName);

            MySQLBase mySQLBase = new MySQLBase();
            DataTable dt = mySQLBase.MySQLDataRequest(command);

            // if (dt.Rows.Count == 0) return "";

            string translateStr = dt.Rows[0][0].ToString();

            return translateStr;
        }

        /// <summary>
        /// テーブル・カラム名を日本語名に変換し、返すメソッド
        /// </summary>
        /// <param name="labeledTextBoxes"></param>
        /// <returns>テーブル・カラム名の日本語名</returns>
        internal List<string> TranslateColumnName(string tableName, List<string> columnNames)
        {
            // 翻訳マスタの「japanese」カラムを格納するリスト
            List<string> translatorStrings = new List<string>();

            MySQLBase mySQLBase = new MySQLBase();
            foreach (string columnName in columnNames)
            {
                // SQL文を生成するメソッドを使用
                string command = MySQLCommand(tableName, columnName);
                DataTable dt = mySQLBase.MySQLDataRequest(command);

                // 「japanese」カラムの値を格納
                translatorStrings.Add(dt.Rows[0][0].ToString());
            }

            return translatorStrings;
        }

        /// <summary>
        /// 日本語名をカラム名に変換し、返すメソッド
        /// </summary>
        /// <param name="labeledTextBoxes"></param>
        /// <returns>テーブル・カラム名の日本語名</returns>
        internal List<string> TranslateJapanese(string tableName, List<string> japaneseNames)
        {
            // 翻訳マスタの「column_name」カラムを格納するリスト
            List<string> translatorStrings = new List<string>();

            MySQLBase mySQLBase = new MySQLBase();
            foreach (string japaneseName in japaneseNames)
            {
                // SQL文を生成するメソッドを使用
                string command = MySQLSelectColumnNameCommnad(tableName, japaneseName);
                DataTable dt = mySQLBase.MySQLDataRequest(command);

                // 「column_name」カラムの値を格納
                translatorStrings.Add(dt.Rows[0][0].ToString());
            }

            return translatorStrings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private string MySQLCommand(string tableName, string columnName)
        {
            string command = $@"SELECT `japanese`
                                FROM `table_translator`
                                WHERE `table_name` = '{tableName}' AND `column_name` = '{columnName}';";

            return command;
        }

        private string MySQLSelectColumnNameCommnad(string tableName, string japaneseName)
        {
            string command = $@"SELECT `column_name`
                                FROM `table_translator`
                                WHERE `table_name` = '{tableName}' AND `japanese` = '{japaneseName}';";

            return command;
        }
    }
}
