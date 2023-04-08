using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace P1XCS000051
{
    class PutonCenter
    {
        public enum PutOnStyle
        {
            Top = 0,
            TopLeft = 1,
            TopRight = 2,
            Left = 3,
            Right = 4,
            Bottom = 5,
            BottomLeft = 6,
            BottomRight = 7,
            Center = 8
        }

        // OwnerControlの幅・高さを取得する為の変数
        private int ownerWidth = 0;
        private int ownerHeight = 0;

        // ChiledControlの幅・高さを取得するための変数
        private int thisWidth = 0;
        private int thisHeight = 0;

        // OwnerControlの「幅・高さ」を取得する為の変数
        private int thisFromWidth = 0;
        private int thisFormHeight = 0;

        // OwnerControlの「X・Y座標」を取得する為の変数
        private int ownerLocationX = 0;
        private int ownerLocationY = 0;

        // 
        private Control setOwnerForm = new Control();
        private Control setThisForm = new Control();

        /// <summary>
        /// PutonCenterのコンストラクタ
        /// </summary>
        /// <param name="ownerForm">親フォーム</param>
        /// <param name="thisForm">子フォーム</param>
        public PutonCenter(Control ownerForm, Control thisForm)
        {
            setOwnerForm = ownerForm;
            setThisForm = thisForm;

            // OwnerControlの幅・高さを取得
            ownerWidth = ownerForm.Width;
            ownerHeight = ownerForm.Height;

            // ChiledControlの幅・高さを取得
            thisWidth = thisForm.Width;
            thisHeight = thisForm.Height;

            // OwnerControlに対するChiledControlの相対的な原点座標の計算
            thisFromWidth = (ownerWidth - thisWidth) / 2;
            thisFormHeight = (ownerHeight - thisHeight) / 2;

            // OwnerControlの原点座標を取得
            ownerLocationX = ownerForm.Location.X;
            ownerLocationY = ownerForm.Location.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        internal Point PutOnFormControlPosition(PutOnStyle position)
        {
            // X座標用の変数
            // X座標中央
            int xHorizontalCenter = thisFromWidth + ownerLocationX;
            // X座標右
            int xHorizontalLeft = ownerWidth - thisWidth + ownerLocationX;
            
            // Y座標用の変数
            // Y座標中央
            int yVerticalCenter = thisFormHeight + ownerLocationY;
            // Y座標下
            int yVerticalBottom = ownerHeight - thisHeight + ownerLocationY;

            Point point = new Point();
            switch (position.ToString())
            {
                case "Top":
                    point = new Point(xHorizontalCenter, ownerLocationY);
                    break;
                case "TopLeft":
                    point = new Point(xHorizontalLeft, ownerLocationY);
                    break;
                case "TopRignt":
                    point = new Point(ownerLocationX, ownerLocationY);
                    break;
                case "Left":
                    point = new Point(xHorizontalLeft, yVerticalCenter);
                    break;
                case "Right":
                    point = new Point(ownerLocationX, yVerticalCenter);
                    break;
                case "Bottom":
                    point = new Point(xHorizontalCenter, yVerticalBottom);
                    break;
                case "BottomLeft":
                    point = new Point(xHorizontalLeft, yVerticalBottom);
                    break;
                case "BottomRight":
                    point = new Point(ownerLocationX, yVerticalBottom);
                    break;
                case "Center":
                    point = new Point(xHorizontalCenter, yVerticalCenter);
                    break;
            }

            return point;
        }
    }
}
