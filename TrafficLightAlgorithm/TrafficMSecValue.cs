namespace TrafficLightAlgorithm
{
    /// <summary>
    /// ミリ秒設定値構造体
    /// </summary>
    public struct TrafficMSecValue
    {
        /// <summary>
        /// 北方向車用信号機の進行可能ミリ秒
        /// </summary>
        public int CarNMSec;

        /// <summary>
        /// 南方向車用信号機の進行可能ミリ秒
        /// </summary>
        public int CarSMSec;

        /// <summary>
        /// 東方向車用信号機の進行可能ミリ秒
        /// </summary>
        public int CarEMSec;

        /// <summary>
        /// 西方向車用信号機の進行可能ミリ秒
        /// </summary>
        public int CarWMSec;

        /// <summary>
        /// 北南方向歩行者用信号機の進行可能ミリ秒数
        /// </summary>
        public int PedNSMSec;

        /// <summary>
        /// 東西方向歩行者用信号機の進行可能ミリ秒数
        /// </summary>
        public int PedEWMSec;

        /// <summary>
        /// 矢印信号機の点灯ミリ秒数
        /// </summary>
        public int ArwMSec;

        /// <summary>
        /// 構造体の各値を設定する
        /// </summary>
        /// <param name="carNMSec">  北方向車用信号機の進行可能ミリ秒数       </param>
        /// <param name="carSMSec">  南方向車用信号機の進行可能ミリ秒数       </param>
        /// <param name="carEMSec">  東方向車用信号機の進行可能ミリ秒数       </param>
        /// <param name="carWMSec">  西方向車用信号機の進行可能ミリ秒数       </param>
        /// <param name="pedNSMSec"> 北南方向歩行者用信号機の進行可能ミリ秒数 </param>
        /// <param name="pedEWMSec"> 東西方向歩行者用信号機の進行可能ミリ秒数 </param>
        /// <param name="arrowMSec"> 矢印信号機の点灯ミリ秒数                 </param>
        public TrafficMSecValue(int carNMSec, int carSMSec, int carEMSec, int carWMSec, int pedNSMSec, int pedEWMSec, int arrowMSec)
        {
            CarNMSec  = carNMSec;
            CarSMSec  = carSMSec;
            CarEMSec  = carEMSec;
            CarWMSec  = carWMSec;
            PedNSMSec = pedNSMSec;
            PedEWMSec = pedEWMSec;
            ArwMSec   = arrowMSec;
        }
    }
}
