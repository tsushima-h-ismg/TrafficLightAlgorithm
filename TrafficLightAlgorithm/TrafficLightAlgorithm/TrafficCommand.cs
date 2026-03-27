namespace TrafficLightAlgorithm
{
    /// <summary>
    /// 信号機の種類と点灯状態
    /// </summary>
    public class TrafficCommand
    {
        /// <summary>
        /// 信号機の種類
        /// </summary>
        public readonly Signal Signal;

        /// <summary>
        /// 信号機の点灯状態
        /// </summary>
        public readonly LightState State;

        /// <summary>
        /// 信号機の種類と点灯状態を表す値を取得する
        /// </summary>
        /// <param name="traffic"> 信号機の種類   　</param>
        /// <param name="state">   信号機の点灯状態 </param>
        public TrafficCommand(Signal signal, LightState state)
        {
            Signal = signal;
            State  = state;
        }
    }
}