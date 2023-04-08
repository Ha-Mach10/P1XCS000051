using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace P1XCS000051
{
    public partial class MGTCreatePanel : UserControl
    {
        public Label mgtLabel
        {
            get { return label5; }
            set { label5 = value; }
        }
        public Label RecordCountLabel
        {
            get { return label17; }
            set { label17 = value; }
        }
        #region Buttons
        public Button ViewButton
        {
            get { return button1; }
            set { button1 = value; }
        }
        public Button InsertButton
        {
            get { return button2; }
            set { button2 = value; }
        }
        public GroupBox GroupBox
        {
            get { return groupBox2; }
            set { groupBox2 = value; }
        }
        #endregion

        /// <summary>
        /// 「UseCreatePanel」のコンストラクタ
        /// </summary>
        public MGTCreatePanel()
        {
            InitializeComponent();

            List<RadioButton> radioButtons = new List<RadioButton> { radioButton1, radioButton2 };
            foreach (RadioButton radioButton in radioButtons)
            {
                radioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            }

            button1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox4.Enabled = false;
            label5.Text = "";
            label15.Text = "";
            label17.Text = "";
            label18.Text = "";
            radioButton1.Checked = true;
            textBox5.Enabled = false;
            groupBox2.Enabled = false;

            comboBox1.SelectedIndexChanged -= new EventHandler(LanguageComboBox_SelectedIndexChanged);
            LanguageComboItemSetting();
        }
        private void MGTCreatePanel_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();

        }

        /// <summary>
        /// 「言語種別」欄へアイテムをセット
        /// </summary>
        internal void LanguageComboItemSetting()
        {
            string command = $@"SELECT `language_type`
                                FROM `manager_language_type`;";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.DataSource = MySQLSelectCommandQuery(command);
            comboBox1.ValueMember = "language_type";
            comboBox1.DisplayMember = "language_type";

            comboBox1.SelectedIndexChanged += new EventHandler(LanguageComboBox_SelectedIndexChanged);
        }
        /// <summary>
        /// 「使用用途」欄へアイテムをセット
        /// </summary>
        private void UseApplicationsComboItemSetting()
        {
            string command1 = $@"SELECT `use_name_jp`
                                 FROM `manager_use_application`
                                 WHERE `sign` = '1';";
            string command2 = $@"SELECT `use_name_jp`
                                 FROM `manager_use_application`
                                 WHERE `sign` = '2';";

            int count = 0;
            string[] commands = new string[] { command1, command2 };
            List<ComboBox> comboBoxes = new List<ComboBox> { comboBox3, comboBox4 };
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.DataSource = MySQLSelectCommandQuery(commands[count]);
                comboBox.ValueMember = "use_name_jp";
                comboBox.DisplayMember = "use_name_jp";
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox.SelectedIndex = -1;
                count++;
            }
        }

        /// <summary>
        /// SQLコマンドを引数としてDataTableを返すメソッド
        /// </summary>
        /// <param name="command">SQLコマンド</param>
        /// <returns>DataTable</returns>
        private DataTable MySQLSelectCommandQuery(string command)
        {
            try
            {
                DataTable dt = new DataTable();
                DataBaseConnectionStringBuider buider = new DataBaseConnectionStringBuider();
                string conStr = buider.SchemaManegerConnectionString();
                using (MySqlConnection conn = new MySqlConnection(conStr))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, conn))
                {
                    adapter.Fill(dt);
                }

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "SELECTコマンド失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// 「言語種別」欄ComboBoxのインデックス変更時実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (groupBox2.Enabled == true) groupBox2.Enabled = false;
            if (button1.Enabled == true) button1.Enabled = false; 

            comboBox2.SelectedIndexChanged -= new EventHandler(DevelopComboBox_SelectedIndexChanged);

            string languageName = comboBox1.SelectedValue.ToString();

            string scriptTypeCommand = $@"SELECT `script_type`
                                          FROM `manager_language_type`
                                          WHERE `language_type` = '{languageName}';";
            DataTable dt1 = MySQLSelectCommandQuery(scriptTypeCommand);
            string scriptType = dt1.Rows[0][0].ToString();

            string developCommand = $@"SELECT `develop_type`
                                       FROM `manager_develop_type`
                                       WHERE `script_type` = '{scriptType}'";

            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DataSource = MySQLSelectCommandQuery(developCommand);
            comboBox2.ValueMember = "develop_type";
            comboBox2.DisplayMember = "develop_type";
            comboBox2.SelectedIndex = -1;

            comboBox2.Enabled = true;

            comboBox2.SelectedIndexChanged += new EventHandler(DevelopComboBox_SelectedIndexChanged);
        }

        /// <summary>
        /// 「開発種別」欄ComboBoxのインデックス変更時実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevelopComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 「GroupBox2」のEnabledを「false」へ変更
            if (groupBox2.Enabled == true)
            {
                groupBox2.Enabled = false;
            }

            comboBox4.Enabled = false;

            string comboBoxItem1 = comboBox1.SelectedValue.ToString();
            string comboBoxItem2 = comboBox2.SelectedValue.ToString();
            string command1 = $@"SELECT `type_code`
                                 FROM `manager_language_type`
                                 WHERE `language_type` = '{comboBoxItem1}'";
            string command2 = $@"SELECT `type_code`
                                 FROM `manager_develop_type`
                                 WHERE `develop_type` = '{comboBoxItem2}';";
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1 = MySQLSelectCommandQuery(command1);
            dt2 = MySQLSelectCommandQuery(command2);

            string languageCode = dt1.Rows[0][0].ToString();
            string developCode = dt2.Rows[0][0].ToString();

            label5.Text = developCode + languageCode;

            UseApplicationsComboItemSetting();

            if (comboBox2.SelectedIndex > -1) button1.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Enabled = true;
        }
        /// <summary>
        /// 「使用用途」欄のComboBoxを「自動」か「手動」でユーザーのアクセス可否を切り替える
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            List<ComboBox> comboBoxes = new List<ComboBox> { comboBox3, comboBox4 };
            foreach (ComboBox comboBox in comboBoxes)
            {
                if (radioButton1.Checked)
                {
                    comboBox.Enabled = true;
                    textBox5.Enabled = false;
                }
                if (radioButton2.Checked)
                {
                    comboBox.Enabled = false;
                    textBox5.Enabled = true;
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1) return;

            string getMgtType = label5.Text;
            groupBox2.Text = "記番登録処理（" + getMgtType + "）";
            groupBox2.Enabled = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            bool isGetItem1 = textBox1.Text != "";
            bool isGetItem2 = false;
            if (radioButton1.Checked) isGetItem2 = comboBox3.SelectedValue.ToString() != "";
            if (radioButton2.Checked) isGetItem2 = textBox5.Text != "";
            if (!(isGetItem1 && isGetItem2)) return;

            ComboBox ctrlCombo = null;

            string codeHead = label5.Text;
            string devName = textBox1.Text;
            string useApp = "";
            string useApp2 = "";
            if (radioButton1.Checked)
            {
                ctrlCombo = comboBox3;

                if (comboBox3.SelectedIndex != -1)
                {
                    useApp = comboBox3.SelectedValue.ToString();
                }
                if (comboBox4.SelectedIndex != -1)
                {
                    useApp = comboBox3.SelectedValue.ToString();
                    useApp2 = comboBox4.SelectedValue.ToString();
                }
            }
            if (radioButton2.Checked) useApp = textBox5.Text;
            string diversionNum = textBox2.Text;
            string oldNum = textBox3.Text;
            string newNum = textBox4.Text;
            string explanation = "";
            string summary = "";
            if (richTextBox1.Text != "")
            {
                explanation = richTextBox1.Text.Replace("\n", "");
            }
            if (richTextBox2.Text != "")
            {
                summary = richTextBox2.Text.Replace("\n", "");
            }
            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            string command = baseSQLQuerys.mgtInsertQueryCommand(
                codeHead, devName, useApp, useApp2, diversionNum, oldNum, newNum, explanation, summary, ctrlCombo);

            MySQLBase mySQLBase = new MySQLBase();
            mySQLBase.MySQLDataProcessing(command);
        }

        private void UpDateButton_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndexChanged -= new EventHandler(LanguageComboBox_SelectedIndexChanged);
            comboBox1.DataSource = null;
            LanguageComboItemSetting();
            label18.Text = "更新完了";
        }
    }
}
