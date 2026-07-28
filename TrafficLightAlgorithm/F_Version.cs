using System;
using System.Reflection;
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
            try
            {
                Version ver    = typeof(F_Version).Assembly.GetName().Version;
                string  verStr = $"Version {ver.Minor}.{ver.Build}{ver.Revision}";  // 「Version {マイナー番号}.{ビルド番号}{リビジョン番号}」形式の文字列を取得
                
                // 著作権と会社名のカスタム属性を取得
                Assembly assembly = Assembly.GetExecutingAssembly();
                AssemblyCopyrightAttribute copyRight = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute));
                AssemblyCompanyAttribute   company   = (AssemblyCompanyAttribute)  Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute));

                lbl_SoftTitle.Text = Program.SoftTitle;    // プログラムのタイトル
                lbl_Version.Text   = verStr;               // バージョン情報
                lbl_CopyRight.Text = copyRight.Copyright;  // 著作権情報
                lbl_Company.Text   = company.Company;      // 会社名情報
            }
            catch (Exception ex)
            {
                string exStr = "バージョン情報画面の読み込みでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// ラベルクリックでフォームを閉じる
        /// </summary>
        private void Lbl_VerInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                string exStr = "バージョン情報表示部分のクリックでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
