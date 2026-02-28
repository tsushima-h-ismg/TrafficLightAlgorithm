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
        /// フォームロードイベント
        /// </summary>
        private void F_Version_Load(object sender, EventArgs e)
        {
            Version ver    = typeof(F_Version).Assembly.GetName().Version;      // バージョン情報を取得する
            string  verstr = $"Version {ver.Minor}.{ver.Build}{ver.Revision}";  // {マイナー番号}.{ビルド番号}{リビジョン番号}形式の文字列を取得する

            lbl_SoftTitle.Text = Program.SoftTitle;  // ラベルにプログラムのタイトルを表示する
            lbl_Version.Text   = verstr;             // ラベルにバージョン情報を表示する
        }

        /// <summary>
        /// ラベルクリックでフォームを閉じる
        /// </summary>
        private void Lbl_VerInfo_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
