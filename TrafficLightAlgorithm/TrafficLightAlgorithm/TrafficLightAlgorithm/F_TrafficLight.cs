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

        private DateTime GreenLightOnTime;

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
        /// 交差点内の車用信号機のクラスが入る配列
        /// </summary>
        private CarTraffic[] carTrafficArr;

        /// <summary>
        /// 交差点内の歩行者用信号機のクラスが入る配列
        /// </summary>
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

            GreenLightOnTime = DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond);

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
                Console.WriteLine(InterruptTime);
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
                carTrafficArr[i].UpdateStateNoLight();         // 車用信号機の点灯状態を無灯火に設定する
                Change_TrafficLightOnState(carTrafficArr[i]);  // 車用信号機をイメージしたラベルの背景色を変更する
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
                { BlueLightOnSecMin, BlueLightOnSecMin, BlueLightOnSecMin, BlueLightOnSecMin, PrepareSecMin},
                { BlueLightOnSecMax, BlueLightOnSecMax, BlueLightOnSecMax, BlueLightOnSecMax, PrepareSecMax} 
            };

            // テキストボックスに入力された設定値のチェック
            for (int i = 0; i < tlp_InputSecField.RowCount - 1; i++)
            {
                if (!Check_SecText(tlp_InputSecField.GetControlFromPosition(1, i + 1).Text, secArray[1, i], secArray[0, i]))
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

            int n_RedSec = Math.Max(w_GreSec, e_GreSec) + YellowSec + 2 * preSec + Math.Max(0, s_GreSec - n_GreSec);  // 北車用信号機の赤色点灯時間を算出
            int s_RedSec = Math.Max(w_GreSec, e_GreSec) + YellowSec + 2 * preSec + Math.Max(0, n_GreSec - s_GreSec);  // 南車用信号機の赤色点灯時間を算出
            int e_RedSec = Math.Max(n_GreSec, s_GreSec) + YellowSec + 2 * preSec + Math.Max(0, w_GreSec - e_GreSec);  // 東車用信号機の赤色点灯時間を算出
            int w_RedSec = Math.Max(n_GreSec, s_GreSec) + YellowSec + 2 * preSec + Math.Max(0, e_GreSec - w_GreSec);  // 西車用信号機の赤色点灯時間を算出

            int eastMinusSec = -preSec + Math.Min(e_GreSec - w_GreSec, 0);  // 東車用信号機で点灯処理をずらす秒数
            int westMinusSec = -preSec + Math.Min(w_GreSec - e_GreSec, 0);  // 西車用信号機で点灯処理をずらす秒数

            // 車用信号機クラスのインスタンス生成
            NorthLight = new CarTraffic(n_GreSec, YellowSec, n_RedSec, 0, "Green", DateTime.Now);
            SouthLight = new CarTraffic(s_GreSec, YellowSec, s_RedSec, 1, "Green", DateTime.Now);
            EastLight  = new CarTraffic(e_GreSec, YellowSec, e_RedSec, 2, "Red", DateTime.Now.AddSeconds(eastMinusSec));
            WestLight  = new CarTraffic(w_GreSec, YellowSec, w_RedSec, 3, "Red", DateTime.Now.AddSeconds(westMinusSec));

            carTrafficArr = new CarTraffic[]{ NorthLight, SouthLight, EastLight, WestLight };

            // 車用信号機を初期の点灯状態に設定する
            Change_TrafficLightOnState(NorthLight);
            Change_TrafficLightOnState(SouthLight);
            Change_TrafficLightOnState(EastLight);
            Change_TrafficLightOnState(WestLight);
        }

        /// <summary>
        /// 車用信号機の再開状態を設定
        /// </summary>
        private void Resume_CarLight()
        {
            for (int i = 0; i < carTrafficArr.Length; i++)
            {
                carTrafficArr[i].UpdateStateChangeResumeTime(InterruptTime);  // 車用信号機の点灯状態変更時刻を更新する
            }
        }

        /// <summary>
        /// 歩行者用信号機の初期状態を設定
        /// </summary>
        private void Initialize_PedesLight()
        {
            int.TryParse(txt_PrepareSec.Text, out int preSec);
            int ew_CarGreenSec = Math.Min(WestLight.GreenLightOnSec,  EastLight.GreenLightOnSec);   // 東と西の車用信号機が同時に緑色に点灯する時間
            int ns_CarGreenSec = Math.Min(NorthLight.GreenLightOnSec, SouthLight.GreenLightOnSec);  // 北と南の車用信号機が同時に緑色に点灯する時間

            int ns_CarRedSec = Math.Max(NorthLight.RedLightOnSec, SouthLight.RedLightOnSec);  // 北と南の車用信号機で赤色点灯時間が長い方の時間
            int ew_CarRedSec = Math.Max(EastLight.RedLightOnSec, WestLight.RedLightOnSec);    // 東と西の車用信号機で赤色点灯時間が長い方の時間 

            int minusSec = Math.Abs(EastLight.GreenLightOnSec - WestLight.GreenLightOnSec);

            // 歩行者用信号機のインスタンス生成
            NorthPedesLight = new PedesTraffic(ew_CarGreenSec, ew_CarRedSec + YellowSec + 1, 0, "Red",   DateTime.Now.AddSeconds(-preSec - YellowSec - 1 - minusSec));
            SouthPedesLight = new PedesTraffic(ew_CarGreenSec, ew_CarRedSec + YellowSec + 1, 1, "Red",   DateTime.Now.AddSeconds(-preSec - YellowSec - 1 - minusSec));
            EastPedesLight  = new PedesTraffic(ns_CarGreenSec, ns_CarRedSec + YellowSec + 1, 2, "Green", DateTime.Now);
            WestPedesLight  = new PedesTraffic(ns_CarGreenSec, ns_CarRedSec + YellowSec + 1, 3, "Green", DateTime.Now);

            pedesTrafficArr = new PedesTraffic[]{ NorthPedesLight, SouthPedesLight, EastPedesLight, WestPedesLight };

            // 歩行者用信号機を初期の点灯状態に設定する
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
                    carTrafficArr[i].Update_LightOnState();        // 車用信号機の点灯状態を更新する
                    Change_TrafficLightOnState(carTrafficArr[i]);  // 車用信号機をイメージしたラベルの背景色を変更する
                }
            }

            for (int i = 0; i < pedesTrafficArr.Length; i++)
            {
                // 歩行者用信号機の点灯状態を切り替えるか判定
                if (pedesTrafficArr[i].Judge_PedesLightOn())
                {
                    pedesTrafficArr[i].Update_LightOnState();      // 歩行者用信号機の点灯状態を更新する
                    Change_PedesLightOnState(pedesTrafficArr[i]);  // 歩行者用信号機をイメージしたラベルの背景色を変更する
                }
            }
        }

        /// <summary>
        /// 車用信号機の点灯色を変更する
        /// </summary>
        /// <param name="carTraffic"> 車用信号機を表すクラス           </param>
        private void Change_TrafficLightOnState(CarTraffic carTraffic)
        {
            CarLabelArr[carTraffic.CarTrafficNum, 0].BackColor = carTraffic.LightOnColor()[0];  // 車用信号機の緑ランプを表すラベルの背景色を変更する
            CarLabelArr[carTraffic.CarTrafficNum, 1].BackColor = carTraffic.LightOnColor()[1];  // 車用信号機の黄ランプを表すラベルの背景色を変更する 
            CarLabelArr[carTraffic.CarTrafficNum, 2].BackColor = carTraffic.LightOnColor()[2];  // 車用信号機の赤ランプを表すラベルの背景色を変更する 
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態を切り替える
        /// </summary>
        /// <param name="pedesTraffic"> 歩行者用信号機を表すクラス </param>
        private void Change_PedesLightOnState(PedesTraffic pedesTraffic)
        {
            PedesLabelArr[pedesTraffic.TrafficNum, 0].BackColor = pedesTraffic.LightOnColor()[0];  // 歩行者用信号機の１つ目の緑ランプを表すラベルの背景色を変更する
            PedesLabelArr[pedesTraffic.TrafficNum, 1].BackColor = pedesTraffic.LightOnColor()[1];  // 歩行者用信号機の２つ目の緑ランプを表すラベルの背景色を変更する 
            PedesLabelArr[pedesTraffic.TrafficNum, 2].BackColor = pedesTraffic.LightOnColor()[2];  // 歩行者用信号機の１つ目の赤ランプを表すラベルの背景色を変更する 
            PedesLabelArr[pedesTraffic.TrafficNum, 3].BackColor = pedesTraffic.LightOnColor()[3];  // 歩行者用信号機の２つ目の赤ランプを表すラベルの背景色を変更する
        }
    }
}