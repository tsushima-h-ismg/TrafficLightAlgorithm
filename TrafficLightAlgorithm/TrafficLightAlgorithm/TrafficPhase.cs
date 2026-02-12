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
        /// 信号機の種類と点灯状態の配列
        /// </summary>
        public readonly TrafficCommand[] Commands;

        /// <summary>
        /// 信号機アルゴリズムの待機時間・点灯する信号機の種類・点灯状態を表す値を取得する
        /// </summary>
        /// <param name="waitMSec"> 点灯状態変更後の待機時間 </param>
        /// <param name="commands"> 信号機の種類と点灯状態   </param>
        public TrafficPhase(int waitMSec, params TrafficCommand[] commands)
        { 
            WaitMSec = waitMSec; 
            Commands = commands; 
        }
    }
}
