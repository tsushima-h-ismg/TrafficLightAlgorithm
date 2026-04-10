namespace TrafficLightAlgorithm
{
    /// <summary>
    /// 信号機アルゴリズムのフェーズ
    /// </summary>
    public class TrafficPhase
    {
        /// <summary>
        /// 点灯フェーズ毎に割り振る番号
        /// </summary>
        public readonly int PhaseNum;

        /// <summary>
        /// 点灯状態変更後の待機ミリ秒
        /// </summary>
        public readonly int WaitMSec;

        /// <summary>
        /// 信号機点灯状態の内容
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// 信号機の種類と点灯状態
        /// </summary>
        public readonly TrafficCommand[] Commands;

        /// <summary>
        /// 点灯状態変更内容を表す文字列・点灯状態変更後の待機ミリ秒・点灯する信号機の種類・点灯状態を取得する
        /// </summary>
        /// <param name="phaseNum"> 点灯フェーズの番号         </param>
        /// <param name="waitMSec"> 点灯状態変更後の待機ミリ秒 </param>
        /// <param name="message">  点灯状態の内容             </param>
        /// <param name="commands"> 信号機の種類と点灯状態     </param>
        public TrafficPhase(int phaseNum, int waitMSec, string message, params TrafficCommand[] commands)
        { 
            PhaseNum = phaseNum;
            WaitMSec = waitMSec;
            Message  = message;
            Commands = commands; 
        }
    }
}
