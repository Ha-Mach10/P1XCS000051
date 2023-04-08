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
    public partial class MasterEditor : Form
    {
        public MasterEditor()
        {
            InitializeComponent();

            // MasterEditorの最大・最小サイズを現在のフォームサイズで固定する。
            MaximumSize = Size;
            MinimumSize = Size;

            List<RadioButton> radioButtons = new List<RadioButton> { radioButton1, radioButton2, radioButton3 };
            foreach (RadioButton radioButton in radioButtons)
            {
                radioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            }
            radioButton1.Checked = true;

            ComboItemSettings();
            button1.Text = "追加";
            groupBox4.Enabled = false;
            label23.Text = "";

            // DataGridViewの設定
            //
            dataGridView1.AllowUserToAddRows = false;
            // 
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            // 複数行選択しないようにする
            dataGridView1.MultiSelect = false;
            // 行ヘッダを非表示
            dataGridView1.RowHeadersVisible = false;
            // ヘッダを改行しない
            dataGridView1.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            // サイズの自動調整
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // 行全てを選択する
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
        }

        /// <summary>
        /// WindowMessageの処理
        /// フォームの端へカーソルをかざした時のカーソルの変更を阻止する
        /// 参考サイト：https://atmarkit.itmedia.co.jp/ait/articles/0606/30/news131.html
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x84)
            {
                // 下記、WM_NCHITTESTメッセージ
                // 参考サイト：https://learn.microsoft.com/ja-jp/windows/win32/inputdev/wm-nchittest
                // HTMINBUTTON   | 8      : [最小化]ボタンで (または、HTREDUCE)
                // HTMAXBUTTON   | 9      : [最大化]ボタンで (または、HTZOOM)
                // HTLEFT        | 10     : サイズ変更可能なウィンドウの左罫線
                // HTRIGHT       | 11     : サイズ変更可能なウィンドウの右罫線
                // HTTOP         | 12     : ウィンドウ水平方向の上罫線内(ユーザーはマウスをクリックしてウィンドウの垂直方向のサイズを変更できる)
                // HTTOPLEFT     | 13     : ウィンドウ左上隅
                // HTTOPRIGHT    | 14     : ウィンドウ右下隅
                // HTBOTTOM      | 15     : ウィンドウ下水平方向の境界線(ユーザーはマウスをクリックしてウィンドウの垂直方向のサイズを変更できる)
                // HTBOTTOMLEFT  | 16     : ウィンドウ左下隅(ユーザーはマウスをクリックしてウィンドウの斜めサイズを変更できる)
                // HTBOTTOMRIGHT | 17     : ウィンドウ右下隅(※上記に同じ)

                List<int> msgResultInt = new List<int> { 8, 9,10, 11, 12, 13, 14, 15, 16, 17 };
                foreach (int resultInt in msgResultInt)
                {
                    bool result = m.Result == (IntPtr)resultInt;

                    if (result)
                    {
                        // HTNOWHERE | 0 : 画面の背景またはウィンドウ間の分割線
                        m.Result = (IntPtr)0;
                    }
                }
            }
        }

        /// <summary>
        /// MasterEditorをFrom1の中央に表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterEditor_Load(object sender, EventArgs e)
        {
            PutonCenter putonCenter = new PutonCenter(Owner, this);
            
            // From1に対するMasterEditorの相対的な原点座標の計算
            Location = putonCenter.PutOnFormControlPosition(PutonCenter.PutOnStyle.Center);
        }

        /// <summary>
        /// comboBox1のインデックス番号を0にする。
        /// </summary>
        /// <param name="sender">comboBox1</param>
        /// <param name="e">comboBox1</param>
        private void MasterEditor_Shown(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        /// <summary>
        /// 各種テーブルをcomboBox1へ格納
        /// </summary>
        private void ComboItemSettings()
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            DataTable dt = baseSQLQuerys.ShowTableMasterQuery();

            List<string> dtItems = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string itemStr = dt.Rows[i][0].ToString();
                dtItems.Add(itemStr);
            }
            foreach (string dtItem in dtItems)
            {
                if (dtItem != "manager_codes")
                {
                    comboBox1.Items.Add(dtItem);
                }

                // comboBox1.Items.Add(dtItem);
            }
            comboBox1.SelectedIndex = -1;
        }

        /// <summary>
        /// comboBox1のインデックス変更時イベント
        /// </summary>
        /// <param name="sender">comboBox1</param>
        /// <param name="e"></param>
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            //
            if (selectLTB.Count > 0)
            {
                DeleteLabeledTextBox();
                selectLTB = new List<LabeledTextBox>();
                addLTB = new List<LabeledTextBox>();
            }

            // テーブル名を文字列で取得
            string tableName = comboBox1.SelectedItem.ToString();
            
            // MySQLの
            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            int columnCount = baseSQLQuerys.CountMySQLDataBaseTable(BaseSQLQuerys.MatrixLineName.columns, tableName, "id");
            
            // LabeledTextBoxを「panel1」「panel2」へ配置する
            SetLabeledTextBox(columnCount);
            
            // dataGridViewへ指定したテーブルを表示する
            ShowDataGridView(tableName);
        }

        /// <summary>
        /// ラベル付きテキストボックスコントロールのList作成
        /// </summary>
        private List<LabeledTextBox> selectLTB = new List<LabeledTextBox>();
        private List<LabeledTextBox> addLTB = new List<LabeledTextBox>();
        
        /// <summary>
        /// dataGridViewへ表示するテーブルのカラム列数分コントロールを作成する
        /// </summary>
        /// <param name="columnCount"></param>
        private void SetLabeledTextBox(int columnCount)
        {
            if (columnCount == 0) return;

            for (int i = 0; i < columnCount; i++)
            {
                // コントロールのインスタンスを生成
                LabeledTextBox labeledTextSelect = new LabeledTextBox();
                LabeledTextBox labeledTextAdd = new LabeledTextBox();
                
                //
                selectLTB.Add(labeledTextSelect);
                addLTB.Add(labeledTextAdd);

                // 
                int width = 150;
                selectLTB[i].TextBoxWidth = width;
                addLTB[i].TextBoxWidth = width;
            }

            for (int i = columnCount - 1; i >= 0; i--)
            {
                // panel1・2へ生成したコントロールを追加
                panel1.Controls.Add(selectLTB[i]);
                if (i > 0)
                {
                    panel2.Controls.Add(addLTB[i]);
                }


                // コントロールのDockSryleを「Top」に設定
                selectLTB[i].Dock = DockStyle.Top;
                addLTB[i].Dock = DockStyle.Top;
            }
        }

        /// <summary>
        /// LabededTextBoxの削除を行う
        /// </summary>
        private void DeleteLabeledTextBox()
        {
            panel1.Controls.Clear();
            panel2.Controls.Clear();
        }

        /// <summary>
        /// 「追加」「編集」RadioButtonチェック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            // radioButtonの状態を取得
            bool isChecked = radioButton.Checked == true;

            //
            bool radio12 = radioButton1.Checked || radioButton2.Checked;
            bool radio3 = radioButton3.Checked;

            if (radio12 && isChecked)
            {
                if (radioButton.Text == "追加")
                {
                    string text = "追加";
                    groupBox1.Enabled = false;
                    groupBox2.Text = text;
                    button1.Text = text;
                }
                else if (radioButton.Text == "編集")
                {
                    string text = "編集";
                    groupBox1.Enabled = true;
                    groupBox2.Text = text;
                    button1.Text = text;
                }

                groupBox2.Enabled = true;
                groupBox4.Enabled = false;
            }

            else if (radio3 && isChecked == true)
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                groupBox4.Enabled = true;
            }

            if (radioButton1.Checked)
            {
                foreach (LabeledTextBox ltb in addLTB)
                {
                    ltb.TextBoxText = "";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, EventArgs e)
        {
            string tableName = comboBox1.SelectedItem.ToString();
            ShowDataGridView(tableName);

            foreach (LabeledTextBox labeledTextBox in selectLTB)
            {
                labeledTextBox.TextBoxText = "";
            }
        }

        /// <summary>
        /// 「追加」または「編集」ボタンクリックイベント
        /// MySQLの指定したテーブルへInsertまたはUpDate文のクエリを発行する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditOrAddButton_Click(object sender, EventArgs e)
        {
            // 選択中のテーブル名を取得
            string tableName = comboBox1.SelectedItem.ToString();

            // 
            List<string> tmpLabels = new List<string>();

            // 
            List<string> addLtbLabels = new List<string>();
            List<string> addLtbTexts = new List<string>();
            foreach (LabeledTextBox ltbs in addLTB)
            {
                if (ltbs == addLTB[0]) continue;

                tmpLabels.Add(ltbs.LText);
                addLtbTexts.Add(ltbs.TextBoxText);
            }

            // 日本語カラム名をMySQLのカラム名へ変換
            Transrator transrator = new Transrator();
            addLtbLabels.AddRange(transrator.TranslateJapanese(tableName, tmpLabels));

            // 
            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();

            // 「追加」ラジオボタンチェック時
            if (radioButton1.Checked)
            {
                baseSQLQuerys.MasterEditorInsertQuery(tableName, addLtbLabels, addLtbTexts);
            }
            // 「編集」ラジオボタンチェック時
            else if (radioButton2.Checked)
            {
                baseSQLQuerys.MasterEditorUpdateQuery(tableName, addLtbLabels, addLtbTexts);
            }

            // dataGridView1へ指定したテーブル名のレコードを表示する
            ShowDataGridView(tableName);
        }

        /// <summary>
        /// MySQLのテーブルをDataGridViewへセットするメソッド
        /// </summary>
        /// <param name="tableName">MySQLのテーブル名（ComboBox1に設定されている）</param>
        private void ShowDataGridView(string tableName)
        {
            dataGridView1.DataSource = null;

            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            dataGridView1.DataSource = baseSQLQuerys.SelectTableMasterQuery(tableName);

            int colCount = dataGridView1.Columns.Count - 1;
            dataGridView1.Columns[colCount].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            Transrator transrator = new Transrator();
            int count = dataGridView1.Columns.Count;
            for (int i = 0; i < count; i++)
            {
                string columnName = dataGridView1.Columns[i].HeaderText;

                string columnNameTrns = transrator.TranslateColumnName(tableName, columnName);

                selectLTB[i].LText = columnNameTrns;
                addLTB[i].LText = columnNameTrns;

                dataGridView1.Columns[i].HeaderText = columnNameTrns;
            }
        }
        /// <summary>
        /// 指定または選択した「id」行のレコードを削除するボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_CLick(object sender, EventArgs e)
        {
            int textInt = 0;
            bool isInt = int.TryParse(textBox9.Text, out textInt);
            string tableName = comboBox1.SelectedItem.ToString();
            
            if (isInt)
            {
                BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
                baseSQLQuerys.MasterEditorDeleteQuery(tableName, textInt);

                ShowDataGridView(tableName);

                // カラム「id」のレコード数を数える
                int count = baseSQLQuerys.SelectCount(tableName);

                // カラム「id」のAuto_Incrementの数値をリセット
                string upDateCommand = baseSQLQuerys.idNumberReset(tableName);
            }
        }
        /// <summary>
        /// 「dataGridView」の列選択時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // dataGridViewへ表示された行の数を数える
            int colCount = dataGridView1.Columns.Count;

            // 選択行のインデックスを取得
            int currentRow = dataGridView1.CurrentCell.RowIndex;

            if (radioButton3.Checked)
            {
                textBox9.Text = (currentRow + 1).ToString();
            }

            if (radioButton2.Checked == false)
            {
                return;
            }

            // フィールド値格納用の
            List<string> rowItems = new List<string>();

            // dataGridViewの選択行の各フィールドをListへ格納
            for (int i = 0; i < colCount; i++)
            {
                string rowItem = dataGridView1.Rows[currentRow].Cells[i].Value.ToString();
                rowItems.Add(rowItem);
            }

            int inc = 0;
            foreach (LabeledTextBox addLabelTextBox in addLTB)
            {
                addLabelTextBox.TextBoxText = rowItems[inc];
                inc++;
            }
        }
        /// <summary>
        /// レコードを絞るボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectViewButton_Click(object sender, EventArgs e)
        {
            string tableName = comboBox1.SelectedItem.ToString();
            Button btnObj = (Button)sender;

            //
            List<string> tmpLabelItems = new List<string>();

            // 引数用変数
            List<string> labelItems = new List<string>();
            List<string> textItems = new List<string>();

            int itemCount = selectLTB.Count;
            for (int i = 0; i < itemCount; i++)
            {
                tmpLabelItems.Add(selectLTB[i].LText);
                textItems.Add(selectLTB[i].TextBoxText);
            }

            //
            Transrator transrator = new Transrator();
            labelItems.AddRange(transrator.TranslateJapanese(tableName, tmpLabelItems));
            
            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            dataGridView1.DataSource = baseSQLQuerys.MasterEditorSelectQuery(tableName, labelItems, textItems);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
