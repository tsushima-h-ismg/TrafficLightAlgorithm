namespace TrafficLightAlgorithm
{
    /// <summary>
    /// 設定値(ミリ秒)を取得する構造体
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
        /// 交差点の進行方向切り替え準備時間(ミリ秒)
        /// </summary>
        public readonly int PMSec;

        /// <summary>
        /// 構造体の各値を設定する
        /// </summary>
        /// <param name="north">   北車用信号機の緑点灯秒数を表す文字列     </param>
        /// <param name="south">   南車用信号機の緑点灯秒数を表す文字列     </param>
        /// <param name="east">    東車用信号機の緑点灯秒数を表す文字列     </param>
        /// <param name="west">    西車用信号機の緑点灯秒数を表す文字列     </param>
        /// <param name="arrow">   矢印信号機の点灯秒数を表す文字列         </param>
        /// <param name="prepare"> 進行方向切り替え準備時間(秒)を表す文字列 </param>
        public WaitMSec(string north, string south, string east, string west, string arrow, string prepare)
        {
            int.TryParse(north,   out int nMSec);
            int.TryParse(south,   out int sMSec);
            int.TryParse(east,    out int eMSec);
            int.TryParse(west,    out int wMSec);
            int.TryParse(arrow,   out int arrMSec);
            int.TryParse(prepare, out int preMSec);

            NMSec = nMSec   * 1000;
            SMSec = sMSec   * 1000;
            EMSec = eMSec   * 1000;
            WMSec = wMSec   * 1000;
            AMSec = arrMSec * 1000;
            PMSec = preMSec * 1000;
        }
    }
}
