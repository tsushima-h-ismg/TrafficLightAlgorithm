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
                Version ver    = typeof(F_Version).Assembly.GetName().Version;      // バージョン情報を取得する
                string  verStr = $"Version {ver.Minor}.{ver.Build}{ver.Revision}";  // 「Version {マイナー番号}.{ビルド番号}{リビジョン番号}」形式の文字列を取得する

                // 著作権と会社名のカスタム属性を取得する
                Assembly assembly = Assembly.GetExecutingAssembly();
                AssemblyCopyrightAttribute copyRight = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute));
                AssemblyCompanyAttribute   company   = (AssemblyCompanyAttribute)  Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute));

                lbl_SoftTitle.Text = Program.SoftTitle;    // プログラムのタイトルを取得する
                lbl_Version.Text   = verStr;               // バージョン情報を取得する
                lbl_CopyRight.Text = copyRight.Copyright;  // 著作権情報を取得する
                lbl_Company.Text   = company.Company;      // 会社名情報を取得する
            }
            catch (Exception ex)
            {
                string exStr = "バージョン情報フォームのロードでエラーが発生しました。\n" + ex.Message;
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
                string exStr = "バージョン情報フォーム終了でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
