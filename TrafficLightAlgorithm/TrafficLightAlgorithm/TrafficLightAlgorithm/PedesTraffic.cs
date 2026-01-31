using System;
using System.Drawing;

namespace TrafficLightAlgorithm
{
    class PedesTraffic
    {   
        /// <summary>
        /// 信号機の現在の点灯状態
        /// </summary>
        public string LightState;

        /// <summary>
        /// 信号機の点灯状態が切り替わった時間
        /// </summary>
        private DateTime StateChangeTime;

        /// <summary>
        /// 信号機の緑ランプの点滅を行った時刻
        /// </summary>
        public DateTime BlinkTime;

        /// <summary>
        /// 信号機の緑を表す色
        /// </summary>
        private readonly Color TrafficLightGreen = Color.ForestGreen;

        /// <summary>
        /// 信号機の赤を表す色
        /// </summary>
        private readonly Color TrafficLightRed = Color.Red;

        /// <summary>
        /// 信号機の無灯火を表す色
        /// </summary>
        private readonly Color TrafficNoLight = Color.White;

        /// <summary>
        /// 信号機の点灯状態を表す列挙型
        /// </summary>
        private enum LightOnState
        {
            Green,
            Red,
            Blink_Green,
            Blink_White,
            NoLight
        }

        private LightOnState LightOn;

        /// <summary>
        /// 
        /// </summary>
        public readonly int TrafficNum;

        public PedesTraffic(int num)
        {
            LightState      = "";
            BlinkTime       = DateTime.Today;
            StateChangeTime = DateTime.Today;
            TrafficNum = num;
        }

        public void SetStateTime(DateTime stateTime)
        {
            StateChangeTime = stateTime;
        }

        public bool Judge_TrafficLightOn(PedesTraffic pedes)
        {
            

            return false;
        }

        /// <summary>
        /// 点灯状態を更新する
        /// </summary>
        public void Update_LightOnState()
        {
            if (LightOn == LightOnState.Green)
            {

            }
            else if (LightOn == LightOnState.Red)
            {
                
            }
            else if (LightOn == LightOnState.Blink_Green || LightOn == LightOnState.Blink_White)
            {

            }
        }

        public void UpdateStateGreen() 
        {
            LightOn = LightOnState.Green;
        }

        public void UpdateStateRed()
        {
            LightOn = LightOnState.Red;
        }

        public void UpdateStateBlinkGreen()
        {
            LightOn = LightOnState.Blink_Green;
        }

        public void UpdateStateBlinkWhite()
        {
            LightOn = LightOnState.Blink_White;
        }

        public void UpdateStateNoLight()
        {
            LightOn = LightOnState.NoLight;
        }

        /// <summary>
        /// 点灯状態に合わせて信号機の点灯色を返す
        /// </summary>
        /// <returns> 信号機の点灯色を表す色 </returns>
        public Color[] LightOnColor()
        {
            Color[] colorArr = { TrafficNoLight, TrafficNoLight, TrafficNoLight, TrafficNoLight };

            if (LightOn == LightOnState.Green || LightOn == LightOnState.Blink_Green)
            {
                colorArr[0] = TrafficLightGreen;
                colorArr[1] = TrafficLightGreen;
            }
            else if (LightOn == LightOnState.Red)
            {
                colorArr[2] = TrafficLightRed;
                colorArr[3] = TrafficLightRed;
            }

            return colorArr;
        }
    }
}
