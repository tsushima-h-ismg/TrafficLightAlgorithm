namespace TrafficLightAlgorithm
{
    /// <summary>
    /// 信号機の種類・点灯状態・方角
    /// </summary>
    public class TrafficCommand
    {
        /// <summary>
        /// 信号機の方角
        /// </summary>
        public readonly Direction Direction;

        /// <summary>
        /// 信号機の種類
        /// </summary>
        public readonly Signal Signal;

        /// <summary>
        /// 信号機の点灯状態
        /// </summary>
        public readonly LightState State;

        /// <summary>
        /// 信号機の種類・点灯状態・方角を表す値を取得する
        /// </summary>
        /// <param name="state">     信号機の点灯状態     </param>
        /// <param name="signal">    信号機の種類         </param>
        /// <param name="direction"> 信号機を設置した方角 </param>
        public TrafficCommand(Direction direction = Direction.All, Signal signal = Signal.All, LightState state = LightState.Red)
        {
            State     = state;
            Signal    = signal;
            Direction = direction;
        }
    }
}