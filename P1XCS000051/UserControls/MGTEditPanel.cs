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
    public partial class MGTEditPanel : UserControl
    {
        #region TextBoxesプロパティ
        public TextBox TextBoxID
        {
            get { return textBox1; }
            set { textBox1 = value; }
        }
        public TextBox TextBoxDevelopNumber
        {
            get { return textBox2; }
            set { textBox2 = value; }
        }
        public TextBox TextBoxDevelopName
        {
            get { return textBox3; }
            set { textBox3 = value; }
        }
        public TextBox TextBoxMajor
        {
            get { return textBox4; }
            set { textBox4 = value; }
        }
        public TextBox TextBoxMinor
        {
            get { return textBox5; }
            set { textBox5 = value; }
        }
        public TextBox TextBoxBuild
        {
            get { return textBox6; }
            set { textBox6 = value; }
        }
        public TextBox TextBoxDiversionNumber
        {
            get { return textBox7; }
            set { textBox7 = value; }
        }
        public TextBox TextBoxOldNumber
        {
            get { return textBox8; }
            set { textBox8 = value; }
        }
        public TextBox TextBoxNewNumber
        {
            get { return textBox9; }
            set { textBox9 = value; }
        }
        public TextBox TextBoxInheritenceNumber
        {
            get { return textBox10; }
            set { textBox10 = value; }
        }
        public TextBox TextBoxExplanation
        {
            get { return textBox11; }
            set { textBox11 = value; }
        }
        public TextBox TextBoxSummary
        {
            get { return textBox12; }
            set { textBox12 = value; }
        }
        #endregion
        #region ComboBoxesプロパティ
        public ComboBox ComboCreatedYear
        {
            get { return comboBox1; }
            set { comboBox1 = value; }
        }
        public ComboBox ComboCreatedMonth
        {
            get { return comboBox2; }
            set { comboBox2 = value; }
        }
        public ComboBox ComboCreatedDay
        {
            get { return comboBox3; }
            set { comboBox3 = value; }
        }
        public ComboBox ComboUseApplications
        {
            get { return comboBox4; }
            set { comboBox4 = value; }
        }
        public ComboBox ComboRevisionYear
        {
            get { return comboBox5; }
            set { comboBox5 = value; }
        }
        public ComboBox ComboRevisionMonth
        {
            get { return comboBox6; }
            set { comboBox5 = value; }
        }
        public ComboBox ComboRevisionDay
        {
            get { return comboBox7; }
            set { comboBox6 = value; }
        }
        #endregion
        #region Buttonプロパティ
        public Button ViewButton
        {
            get { return button1; }
            set { button1 = value; }
        }
        public Button ChangeButton
        {
            get { return button2; }
            set { button2 = value; }
        }
        public Button DeleteButton
        {
            get { return button3; }
            set { button3 = value; }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private IEnumerable<Control> GetSelfAndChiledrenRecursive(Control parent)
        {
            List<Control> controls = new List<Control>();

            foreach (Control child in parent.Controls)
            {
                controls.AddRange(GetSelfAndChiledrenRecursive(child));
            }

            controls.Add(parent);

            return controls;
        }
        /// <summary>
        /// MGTEditPanelのコンストラクタ
        /// </summary>
        public MGTEditPanel()
        {
            InitializeComponent();

            List<TextBox> textBoxes = new List<TextBox> { textBox4, textBox5, textBox6 };
            foreach(TextBox textBox in textBoxes)
            {
                textBox.KeyPress += new KeyPressEventHandler(TextBoxVersion_KeyPress);
                textBox.KeyUp += new KeyEventHandler(TextBoxVersion_KeyUp);
            }
        }

        private void TextBoxVersion_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            TextBox[] textBoxes = new TextBox[] { textBox4, textBox5, textBox6 };

            int count = 0;
            for(int i = 0; i < textBoxes.Length; i++)
            {
                if (textBox == textBoxes[i])
                {
                    count = i + 1;
                }
            }

            char key = e.KeyChar;
            char enterKey = (char)Keys.Enter;

            if (key == enterKey)
            {
                e.Handled = true;
                if (count == 3) return;
                textBoxes[count].Focus();
            }
        }
        private void TextBoxVersion_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            TextBox[] textBoxes = new TextBox[] { textBox4, textBox5, textBox6 };

            int count = 0;
            for (int i = 0; i < textBoxes.Length; i++)
            {
                if (textBox == textBoxes[i])
                {
                    count = i + 1;
                }
            }

            Keys key = e.KeyCode;
            Keys leftKey = Keys.Left;
            Keys rightKey = Keys.Right;
            if (key == rightKey)
            {
                e.Handled = true;
                if (count == 3) return;
                textBoxes[count].Focus();
            }
            else if (key == leftKey)
            {
                e.Handled = true;
                if (count == 1) return;
                count = count - 2;
                textBoxes[count].Focus();
            }
        }
        private void ControlsShiftEnter_KeyPress(object sender, KeyPressEventArgs e)
        {
            char key = e.KeyChar;
            char keyEnter = (char)Keys.LineFeed;
            bool pressingCtrlKey = ModifierKeys == Keys.Control;
            if (key == keyEnter && pressingCtrlKey)
            {
                e.Handled = true;
                button1.PerformClick();
            }
        }
    }
}
