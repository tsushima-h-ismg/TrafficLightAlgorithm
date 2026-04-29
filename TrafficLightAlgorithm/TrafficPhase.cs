namespace TrafficLightAlgorithm
{
    /// <summary>
    /// 信号機アルゴリズムのフェーズ
    /// </summary>
    public class TrafficPhase
    {
        /// <summary>
        /// 点灯状態変更後の待機ミリ秒
        /// </summary>
        public readonly int WaitMSec;

        /// <summary>
        /// 点滅状態の合計ミリ秒
        /// </summary>
        public readonly int BlinkMSec;

        /// <summary>
        /// 点滅開始フェーズの場合はtrue、それ以外の場合はfalse
        /// </summary>
        public readonly bool IsBlinkStart;

        /// <summary>
        /// 点滅を行うフェーズの場合はtrue、それ以外の場合はfalse
        /// </summary>
        public readonly bool IsBlink;

        /// <summary>
        /// 信号機の種類と点灯状態
        /// </summary>
        public readonly TrafficCommand[] Commands;

        /// <summary>
        /// 点灯状態変更後の待機ミリ秒・点灯する信号機の種類・点灯状態を取得する
        /// </summary>
        /// <param name="waitMSec">     点灯状態変更後の待機ミリ秒                                        </param>
        /// <param name="commands">     信号機の種類と点灯状態                                            </param>
        /// <param name="isBlinkStart"> 信号機の点滅を開始するフェーズの場合はtrue、それ以外の場合はfalse </param>
        /// <param name="isBlink">      信号機の点滅を行うフォームの場合はtrue、それ以外の場合はfalse     </param>
        /// <param name="phaseCount">   点滅を行うフェーズの数                                            </param>
        public TrafficPhase(int waitMSec, TrafficCommand[] commands, bool isBlinkStart = false, bool isBlink = false, int phaseCount = 0)
        { 
            WaitMSec     = waitMSec;
            Commands     = commands;
            IsBlinkStart = isBlinkStart;
            IsBlink      = isBlink;
            BlinkMSec    = waitMSec * phaseCount;
        }

        /// <summary>
        /// 点灯状態変更内容を返す
        /// </summary>
        /// <returns> 点灯状態変更内容を表す文字列 </returns>
        public string GetMsg()
        {
            string msg = "";

            for(int i = 0; i < Commands.Length; i++)
            {
                // 方角
                if      (Commands[i].Direction == Direction.All)        msg += "全";
                else if (Commands[i].Direction == Direction.NorthSouth) msg += "北南";
                else if (Commands[i].Direction == Direction.EastWest)   msg += "東西";
                else if (Commands[i].Direction == Direction.North)      msg += "北";
                else if (Commands[i].Direction == Direction.South)      msg += "南";
                else if (Commands[i].Direction == Direction.East)       msg += "東";
                else if (Commands[i].Direction == Direction.West)       msg += "西";

                // 信号機
                if      (Commands[i].Signal == Signal.All)   msg += "車用・歩行者用信号が";
                else if (Commands[i].Signal == Signal.Car && Commands[i].State != LightState.ArrowRed) msg += "車用信号が";
                else if (Commands[i].Signal == Signal.Car && Commands[i].State == LightState.ArrowRed) msg += "矢印信号が";
                else if (Commands[i].Signal == Signal.Pedes) msg += "歩行者用信号が";

                // 点灯状態
                if (IsBlink)
                {
                    msg += "点滅しました。";
                }
                else if (Commands[i].State == LightState.ArrowRed)
                {
                    msg += "点灯しました。";
                }
                else
                {
                    if      (Commands[i].State == LightState.Green)   msg += "緑";
                    else if (Commands[i].State == LightState.Yellow)  msg += "黄";
                    else if (Commands[i].State == LightState.Red)     msg += "赤";
                    else if (Commands[i].State == LightState.NoLight) msg += "無灯火";

                    if (i == Commands.Length - 1)
                    {
                        if      (Commands[i].State != LightState.NoLight) msg += "に点灯しました。";
                        else if (Commands[i].State == LightState.NoLight) msg += "になりました。";
                    }
                    else
                    {
                        msg += "・";
                    }
                }
            }

            int mSecValue = WaitMSec;                 // ミリ秒数の値が入る
            if (IsBlinkStart) mSecValue = BlinkMSec;  // 点滅開始フェーズの場合、点滅にかける待機ミリ秒の合計が入る
            msg += mSecValue / 1000F + "秒待機します。";

            return msg;
        }
    }
}
