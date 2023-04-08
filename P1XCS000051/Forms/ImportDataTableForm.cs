using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P1XCS000051
{
    public partial class ImportDataTableForm : Form
    {
        /// <summary>
        /// 「ImportDataTableForm」のコンストラクタ
        /// </summary>
        /// <param name="dt"></param>
        public ImportDataTableForm(DataTable dt)
        {
            InitializeComponent();
            ComboBoxItemsSetting();
            comboBox1.SelectedIndexChanged += new EventHandler(ComboBox1_SelectedIndexChanged);

            label1.Text = "";
            label4.Text = "";
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            DataGridViewAddDataTable(dt);
        }

        private void DataGridViewAddDataTable (DataTable dt)
        {
            dataGridView1.DataSource = dt;
            int colCount = dataGridView1.Columns.Count - 1;
            dataGridView1.Columns[colCount].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        /// <summary>
        /// comboBox1へアイテムをセットするメソッド
        /// </summary>
        private void ComboBoxItemsSetting()
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            string member = "Tables_in_manager";

            // 「manager」スキーマのすべてのテーブル名を取得するSQL文を発行
            string showCommand = $@"SHOW TABLES;";

            // DataTableへクエリの結果を格納する
            MySQLBase mySQLBase = new MySQLBase();
            DataTable dt = mySQLBase.MySQLDataRequest(showCommand);
            
            List<string> tableNames = new List<string>();
            
            Transrator transrator = new Transrator();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string tableName = dt.Rows[i][0].ToString();
                tableNames.Add(transrator.TranslateTableName(tableName));
            }

            comboBox1.DataSource = tableNames;
            comboBox1.SelectedIndex = -1;
            //comboBox1.ValueMember = member;
            //comboBox1.DisplayMember = member;
        }

        private static List<string> setColName = null;
        private Label[] labels;
        private int labelFlag = 0;

        /// <summary>
        /// ccomboBox1インデックス変更時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // comboBox1の選択インデックスが-1の時、イベントを抜ける
            if (comboBox1.SelectedIndex == -1) return;


            string selectedItem = comboBox1.SelectedValue.ToString();

            // comboBox1の選択した値をSQL文へ組み込む
            string translateCommand = $@"SELECT `table_name`
                                         FROM `table_translator`
                                         WHERE `japanese` = '{selectedItem}';";

            //List<string> columnNames = new List<string>();

            MySQLBase mySQLBase = new MySQLBase();
            //Transrator transrator = new Transrator(); 
            DataTable dt = mySQLBase.MySQLDataRequest(translateCommand);
            string tableName = dt.Rows[0][0].ToString();
            /*for (int i = 0; i < dt.Columns.Count; i++)
            {
                // 
                string columnName = dt.Rows[i][1].ToString();

                // 
                columnNames.Add(transrator.TranslateColumnName(tableName, columnName));
            }*/

            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            var targetColumn = BaseSQLQuerys.MatrixLineName.columns;
            int columnsCount = baseSQLQuerys.CountMySQLDataBaseTable(targetColumn, tableName, "");
            label4.Text = columnsCount.ToString();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var rows = dataGridView1.Rows;
            foreach (DataGridViewRow row in rows)
            {
                string devCode = row.Cells[1].Value.ToString();
                string devName = row.Cells[2].Value.ToString();
                string createDate = row.Cells[3].Value.ToString();
                string useApp = row.Cells[4].Value.ToString();
                string explanation = row.Cells[5].Value.ToString();
                string summary = row.Cells[6].Value.ToString();

                Label label = new Label();
                BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
                string command = baseSQLQuerys.mgtInsertQueryCommand(devCode, devName, useApp, createDate, "", "", "", explanation, summary, label);
                MySQLBase mySQLBase = new MySQLBase();
                mySQLBase.MySQLDataProcessing(command);
            }
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
