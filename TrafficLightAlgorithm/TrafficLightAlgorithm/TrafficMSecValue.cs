namespace TrafficLightAlgorithm
{
    /// <summary>
    /// 信号機アルゴリズムの設定値構造体
    /// </summary>
    public readonly struct WaitMSec
    {
        /// <summary>
        /// 北方向への進行可能ミリ秒
        /// </summary>
        public readonly int NMSec;

        /// <summary>
        /// 南方向への進行可能ミリ秒
        /// </summary>
        public readonly int SMSec;

        /// <summary>
        /// 東方向への進行可能ミリ秒
        /// </summary>
        public readonly int EMSec;

        /// <summary>
        /// 西方向への進行可能ミリ秒
        /// </summary>
        public readonly int WMSec;

        /// <summary>
        /// 矢印信号機の点灯ミリ秒
        /// </summary>
        public readonly int AMSec;

        /// <summary> 
        /// 全信号機の赤点灯ミリ秒
        /// </summary>
        public readonly int RMSec;

        /// <summary>
        /// 構造体の各値を設定する
        /// </summary>
        /// <param name="nSecStr"> 北方向への進行可能秒数を表す文字列 </param>
        /// <param name="sSecStr"> 南方向への進行可能秒数を表す文字列 </param>
        /// <param name="eSecStr"> 東方向への進行可能秒数を表す文字列 </param>
        /// <param name="wSecStr"> 西方向への進行可能秒数を表す文字列 </param>
        /// <param name="aSecStr"> 矢印信号機の点灯秒数を表す文字列   </param>
        /// <param name="rSecStr"> 全信号機の赤点灯秒数を表す文字列   </param>
        public WaitMSec(string nSecStr, string sSecStr, string eSecStr, string wSecStr, string aSecStr, string rSecStr)
        {
            // 文字列をint型変数に変換する
            int.TryParse(nSecStr, out int nSec);
            int.TryParse(sSecStr, out int sSec);
            int.TryParse(eSecStr, out int eSec);
            int.TryParse(wSecStr, out int wSec);
            int.TryParse(aSecStr, out int aSec);
            int.TryParse(rSecStr, out int rSec);

            // 秒数を表す数値をミリ秒に変換する
            NMSec = nSec * 1000;
            SMSec = sSec * 1000;
            EMSec = eSec * 1000;
            WMSec = wSec * 1000;
            AMSec = aSec * 1000;
            RMSec = rSec * 1000;
        }
    }
}
