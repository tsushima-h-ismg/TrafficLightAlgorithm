using System;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    internal class CarTraffic
    {
        /// <summary>
        /// 信号機の緑ランプを表すラベル
        /// </summary>
        public readonly Label GreenLight;

        /// <summary>
        /// 信号機の黄ランプを表すラベル
        /// </summary>
        public readonly Label YellowLight;

        /// <summary>
        /// 信号機の赤ランプを表すラベル
        /// </summary>
        public readonly Label RedLight;

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

        public CarTraffic(Label lblGreen, Label lblYellow, Label lblRed, int greenSec, int yellowSec, int redSec, DateTime dateTime) 
        {
            GreenLight  = lblGreen;
            YellowLight = lblYellow;
            RedLight    = lblRed;

            GreenLightOnSec  = greenSec;
            YellowLightOnSec = yellowSec;
            RedLightOnSec    = redSec;

            CurrentTime = dateTime;
        }
    }
}
