namespace TrafficLightAlgorithm
{
    /// <summary>
    /// 車用・歩行者用信号機の点灯状態
    /// </summary>
    public enum LightState
    {
        /// <summary>
        /// 緑点灯状態
        /// </summary>
        Green,

        /// <summary>
        /// 黄点灯状態
        /// </summary>
        Yellow,

        /// <summary>
        /// 赤点灯状態
        /// </summary>
        Red,

        /// <summary>
        /// 矢印信号機点灯状態
        /// </summary>
        Arrow,

        /// <summary>
        /// 無灯火状態
        /// </summary>
        NoLight
    }
    
    /// <summary>
    /// 車用・歩行者用信号機の方角
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// 北方向
        /// </summary>
        North,

        /// <summary>
        /// 南方向
        /// </summary>
        South,

        /// <summary>
        /// 東方向
        /// </summary>
        East,

        /// <summary>
        /// 西方向
        /// </summary>
        West,

        /// <summary>
        /// 北南方向
        /// </summary>
        NorthSouth,

        /// <summary>
        /// 東西方向
        /// </summary>
        EastWest,

        /// <summary>
        /// 全方向
        /// </summary>
        All
    }

    /// <summary>
    /// 信号機の種類
    /// </summary>
    public enum Signal
    {
        /// <summary>
        /// 車用信号機
        /// </summary>
        Car,

        /// <summary>
        /// 歩行者用信号機
        /// </summary>
        Pedes,

        /// <summary>
        /// 全信号機
        /// </summary>
        All
    }
}
