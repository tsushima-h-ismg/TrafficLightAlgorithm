using System;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    internal static class Program
    {
        // 固定メンバー配置
        public static readonly string SoftTitle = "信号機プログラム";

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new F_TrafficLight());
        }
    }
}
