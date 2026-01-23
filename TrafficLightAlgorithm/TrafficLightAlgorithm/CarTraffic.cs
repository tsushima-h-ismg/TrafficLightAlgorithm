using System;

namespace TrafficLightAlgorithm
{
    internal class CarTraffic
    {
        /// <summary>
        /// 信号機の緑ランプを表すラベルの名称
        /// </summary>
        public readonly string GreenLabelName;

        /// <summary>
        /// 信号機の黄ランプを表すラベルの名称
        /// </summary>
        public readonly string YellowLabelName;

        /// <summary>
        /// 信号機の赤ランプを表すラベルの名称
        /// </summary>
        public readonly string RedLabelName;

        /// <summary>
        /// 信号機の緑色灯火時間
        /// </summary>
        public readonly int GreenLightOnSec;

        /// <summary>
        /// 信号機の黄色灯火時間
        /// </summary>
        public readonly int YellowLightOnSec;

        /// <summary>
        /// 信号機の赤色灯火時間
        /// </summary>
        public readonly int RedLightOnSec;

        /// <summary>
        /// 信号機の現在の点灯状態
        /// </summary>
        public string LightState;

        /// <summary>
        /// 信号機の点灯状態が切り替わった時間
        /// </summary>
        public DateTime CurrentTime;

        public CarTraffic(string lblGreenName, string lblYellowName, string lblRedName, int greenSec, int yellowSec, int redSec, DateTime dateTime) 
        {
            GreenLabelName  = lblGreenName;
            YellowLabelName = lblYellowName;
            RedLabelName    = lblRedName;

            GreenLightOnSec  = greenSec;
            YellowLightOnSec = yellowSec;
            RedLightOnSec    = redSec;

            CurrentTime = dateTime.AddMilliseconds(-dateTime.Millisecond);
        }
    }
}
