using System;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    public partial class F_Version : Form
    {
        public F_Version()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ラベルクリックでフォームを閉じる
        /// </summary>
        private void Lbl_verInfo_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
