using System;
using System.Drawing;

namespace TrafficLightAlgorithm
{
    class CarTraffic
    {
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
        /// 車用信号機に割り振る番号
        /// </summary>
        public readonly int CarTrafficNum;

        /// <summary>
        /// 信号機の点灯状態が切り替わった時間
        /// </summary>
        private DateTime StateChangeTime;

        /// <summary>
        /// 信号機の緑を表す色
        /// </summary>
        private readonly Color TrafficLightGreen = Color.ForestGreen;

        /// <summary>
        /// 信号機の黄を表す色
        /// </summary>
        private readonly Color TrafficLightYellow = Color.Yellow;

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
            Yellow, 
            Red,
            NoLight
        }

        /// <summary>
        /// 信号機の点灯状態
        /// </summary>
        private LightOnState LightOn;

        public CarTraffic(int greenSec, int yellowSec, int redSec, int num) 
        {
            GreenLightOnSec  = greenSec;
            YellowLightOnSec = yellowSec;
            RedLightOnSec    = redSec;
            CarTrafficNum    = num;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateTime"></param>
        public void SetStateTime(DateTime stateTime)
        {
            StateChangeTime = stateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime GetStateTime()
        {
            return StateChangeTime;
        }

        public bool Judge_TrafficLightOn(DateTime nowTime)
        {
            if (LightOn == LightOnState.Green)
            {
                if (nowTime >= StateChangeTime.AddSeconds(GreenLightOnSec)) return true;
            }
            else if (LightOn == LightOnState.Yellow) 
            {
                if (nowTime >= StateChangeTime.AddSeconds(YellowLightOnSec)) return true;
            }
            else if (LightOn == LightOnState.Red)
            {
                if (nowTime >= StateChangeTime.AddSeconds(RedLightOnSec)) return true;
            }

            return false;
        }

        /// <summary>
        /// 車用信号機の点灯を再開する際に、点灯状態を変更した時刻を更新する
        /// </summary>
        /// <param name="interruptTime"> 信号機の点灯処理を中断した時刻 </param>
        public void Update_StateChangeTime(DateTime interruptTime)
        {
            StateChangeTime = DateTime.Now.AddSeconds(StateChangeTime.Second - interruptTime.Second).AddMilliseconds(-DateTime.Now.Millisecond);
        }

        /// <summary>
        /// 点灯状態を更新する
        /// </summary>
        public void Update_LightOnState()
        {
            if (LightOn == LightOnState.Green)
            {
                LightOn = LightOnState.Yellow;
            }
            else if (LightOn == LightOnState.Yellow)
            {
                LightOn = LightOnState.Red;
            }
            else if (LightOn == LightOnState.Red)
            {
                LightOn = LightOnState.Green;
            }
        }

        public void UpdateStateGreen()
        {
            LightOn = LightOnState.Green;
        }

        public void UpdateStateYellow()
        {
            LightOn = LightOnState.Yellow;
        }

        public void UpdateStateRed()
        {
            LightOn = LightOnState.Red;
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
            Color[] colorArr = { TrafficNoLight, TrafficNoLight, TrafficNoLight };

            if (LightOn == LightOnState.Green)
            {
                colorArr[0] = TrafficLightGreen;
            }
            else if (LightOn == LightOnState.Yellow)
            {
                colorArr[1] = TrafficLightYellow;
            }
            else if (LightOn == LightOnState.Red)
            {
                colorArr[2] = TrafficLightRed;
            }

            return colorArr;
        }
    }
}
