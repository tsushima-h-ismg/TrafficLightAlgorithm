using System;
using System.Drawing;

namespace TrafficLightAlgorithm
{
    class PedesTraffic
    {   
        /// <summary>
        /// 歩行者用信号機が緑もしくは点滅している時間
        /// </summary>
        private readonly int GreenSec;

        /// <summary>
        /// 歩行者用信号機が赤に点灯する時間
        /// </summary>
        private readonly int RedSec;

        /// <summary>
        /// 歩行者用信号機が点滅する時間
        /// </summary>
        private const int BlinkSec = 3;

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

        /// <summary>
        /// 歩行者用信号機の緑ランプの点灯色
        /// </summary>
        public Color LightOnGreen;

        /// <summary>
        /// 歩行者用信号機の赤ランプの点灯色
        /// </summary>
        public Color LightOnRed;

        public PedesTraffic(int greenSec, int redSec, DateTime startTime, bool greenStart)
        {
            BlinkTime     = DateTime.Today;
            GreenSec      = greenSec;
            RedSec        = redSec;
            UpdateStateChangeTime(startTime);

            LightOn = LightOnState.Red;
            if (greenStart) LightOn = LightOnState.Green;
        }
        
        /// <summary>
        /// 点灯状態の変更を行うか判定を行う
        /// </summary>
        /// <returns> 点灯状態の変更を行う場合はtrue、それ以外の場合はfalse </returns>
        public bool JudgePedesLightOn()
        {
            // 緑点灯で点灯状態更新からGreenSec - 4秒以上経過した場合に点灯状態を変更する
            if (LightOn == LightOnState.Green && DateTime.Now >= StateChangeTime.AddSeconds(GreenSec - 4)) return true;
            
            // 点滅状態で点灯状態更新からBlinkSec秒経過、もしくは現在時刻がBlinkTime以降になった場合に点灯状態を変更する
            if (LightOn == LightOnState.Blink && (DateTime.Now >= StateChangeTime.AddSeconds(BlinkSec) || DateTime.Now >= BlinkTime)) return true;
            
            // 赤点灯で点灯状態更新からRedSec秒以上経過した場合に点灯状態を変更する
            if (LightOn == LightOnState.Red && DateTime.Now >= StateChangeTime.AddSeconds(RedSec)) return true;
            
            return false;
        }

        /// <summary>
        /// 点灯状態を更新する
        /// </summary>
        public void UpdateLightOnState()
        {
            LightOnState nowState = LightOn;  // 点灯状態を取得する

            if (LightOn == LightOnState.Green)
            {
                LightOn    = LightOnState.Blink;  // 緑点灯から点滅
                BlinkState = BlinkMem.Green;      // 緑に点滅

                BlinkTime  = DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond + PedesBlinkMSec);  // 点滅を実行する時刻を更新
            }
            else if (LightOn == LightOnState.Blink)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(BlinkSec))
                {
                    LightOn = LightOnState.Red;  // 点滅から赤点灯
                }
                else if (DateTime.Now >= BlinkTime)
                {
                    if (BlinkState == BlinkMem.White)
                    {
                        BlinkState = BlinkMem.Green;  // 点滅状態を緑にする
                    }
                    else if (BlinkState == BlinkMem.Green)
                    {
                        BlinkState = BlinkMem.White;  // 点滅状態を無灯火にする
                    }

                    BlinkTime = BlinkTime.AddMilliseconds(PedesBlinkMSec);  // 点滅を実行する時刻を更新
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
    }
}