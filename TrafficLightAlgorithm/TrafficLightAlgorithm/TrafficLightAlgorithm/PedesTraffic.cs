using System;
using System.Drawing;

namespace TrafficLightAlgorithm
{
    class PedesTraffic
    {   
        /// <summary>
        /// 歩行者用信号機が緑もしくは点滅している時間
        /// </summary>
        private readonly int GreenBlinkSec;

        /// <summary>
        /// 歩行者用信号機が赤に点灯する時間
        /// </summary>
        private readonly int RedSec;

        /// <summary>
        /// 歩行者用信号機に割り振る番号
        /// </summary>
        public readonly int PedesNum;

        /// <summary>
        /// 歩行者用信号機の緑ランプ点滅間隔
        /// </summary>
        private const int PedesBlinkMSec = 500;

        /// <summary>
        /// 信号機の点灯状態が切り替わった時間
        /// </summary>
        private DateTime StateChangeTime;

        /// <summary>
        /// 信号機の緑ランプの点滅を行った時刻
        /// </summary>
        private DateTime BlinkTime;

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
            Blink,
            Blink_Green,
            Blink_White,
            NoLight
        }

        /// <summary>
        /// 信号機の点灯状態
        /// </summary>
        private LightOnState LightOn;

        /// <summary>
        /// 信号機の点滅状態を表す列挙型
        /// </summary>
        private enum BlinkMem
        {
            Green,
            White
        }

        /// <summary>
        /// 信号機の点滅状態
        /// </summary>
        private BlinkMem BlinkState;

        public PedesTraffic(int num, int carGreenSec, int redOnSec, string colorName, DateTime startTime)
        {
            PedesNum    = num;
            BlinkTime     = DateTime.Today;
            GreenBlinkSec = carGreenSec;
            RedSec        = redOnSec;
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
        /// 点灯状態の変更を行うか判定を行う
        /// </summary>
        /// <returns> 点灯状態の変更を行う場合はtrue、それ以外の場合はfalse </returns>
        public bool JudgePedesLightOn()
        {
            if (LightOn == LightOnState.Green)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(GreenBlinkSec - 4).AddMilliseconds(-StateChangeTime.Millisecond))
                {
                    return true;
                }
            }
            else if (LightOn == LightOnState.Blink)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(GreenBlinkSec - 2).AddMilliseconds(-StateChangeTime.Millisecond))
                {
                    return true;
                }
                else if (DateTime.Now >= BlinkTime)
                {
                    return true;
                }
            }
            else if (LightOn == LightOnState.Red)
            {
                if (DateTime.Now > StateChangeTime.AddSeconds(RedSec).AddMilliseconds(-StateChangeTime.Millisecond))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 点灯状態を更新する
        /// </summary>
        public void UpdateLightOnState()
        {
            LightOnState nowState = LightOn;

            if (LightOn == LightOnState.Green)
            {
                LightOn    = LightOnState.Blink;  // 緑点灯から点滅
                BlinkState = BlinkMem.Green;
                BlinkTime  = DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond + PedesBlinkMSec);
            }
            else if (LightOn == LightOnState.Blink)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(GreenBlinkSec - 2).AddMilliseconds(-StateChangeTime.Millisecond))
                {
                 
                    LightOn = LightOnState.Red;   // 点滅から赤点灯
                }
                else if (DateTime.Now >= BlinkTime)
                {
                    // 点滅状態を更新する
                    if (BlinkState == BlinkMem.White)
                    {
                        BlinkState = BlinkMem.Green;
                    }
                    else if (BlinkState == BlinkMem.Green)
                    {
                        BlinkState = BlinkMem.White;
                    }

                    BlinkTime = BlinkTime.AddMilliseconds(PedesBlinkMSec);
                }
            }
            else if (LightOn == LightOnState.Red)
            {
                LightOn = LightOnState.Green;  // 赤点灯から緑点灯
            }

            // 点灯状態が変更した場合、点灯状態変更時刻を更新する
            if (nowState != LightOn) UpdateStateChangeTime(DateTime.Now);
        }

        /// <summary>
        /// 点灯状態を無灯火に更新する
        /// </summary>
        public void UpdateStateNoLight()
        {
            LightOn = LightOnState.NoLight;
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
        /// 歩行者用信号機の点灯を中断状態から再開する際に、点灯状態を変更した時刻を更新する
        /// </summary>
        /// <param name="interruptTime"> 信号機の点灯処理を中断した時刻 </param>
        public void UpdateStateChangeResumeTime(DateTime interruptTime)
        {
            StateChangeTime = DateTime.Now.AddSeconds(StateChangeTime.Second - interruptTime.Second).AddMilliseconds(-DateTime.Now.Millisecond);
            BlinkTime       = DateTime.Now.AddSeconds(BlinkTime.Second       - interruptTime.Second).AddMilliseconds(-DateTime.Now.Millisecond);
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
                // 点灯状態が緑もしくは点滅状態が緑の場合
                colorArr[0] = TrafficLightGreen;
                colorArr[1] = TrafficLightGreen;
            }
            else if (LightOn == LightOnState.Red)
            {
                // 点灯状態が赤の場合
                colorArr[2] = TrafficLightRed;
                colorArr[3] = TrafficLightRed;
            }

            return colorArr;
        }
    }
}