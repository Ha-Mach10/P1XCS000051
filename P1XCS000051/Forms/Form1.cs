using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;

using MySql.Data.MySqlClient;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace P1XCS000051
{
    public partial class Form1 : Form
    {
        private MGTCreatePanel mGTCreatePanel = new MGTCreatePanel();
        private DWGCreatePanel dWGCreatePanel = new DWGCreatePanel();
        private MGTEditPanel mGTEditPanel = new MGTEditPanel();

        private DataGridView dataGridView = new DataGridView();
        ContextMenuStrip dataGridViewMenuStrip = new ContextMenuStrip();

        /// <summary>
        /// Form1のコンストラクタ
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            radioButton1.Checked = true;
            radioButton3.Checked = true;

            // dataGridView用のContextMenuStrip
            dataGridViewMenuStrip.Items.AddRange(DataGridViewMenuStripItemSetting());
            dataGridViewMenuStrip.Name = "dataGridViewMenuStrip";

            //
            panel2.Controls.Add(dataGridView);
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.ContextMenuStrip = dataGridViewMenuStrip;
            dataGridView.CellClick += new DataGridViewCellEventHandler(DataGridView_CellClick);

            //
            panel2.Controls.Add(mGTEditPanel);
            mGTEditPanel.Dock = DockStyle.Top;
            mGTEditPanel.ViewButton.Enabled = true;
            mGTEditPanel.ChangeButton.Enabled = false;
            mGTEditPanel.DeleteButton.Enabled = false;

            // 各種イベントハンドラセット
            radioButton1.CheckedChanged += new EventHandler(CreatePanelSetRadioButton_CheckedChanged);
            radioButton2.CheckedChanged += new EventHandler(CreatePanelSetRadioButton_CheckedChanged);

            radioButton3.CheckedChanged += new EventHandler(SearchRadioButton_CheckedChanged);
            radioButton4.CheckedChanged += new EventHandler(EditRadioButton_CheckedChanged);
            radioButton5.CheckedChanged += new EventHandler(DeleteRadioButton_CheckedChanged);

            MGTCreatePanelSetting();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ToolStripItem[] DataGridViewMenuStripItemSetting()
        {
            //
            ToolStripMenuItem tsmp1 = new ToolStripMenuItem();
            tsmp1.Name = "tsmp1";
            tsmp1.Text = "プロパティ";
            tsmp1.Click += new EventHandler(Tsmp1_Click);

            // 
            ToolStripMenuItem tsmp2 = new ToolStripMenuItem();
            tsmp2.Name = "tsmp2";
            tsmp2.Text = "検索";


            ToolStripItem[] toolStripItems = new ToolStripItem[]
            {
                tsmp1
            };

            return toolStripItems;
        }

        #region

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tsmp1_Click(object sender, EventArgs e)
        {
            int rowsCount = dataGridView.Rows.Count;
            if (rowsCount > 0)
            {
                int currentIndex = dataGridView.CurrentCell.RowIndex;

                List<string> rowItems = new List<string>();
                List<string> columnItems = new List<string>();

                for (int i = 0; i < dataGridView.ColumnCount; i++)
                {
                    string rowItem = dataGridView.Rows[currentIndex].Cells[i].Value.ToString();
                    rowItems.Add(rowItem);

                    string columnItem = dataGridView.Columns[i].HeaderText.ToString();
                    columnItems.Add(columnItem);
                }

                // 
                MGTPropertyViewEditor mGTPropertyViewEditor = new MGTPropertyViewEditor(columnItems, rowItems);
                mGTPropertyViewEditor.ShowDialog(this);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePanelSetRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                DWGCreatePanelRemove();
                MGTCreatePanelSetting();
            }
            else if (radioButton2.Checked)
            {
                MGTCreatePanrlRemove();
                DWGCreatePanelSetting();
            }
        }

        #region DWGCreatePanelの設置及び削除メソッド群

        /// <summary>
        /// DWGCreatePanelの設置用メソッド
        /// </summary>
        private void DWGCreatePanelSetting()
        {
            panel1.Controls.Add(dWGCreatePanel);
            dWGCreatePanel.Dock = DockStyle.Top;
            dWGCreatePanel.BringToFront();

            // マスタ登録用ボタンクリックイベント
        }

        /// <summary>
        /// DWGCreatePanelの削除用メソッド
        /// </summary>
        private void DWGCreatePanelRemove()
        {
            panel1.Controls.Remove(dWGCreatePanel);
        }
        #endregion

        #region MGTCreatePanelの設置及び削除メソッド群

        /// <summary>
        /// MGTCreatePanelの設置用メソッド
        /// </summary>
        private void MGTCreatePanelSetting()
        {
            panel1.Controls.Add(mGTCreatePanel);
            mGTCreatePanel.Dock = DockStyle.Top;
            mGTCreatePanel.BringToFront();

            mGTCreatePanel.ViewButton.Click += new EventHandler(MgtCreatePanelViewButton_Click);
        }

        /// <summary>
        /// MGTCreatePanelの削除用メソッド
        /// </summary>
        private void MGTCreatePanrlRemove()
        {
            panel1.Controls.Remove(mGTCreatePanel);
        }
        #endregion

        #region MGTEditPanel各種「RadioButton」イベント

        /// <summary>
        /// 「検索」ラジオボタンチェック時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            mGTEditPanel.ViewButton.Enabled = radioButton3.Checked;
        }

        /// <summary>
        /// 「台帳編集」ラジオボタンチェック時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            bool radioChecked = !radioButton4.Checked;

            mGTEditPanel.TextBoxID.Enabled = radioChecked;
            mGTEditPanel.TextBoxDevelopNumber.Enabled = radioChecked;
            mGTEditPanel.ComboCreatedYear.Enabled = radioChecked;
            mGTEditPanel.ComboCreatedMonth.Enabled = radioChecked;
            mGTEditPanel.ComboCreatedDay.Enabled = radioChecked;
            mGTEditPanel.TextBoxMajor.Enabled = radioChecked;
            mGTEditPanel.TextBoxMinor.Enabled = radioChecked;
            mGTEditPanel.TextBoxBuild.Enabled = radioChecked;
            mGTEditPanel.ComboRevisionYear.Enabled = radioChecked;
            mGTEditPanel.ComboRevisionMonth.Enabled = radioChecked;
            mGTEditPanel.ComboRevisionDay.Enabled = radioChecked;

            mGTEditPanel.ChangeButton.Enabled = !radioChecked;
        }

        /// <summary>
        /// 「台帳項目削除」ラジオボタンチェック時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            mGTEditPanel.DeleteButton.Enabled = radioButton5.Checked;
        }

        #endregion
        private void MgtCreatePanelViewButton_Click(object sender, EventArgs e)
        {
            string mgtText = mGTCreatePanel.mgtLabel.Text;
            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            dataGridView.DataSource = baseSQLQuerys.SelectTableManagerQuery(mgtText);
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            mGTCreatePanel.RecordCountLabel.Text = dataGridView.RowCount.ToString();

            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;

            // ダブルバッファ
            Type doubleBuffer = typeof(DataGridView);
            PropertyInfo propertyInfo = doubleBuffer.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            propertyInfo.SetValue(dataGridView, true, null);
        }

        /// <summary>
        /// 「台帳追加 - (CSV)」押下時実行処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMgtCSV_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = false;
            cofd.Multiselect = false;
            cofd.Filters.Add(new CommonFileDialogFilter("csv", "*.csv"));
            
            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                CSVConverter cSVConverter = new CSVConverter();
                DataTable dt = new DataTable();
                dt = cSVConverter.CreateDataTable(cofd.FileName, Encoding.UTF8);
                ImportDataTableForm importData = new ImportDataTableForm(dt);
                importData.Show();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterStrip_Click(object sender, EventArgs e)
        {
            MasterEditor masterEditor = new MasterEditor();
            masterEditor.ShowDialog(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="me"></param>
        private void DataGridView_CellClick(object sender, EventArgs e)
        {
            // カーソルの座標を取得
            Point p = Cursor.Position;

            if ((MouseButtons & MouseButtons.Right) == MouseButtons.Right)
            {
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            var propDef = Properties.Settings.Default;

            if (propDef.MainFormSize.Width == 0) propDef.Upgrade();

            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                Screen setScreen = Screen.AllScreens[i];
                if (setScreen.DeviceName == propDef.screenName)
                {
                    StartPosition = FormStartPosition.Manual;
                    Location = setScreen.Bounds.Location;
                }
            }

            if (propDef.MainFormSize.Width == 0 || propDef.MainFormSize.Height == 0)
            {
                // 現在は処理不要
            }
            else
            {
                WindowState = propDef.MainFormState;

                if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;

                Location = propDef.MainFormLocation;
                Size = propDef.MainFormSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            var propDef = Properties.Settings.Default;

            propDef.MainFormState = WindowState;

            if (WindowState == FormWindowState.Normal)
            {
                // WindowStateがNormalの場合には位置(Location)とサイズ(Size)を記憶
                propDef.MainFormLocation = Location;
                propDef.MainFormSize = Size;
            }
            else
            {
                // 最小化(Minimizes)または最大化(Maximized)の場合には、RestoreBoundsを記憶
                propDef.MainFormLocation = RestoreBounds.Location;
                propDef.MainFormSize = RestoreBounds.Size;
            }

            // 現在のスクリーン名を取得する
            propDef.screenName = Screen.FromControl(this).DeviceName;


            propDef.Save();
        }
    }
}
