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
                Version ver = typeof(F_Version).Assembly.GetName().Version;
                Assembly assembly = Assembly.GetExecutingAssembly();
                AssemblyCopyrightAttribute copyRight = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute));
                AssemblyCompanyAttribute   company   = (AssemblyCompanyAttribute)  Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute));

                lbl_SoftTitle.Text = Program.SoftTitle;                                 // プログラムのタイトル
                lbl_Version.Text   = $"Version {ver.Minor}.{ver.Build}{ver.Revision}";  // バージョン情報
                lbl_CopyRight.Text = copyRight.Copyright;                               // 著作権
                lbl_Company.Text   = company.Company;                                   // 会社名
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
