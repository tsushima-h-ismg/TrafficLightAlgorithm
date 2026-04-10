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
    /// 信号機の種類
    /// </summary>
    public enum Signal
    {
        /// <summary>
        /// 北車用信号機
        /// </summary>
        CarNorth,

        /// <summary>
        /// 南車用信号機
        /// </summary>
        CarSouth,
        
        /// <summary>
        /// 東車用信号機
        /// </summary>
        CarEast,
        
        /// <summary>
        /// 西車用信号機
        /// </summary>
        CarWest,

        /// <summary>
        /// 北歩行者用信号機
        /// </summary>
        PedesNorth,
        
        /// <summary>
        /// 南歩行者用信号機
        /// </summary>
        PedesSouth,
        
        /// <summary>
        /// 東歩行者用信号機
        /// </summary>
        PedesEast,
        
        /// <summary>
        /// 西歩行者用信号機
        /// </summary>
        PedesWest
    }
}
