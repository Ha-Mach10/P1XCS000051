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
    public partial class LabeledTextBox : Panel
    {
        /// <summary>
        /// 
        /// </summary>
        internal Action _CallBack = null;
        public void CallBack()
        {
            Refresh();
        }

        /// <summary>
        /// TextBoxのプロパティ
        /// </summary>
        private TextBox _textBox = new TextBox();
        public string TextBoxText
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; _CallBack(); }
        }
        public int TextBoxWidth
        {
            get
            {
                if (_textBox.Width >= 100)
                {
                    return _textBox.Width;
                }
                return 100;
            }
            set { _textBox.Width = value; _CallBack(); }
        }
        public bool TextBoxReadOnry
        {
            get { return _textBox.ReadOnly; }
            set { _textBox.ReadOnly = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LText { get; set; } 

        /// <summary>
        /// LabeledTextBoxのコンストラクタ
        /// </summary>
        public LabeledTextBox()
        {
            _CallBack = CallBack;

            InitializeComponent();
            Paint += new PaintEventHandler(LabeldTextBox_Paint);

            // コントロールの初期サイズの設定
            Width = 200;
            Height = 24;

            // コントロールの最大サイズの設定
            int maxWidth = 5000;
            int maxHeight = 18 + 6;
            MaximumSize = new Size(maxWidth, maxHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pe"></param>
        /// <param name="font"></param>
        private void CreateTextRectangle(PaintEventArgs pe, Font font)
        {
            // 最適な矩形を取得するための初期文字列
            string text = "text";

            // 初期文字列をコントロールへ描画
            TextRenderer.DrawText(pe.Graphics, text, font, new Point(0, 0), Color.Black);

            // 描画したテキストのRectangleをサイズオブジェクトで取得する
            Size textSize = TextRenderer.MeasureText(pe.Graphics, text, font);

            // 塗りつぶし用ブラシ作成
            Brush brush = new SolidBrush(SystemColors.Control);

            // initialTextを削除するための矩形作成
            Rectangle deleteRec = new Rectangle(0, 0, textSize.Width, textSize.Height);

            // 塗りつぶし削除
            pe.Graphics.FillRectangle(brush, deleteRec);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pe"></param>
        /// <param name="pen"></param>
        /// <param name="font"></param>
        /// <param name="color"></param>
        private void DrawTextAndColon(PaintEventArgs pe, Font font, Color color)
        {
            _CallBack = CallBack;

            // テキスト描画用の矩形を作成
            Rectangle rectangle = new Rectangle(0, 3, Width - _textBox.Width, 18);

            // LabeledTextBoxのWidthを決める
            Width = rectangle.Width + _textBox.Width;

            // テキストを描画
            TextFormatFlags textFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(pe.Graphics, LText, font, rectangle, color, textFlags);

            // テキストとtextBox間に「:」を描画する
            TextFormatFlags colonFlags = TextFormatFlags.Right | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(pe.Graphics, "：", font, rectangle, color, colonFlags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pe"></param>
        private void LabeldTextBox_Paint(object sender, PaintEventArgs pe)
        {
            _CallBack = CallBack;

            _textBox.Location = new Point(Width - _textBox.Width, 3);
            Controls.Add(_textBox);

            //
            Color color = Color.Black;
            Font InitialFont = new Font("ＭＳ ゴシック", 9);

            DrawTextAndColon(pe, Font, color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
