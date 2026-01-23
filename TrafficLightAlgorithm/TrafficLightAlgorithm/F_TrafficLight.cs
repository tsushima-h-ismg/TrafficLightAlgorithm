using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    public partial class F_TrafficLight : Form
    {
        /// <summary>
        /// 車用信号機の青色灯火秒数の最大値
        /// </summary>
        private const int BlueLightOnSecMax = 20;

        /// <summary>
        /// 車用信号機の青色灯火秒数の最小値
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
        /// 信号機の緑を表す色
        /// </summary>
        private readonly Color TrafficLightGreen = Color.ForestGreen;

        /// <summary>
        /// 信号機の黄を表す色
        /// </summary>
        private readonly Color TrafficLightYellow = Color.Yellow;

        /// <summary>
        /// 信号機の赤を表す色
        /// </summary>
        private readonly Color TrafficLightRed = Color.Red;

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
                DialogResult dialogResult = MessageBox.Show("信号機の点灯処理を中断しています。処理を最初から実行しますか？", Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogResult == DialogResult.No) return;
            }

            IsTrafficEnable = true;
            IsInterrupt     = false;

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
                while (DateTime.Now.Millisecond < 100 || DateTime.Now.Millisecond > 200)
                {
                    Thread.Sleep(10);
                }

                Resume_CarLight();   // 車用信号機を再開状態に設定する
                Resume_PedesLight(); // 歩行者用信号機を再開状態に設定する
                
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

            CarTrafficNoLight(NorthLight);  // 北方向車用信号機を無灯火にする
            CarTrafficNoLight(SouthLight);  // 南方向車用信号機を無灯火にする
            CarTrafficNoLight(EastLight);   // 東方向車用信号機を無灯火にする
            CarTrafficNoLight(WestLight);   // 西方向車用信号機を無灯火にする

            PedesNoLight(NorthPedesLight);  // 北方向歩行者用信号機を無灯火にする
            PedesNoLight(SouthPedesLight);  // 南方向歩行者用信号機を無灯火にする
            PedesNoLight(EastPedesLight);   // 東方向歩行者用信号機を無灯火にする
            PedesNoLight(WestPedesLight);   // 西方向歩行者用信号機を無灯火にする

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

            // 北信号機の青色灯火時間の入力値のチェック
            if (!Check_SecText(txt_NLightOnSec.Text, BlueLightOnSecMax, BlueLightOnSecMin)) errStr += $"「{lbl_NLightOnSec.Text}」には{BlueLightOnSecMin}から{BlueLightOnSecMax}の整数を入力してください。\n";

            // 東信号機の青色灯火時間の入力値のチェック
            if (!Check_SecText(txt_ELightOnSec.Text, BlueLightOnSecMax, BlueLightOnSecMin)) errStr += $"「{lbl_ELightOnSec.Text}」には{BlueLightOnSecMin}から{BlueLightOnSecMax}の整数を入力してください。\n";

            // 西信号機の青色灯火時間の入力値のチェック
            if (!Check_SecText(txt_WLightOnSec.Text, BlueLightOnSecMax, BlueLightOnSecMin)) errStr += $"「{lbl_WLightOnSec.Text}」には{BlueLightOnSecMin}から{BlueLightOnSecMax}の整数を入力してください。\n";

            // 南信号機の青色灯火時間の入力値のチェック
            if (!Check_SecText(txt_SLightOnSec.Text, BlueLightOnSecMax, BlueLightOnSecMin)) errStr += $"「{lbl_SLightOnSec.Text}」には{BlueLightOnSecMin}から{BlueLightOnSecMax}の整数を入力してください。\n";
            
            // 交差点進行方向切り替え準備時間の入力値のチェック
            if (!Check_SecText(txt_PrepareSec.Text, PrepareSecMax, PrepareSecMin)) errStr += $"「{lbl_PrepareSec.Text}」には{PrepareSecMin}から{PrepareSecMax}の整数を入力してください。\n";

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
            int.TryParse(txt_NLightOnSec.Text, out int n_GreSec);  // 北信号機の青色灯火時間を取得
            int.TryParse(txt_SLightOnSec.Text, out int s_GreSec);  // 南信号機の青色灯火時間を取得
            int.TryParse(txt_ELightOnSec.Text, out int e_GreSec);  // 東信号機の青色灯火時間を取得
            int.TryParse(txt_WLightOnSec.Text, out int w_GreSec);  // 西信号機の青色灯火時間を取得
            int.TryParse(txt_PrepareSec.Text,  out int preSec);    // 進行方向切り替え準備時間を取得

            int ns_GreSecMax = s_GreSec;
            int ew_GreSecMax = w_GreSec;
            if (n_GreSec > s_GreSec) ns_GreSecMax = n_GreSec;  // 北信号機と南信号機で青色灯火時間の長い方の秒数を取得する
            if (e_GreSec > w_GreSec) ew_GreSecMax = e_GreSec;  // 東信号機と西信号機で青色灯火時間の長い方の秒数を取得する

            int northRedLightOn = ew_GreSecMax + YellowSec + 2 * preSec + ns_GreSecMax - n_GreSec;  // 北信号機の赤色点灯時間を算出
            int southRedLightOn = ew_GreSecMax + YellowSec + 2 * preSec + ns_GreSecMax - s_GreSec;  // 南信号機の赤色点灯時間を算出
            int eastRedLightOn  = ns_GreSecMax + YellowSec + 2 * preSec + ew_GreSecMax - e_GreSec;  // 東信号機の赤色点灯時間を算出
            int westRedLightOn  = ns_GreSecMax + YellowSec + 2 * preSec + ew_GreSecMax - w_GreSec;  // 西信号機の赤色点灯時間を算出

            int eastMinusSec = -preSec;
            int westMinusSec = -preSec; 
            if (e_GreSec < w_GreSec) eastMinusSec -= Math.Abs(w_GreSec - e_GreSec);  // 東信号機で点灯処理をずらす秒数
            if (e_GreSec > w_GreSec) westMinusSec -= Math.Abs(e_GreSec - w_GreSec);  // 西信号機で点灯処理をずらす秒数

            // 車用信号機クラスのインスタンス生成
            NorthLight = new CarTraffic(lbl_NorthGreen.Name, lbl_NorthYellow.Name, lbl_NorthRed.Name, n_GreSec, YellowSec, northRedLightOn, DateTime.Now);
            SouthLight = new CarTraffic(lbl_SouthGreen.Name, lbl_SouthYellow.Name, lbl_SouthRed.Name, s_GreSec, YellowSec, southRedLightOn, DateTime.Now);
            EastLight  = new CarTraffic(lbl_EastGreen.Name,  lbl_EastYellow.Name,  lbl_EastRed.Name,  e_GreSec, YellowSec, eastRedLightOn,  DateTime.Now.AddSeconds(eastMinusSec));
            WestLight  = new CarTraffic(lbl_WestGreen.Name,  lbl_WestYellow.Name,  lbl_WestRed.Name,  w_GreSec, YellowSec, westRedLightOn,  DateTime.Now.AddSeconds(westMinusSec));

            // 車用信号機を初期の点灯状態に設定する
            Change_TrafficLightOnState(NorthLight, "Green", NorthLight.RedLabelName,   NorthLight.GreenLabelName, TrafficLightGreen);
            Change_TrafficLightOnState(SouthLight, "Green", SouthLight.RedLabelName,   SouthLight.GreenLabelName, TrafficLightGreen);
            Change_TrafficLightOnState(EastLight,  "Red",   EastLight.YellowLabelName, EastLight.RedLabelName,    TrafficLightRed);
            Change_TrafficLightOnState(WestLight,  "Red",   WestLight.YellowLabelName, WestLight.RedLabelName,    TrafficLightRed);
        }

        /// <summary>
        /// 車用信号機の再開状態を設定
        /// </summary>
        private void Resume_CarLight()
        {
            NorthLight.CurrentTime = DateTime.Now.AddSeconds(NorthLight.CurrentTime.Second - InterruptTime.Second).AddMilliseconds(-DateTime.Now.Millisecond);
            SouthLight.CurrentTime = DateTime.Now.AddSeconds(SouthLight.CurrentTime.Second - InterruptTime.Second).AddMilliseconds(-DateTime.Now.Millisecond);
            EastLight.CurrentTime  = DateTime.Now.AddSeconds(EastLight.CurrentTime.Second  - InterruptTime.Second).AddMilliseconds(-DateTime.Now.Millisecond);
            WestLight.CurrentTime  = DateTime.Now.AddSeconds(WestLight.CurrentTime.Second  - InterruptTime.Second).AddMilliseconds(-DateTime.Now.Millisecond);
        }

        /// <summary>
        /// 歩行者用信号機の初期状態を設定
        /// </summary>
        private void Initialize_PedesLight()
        {
            // 歩行者用信号機のインスタンス生成
            NorthPedesLight = new PedesTraffic(lbl_NorthPedesGreenOne.Name, lbl_NorthPedesGreenTwo.Name, lbl_NorthPedesRedOne.Name, lbl_NorthPedesRedTwo.Name);
            SouthPedesLight = new PedesTraffic(lbl_SouthPedesGreenOne.Name, lbl_SouthPedesGreenTwo.Name, lbl_SouthPedesRedOne.Name, lbl_SouthPedesRedTwo.Name);
            EastPedesLight  = new PedesTraffic(lbl_EastPedesGreenOne.Name,  lbl_EastPedesGreenTwo.Name,  lbl_EastPedesRedOne.Name,  lbl_EastPedesRedTwo.Name);
            WestPedesLight  = new PedesTraffic(lbl_WestPedesGreenOne.Name,  lbl_WestPedesGreenTwo.Name,  lbl_WestPedesRedOne.Name,  lbl_WestPedesRedTwo.Name);

            // 歩行者用信号機を初期の点灯状態に設定する。
            Change_PedesLightOnState(NorthPedesLight, "Red");
            Change_PedesLightOnState(SouthPedesLight, "Red");
            Change_PedesLightOnState(EastPedesLight,  "Green");
            Change_PedesLightOnState(WestPedesLight,  "Green");
        }

        /// <summary>
        /// 歩行者用信号機の再開状態を設定
        /// </summary>
        private void Resume_PedesLight()
        {

        }

        /// <summary>
        /// 信号機点灯制御
        /// </summary>
        private void TrafficLightControl()
        {
            Judge_TrafficLightOn(NorthLight);  // 北方向車用信号機の点灯状態を切り替えるか判定
            Judge_TrafficLightOn(SouthLight);  // 南方向車用信号機の点灯状態を切り替えるか判定
            Judge_TrafficLightOn(EastLight);   // 東方向車用信号機の点灯状態を切り替えるか判定
            Judge_TrafficLightOn(WestLight);   // 西方向車用信号機の点灯状態を切り替えるか判定

            Judge_PedesLightOn(NorthPedesLight, SouthPedesLight);  // 北方向と南方向の歩行者用信号機の点灯状態を切り替えるか判定
            Judge_PedesLightOn(EastPedesLight,  WestPedesLight);   // 東方向と西方向の歩行者用信号機の点灯状態を切り替えるか判定
        }

        /// <summary>
        /// 車用信号機の点灯状態を切り替えるか判定する
        /// </summary>
        /// <param name="carTraffic"> 点灯する車用信号機を表す車用信号機クラス </param>
        private void Judge_TrafficLightOn(CarTraffic carTraffic)
        {
            if (carTraffic.LightState == "Green")
            {
                if (DateTime.Now >= carTraffic.CurrentTime.AddSeconds(carTraffic.GreenLightOnSec))
                {
                    // 点灯状態が切り替わってから青色灯火時間以上経過している場合、黄色に点灯する
                    Change_TrafficLightOnState(carTraffic, "Yellow", carTraffic.GreenLabelName, carTraffic.YellowLabelName, TrafficLightYellow);
                    carTraffic.CurrentTime = DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond);
                }
            }
            else if (carTraffic.LightState == "Yellow")
            {
                if (DateTime.Now >= carTraffic.CurrentTime.AddSeconds(carTraffic.YellowLightOnSec))
                {
                    // 点灯状態が切り替わってから黄色灯火時間以上経過している場合、赤色に点灯する
                    Change_TrafficLightOnState(carTraffic, "Red", carTraffic.YellowLabelName, carTraffic.RedLabelName, TrafficLightRed);
                    carTraffic.CurrentTime = DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond);
                }
            }
            else if (carTraffic.LightState == "Red")
            {
                if (DateTime.Now >= carTraffic.CurrentTime.AddSeconds(carTraffic.RedLightOnSec))
                {
                    // 点灯状態が切り替わってから赤色灯火時間以上経過している場合、緑色に点灯する
                    Change_TrafficLightOnState(carTraffic, "Green", carTraffic.RedLabelName, carTraffic.GreenLabelName, TrafficLightGreen);
                    carTraffic.CurrentTime = DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond);
                }
            }
        }

        /// <summary>
        /// 車用信号機の点灯状態を切り替える
        /// </summary>
        /// <param name="carTraffic">     車用信号機を表すクラス                       </param>
        /// <param name="state">          車用信号機の点灯状態を表す文字列             </param>
        /// <param name="lblNoLightName"> 無灯火にする信号機のランプを表すラベルの名称 </param>
        /// <param name="lblLightOnName"> 点灯する信号機のランプを表すラベルの名称     </param>
        /// <param name="lblBackColor">   lblLightOnNameが表すラベルの背景色           </param>
        private void Change_TrafficLightOnState(CarTraffic carTraffic, string state, string lblNoLightName, string lblLightOnName, Color lblBackColor)
        {
            carTraffic.LightState                = state;         // 車用信号機の点灯状態を更新する
            GetControl(lblNoLightName).BackColor = Color.White;   // NameプロパティがlblNoLightNameのラベル背景色を白に変更する
            GetControl(lblLightOnName).BackColor = lblBackColor;  // NameプロパティがlblLightOnNameのラベル背景色をlblBackColorに変更する
        }

        /// <summary>
        /// 車用信号機のランプを全て無灯火にする
        /// </summary>
        /// <param name="carTraffic"> 無灯火にする車用信号機を表すクラス </param>
        private void CarTrafficNoLight(CarTraffic carTraffic)
        {
            GetControl(carTraffic.GreenLabelName).BackColor  = Color.White;  // 車用信号機の緑ランプを表すラベル背景色を白に変更する
            GetControl(carTraffic.YellowLabelName).BackColor = Color.White;  // 車用信号機の黄ランプを表すラベル背景色を白に変更する
            GetControl(carTraffic.RedLabelName).BackColor    = Color.White;  // 車用信号機の赤ランプを表すラベル背景色を白に変更する
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態を切り替えるか判定する
        /// </summary>
        /// <param name="pedesTraffic1"> 歩行者用信号機を表すクラス1 </param>
        /// <param name="pedesTraffic2"> 歩行者用信号機を表すクラス2 </param>
        private void Judge_PedesLightOn(PedesTraffic pedesTraffic1, PedesTraffic pedesTraffic2)
        {
            if (pedesTraffic1.LightState == "Green")
            {
                int      greenSec         = 0;             // 車用信号機の青色点灯時間を取得する
                DateTime carTrafficChange = DateTime.Now;  // 車用信号機の点灯状態更新時刻を取得する

                if (pedesTraffic1 == NorthPedesLight)
                {
                    // 東と西の車用信号機の青色点灯時間を比較して小さい方の値を取得する
                    greenSec = Math.Min(EastLight.GreenLightOnSec, WestLight.GreenLightOnSec);
                    carTrafficChange = EastLight.CurrentTime;
                }
                else if (pedesTraffic1 == EastPedesLight)
                {
                    // 北と南の車用信号機の青色点灯時間を比較して小さい方の値を取得する
                    greenSec = Math.Min(NorthLight.GreenLightOnSec, SouthLight.GreenLightOnSec);
                    carTrafficChange = SouthLight.CurrentTime;
                }

                if (DateTime.Now >= carTrafficChange.AddSeconds(greenSec - 3))
                {
                    Change_PedesLightOnState(pedesTraffic1, "Blink");
                    Change_PedesLightOnState(pedesTraffic2, "Blink");
                    pedesTraffic1.BlinkStartTime = DateTime.Now;
                    pedesTraffic2.BlinkStartTime = DateTime.Now;
                }
            }
            else if (pedesTraffic1.LightState == "Red")
            {
                if ((pedesTraffic1 == NorthPedesLight && EastLight.LightState  == "Green" && WestLight.LightState  == "Green") ||
                    (pedesTraffic1 == EastPedesLight  && NorthLight.LightState == "Green" && SouthLight.LightState == "Green"))
                {
                    Change_PedesLightOnState(pedesTraffic1, "Green");
                    Change_PedesLightOnState(pedesTraffic2, "Green");
                }
            }
            else if (pedesTraffic1.LightState == "Blink")
            {
                if (DateTime.Now >= pedesTraffic1.BlinkStartTime.AddSeconds(1))
                {
                    Change_PedesLightOnState(pedesTraffic1, "Red");
                    Change_PedesLightOnState(pedesTraffic2, "Red");
                }
                else if (DateTime.Now < pedesTraffic1.BlinkStartTime.AddSeconds(1))
                {
                    Change_PedesLightOnState(pedesTraffic1, "Blink");
                    Change_PedesLightOnState(pedesTraffic2, "Blink");
                }
            }
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態を切り替える
        /// </summary>
        /// <param name="pedesTraffic"> 歩行者用信号機を表すクラス           </param>
        /// <param name="state">        歩行者用信号機の点灯状態を表す文字列 </param>
        private void Change_PedesLightOnState(PedesTraffic pedesTraffic, string state)
        {
            pedesTraffic.LightState = state;

            if (state == "Green")
            {
                GetControl(pedesTraffic.GreenLabelName).BackColor  = TrafficLightGreen;
                GetControl(pedesTraffic.GreenLabelName2).BackColor = TrafficLightGreen;
                GetControl(pedesTraffic.RedLabelName).BackColor    = Color.White;
                GetControl(pedesTraffic.RedLabelName2).BackColor   = Color.White;
            }
            else if (state == "Red")
            {
                GetControl(pedesTraffic.GreenLabelName).BackColor  = Color.White;
                GetControl(pedesTraffic.GreenLabelName2).BackColor = Color.White;
                GetControl(pedesTraffic.RedLabelName).BackColor    = TrafficLightRed;
                GetControl(pedesTraffic.RedLabelName2).BackColor   = TrafficLightRed;
            }
            else if (state == "Blink")
            {
                Color greenLabelColor = TrafficLightGreen;

                GetControl(pedesTraffic.GreenLabelName).BackColor  = greenLabelColor;
                GetControl(pedesTraffic.GreenLabelName2).BackColor = greenLabelColor;
            }
        }

        /// <summary>
        /// 歩行者用信号機のランプを全て無灯火にする
        /// </summary>
        /// <param name="pedesTraffic"> 無灯火にする歩行者用信号機を表すクラス </param>
        private void PedesNoLight(PedesTraffic pedesTraffic)
        {
            GetControl(pedesTraffic.GreenLabelName).BackColor  = Color.White;
            GetControl(pedesTraffic.GreenLabelName2).BackColor = Color.White;
            GetControl(pedesTraffic.RedLabelName).BackColor    = Color.White;
            GetControl(pedesTraffic.RedLabelName2).BackColor   = Color.White;
        }

        /// <summary>
        /// Nameプロパティからフォーム内のコントロールを取得する
        /// </summary>
        /// <param name="ctlName"> コントロールのNameプロパティ値 </param>
        /// <returns> Nameプロパティ値がctlNameと一致するコントロール </returns>
        private Control GetControl(string ctlName)
        {
            Control[] ctl_Array = Controls.Find(ctlName, true);  // NameプロパティがctlNameのコントロールを取得する
            return ctl_Array[0];
        }
    }
}