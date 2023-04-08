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
    public partial class DWGCreatePanel : UserControl
    {
        public DWGCreatePanel()
        {
            InitializeComponent();
            ComboItemsSetting();

            List<ComboBox> comboBoxes = new List<ComboBox> { comboBox1, comboBox2, comboBox3, comboBox4, comboBox5 };
            foreach(ComboBox comboBox in comboBoxes)
            {
                // comboBox1以外のEnabledをfalseへ変更
                if (comboBox != comboBox1) comboBox.Enabled = false;

                comboBox.SelectedIndex = -1;
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            comboBox1.SelectedIndexChanged += new EventHandler(ComboBox1_SelectedIndexChanged);
        }

        #region 図番種別選択欄処理群

        /// <summary>
        /// 図面サイズ
        /// </summary>
        private static string[,] aSize = new string[,] { { "A0", "0" }, { "A1", "1" }, { "A2", "2" }, { "A3", "3" }, { "A4", "4" } };

        private string dwgType = "";
        private string dwgSize = "";
        private string dwgClassifications = "";
        private string dwgProject = "";
        private string dwgProcess = "";

        /// <summary>
        /// 
        /// </summary>
        private void ComboItemsSetting()
        {
            string command = $@"SELECT `classifications`
                                FROM `classifications_master`
                                WHERE `classifications_name` = '図面区分';";
            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            DataTable dt = baseSQLQuerys.SelectCommandQuery(command);
            comboBox1.DataSource = dt;
            ComboMenberSetting(comboBox1);

            // ComboBox2の設定
            int aSizeLen = aSize.Length / 2;
            string[] aSizeChild = new string[aSizeLen];
            for (int i = 0; i < aSizeLen; i++)
            {
                aSizeChild[i] = aSize[i, 0];
            }
            comboBox2.DataSource = aSizeChild;

            // 「図番種別選択」各種ComboBoxプロパティ一括設定
            List<ComboBox> comboBoxes = new List<ComboBox> { comboBox1, comboBox2, comboBox3, comboBox4, comboBox5 };
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.SelectedIndex = -1;
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classificationName"></param>
        /// <returns></returns>
        private string ClassificationCommand(string classificationName)
        {
            string command = $@"SELECT `classifications`
                                FROM `classifications_master`
                                WHERE `classifications_name` = '{classificationName}';";

            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        private static string _Classifications = "classifications";
        
        /// <summary>
        /// 各comboBoxの「Display/ValueMember」設定
        /// </summary>
        /// <param name="comboBox"></param>
        private void ComboMenberSetting(ComboBox comboBox)
        {
            comboBox.ValueMember = _Classifications;
            comboBox.DisplayMember = _Classifications;
        }

        /// <summary>
        /// テーブルから抜き出してセットする
        /// </summary>
        /// <param name="comboBox"></param>
        /// <returns></returns>
        private string SelectCommandQueryStringReturn(ComboBox comboBox)
        {
            string tableName = "";
            string selectTableName = "";
            string whereTableName = "";

            string selectedItem = comboBox.SelectedValue.ToString();

            if (comboBox == comboBox2) return "";

            if (comboBox == comboBox5)
            {
                tableName = "manager_project_master";
                selectTableName = "project_number";
                whereTableName = "project_name";
            }
            else
            {
                tableName = "classifications_master";
                selectTableName = "classifications_sign";
                whereTableName = "classifications";
            }

            string command = $@"SELECT `{selectTableName}`
                               FROM `{tableName}`
                               WHERE `{whereTableName}` = '{selectedItem}';";

            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            DataTable dt = baseSQLQuerys.SelectCommandQuery(command);

            int dtItemLen = dt.Rows.Count;
            string dtItem = "";
            if (dtItemLen == 2)
            {
                dtItem = dt.Rows[1][0].ToString();
            }
            else
            {
                dtItem = dt.Rows[0][0].ToString();
            }

            return dtItem;
        }

        /// <summary>
        /// ComboBox1の項目選択時イベント
        /// </summary>
        /// <param name="sender">comboBox1</param>
        /// <param name="e">comboBox1</param>
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.SelectedIndexChanged -= new EventHandler(ComboBox3_SelectedIndexChanged);
            comboBox4.SelectedIndexChanged -= new EventHandler(ComboBox4_SelectedIndexChanged);
            comboBox5.SelectedIndexChanged -= new EventHandler(ComboBox5_SelectedIndexChanged);

            List<ComboBox> comboBoxes = new List<ComboBox> { comboBox4, comboBox5 };
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.Enabled = false;
            }

            string command = ClassificationCommand("分類区分");
            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            DataTable dt = baseSQLQuerys.SelectCommandQuery(command);

            ComboBox comboObj = (ComboBox)sender;
            string selectItem = comboObj.SelectedValue.ToString();

            DataRow dr = null;
            if (selectItem == "回路図面")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    string rowItem = dt.Rows[i][0].ToString();
                    if (rowItem == "機械") break;
                }
                dt.Rows.Remove(dr);
            }

            comboBox3.DataSource = dt;
            ComboMenberSetting(comboBox3);

            comboBoxes = new List<ComboBox> { comboBox2, comboBox3, comboBox4, comboBox5 };
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.Enabled = true;
                comboBox.SelectedIndex = -1;
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            comboBox2.SelectedIndexChanged += new EventHandler(ComboBox2_SelectedIndexChanged);
            comboBox3.SelectedIndexChanged += new EventHandler(ComboBox3_SelectedIndexChanged);

            comboBox4.Enabled = false;
            comboBox5.Enabled = false;

            dwgType = SelectCommandQueryStringReturn(comboObj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1) return;
            string selectItem = comboBox2.SelectedItem.ToString();

            int aSizeLen = aSize.Length / 2;
            for (int i = 0; i < aSizeLen; i++)
            {
                if (comboBox2.SelectedItem.ToString() == aSize[i, 0])
                {
                    dwgSize = aSize[i, 1];
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.SelectedIndexChanged -= new EventHandler(ComboBox4_SelectedIndexChanged);
            comboBox5.SelectedIndexChanged -= new EventHandler(ComboBox5_SelectedIndexChanged);

            List<ComboBox> comboBoxes = new List<ComboBox> { comboBox4, comboBox5 };
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.SelectedIndex = -1;
                comboBox.Enabled = false;
            }

            string command = ClassificationCommand("加工区分");
            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            DataTable dt = baseSQLQuerys.SelectCommandQuery(command);

            ComboBox comboObj = (ComboBox)sender;
            if (comboObj.SelectedIndex == -1) return;
            string selectItem = comboBox1.SelectedValue.ToString();
            string selectItem2 = comboBox3.SelectedValue.ToString();
            int rowsCount = dt.Rows.Count;
            DataRow dr = null;
            if (selectItem == "回路図面" || selectItem2 == "電気・電子")
            {
                for (int i = 0; i < rowsCount; i++)
                {
                    if (dt.Rows[0][0].ToString() != "その他")
                    {
                        dr = dt.Rows[0];
                        dt.Rows.Remove(dr);
                    }
                }
            }

            comboBox4.DataSource = dt;
            ComboMenberSetting(comboBox4);

            comboBox4.SelectedIndex = -1;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;

            if (comboBox4.Enabled == false)
            {
                comboBox4.Enabled = true;
                comboBox4.SelectedIndexChanged += new EventHandler(ComboBox4_SelectedIndexChanged);
            }
            else
            {
                comboBox4.Enabled = false;
                comboBox4.SelectedIndexChanged -= new EventHandler(ComboBox4_SelectedIndexChanged);
            }

            dwgClassifications = SelectCommandQueryStringReturn(comboObj);
        }

        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ComboBox5の設定
            string command = $@"SELECT `project_name`
                                FROM `manager_project_master`;";
            BaseSQLQuerys baseSQLQuerys = new BaseSQLQuerys();
            DataTable dt = baseSQLQuerys.SelectCommandQuery(command);
            comboBox5.DataSource = dt;
            comboBox5.DisplayMember = "project_name";
            comboBox5.ValueMember = "project_name";

            comboBox5.SelectedItem = -1;
            comboBox5.DropDownStyle = ComboBoxStyle.DropDownList;
            if (comboBox5.Enabled == false)
            {
                comboBox5.Enabled = true;
                comboBox5.SelectedIndexChanged += new EventHandler(ComboBox5_SelectedIndexChanged);
            }
            else
            {
                comboBox5.Enabled = false;
                comboBox5.SelectedIndexChanged -= new EventHandler(ComboBox5_SelectedIndexChanged);
            }

            ComboBox comboBox = (ComboBox)sender;
            dwgProcess = SelectCommandQueryStringReturn(comboBox);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == -1 ||
                comboBox4.SelectedValue.ToString() == "") return;

            ComboBox comboBox = (ComboBox)sender;
            dwgProject = SelectCommandQueryStringReturn(comboBox);
            
            string dwgNumber = dwgType + dwgSize + dwgClassifications + dwgProject + dwgProcess;

            label5.Text = dwgNumber;
        }

        #endregion

        #region



        #endregion
    }
}
