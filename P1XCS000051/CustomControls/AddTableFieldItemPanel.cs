using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Dynamic;

namespace P1XCS000051
{
    public partial class AddTableFieldItemPanel : GroupBox
    {
        // 
        private int _tableCount;
        public int TableCount
        {
            get
            {
                if (_tableCount >= 0)
                {
                    return _tableCount;
                }
                return _tableCount = 0;
            }
            set { _tableCount = value; _CallBack(); }
        }

        // 表示するテキストボックスの幅用プロパティ
        private int _textBoxWidth;
        public int TextBoxWidth
        {
            get
            {
                if (_textBoxWidth > 100)
                {
                    return _textBoxWidth;
                }
                return _textBoxWidth = 100;
            }
            set { _textBoxWidth = value; _CallBack(); }
        }

        private LabeledTextBox[] _labeledTextBoxes;
        public LabeledTextBox[] LabeledTextBoxes
        {
            get { return _labeledTextBoxes; }
            set
            {
                for (int i = 0; i < _labeledTextBoxes.Count(); i++)
                {
                    if (Equals(_labeledTextBoxes[i], value[i])) return;
                    _labeledTextBoxes[i] = value[i];
                }
                _CallBack();
            }
        }
        private List<string> Values { get; set; } = new List<string>();
        public string this[int index] { get { return Values[index]; } set{ Values[index] = value; } }

        /// <summary>
        /// コールバック変数及び関数の定義
        /// </summary>
        internal Action _CallBack = null;
        public void CallBack()
        {
            Refresh();
        }

        /// <summary>
        /// AddTableFieldItemPanelのコンストラクタ
        /// </summary>
        public AddTableFieldItemPanel()
        {
            _CallBack = CallBack;

            InitializeComponent();
        }

        private int ctrlCount = 0;
        protected override void OnPaint(PaintEventArgs pe)
        {
            _CallBack = CallBack;

            int lTextCount = 0;
            if (_labeledTextBoxes != null || ctrlCount != 0) lTextCount = _labeledTextBoxes.Length;
            if (_labeledTextBoxes == null || ctrlCount != lTextCount) Controls.Clear();

            _labeledTextBoxes = null;

            _labeledTextBoxes = new LabeledTextBox[_tableCount];
            Height = 18 + _tableCount * 22 + 3;

            for (int i = 0; i < _tableCount; i++)
            {
                _labeledTextBoxes[i] = new LabeledTextBox();

                _labeledTextBoxes[i].Location = new Point(0, 18 + i * 22);

                _labeledTextBoxes[i].TextBoxWidth = TextBoxWidth;

                Controls.Add(_labeledTextBoxes[i]);
            }

            ctrlCount = lTextCount;

            base.OnPaint(pe);
        }
    }
}
