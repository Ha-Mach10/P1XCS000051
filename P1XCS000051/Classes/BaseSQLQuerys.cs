using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace P1XCS000051
{
    class BaseSQLQuerys
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mgtText"></param>
        /// <returns></returns>
        public DataTable SelectTableManagerQuery(string mgtText)
        {
            try
            {
                string command = $@"SELECT *
                                    FROM `manager_codes`
                                    WHERE `develop_number`
                                    LIKE '{mgtText}%';";
                DataBaseConnectionStringBuider buider = new DataBaseConnectionStringBuider();
                string conStr = buider.SchemaManegerConnectionString();
                DataTable dt = new DataTable();
                using (MySqlConnection conn = new MySqlConnection(conStr))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, conStr))
                {
                    adapter.Fill(dt);
                }

                return TableColumnsNameTranslator(dt);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// テーブルのカラム「id」のレコード数を数え、返す
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal int SelectCount(string tableName)
        {
            string countCommand = $@"SELECT COUNT(DISTINCT id) FROM `{tableName}`;";

            DataBaseConnectionStringBuider buider = new DataBaseConnectionStringBuider();
            string conStr = buider.SchemaManegerConnectionString();

            using (MySqlConnection conn = new MySqlConnection(conStr))
            using (MySqlCommand command = new MySqlCommand(countCommand, conn))
            {
                try
                {
                    conn.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeHead"></param>
        /// <returns></returns>
        private string ManagerCodesTableRowsCount(string codeHead)
        {
            try
            {
                DataBaseConnectionStringBuider buider = new DataBaseConnectionStringBuider();
                string conStr = buider.SchemaManegerConnectionString();
                string command = $@"SELECT *
                                    FROM `manager_codes`
                                    WHERE `develop_number` LIKE '{codeHead}%';";

                DataTable dt = new DataTable();
                using (MySqlConnection conn = new MySqlConnection(conStr))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, conn))
                {
                    adapter.Fill(dt);
                }

                int rowsCount = dt.Rows.Count + 1;
                string rowsCountStr = rowsCount.ToString("000000");
                return codeHead + rowsCountStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "取得失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }
        /// <summary>
        /// 「使用用途」欄に入力したコントロールが「ComboBox」の場合、入力値からデータベースへ問い合わせ、値を取得する。
        /// </summary>
        /// <param name="comboItem"></param>
        /// <returns></returns>
        private string SingleSelectTableQuery(string comboItem)
        {
            try
            {
                string command = $@"SELECT `use_name_en`
                                    FROM `manager_use_application`
                                    WHERE `use_name_jp` = '{comboItem}'";
                DataBaseConnectionStringBuider buider = new DataBaseConnectionStringBuider();
                string conStr = buider.SchemaManegerConnectionString();
                DataTable dt = new DataTable();
                using (MySqlConnection conn = new MySqlConnection(conStr))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, conn))
                {
                    adapter.Fill(dt);
                }

                string getItem = dt.Rows[0][0].ToString();
                return getItem;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 引数へ入力されたDataTableのカラム名を英語から日本語へ翻訳するメソッド
        /// 「manager_codes」テーブル専用
        /// </summary>
        /// <param name="tableArg"></param>
        /// <returns></returns>
        private DataTable TableColumnsNameTranslator(DataTable tableArg)
        {
            DataTable dt = new DataTable();
            dt = tableArg;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var column = dt.Columns[i];
                switch (i)
                {
                    case 1:
                        column.ColumnName = "開発番号";
                        break;
                    case 2:
                        column.ColumnName = "開発名称";
                        break;
                    case 3:
                        column.ColumnName = "コードネーム";
                        break;
                    case 4:
                        column.ColumnName = "作成日";
                        break;
                    case 5:
                        column.ColumnName = "使用用途";
                        break;
                    case 6:
                        column.ColumnName = "バージョン";
                        break;
                    case 7:
                        column.ColumnName = "改定日";
                        break;
                    case 8:
                        column.ColumnName = "流用番号";
                        break;
                    case 9:
                        column.ColumnName = "旧番号";
                        break;
                    case 10:
                        column.ColumnName = "新番号";
                        break;
                    case 11:
                        column.ColumnName = "継承元番号";
                        break;
                    case 12:
                        column.ColumnName = "説明";
                        break;
                    case 13:
                        column.ColumnName = "摘要";
                        break;
                }
            }

            return dt;
        }
        /// <summary>
        /// MySQLの「manager」内テーブルを全て列挙してDataTableで返す
        /// </summary>
        /// <returns>Manager-tables in DataTable</returns>
        internal DataTable ShowTableMasterQuery()
        {
            try
            {
                string command = $@"SHOW TABLES";
                DataBaseConnectionStringBuider buider = new DataBaseConnectionStringBuider();
                string conStr = buider.SchemaManegerConnectionString();
                DataTable dt = new DataTable();
                using (MySqlConnection conn = new MySqlConnection(conStr))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, conStr))
                {
                    adapter.Fill(dt);
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal DataTable SelectTableMasterQuery(string tableName)
        {
            try
            {
                string command = $@"SELECT *
                                    FROM `{tableName}`";
                DataBaseConnectionStringBuider buider = new DataBaseConnectionStringBuider();
                string conStr = buider.SchemaManegerConnectionString();
                DataTable dt = new DataTable();
                using (MySqlConnection conn = new MySqlConnection(conStr))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, conn))
                {
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal string mgtInsertQueryCommand(string codeHead, string devName, string useApp1, string useApp2, string diversionNum,
                                              string oldNumber, string newNumber, string explanation, string summary, Control setControl)
        {
            string useApp = "";
            if (setControl is ComboBox)
            {
                string useAppItem1 = SingleSelectTableQuery(useApp1);
                string useAppItem2 = SingleSelectTableQuery(useApp2);
                useApp = useAppItem1 + useAppItem2;
            }
            else
            {
                useApp = useApp1;
            }

            string createDate = useApp2;
            string devCode = codeHead;
            // setControlの型がComboBoxのとき、正規の号番追加が行われる
            // setControlがLabelでない場合
            if (!(setControl is Label))
            {
                createDate = DateTime.Today.ToShortDateString();
                devCode = ManagerCodesTableRowsCount(codeHead);
            }

            string command = $@"INSERT INTO `manager_codes` (`develop_number`, `develop_name`, `create_date`, `use_applications`, `diversion_number`, `old_number`, `new_number`, `explanation`, `summary`)
                                VALUES ('{devCode}', '{devName}', '{createDate}', '{useApp}', '{diversionNum}', '{oldNumber}', '{newNumber}', '{explanation}', '{summary}');";
            return command;
        }


        //--------------------------------------------------------
        // 「MasterEditor.cs」のクエリ用メソッド
        //--------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal DataTable SelectCommandQuery(string command)
        {
            try
            {
                DataBaseConnectionStringBuider buider = new DataBaseConnectionStringBuider();
                string conStr = buider.SchemaManegerConnectionString();

                using (MySqlConnection conn = new MySqlConnection(conStr))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// マスタのInsertクエリ文字列を返すメソッド
        /// </summary>
        /// <param name="tableName">MySQLのテーブル名</param>
        /// <param name="value">MySQLのテーブルのIDを除く各カラム名</param>
        /// <returns></returns>
        internal string MasterInsertIntoQuery(string tableName, string[] value)
        {
            string firstColumn = "";
            string secondColumn = "";
            string thirdColumn = "";

            switch (tableName)
            {
                case "manager_language_type":
                    firstColumn = "language_type";
                    secondColumn = "type_code";
                    thirdColumn = "script_type";
                    break;
                case "manager_develop_type":
                    firstColumn = "develop_type";
                    secondColumn = "type_code";
                    thirdColumn = "script_type";
                    break;
                case "manager_use_application":
                    firstColumn = "use_name_jp";
                    secondColumn = "use_name_en";
                    thirdColumn = "sign";
                    break;
                case "manager_project_master":
                    firstColumn = "project_number";
                    secondColumn = "project_name";
                    thirdColumn = "project_explanation";
                    break;
                case "classifications_master":
                    firstColumn = "classifications_name";
                    secondColumn = "classifications";
                    thirdColumn = "classifications_sign";
                    break;
            }

            int valueLen = value.Length;
            string command;
            if (valueLen == 4)
            {
                command = $@"INSERT INTO `{tableName}` (`id`, `{firstColumn}`, `{secondColumn}`, `{thirdColumn}`)
                             VALUES ('{value[0]}', '{value[1]}', '{value[2]}', '{value[3]}');";
            }
            else
            {
                command = $@"INSERT INTO `{tableName}` (`{firstColumn}`, `{secondColumn}`, `{thirdColumn}`)
                             VALUES ('{value[0]}', '{value[1]}', '{value[2]}');";
            }

            return command;
        }

        /// <summary>
        /// マスタのInsertクエリ文字列を返すメソッド
        /// </summary>
        /// <param name="tableName">MySQLのテーブル名</param>
        /// <param name="value">MySQLのテーブルのIDを除く各カラム名</param>
        /// <returns></returns>
        internal string MasterSelectQueryCommand(string tableName, string[] columns, string[] values, int count)
        {
            if (count == -1) return "";

            string whereCommand = "";

            switch (count - 1)
            {
                case 0:
                    whereCommand = $@"WHERE `{columns[0]}` = '{values[0]}'";
                    break;
                case 1:
                    whereCommand = $@"WHERE `{columns[0]}` = '{values[0]}'
                                      AND `{columns[1]}` = '{values[1]}'";
                    break;
                case 2:
                    whereCommand = $@"WHERE `{columns[0]}` = '{values[0]}'
                                      AND `{columns[1]}` = '{values[1]}'
                                      AND `{columns[2]}` = '{values[2]}'";
                    break;
                case 3:
                    whereCommand = $@"WHERE `{columns[0]}` = '{values[0]}'
                                      AND `{columns[1]}` = '{values[1]}'
                                      AND `{columns[2]}` = '{values[2]}'
                                      AND `{columns[3]}` = '{values[3]}'";
                    break;
            }
            string command = $@"SELECT *
                                FROM `{tableName}`
                                {whereCommand};";

            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal string idNumberReset(string tableName)
        {
            string command = $@"ALTER TABLE `{tableName}` AUTO_INCREMENT = 1;";
            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        /// <param name="count"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal string UpDateQueryCommand(string tableName, string[] columns, string[] values, int count, string id)
        {
            if (count == -1) return "";

            string setCommand = "";
            switch (count - 1)
            {
                case 0:
                    setCommand = $@"SET `{columns[0]}` = '{values[0]}'";
                    break;
                case 1:
                    setCommand = $@"SET `{columns[0]}` = '{values[0]}',
                                    `{columns[1]}` = '{values[1]}'";
                    break;
                case 2:
                    setCommand = $@"SET `{columns[0]}` = '{values[0]}',
                                    `{columns[1]}` = '{values[1]}',
                                    `{columns[2]}` = '{values[2]}'";
                    break;
            }

            string command = $@"UPDATE `{tableName}`
                                {setCommand}
                                WHERE `id` = '{id}';";
            return command;
        }

        /// <summary>
        /// 「MySQLDataBaseMatrixCount」用列挙型
        /// </summary>
        internal enum MatrixLineName
        {
            columns = 1,
            rows = 2
        }

        /// <summary>
        /// MySQLのテーブルから行または列の数を返すメソッド
        /// </summary>
        /// <param name="columnsOrRows">列：columns(1), 行：rows(2)</param>
        /// <param name="tableName">MySQLテーブル名</param>
        /// <param name="columnName">columnsOrRowsの値が「rows」のとき使用、それ以外は""</param>
        /// <returns></returns>
        internal int CountMySQLDataBaseTable(MatrixLineName columnsOrRows, string tableName, string columnName)
        {
            string command = "";

            if (columnsOrRows == MatrixLineName.rows)
            {
                command = $@"SELECT COUNT(DISTINCT `{columnName}`)
                             FROM `{tableName}`;";
            }
            else if (columnsOrRows == MatrixLineName.columns)
            {
                command = $@"SELECT COUNT(*)
                             FROM information_schema.columns
                             WHERE table_name = '{tableName}';";
            }

            MySQLBase mySQLBase = new MySQLBase();
            DataTable dt = mySQLBase.MySQLDataRequest(command);

            var matrixCountStr = dt.Rows[0][0].ToString();
            int matrixCount;
            if (int.TryParse(matrixCountStr, out matrixCount)) return matrixCount;

            return -1;
        }

        /// <summary>
        /// 特定のレコードを検索する為の「SELECT」クエリ実行用メソッド
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereColunmNames"></param>
        /// <param name="whereValues"></param>
        /// <returns></returns>
        internal DataTable MasterEditorSelectQuery(string tableName, List<string> whereColunmNames, List<string> whereValues)
        {
            //
            int count = 0;
            List<int> memoryIndex = new List<int>();

            // whereValuesの値が「""」のとき、continue文で「""」を無視する
            foreach (string value in whereValues)
            {
                if (value != "")
                {
                    // 空文字列のインデックスを取得する
                    memoryIndex.Add(count);
                    continue;
                }
                count++;
            }

            //
            List<string> whereColumn = new List<string>();
            List<string> whereValue = new List<string>();
            for (int i = 0; i < memoryIndex.Count; i++)
            {
                // 取得したインデックス番号を格納
                int index = memoryIndex[i];

                //if (whereValues[index] == "") continue;

                // 取得したインデックス番号から空文字列でないカラム名を新しいListへ格納
                whereColumn.Add(whereColunmNames[index]);
                whereValue.Add(whereValues[index]);
            }

            // 新しく値を格納した「whereColumn」「whereValue」から動的にSELECT文を生成
            string whereCommand = string.Empty;
            for (int i = 0; i < whereColumn.Count; i++)
            {
                if (i == 0)
                {
                    whereCommand = $"WHERE `{whereColumn[i]}` = '{whereValue[i]}'";
                }
                else if (i > 0)
                {
                    whereCommand = $"{whereCommand} AND `{whereColumn[i]}` = '{whereValue[i]}'";
                }
            }

            // 「SELECT」SQLコマンド生成
            string command = $@"SELECT *
                                FROM `{tableName}`
                                {whereCommand};";

            // 「SELECT」クエリの実行
            MySQLBase mySQLBase = new MySQLBase();
            DataTable dt = mySQLBase.MySQLDataRequest(command);

            return dt;
        }

        /// <summary>
        /// 各種テーブルに対して、「UPDATE」のクエリを実行するためのメソッド
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnNames"></param>
        /// <param name="values"></param>
        internal void MasterEditorUpdateQuery(string tableName, List<string> columnNames, List<string> values)
        {
            string setCommand = string.Empty;
            //
            int count = values.Count;
            for (int i = 1; i < count; i++)
            {
                // 「id」カラムを除いた全てのカラムの値をsetCommandへ格納
                if (i < count - 1)
                {
                    setCommand = $"{setCommand}`{columnNames[i]}` = '{values[i]}', ";
                }
                else if (i == count - 1)
                {
                    setCommand = $"{setCommand}`{columnNames[i]}` = '{values[i]}'";
                }
            }

            // 「UPDATE」SQLコマンド生成
            string command = $@"UPDATE `{tableName}` SET {setCommand} WHERE `{columnNames[0]}` = '{values[0]}';";

            // 「UPDATE」クエリの実行
            MySQLBase mySQLBase = new MySQLBase();
            mySQLBase.MySQLDataProcessing(command);
        }

        /// <summary>
        /// 各種テーブルに対して、「DELETE」のクエリを実行するためのメソッド
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        internal void MasterEditorDeleteQuery(string tableName, int id)
        {
            // 「DELETE」SQLコマンド生成
            string command = $@"DELETE
                                FROM `{tableName}`
                                WHERE id = '{id}';";

            // 「DELETE」クエリの実行
            MySQLBase mySQLBase = new MySQLBase();
            mySQLBase.MySQLDataProcessing(command);
        }

        /// <summary>
        /// 各種テーブルに対して、「INSERT」のクエリを実行するためのメソッド
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="args"></param>
        internal void MasterEditorInsertQuery(string tableName, List<string> columnNames, List<string> values)
        {
            // 
            string columnNameStrings = string.Empty;            
            string commandStrings = string.Empty;

            // 
            string command = string.Empty;

            //
            int argsCount = values.Count;
            for (int i = 0; i < argsCount; i++)
            {
                if (i < argsCount - 1)
                {
                    columnNameStrings = columnNameStrings + $"`{columnNames[i]}`, ";
                    commandStrings = commandStrings + $"'{values[i]}', ";
                }
                else if (i == argsCount - 1)
                {
                    columnNameStrings = columnNameStrings + $"`{columnNames[i]}`";
                    commandStrings = commandStrings + $"'{values[i]}'";
                }

                // 「INSERT」SQLコマンド生成
                command = $@"INSERT INTO `{tableName}` ({columnNameStrings})
                             VALUES ({commandStrings});";
            }

            // 「INSERT」クエリの実行
            MySQLBase mySQLBase = new MySQLBase();
            mySQLBase.MySQLDataProcessing(command);
        }

        /// <summary>
        /// 引数に渡したMySQLのテーブル名からテーブルのカラム名を取得し、List<string>で返す
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal List<string> ToListTableColumns(string tableName)
        {
            string command = $@"SELECT * FROM `{tableName}`;";

            MySQLBase mySQLBase = new MySQLBase();
            DataTable dt = mySQLBase.MySQLDataRequest(command);

            List<string> strings = new List<string>();

            int columnsCount = dt.Columns.Count;
            for (int i = 0; i < columnsCount; i++)
            {
                strings.Add(dt.Columns.ToString());
            }

            return strings;
        }
    }
}
