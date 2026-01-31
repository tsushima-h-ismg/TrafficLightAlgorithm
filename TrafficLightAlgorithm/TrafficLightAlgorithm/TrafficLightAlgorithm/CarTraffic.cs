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
        /// 矢印信号機が点灯する前の黄色灯火時間
        /// </summary>
        private readonly int YellowOneLightOnSec;

        /// <summary>
        /// 矢印信号機が点灯した後の黄色灯火時間
        /// </summary>
        private readonly int YellowTwoLightOnSec;

        /// <summary>
        /// 矢印信号機が点灯する前の赤色灯火時間
        /// </summary>
        private readonly int RedOneLightOnSec;

        /// <summary>
        /// 矢印信号機が点灯する前の赤色灯火時間
        /// </summary>
        private readonly int RedTwoLightOnSec;

        /// <summary>
        /// 矢印信号機の灯火時間
        /// </summary>
        private readonly int ArrowLightOnSec;

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
        /// 矢印信号機の緑を表す色
        /// </summary>
        private readonly Color ArrowGreen = Color.Green;

        /// <summary>
        /// 矢印信号機の無灯火を表す色
        /// </summary>
        private readonly Color ArrowDefault = Color.Black;

        /// <summary>
        /// 信号機の点灯状態を表す列挙型
        /// </summary>
        private enum LightOnState
        {
            Green,
            Yellow_One, 
            Red_One,
            Arrow,
            Yellow_Two,
            Red_Two,
            NoLight
        }

        /// <summary>
        /// 信号機の点灯状態
        /// </summary>
        private LightOnState LightOn;

        public CarTraffic(int[] setArray, string colorName)
        {
            CarTrafficNum       = setArray[0];
            GreenLightOnSec     = setArray[1];
            YellowOneLightOnSec = setArray[2];
            YellowTwoLightOnSec = setArray[2];
            RedOneLightOnSec    = setArray[3];
            RedTwoLightOnSec    = setArray[4];
            ArrowLightOnSec     = setArray[5];

            if (ArrowLightOnSec == 0) YellowTwoLightOnSec = 0;

            if (colorName == "Green")
            {
                LightOn = LightOnState.Green;
                UpdateStateChangeTime(DateTime.Now);
            }
            else if (colorName == "Red")
            {
                LightOn = LightOnState.Red_Two;
                UpdateStateChangeTime(DateTime.Now.AddSeconds(setArray[6]));
            }
        }

        /// <summary>
        /// 車用信号機の点灯状態が一巡するまでに必要な秒数を返す
        /// </summary>
        /// <returns> 秒数を表す数値 </returns>
        public int SecCount()
        {
            return GreenLightOnSec + YellowOneLightOnSec + YellowTwoLightOnSec + RedOneLightOnSec + RedTwoLightOnSec + ArrowLightOnSec;
        }

        /// <summary>
        /// 点灯状態を更新するか判定する
        /// </summary>
        /// <returns> 点灯状態を更新する場合はtrue、それ以外の場合はfalse </returns>
        public bool JudgeTrafficLightOn()
        {
            if (LightOn == LightOnState.Green)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(GreenLightOnSec)) return true;
            }
            else if (LightOn == LightOnState.Yellow_One)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(YellowOneLightOnSec)) return true;
            }
            else if (LightOn == LightOnState.Red_One)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(RedOneLightOnSec)) return true;
            }
            else if (LightOn == LightOnState.Arrow)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(ArrowLightOnSec)) return true;
            }
            else if (LightOn == LightOnState.Yellow_Two)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(YellowTwoLightOnSec)) return true;
            }
            else if (LightOn == LightOnState.Red_Two)
            {
                if (DateTime.Now >= StateChangeTime.AddSeconds(RedTwoLightOnSec)) return true;
            }

            return false;
        }

        /// <summary>
        /// 点灯状態を更新する
        /// </summary>
        public void UpdateLightOnState()
        {
            if (LightOn == LightOnState.Red_Two)
            {
                LightOn = LightOnState.Green;  // 点灯状態がRed_Twoの場合、Greenに移行する
            }
            else if (LightOn == LightOnState.Yellow_One && ArrowLightOnSec == 0)
            {
                LightOn = LightOnState.Red_Two;  // 矢印信号機が存在しない場合、点灯状態はYellow_OneからRed_Twoへ移行する
            }
            else if (LightOn != LightOnState.NoLight)
            {
                LightOn++;  // 次の点灯状態へ移行する
            }

            UpdateStateChangeTime(DateTime.Now);
        }

        /// <summary>
        /// 信号機の点灯状態を無灯火に更新する
        /// </summary>
        public void UpdateStateNoLight()
        {
            LightOn = LightOnState.NoLight;
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
        /// <returns> 信号機の点灯色を表すColor型配列 </returns>
        public Color[] LightOnColor()
        {
            Color[] colorArr = { TrafficNoLight, TrafficNoLight, TrafficNoLight, ArrowDefault};

            if (LightOn == LightOnState.Green)
            {
                colorArr[0] = TrafficLightGreen;
            }
            else if (LightOn == LightOnState.Yellow_One || LightOn == LightOnState.Yellow_Two)
            {
                colorArr[1] = TrafficLightYellow;
            }
            else if (LightOn == LightOnState.Red_One || LightOn == LightOnState.Red_Two)
            {
                colorArr[2] = TrafficLightRed;
            }
            else if (LightOn == LightOnState.Arrow)
            {
                colorArr[2] = TrafficLightRed;
                colorArr[3] = ArrowGreen;
            }

            return colorArr;
        }
    }
}
