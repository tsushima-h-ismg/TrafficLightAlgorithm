namespace TrafficLightAlgorithm
{
    /// <summary>
    /// ミリ秒設定値
    /// </summary>
    public class TrafficMSecValues
    {
        /// <summary>
        /// 北方向車用信号機の進行可能ミリ秒
        /// </summary>
        public readonly int CarNMSec;

        /// <summary>
        /// 南方向車用信号機の進行可能ミリ秒
        /// </summary>
        public readonly int CarSMSec;

        /// <summary>
        /// 東方向車用信号機の進行可能ミリ秒
        /// </summary>
        public readonly int CarEMSec;

        /// <summary>
        /// 西方向車用信号機の進行可能ミリ秒
        /// </summary>
        public readonly int CarWMSec;

        /// <summary>
        /// 北南方向歩行者用信号機の進行可能ミリ秒数
        /// </summary>
        public readonly int PedNSMSec;

        /// <summary>
        /// 東西方向歩行者用信号機の進行可能ミリ秒数
        /// </summary>
        public readonly int PedEWMSec;

        /// <summary>
        /// 矢印信号機の点灯ミリ秒数
        /// </summary>
        public readonly int ArwMSec;

        /// <summary>
        /// 各ミリ秒数の値を設定する
        /// </summary>
        /// <param name="carNMSec">  北方向車用信号機の進行可能ミリ秒数       </param>
        /// <param name="carSMSec">  南方向車用信号機の進行可能ミリ秒数       </param>
        /// <param name="carEMSec">  東方向車用信号機の進行可能ミリ秒数       </param>
        /// <param name="carWMSec">  西方向車用信号機の進行可能ミリ秒数       </param>
        /// <param name="pedNSMSec"> 北南方向歩行者用信号機の進行可能ミリ秒数 </param>
        /// <param name="pedEWMSec"> 東西方向歩行者用信号機の進行可能ミリ秒数 </param>
        /// <param name="arrowMSec"> 矢印信号機の点灯ミリ秒数                 </param>
        public TrafficMSecValues(int carNMSec, int carSMSec, int carEMSec, int carWMSec, int pedNSMSec, int pedEWMSec, int arrowMSec)
        {
            CarNMSec = carNMSec;
            CarSMSec = carSMSec;
            CarEMSec = carEMSec;
            CarWMSec = carWMSec;
            PedNSMSec = pedNSMSec;
            PedEWMSec = pedEWMSec;
            ArwMSec = arrowMSec;
        }

        /// <summary>
        /// 信号機ごとにミリ秒数の値を変更する
        /// </summary>
        /// <param name="avaiMSec">  信号機の進行可能ミリ秒数         </param>
        /// <param name="arwMSec">   矢印信号機の点灯ミリ秒数         </param>
        /// <param name="signal">    信号機の種類を表す列挙型         </param>
        /// <param name="direction"> 信号機を設置した方角を表す列挙型 </param>
        /// <returns> 変更後の </returns>
        public TrafficMSecValues ChangeMSec(int avaiMSec, int arwMSec, Signal signal, Direction direction)
        {
            if (signal == Signal.Car)
            {
                switch (direction)
                {
                    case Direction.North:
                        return new TrafficMSecValues(avaiMSec, CarSMSec, CarEMSec, CarWMSec, PedNSMSec, PedEWMSec, arwMSec);
                    case Direction.South:
                        return new TrafficMSecValues(CarNMSec, avaiMSec, CarEMSec, CarWMSec, PedNSMSec, PedEWMSec, arwMSec);
                    case Direction.East:
                        return new TrafficMSecValues(CarNMSec, CarSMSec, avaiMSec, CarWMSec, PedNSMSec, PedEWMSec, arwMSec);
                    case Direction.West:
                        return new TrafficMSecValues(CarNMSec, CarSMSec, CarEMSec, avaiMSec, PedNSMSec, PedEWMSec, arwMSec);
                }
            }
            else if (signal == Signal.Pedes)
            {
                switch (direction)
                {
                    case Direction.North:
                    case Direction.South:
                        return new TrafficMSecValues(CarNMSec, CarSMSec, CarEMSec, CarWMSec, avaiMSec, PedEWMSec, arwMSec);
                    case Direction.East:
                    case Direction.West:
                        return new TrafficMSecValues(CarNMSec, CarSMSec, CarEMSec, CarWMSec, PedNSMSec, avaiMSec, arwMSec);
                }
            }

            return new TrafficMSecValues(CarNMSec, CarSMSec, CarEMSec, CarWMSec, PedNSMSec, PedEWMSec, arwMSec);
        }
    }
}
