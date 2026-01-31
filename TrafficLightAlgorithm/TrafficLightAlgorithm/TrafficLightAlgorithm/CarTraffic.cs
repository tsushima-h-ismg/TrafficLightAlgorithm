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
        
        public CarTraffic(int greenSec, int yellowSec, int redSec, int num, string colorName, DateTime startTime) 
        {
            GreenLightOnSec  = greenSec;
            YellowLightOnSec = yellowSec;
            RedLightOnSec    = redSec;
            CarTrafficNum    = num;
            UpdateStateChangeTime(startTime);

            if (colorName == "Green")
            {
                LightOn = LightOnState.Green;
            }
            else if (colorName == "Red")
            {
                LightOn = LightOnState.Red;
            }
        }

        /// <summary>
        /// 点灯状態を更新するか判定する
        /// </summary>
        /// <param name="nowTime"> 判定を行う時刻 </param>
        /// <returns> 判定結果を表すbool値 </returns>
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
        /// 点灯状態を更新する
        /// </summary>
        public void Update_LightOnState()
        {
            if (LightOn == LightOnState.Green)
            {
                LightOn = LightOnState.Yellow;  // 点灯状態を黄色に更新する
            }
            else if (LightOn == LightOnState.Yellow)
            {
                LightOn = LightOnState.Red;     // 点灯状態を赤色に更新する
            }
            else if (LightOn == LightOnState.Red)
            {
                LightOn = LightOnState.Green;   // 点灯状態を緑色に更新する
            }

            UpdateStateChangeTime(DateTime.Now);
        }

        /// <summary>
        /// 信号機の点灯状態を無灯火に更新する
        /// </summary>
        public void UpdateStateNoLight()
        {
            LightOn = LightOnState.NoLight;
            UpdateStateChangeTime(DateTime.Now);
        }
        
        /// <summary>
        /// 点灯状態変更時刻を更新する
        /// </summary>
        /// <param name="stateTime"> 点灯状態を更新した時刻 </param>
        private void UpdateStateChangeTime(DateTime stateTime)
        {
            StateChangeTime = stateTime.AddMilliseconds(-stateTime.Millisecond);
        }

        /// <summary>
        /// 車用信号機の点灯を中断状態から再開する際に、点灯状態を変更した時刻を更新する
        /// </summary>
        /// <param name="interruptTime"> 信号機の点灯処理を中断した時刻 </param>
        public void UpdateStateChangeResumeTime(DateTime interruptTime)
        {
            StateChangeTime = DateTime.Now.AddSeconds(StateChangeTime.Second - interruptTime.Second).AddMilliseconds(-DateTime.Now.Millisecond);
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
