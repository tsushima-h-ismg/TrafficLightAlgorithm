using System;
using System.Drawing;

namespace TrafficLightAlgorithm
{
    class PedesTraffic
    {   
        /// <summary>
        /// 歩行者用信号機に割り振る番号
        /// </summary>
        public readonly int TrafficNum;

        private readonly int GreenOnSec;

        private readonly int RedOnSec;

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
        /// 歩行者用信号機の緑ランプ点滅間隔
        /// </summary>
        private const int PedesBlinkMSec = 500;

        /// <summary>
        /// 信号機の点灯状態を表す列挙型
        /// </summary>
        private enum LightOnState
        {
            Green,
            Red,
            Blink,
            NoLight
        }

        /// <summary>
        /// 信号機の点灯状態
        /// </summary>
        private LightOnState LightOn;

        private enum BlinkMem
        {
            Green,
            White
        }

        private BlinkMem BlinkState;

        public PedesTraffic(int carGreenSec, int redOnSec, int num, string colorName, DateTime startTime)
        {
            BlinkTime  = DateTime.Today;
            GreenOnSec = carGreenSec;
            RedOnSec   = redOnSec;
            TrafficNum = num;
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
        /// 点灯状態変更時刻を更新する
        /// </summary>
        /// <param name="stateTime"></param>
        private void UpdateStateChangeTime(DateTime stateTime)
        {
            StateChangeTime = stateTime.AddMilliseconds(-stateTime.Millisecond);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carTra1"></param>
        /// <param name="carTra2"></param>
        /// <returns></returns>
        public bool Judge_PedesLightOn()
        {
            if (LightOn == LightOnState.Green)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(GreenOnSec - 3).AddMilliseconds(-PedesBlinkMSec))
                {
                    return true;
                }
            }
            else if (LightOn == LightOnState.Blink)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(GreenOnSec - 2))
                {
                    return true;
                }
                else if (DateTime.Now >= BlinkTime.AddMilliseconds(PedesBlinkMSec))
                {
                    return true;
                }
            }
            else if (LightOn == LightOnState.Red)
            {
                if (DateTime.Now > StateChangeTime.AddSeconds(RedOnSec))
                {
                    return true;
                }
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
                LightOn = LightOnState.Blink;
                UpdateStateChangeTime(DateTime.Now);
            }
            else if (LightOn == LightOnState.Blink)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(GreenOnSec - 2))
                {
                    LightOn = LightOnState.Red;
                    UpdateStateChangeTime(DateTime.Now);
                }
                else if (DateTime.Now >= BlinkTime.AddMilliseconds(PedesBlinkMSec))
                {
                    if (BlinkState == BlinkMem.White)
                    {
                        BlinkState = BlinkMem.Green;
                    }
                    else if (BlinkState == BlinkMem.White)
                    {
                        BlinkState = BlinkMem.White;
                    }

                    BlinkTime = BlinkTime.AddMilliseconds(PedesBlinkMSec);
                }
            }
            else if (LightOn == LightOnState.Red)
            {
                LightOn = LightOnState.Green;
                UpdateStateChangeTime(DateTime.Now);
            }
        }

        /// <summary>
        /// 点灯状態を無灯火に更新する
        /// </summary>
        public void UpdateStateNoLight()
        {
            LightOn = LightOnState.NoLight;
        }

        /// <summary>
        /// 点灯状態に合わせて信号機の点灯色を返す
        /// </summary>
        /// <returns> 信号機の点灯色を表すColor型配列 </returns>
        public Color[] LightOnColor()
        {
            Color[] colorArr = { TrafficNoLight, TrafficNoLight, TrafficNoLight, TrafficNoLight };

            if (LightOn == LightOnState.Green || (LightOn == LightOnState.Blink && BlinkState == BlinkMem.Green))
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
