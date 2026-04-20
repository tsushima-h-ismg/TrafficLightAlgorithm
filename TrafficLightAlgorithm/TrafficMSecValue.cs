namespace TrafficLightAlgorithm
{
    /// <summary>
    /// ミリ秒設定値構造体
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
        /// 構造体の各値を設定する
        /// </summary>
        /// <param name="nMSec"> 北方向への進行可能ミリ秒数 </param>
        /// <param name="sMSec"> 南方向への進行可能ミリ秒数 </param>
        /// <param name="eMSec"> 東方向への進行可能ミリ秒数 </param>
        /// <param name="wMSec"> 西方向への進行可能ミリ秒数 </param>
        /// <param name="aMSec"> 矢印信号機の点灯ミリ秒数   </param>
        public WaitMSec(int nMSec, int sMSec, int eMSec, int wMSec, int aMSec)
        {
            NMSec = nMSec;
            SMSec = sMSec;
            EMSec = eMSec;
            WMSec = wMSec;
            AMSec = aMSec;
        }
    }
}
