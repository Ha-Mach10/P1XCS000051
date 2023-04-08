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
    public partial class MGTPropertyViewEditor : Form
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mgtRecord"></param>
        public MGTPropertyViewEditor(List<string> mgtColumns, List<string> mgtRecord)
        {
            InitializeComponent();

            // 
            int recordCount = mgtRecord.Count;
            LabeledTextBoxesItemsSetting(recordCount);
            Panel2HeightSetting(recordCount);

            // 
            for (int i = 0; i < recordCount; i++)
            {
                labeledTextBoxes[i].LText = mgtColumns[i];
                labeledTextBoxes[i].TextBoxText = mgtRecord[i];
                labeledTextBoxes[i].TextBoxWidth = 460;
            }

            // 
            Load += new EventHandler(MGTPropertyViewEditor_Load);

            // 
            List<RadioButton> radioButtons = new List<RadioButton> { radioButton1, radioButton2 };
            foreach (RadioButton radioButton in radioButtons)
            {
                radioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            }
            radioButton1.Checked = true;

            // 
            button1.Click += new EventHandler(EditButton_Click);
            button2.Click += new EventHandler(CloseButton_Click);
        }

        private void MGTPropertyViewEditor_Load(object sender, EventArgs e)
        {
            // 
            PutonCenter putonCenter = new PutonCenter(Owner, this);
            Location = putonCenter.PutOnFormControlPosition(PutonCenter.PutOnStyle.Center);
        }

        /// <summary>
        /// 
        /// </summary>
        private List<LabeledTextBox> labeledTextBoxes = new List<LabeledTextBox>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlCount"></param>
        private void LabeledTextBoxesItemsSetting(int controlCount)
        {
            for (int i = 0; i < controlCount; i++)
            {
                LabeledTextBox labeledTextBox = new LabeledTextBox();
                labeledTextBoxes.Add(labeledTextBox);
            }

            for (int i = controlCount - 1; i >= 0; i--)
            {
                panel2.Controls.Add(labeledTextBoxes[i]);
                labeledTextBoxes[i].Dock = DockStyle.Top;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlCount"></param>
        private void Panel2HeightSetting(int controlCount)
        {
            // 
            int otherHeight = 39 + 46 + 29;

            int controlsHeight = labeledTextBoxes[0].Height + 3;

            int totalHeight = 0;
            for (int i = 0; i < controlCount; i++)
            {
                totalHeight = totalHeight + controlsHeight;
            }

            panel2.Height = totalHeight;

            Height = otherHeight + totalHeight + 3;

            MaximumSize = Size;
            MinimumSize = Size;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                LabeledTextBoxesReadOnlySet(true);
                button1.Enabled = false;
            }
            else if (radioButton2.Checked)
            {
                LabeledTextBoxesReadOnlySet(false);
                button1.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isReadOnry"></param>
        private void LabeledTextBoxesReadOnlySet(bool isReadOnry)
        {
            foreach (LabeledTextBox labeledTextBox in labeledTextBoxes)
            {
                labeledTextBox.TextBoxReadOnry = isReadOnry;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
