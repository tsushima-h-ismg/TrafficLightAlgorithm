using System;
using System.Drawing;

namespace TrafficLightAlgorithm
{
    class CarTraffic
    {
        /// <summary>
        /// 信号機の緑色灯火時間
        /// </summary>
        private readonly int GreenLightOnSec;

        /// <summary>
        /// 信号機の黄色灯火時間
        /// </summary>
        private readonly int YellowLightOnSec;

        /// <summary>
        /// 信号機の赤色灯火時間
        /// </summary>
        private int RedLightOnSec;

        /// <summary>
        /// 矢印信号機の灯火時間
        /// </summary>
        private readonly int ArrowLightOnSec;

        /// <summary>
        /// 信号機の点灯状態が切り替わった時間
        /// </summary>
        private DateTime StateChangeTime;

        /// <summary>
        /// 信号機の緑を表す色
        /// </summary>
        private readonly Color TrafficLightGreen  = Color.ForestGreen;

        /// <summary>
        /// 信号機の黄を表す色
        /// </summary>
        private readonly Color TrafficLightYellow = Color.Yellow;

        /// <summary>
        /// 信号機の赤を表す色
        /// </summary>
        private readonly Color TrafficLightRed    = Color.Red;

        /// <summary>
        /// 信号機の無灯火を表す色
        /// </summary>
        private readonly Color TrafficNoLight     = Color.White;
        
        /// <summary>
        /// 矢印信号機の緑を表す色
        /// </summary>
        private readonly Color ArrowGreen         = Color.Green;

        /// <summary>
        /// 矢印信号機の無灯火を表す色
        /// </summary>
        private readonly Color ArrowDefault       = Color.Black;

        /// <summary>
        /// 矢印信号機の操作を行う場合はtrue、それ以外の場合はfalse
        /// </summary>
        public readonly bool IsArrow;

        /// <summary>
        /// 矢印信号機が存在して矢印信号機が点灯する前の場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsBeforeArrLightOn;

        /// <summary>
        /// 信号機の緑ランプの点灯色
        /// </summary>
        public Color LightOnGreen;

        /// <summary>
        /// 信号機の黄ランプの点灯色
        /// </summary>
        public Color LightOnYellow;

        /// <summary>
        /// 信号機の赤ランプの点灯色
        /// </summary>
        public Color LightOnRed;

        /// <summary>
        /// 矢印信号機の点灯色
        /// </summary>
        public Color LightOnArrow;

        /// <summary>
        /// 信号機の点灯状態を表す列挙型
        /// </summary>
        private enum LightOnState
        {
            Green,
            Yellow,
            Red,
            Arrow,
            NoLight
        }

        /// <summary>
        /// 信号機の点灯状態
        /// </summary>
        private LightOnState LightOn;
        
        public CarTraffic(int greenSec, int yellowSec, int arrowSec, bool isArrow, bool greenStart)
        {
            GreenLightOnSec  = greenSec;
            YellowLightOnSec = yellowSec;
            ArrowLightOnSec  = arrowSec;

            IsArrow = isArrow;
            IsBeforeArrLightOn = true;

            LightOn = LightOnState.Red;
            if (greenStart) LightOn = LightOnState.Green;

            UpdateStateChangeTime(DateTime.Now);
            SetLightOnColor();
        }

        /// <summary>
        /// 車用信号機の点灯状態が一巡するまでに必要な秒数を返す
        /// </summary>
        /// <returns> 秒数を表す数値 </returns>
        public int SecCount()
        {
            return 10;
        }

        /// <summary>
        /// 点灯状態を更新するか判定する
        /// </summary>
        /// <returns> 点灯状態を更新する場合はtrue、それ以外の場合はfalse </returns>
        public bool JudgeTrafficLightOn()
        {
            if (LightOn == LightOnState.Green  && DateTime.Now >= StateChangeTime.AddSeconds(GreenLightOnSec))  return true;
            if (LightOn == LightOnState.Yellow && DateTime.Now >= StateChangeTime.AddSeconds(YellowLightOnSec)) return true;
            if (LightOn == LightOnState.Red    && DateTime.Now >= StateChangeTime.AddSeconds(RedLightOnSec))    return true;
            if (LightOn == LightOnState.Arrow  && DateTime.Now >= StateChangeTime.AddSeconds(ArrowLightOnSec))  return true; 
            return false;
        }

        /// <summary>
        /// 点灯状態を更新する
        /// </summary>
        public void UpdateLightOnState()
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
                if (IsArrow && IsBeforeArrLightOn)
                {
                    LightOn = LightOnState.Arrow;
                    IsBeforeArrLightOn = false;
                }
                else
                {
                    LightOn = LightOnState.Green;
                    IsBeforeArrLightOn = true;
                }
            }
            else if (LightOn == LightOnState.Arrow)
            {
                LightOn = LightOnState.Yellow;
            }

            UpdateStateChangeTime(DateTime.Now);
            SetLightOnColor();
        }

        /// <summary>
        /// 信号機の点灯状態を無灯火に更新する
        /// </summary>
        public void UpdateStateNoLight()
        {
            LightOn = LightOnState.NoLight;
            SetLightOnColor();
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
        /// 点灯状態に合わせて信号機の点灯色を設定する
        /// </summary>
        private void SetLightOnColor()
        {
            LightOnGreen  = TrafficNoLight;
            LightOnYellow = TrafficNoLight;
            LightOnRed    = TrafficNoLight;
            LightOnArrow  = ArrowDefault;
            
            if (LightOn == LightOnState.Green)
            {
                LightOnGreen = TrafficLightGreen;
            }
            else if (LightOn == LightOnState.Yellow)
            {
                LightOnYellow = TrafficLightYellow;
            }
            else if (LightOn == LightOnState.Red)
            {
                LightOnRed = TrafficLightRed;
            }
            else if (LightOn == LightOnState.Arrow)
            {
                LightOnRed = TrafficLightRed;
                LightOnArrow = ArrowGreen;
            }
        }
    }
}
