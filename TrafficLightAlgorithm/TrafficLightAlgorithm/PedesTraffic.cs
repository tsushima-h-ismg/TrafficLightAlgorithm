using System;

namespace TrafficLightAlgorithm
{
    internal class PedesTraffic
    {
        /// <summary>
        /// 信号機の緑ランプを表すラベルの名称
        /// </summary>
        public readonly string GreenLabelName;

        /// <summary>
        /// 信号機の緑ランプを表すラベルの名称2
        /// </summary>
        public readonly string GreenLabelName2;

        /// <summary>
        /// 信号機の赤ランプを表すラベルの名称
        /// </summary>
        public readonly string RedLabelName;

        /// <summary>
        /// 信号機の赤ランプを表すラベルの名称2
        /// </summary>
        public readonly string RedLabelName2;
        
        /// <summary>
        /// 信号機の現在の点灯状態
        /// </summary>
        public string LightState;

        /// <summary>
        /// 信号機青ランプの点滅開始時刻
        /// </summary>
        public DateTime BlinkStartTime;

        public PedesTraffic(string greLabelName, string greLabelName2, string redLabelName, string redLabelName2)
        {
            GreenLabelName  = greLabelName;
            GreenLabelName2 = greLabelName2;
            RedLabelName    = redLabelName;
            RedLabelName2   = redLabelName2;
            LightState      = "";
            BlinkStartTime  = DateTime.Now;
        }
    }
}
