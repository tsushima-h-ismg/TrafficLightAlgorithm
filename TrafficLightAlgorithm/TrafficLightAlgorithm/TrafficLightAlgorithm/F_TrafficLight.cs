using System;
using System.Threading;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    public partial class F_TrafficLight : Form
    {
        /// <summary>
        /// 車用信号機の緑色灯火秒数の最大値
        /// </summary>
        private const int BlueLightOnSecMax = 20;

        /// <summary>
        /// 車用信号機の緑色灯火秒数の最小値
        /// </summary>
        private const int BlueLightOnSecMin = 5;

        /// <summary>
        /// 交差点の進行方向切り替え準備秒数の最大値
        /// </summary>
        private const int PrepareSecMax = 5;

        /// <summary>
        /// 交差点の進行方向切り替え準備秒数の最小値
        /// </summary>
        private const int PrepareSecMin = 1;

        /// <summary>
        /// 車用信号機の黄色灯火時間
        /// </summary>
        private const int YellowSec = 1;

        /// <summary>
        /// 信号機の点灯処理を中断した時刻
        /// </summary>
        private DateTime InterruptTime;

        /// <summary>
        /// フォーム画面の「開始」ボタンクリックでtrue、「終了」ボタンクリックもしくはフォームロード時でfalse
        /// </summary>
        private bool IsTrafficEnable;

        /// <summary>
        /// 信号機点灯処理を中断している場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsInterrupt;

        /// <summary>
        /// 北方向車用信号機
        /// </summary>
        private CarTraffic NorthLight;

        /// <summary>
        /// 南方向車用信号機
        /// </summary>
        private CarTraffic SouthLight;
        
        /// <summary>
        /// 東方向車用信号機
        /// </summary>
        private CarTraffic EastLight;
        
        /// <summary>
        /// 西方向車用信号機
        /// </summary>
        private CarTraffic WestLight;

        /// <summary>
        /// 北方向歩行者用信号機
        /// </summary>
        private PedesTraffic NorthPedesLight;

        /// <summary>
        /// 南方向歩行者用信号機
        /// </summary>
        private PedesTraffic SouthPedesLight;

        /// <summary>
        /// 東方向歩行者用信号機
        /// </summary>
        private PedesTraffic EastPedesLight;

        /// <summary>
        /// 西方向歩行者用信号機
        /// </summary>
        private PedesTraffic WestPedesLight;

        /// <summary>
        /// 歩行者用信号機の緑ランプ点滅間隔
        /// </summary>
        private const int PedesBlinkMSec = 500;


        private CarTraffic[] carTrafficArr = new CarTraffic[4];

        private PedesTraffic[] pedesTrafficArr;

        /// <summary>
        /// 車用信号機イメージラベルからなる配列
        /// </summary>
        private Label[,] CarLabelArr;

        /// <summary>
        /// 歩行者用信号機イメージラベルからなる配列
        /// </summary>
        private Label[,] PedesLabelArr;

        public F_TrafficLight()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロードイベント
        /// </summary>
        private void F_TrafficLight_Load(object sender, EventArgs e)
        {
            IsInterrupt     = false;
            IsTrafficEnable = false;

            CarLabelArr = new Label[,]{
                { lbl_NorthGreen, lbl_NorthYellow, lbl_NorthRed },
                { lbl_SouthGreen, lbl_SouthYellow, lbl_SouthRed },
                { lbl_EastGreen,  lbl_EastYellow,  lbl_EastRed },
                { lbl_WestGreen,  lbl_WestYellow,  lbl_WestRed }
            };

            PedesLabelArr = new Label[,] {
                { lbl_NorthPedesGreenOne, lbl_NorthPedesGreenTwo, lbl_NorthPedesRedOne, lbl_NorthPedesRedTwo },
                { lbl_SouthPedesGreenOne, lbl_SouthPedesGreenTwo, lbl_SouthPedesRedOne, lbl_SouthPedesRedTwo },
                { lbl_EastPedesGreenOne,  lbl_EastPedesGreenTwo,  lbl_EastPedesRedOne,  lbl_EastPedesRedTwo },
                { lbl_WestPedesGreenOne,  lbl_WestPedesGreenTwo,  lbl_WestPedesRedOne,  lbl_WestPedesRedTwo }
            };

            pedesTrafficArr = new PedesTraffic[] { NorthPedesLight, SouthPedesLight, EastPedesLight, WestPedesLight };
        }

        /// <summary>
        /// 「開始」ボタンクリック時イベント
        /// </summary>
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            string inputErrStr = Create_ErrMsg();  // エラーメッセージを取得する

            // エラーメッセージ表示
            if (inputErrStr != "")
            {
                MessageBox.Show(inputErrStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 信号機点灯処理を中断している場合
            if (IsInterrupt)
            {
                string errMsg = "信号機の点灯処理を中断しています。処理を最初から実行しますか？";
                DialogResult dialogResult = MessageBox.Show(errMsg, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.No) return;
            }

            IsTrafficEnable = true;
            IsInterrupt     = false;
            
            // 現在時刻のミリ秒が100から200の間の秒数になるまでスレッドを中断する
            while (DateTime.Now.Millisecond < 100 || DateTime.Now.Millisecond > 200)
            {
                Thread.Sleep(10);
            }

            Change_InterruptResumeBtnTxt(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            Change_TxtEnable(false);                    // テキストボックスのEnabledプロパティ値変更
            Initialize_CarLight();                      // 車用信号機を初期状態に設定する
            Initialize_PedesLight();                    // 歩行者用信号機を初期状態に設定する
            Timer_Traffic.Start();
        }

        /// <summary>
        /// 「中断/再開」ボタンクリック時イベント
        /// </summary>
        private void Btn_InterruptResume_Click(object sender, EventArgs e)
        {
            if (IsTrafficEnable)
            {
                // 開始ボタンが押されて終了ボタンが押されていない間はIsInterruptのtrue/falseを切り替える
                if (IsInterrupt)
                {
                    IsInterrupt = false;
                }
                else
                {
                    IsInterrupt = true;
                }
            }
            else
            {
                return;
            }

            Change_InterruptResumeBtnTxt(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更

            if (IsInterrupt)
            {
                InterruptTime = DateTime.Now;  // 中断時の時刻を取得する
                Timer_Traffic.Stop();
            }
            else
            {
                // 現在時刻のミリ秒が100から200の間の秒数になるまでスレッドを中断する
                while (DateTime.Now.Millisecond < 100 || DateTime.Now.Millisecond > 200)
                {
                    Thread.Sleep(10);
                }

                Resume_CarLight();      // 車用信号機を再開状態に設定する
                Timer_Traffic.Start();
            }
        }

        /// <summary>
        /// 「終了」ボタンクリック時イベント
        /// </summary>
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            if (IsTrafficEnable)
            {
                DialogResult = MessageBox.Show("信号機の点灯処理を終了しますか？", Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DialogResult == DialogResult.No) return;
            }

            for (int i = 0; i < carTrafficArr.Length; i++)
            {
                carTrafficArr[i].UpdateStateNoLight();                       // 車用信号機の点灯状態を無灯火に設定する
                Change_TrafficLightOnState(carTrafficArr[i], DateTime.Now);  // 車用信号機をイメージしたラベルの背景色を変更する
            }

            for (int j = 0; j < pedesTrafficArr.Length; j++)
            {
                pedesTrafficArr[j].UpdateStateNoLight();       // 歩行者用信号機の点灯状態を無灯火に設定する
                Change_PedesLightOnState(pedesTrafficArr[j]);  // 歩行者用信号機をイメージしたラベルの背景色を変更する
            }

            IsTrafficEnable = false;
            IsInterrupt     = false;
            Change_InterruptResumeBtnTxt(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            Change_TxtEnable(true);                     // テキストボックスのEnabledプロパティ値変更
            Timer_Traffic.Stop();
        }

        /// <summary>
        /// タイマーTickイベント
        /// </summary>
        private void Timer_Traffic_Tick(object sender, EventArgs e)
        {
            TrafficLightControl();
        }

        /// <summary>
        /// エラーメッセージを作成する
        /// </summary>
        /// <returns> エラーメッセージを表す文字列 </returns>
        private string Create_ErrMsg()
        {
            string errStr = "";  // エラーメッセージが入る

            // 設定値のチェックを満たす最大値と最小値からなる配列
            int[,] secArray = 
            { 
                { BlueLightOnSecMax, BlueLightOnSecMax, BlueLightOnSecMax, BlueLightOnSecMax, PrepareSecMax}, 
                { BlueLightOnSecMin, BlueLightOnSecMin, BlueLightOnSecMin, BlueLightOnSecMin, PrepareSecMin}
            };

            // テキストボックスに入力された設定値のチェック
            for (int i = 0; i < tlp_InputSecField.RowCount - 1; i++)
            {
                if (!Check_SecText(tlp_InputSecField.GetControlFromPosition(1, i + 1).Text, secArray[0, i], secArray[1, i]))
                {
                    errStr += $"「{tlp_InputSecField.GetControlFromPosition(0, i + 1).Text}」には{secArray[0, i]}から{secArray[1, i]}の整数を入力してください。\n";
                }
            }

            return errStr;
        }

        /// <summary>
        /// 文字列が最大値と最小値の範囲内の整数を表す値かチェックを行う
        /// </summary>
        /// <param name="checkText"> チェック対象の文字列         </param>
        /// <param name="maxValue">  チェックを満たす整数の最大値 </param>
        /// <param name="minValue">  チェックを満たす整数の最小値 </param>
        /// <returns> checkTextをint型に変換した値がmaxValue以下でminValue以上の整数の場合はtrue。それ以外の場合はfalse </returns>
        private bool Check_SecText(string checkText, int maxValue, int minValue)
        {
            if (!int.TryParse(checkText, out int checkValue))   return false;  // チェック対象の文字列がint型に変換できない場合は終了する
            if (checkValue > maxValue || checkValue < minValue) return false;  // int型に変換した値がmaxValueより大きい、もしくはminValueより小さい場合は終了する
            return true;
        }

        /// <summary>
        /// 「中断/再開」ボタンのtextプロパティ値変更
        /// </summary>
        /// <param name="isInterrupt"> 信号機点灯処理が中断している場合はtrue、それ以外の場合はfalse </param>
        private void Change_InterruptResumeBtnTxt(bool isInterrupt)
        {
            if (isInterrupt) 
            { 
                btn_InterruptResume.Text = "再開";            
            }
            else
            {
                btn_InterruptResume.Text = "中断";
            }
        }

        /// <summary>
        /// テキストボックスのEnabledプロパティの設定を変更する
        /// </summary>
        /// <param name="enable"> Enabledプロパティを有効にする場合はtrue、それ以外の場合はfalse </param>
        private void Change_TxtEnable(bool enable)
        {
            txt_NLightOnSec.Enabled = enable;
            txt_SLightOnSec.Enabled = enable;
            txt_ELightOnSec.Enabled = enable;
            txt_WLightOnSec.Enabled = enable;
            txt_PrepareSec.Enabled  = enable;
        }

        /// <summary>
        /// 車用信号機の初期状態を設定
        /// </summary>
        private void Initialize_CarLight()
        {
            int.TryParse(txt_NLightOnSec.Text, out int n_GreSec);  // 北車用信号機の緑色灯火時間を取得
            int.TryParse(txt_SLightOnSec.Text, out int s_GreSec);  // 南車用信号機の緑色灯火時間を取得
            int.TryParse(txt_ELightOnSec.Text, out int e_GreSec);  // 東車用信号機の緑色灯火時間を取得
            int.TryParse(txt_WLightOnSec.Text, out int w_GreSec);  // 西車用信号機の緑色灯火時間を取得
            int.TryParse(txt_PrepareSec.Text,  out int preSec);    // 進行方向切り替え準備時間を取得

            int northRedLightOn = Math.Max(w_GreSec, e_GreSec) + YellowSec + 2 * preSec + Math.Max(n_GreSec, s_GreSec) - n_GreSec;  // 北車用信号機の赤色点灯時間を算出
            int southRedLightOn = Math.Max(w_GreSec, e_GreSec) + YellowSec + 2 * preSec + Math.Max(n_GreSec, s_GreSec) - s_GreSec;  // 南車用信号機の赤色点灯時間を算出
            int eastRedLightOn  = Math.Max(n_GreSec, s_GreSec) + YellowSec + 2 * preSec + Math.Max(w_GreSec, e_GreSec) - e_GreSec;  // 東車用信号機の赤色点灯時間を算出
            int westRedLightOn  = Math.Max(n_GreSec, s_GreSec) + YellowSec + 2 * preSec + Math.Max(w_GreSec, e_GreSec) - w_GreSec;  // 西車用信号機の赤色点灯時間を算出

            int eastMinusSec = -preSec;
            int westMinusSec = -preSec;
            if (e_GreSec < w_GreSec) eastMinusSec = -preSec - Math.Abs(w_GreSec - e_GreSec);  // 東車用信号機で点灯処理をずらす秒数
            if (e_GreSec > w_GreSec) westMinusSec = -preSec - Math.Abs(e_GreSec - w_GreSec);  // 西車用信号機で点灯処理をずらす秒数

            // 車用信号機クラスのインスタンス生成
            NorthLight = new CarTraffic(n_GreSec, YellowSec, northRedLightOn, 0);
            SouthLight = new CarTraffic(s_GreSec, YellowSec, southRedLightOn, 1);
            EastLight  = new CarTraffic(e_GreSec, YellowSec, eastRedLightOn,  2);
            WestLight  = new CarTraffic(w_GreSec, YellowSec, westRedLightOn,  3);

            carTrafficArr = new CarTraffic[]{ NorthLight, SouthLight, EastLight, WestLight };

            // 車用信号機を初期の点灯状態に設定する
            NorthLight.UpdateStateGreen();
            SouthLight.UpdateStateGreen();
            EastLight.UpdateStateRed();
            WestLight.UpdateStateRed();

            Change_TrafficLightOnState(NorthLight, DateTime.Now);
            Change_TrafficLightOnState(SouthLight, DateTime.Now);
            Change_TrafficLightOnState(EastLight, DateTime.Now.AddSeconds(eastMinusSec));
            Change_TrafficLightOnState(WestLight, DateTime.Now.AddSeconds(westMinusSec));
        }

        /// <summary>
        /// 車用信号機の再開状態を設定
        /// </summary>
        private void Resume_CarLight()
        {
            for (int i = 0; i < carTrafficArr.Length; i++)
            {
                carTrafficArr[i].Update_StateChangeTime(InterruptTime);
            }
        }

        /// <summary>
        /// 歩行者用信号機の初期状態を設定
        /// </summary>
        private void Initialize_PedesLight()
        {
            // 歩行者用信号機のインスタンス生成
            NorthPedesLight = new PedesTraffic(0);
            SouthPedesLight = new PedesTraffic(1);
            EastPedesLight  = new PedesTraffic(2);
            WestPedesLight  = new PedesTraffic(3);

            // 歩行者用信号機を初期の点灯状態に設定する。
            Change_PedesLightOnState(NorthPedesLight);
            Change_PedesLightOnState(SouthPedesLight);
            Change_PedesLightOnState(EastPedesLight);
            Change_PedesLightOnState(WestPedesLight);
        }

        /// <summary>
        /// 信号機点灯制御
        /// </summary>
        private void TrafficLightControl()
        {
            for (int i = 0; i < carTrafficArr.Length; i++)
            {
                // 車用信号機の点灯状態を切り替えるか判定
                if (carTrafficArr[i].Judge_TrafficLightOn(DateTime.Now))
                {
                    carTrafficArr[i].Update_LightOnState();                      // 車用信号機の点灯状態を更新する
                    Change_TrafficLightOnState(carTrafficArr[i], DateTime.Now);  // 車用信号機をイメージしたラベルの背景色を変更する
                }
            }

            Judge_PedesLightOn(NorthPedesLight, SouthPedesLight);  // 北方向と南方向の歩行者用信号機の点灯状態を切り替えるか判定
            Judge_PedesLightOn(EastPedesLight,  WestPedesLight);   // 東方向と西方向の歩行者用信号機の点灯状態を切り替えるか判定
        }

        /// <summary>
        /// 車用信号機の点灯色を変更する
        /// </summary>
        /// <param name="carTraffic"> 車用信号機を表すクラス           </param>
        private void Change_TrafficLightOnState(CarTraffic carTraffic, DateTime dateTime)
        {
            CarLabelArr[carTraffic.CarTrafficNum, 0].BackColor = carTraffic.LightOnColor()[0];  // 車用信号機の緑ランプを表すラベルの背景色を変更する
            CarLabelArr[carTraffic.CarTrafficNum, 1].BackColor = carTraffic.LightOnColor()[1];  // 車用信号機の黄ランプを表すラベルの背景色を変更する 
            CarLabelArr[carTraffic.CarTrafficNum, 2].BackColor = carTraffic.LightOnColor()[2];  // 車用信号機の赤ランプを表すラベルの背景色を変更する 
            carTraffic.SetStateTime(dateTime.AddMilliseconds(-dateTime.Millisecond));           // ラベル背景色を変更した時刻を取得する
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態を切り替えるか判定する
        /// </summary>
        /// <param name="pedesTraffic1"> 歩行者用信号機を表すクラス1 </param>
        /// <param name="pedesTraffic2"> 歩行者用信号機を表すクラス2 </param>
        private void Judge_PedesLightOn(PedesTraffic pedesTraffic1, PedesTraffic pedesTraffic2)
        {
            int        greenSec;          // 車用信号機の緑色点灯時間を取得する
            DateTime   carTrafficChange;  // 車用信号機の点灯状態更新時刻を取得する
            CarTraffic carTra1 = null;    // 車用信号機クラス1
            CarTraffic carTra2 = null;    // 車用信号機クラス2

            if (pedesTraffic1 == NorthPedesLight)
            {
                carTra1 = EastLight;  // 東方向の車用信号機
                carTra2 = WestLight;  // 西方向の車用信号機
            }
            else if (pedesTraffic1 == EastPedesLight)
            {
                carTra1 = NorthLight;  // 北方向の車用信号機
                carTra2 = SouthLight;  // 南方向の車用信号機
            }

            greenSec = Math.Min(carTra1.GreenLightOnSec, carTra2.GreenLightOnSec);  // 2つの車用信号機の緑色点灯時間を比較し、短い方の秒数を取得する
            
            carTrafficChange = carTra1.GetStateTime();  // 車用信号機の点灯状態更新時刻を取得する

            if (pedesTraffic1.LightState == "Green")
            {
                // 緑信号の点灯時間が残り(3秒 - PedesBlinkミリ秒)になった場合に点滅を開始する
                if (DateTime.Now >= carTrafficChange.AddSeconds(greenSec - 3).AddMilliseconds(-PedesBlinkMSec))
                {
                    Change_PedesLightOnState(pedesTraffic1);
                    Change_PedesLightOnState(pedesTraffic2);
                    pedesTraffic1.BlinkTime = DateTime.Now;
                    pedesTraffic2.BlinkTime = DateTime.Now;
                }
            }
            else if (pedesTraffic1.LightState == "Red")
            {
                // 車用信号機carTra1とcarTra2の点灯状態が緑で、車用信号機の緑色点灯時間が残り1秒より長い場合に歩行者用信号機を緑に点灯する
                //if (carTra1.LightState == "Green" && carTra2.LightState == "Green" && (DateTime.Now < carTrafficChange.AddSeconds(greenSec - 1)))
                //{
                //    Change_PedesLightOnState(pedesTraffic1);
                //    Change_PedesLightOnState(pedesTraffic2);
                //}
            }
            else if (pedesTraffic1.LightState == "Blink_Green" || pedesTraffic1.LightState == "Blink_White")
            {
                if (DateTime.Now >= carTrafficChange.AddSeconds(greenSec - 1))
                {
                    // 車用信号機の緑色点灯時間が残り1秒になった場合に歩行者用信号機の点灯状態を赤に設定する
                    Change_PedesLightOnState(pedesTraffic1);
                    Change_PedesLightOnState(pedesTraffic2);
                }
                else if (DateTime.Now > pedesTraffic1.BlinkTime.AddMilliseconds(PedesBlinkMSec))
                {
                    // 歩行者用信号機緑ランプの点滅状態を切り替える
                    if (pedesTraffic1.LightState == "Blink_Green")
                    { 
                        pedesTraffic1.LightState = "Blink_White";
                        pedesTraffic1.BlinkTime  = DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond + PedesBlinkMSec);
                    }
                    else if (pedesTraffic1.LightState == "Blink_White")
                    {
                        pedesTraffic1.LightState = "Blink_Green";
                        pedesTraffic1.BlinkTime  = DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond);
                    }

                    // PedesBlinkMSecミリ秒の間隔で歩行者用信号機の緑ランプを点滅する
                    Change_PedesLightOnState(pedesTraffic1);
                    Change_PedesLightOnState(pedesTraffic2);
                }
            }
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態を切り替える
        /// </summary>
        /// <param name="pedesTraffic"> 歩行者用信号機を表すクラス           </param>
        private void Change_PedesLightOnState(PedesTraffic pedesTraffic)
        {
            //pedesTraffic.Update_LightOnState();  // 歩行者用信号機の点灯状態を更新する

            //PedesLabelArr[pedesTraffic.TrafficNum, 0].BackColor = pedesTraffic.LightOnColor()[0];  // 歩行者用信号機の１つ目の緑ランプを表すラベルの背景色を変更する
            //PedesLabelArr[pedesTraffic.TrafficNum, 1].BackColor = pedesTraffic.LightOnColor()[1];  // 歩行者用信号機の２つ目の緑ランプを表すラベルの背景色を変更する 
            //PedesLabelArr[pedesTraffic.TrafficNum, 2].BackColor = pedesTraffic.LightOnColor()[2];  // 歩行者用信号機の１つ目の赤ランプを表すラベルの背景色を変更する 
            //PedesLabelArr[pedesTraffic.TrafficNum, 3].BackColor = pedesTraffic.LightOnColor()[3];  // 歩行者用信号機の２つ目の赤ランプを表すラベルの背景色を変更する
            //pedesTraffic.SetStateTime(DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond));    // 点灯状態が更新された時刻を取得する
        }
    }
}