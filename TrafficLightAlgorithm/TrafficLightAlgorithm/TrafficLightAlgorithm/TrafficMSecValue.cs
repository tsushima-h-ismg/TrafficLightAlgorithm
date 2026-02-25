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
        /// <param name="nSec"> 北方向への進行可能秒数 </param>
        /// <param name="sSec"> 南方向への進行可能秒数 </param>
        /// <param name="eSec"> 東方向への進行可能秒数 </param>
        /// <param name="wSec"> 西方向への進行可能秒数 </param>
        /// <param name="aSec"> 矢印信号機の点灯秒数   </param>
        /// <param name="rSec"> 全信号機の赤点灯秒数   </param>
        public WaitMSec(int nSec, int sSec, int eSec, int wSec, int aSec, int rSec)
        {
            // 取得した秒数からミリ秒を算出する
            NMSec = nSec * 1000;
            SMSec = sSec * 1000;
            EMSec = eSec * 1000;
            WMSec = wSec * 1000;
            AMSec = aSec * 1000;
            RMSec = rSec * 1000;
        }
    }
}
