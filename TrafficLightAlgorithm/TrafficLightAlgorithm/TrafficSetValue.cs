namespace TrafficLightAlgorithm
{
    /// <summary>
    /// 信号機アルゴリズムの設定値構造体
    /// </summary>
    public readonly struct WaitMSec
    {
        /// <summary>
        /// 北車用信号機の緑点灯ミリ秒
        /// </summary>
        public readonly int NMSec;

        /// <summary>
        /// 南車用信号機の緑点灯ミリ秒
        /// </summary>
        public readonly int SMSec;

        /// <summary>
        /// 東車用信号機の緑点灯ミリ秒
        /// </summary>
        public readonly int EMSec;

        /// <summary>
        /// 西車用信号機の緑点灯ミリ秒
        /// </summary>
        public readonly int WMSec;

        /// <summary>
        /// 矢印信号機の点灯ミリ秒
        /// </summary>
        public readonly int AMSec;

        /// <summary>
        /// 交差点の進行方向切り替え準備ミリ秒
        /// </summary>
        public readonly int PMSec;

        /// <summary>
        /// 構造体の各値を設定する
        /// </summary>
        /// <param name="nSecStr"> 北車用信号機の緑点灯秒数を表す文字列     </param>
        /// <param name="sSecStr"> 南車用信号機の緑点灯秒数を表す文字列     </param>
        /// <param name="eSecStr"> 東車用信号機の緑点灯秒数を表す文字列     </param>
        /// <param name="wSecStr"> 西車用信号機の緑点灯秒数を表す文字列     </param>
        /// <param name="aSecStr"> 矢印信号機の点灯秒数を表す文字列         </param>
        /// <param name="pSecStr"> 進行方向切り替え準備時間(秒)を表す文字列 </param>
        public WaitMSec(string nSecStr, string sSecStr, string eSecStr, string wSecStr, string aSecStr, string pSecStr)
        {
            // 文字列をint型変数に変換する
            int.TryParse(nSecStr, out int nSec);
            int.TryParse(sSecStr, out int sSec);
            int.TryParse(eSecStr, out int eSec);
            int.TryParse(wSecStr, out int wSec);
            int.TryParse(aSecStr, out int aSec);
            int.TryParse(pSecStr, out int pSec);

            // 秒数を表す数値をミリ秒に変換する
            NMSec = nSec * 1000;
            SMSec = sSec * 1000;
            EMSec = eSec * 1000;
            WMSec = wSec * 1000;
            AMSec = aSec * 1000;
            PMSec = pSec * 1000;
        }
    }
}
