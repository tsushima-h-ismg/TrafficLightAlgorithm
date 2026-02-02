namespace TrafficLightAlgorithm
{
    /// <summary>
    /// 信号機の種類と点灯状態の情報
    /// </summary>
    public class TrafficCommand
    {
        /// <summary>
        /// 設置した信号機の種類
        /// </summary>
        public readonly Traffic Traffic;

        /// <summary>
        /// 信号機の点灯状態
        /// </summary>
        public readonly LightState State;

        /// <summary>
        /// 信号機と点灯状態を初期化する
        /// </summary>
        /// <param name="traffic"> 信号機の種類   　</param>
        /// <param name="state">   信号機の点灯状態 </param>
        public TrafficCommand(Traffic traffic, LightState state)
        {
            Traffic = traffic;
            State = state;
        }
    }
}
