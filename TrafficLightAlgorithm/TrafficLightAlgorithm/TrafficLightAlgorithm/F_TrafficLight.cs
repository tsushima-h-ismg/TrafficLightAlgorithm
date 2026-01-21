using System;
using System.Drawing;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    public partial class F_TrafficLight : Form
    {
        /// <summary>
        /// 車用信号機の青色灯火秒数の最大値
        /// </summary>
        private const int BlueLightOnSecMax = 10;

        /// <summary>
        /// 車用信号機の青色灯火秒数の最小値
        /// </summary>
        private const int BlueLightOnSecMin = 1;

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
            Change_InterruptResumeBtnTxt(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更

            Initialize_CarLight();  // 車用信号機を初期状態に設定する
            Timer_Traffic.Start();
        }

        /// <summary>
        /// 「中断」・「再開」ボタンクリック時イベント
        /// </summary>
        private void Btn_InterruptResume_Click(object sender, EventArgs e)
        {
            if (IsTrafficEnable)
            {
                // 信号機の点灯処理を実行している場合
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
                Timer_Traffic.Stop();
            }
            else
            {
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

            IsTrafficEnable = false;
            IsInterrupt     = false;
            Change_InterruptResumeBtnTxt(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更

            Timer_Traffic.Stop();
        }

        /// <summary>
        /// タイマーティックイベント
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
        /// <returns> checkTextをint型に変換した値がmaxValue以下、もしくはminValue以上の整数の場合はtrue。それ以外の場合はfalse </returns>
        private bool Check_SecText(string checkText, int maxValue, int minValue)
        {
            if (!int.TryParse(checkText, out int checkValue))   return false;  // チェック対象の文字列がint型に変換できない場合は終了する
            if (checkValue > maxValue || checkValue < minValue) return false;  // int型に変換した値がmaxValueより大きい、もしくはminValueより小さい場合は終了する
            return true;
        }

        /// <summary>
        /// 「中断/再開」ボタンのtextプロパティ値変更
        /// </summary>
        /// <param name="isInterrupt"> 「中断/再開」ボタンの状態を表すbool値 </param>
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
        /// 車用信号機の初期状態を設定
        /// </summary>
        private void Initialize_CarLight()
        {
            int.TryParse(txt_NLightOnSec.Text, out int n_GreSec);  // 北信号機の青色灯火時間を取得
            int.TryParse(txt_SLightOnSec.Text, out int s_GreSec);  // 南信号機の青色灯火時間を取得
            int.TryParse(txt_ELightOnSec.Text, out int e_GreSec);  // 東信号機の青色灯火時間を取得
            int.TryParse(txt_WLightOnSec.Text, out int w_GreSec);  // 西信号機の青色灯火時間を取得
            int.TryParse(txt_PrepareSec.Text,  out int preSec);    // 進行方向切り替え準備時間を取得

            // 車用信号機クラスのインスタンス生成
            NorthLight = new CarTraffic(lbl_NorthGreen, lbl_NorthYellow, lbl_NorthRed, n_GreSec, YellowSec, n_GreSec + YellowSec + preSec, DateTime.Now);
            SouthLight = new CarTraffic(lbl_SouthGreen, lbl_SouthYellow, lbl_SouthRed, e_GreSec, YellowSec, e_GreSec + YellowSec + preSec, DateTime.Now);
            EastLight  = new CarTraffic(lbl_EastGreen,  lbl_EastYellow,  lbl_EastRed,  s_GreSec, YellowSec, s_GreSec + YellowSec + preSec, DateTime.Now);
            WestLight  = new CarTraffic(lbl_WestGreen,  lbl_WestYellow,  lbl_WestRed,  w_GreSec, YellowSec, w_GreSec + YellowSec + preSec, DateTime.Now);

            // 車用信号機の初期の点灯色を設定
            NorthLight.LightState = "Green";
            SouthLight.LightState = "Green";
            EastLight.LightState  = "Red";
            WestLight.LightState  = "Red";

            // 車用信号機の初期の点灯色を反映
            NorthLight.GreenLight.BackColor = TrafficLightGreen;
            SouthLight.GreenLight.BackColor = TrafficLightGreen;
            EastLight.RedLight.BackColor    = TrafficLightRed;
            WestLight.RedLight.BackColor    = TrafficLightRed;
        }

        /// <summary>
        /// 信号機点灯制御
        /// </summary>
        private void TrafficLightControl()
        {
            TrafficLightOn(NorthLight);
            TrafficLightOn(SouthLight);
            TrafficLightOn(EastLight);
            TrafficLightOn(WestLight);
        }

        /// <summary>
        /// 車用信号機の点灯
        /// </summary>
        /// <param name="carLight"> 点灯させる車用信号機を表す車用信号機クラス </param>
        private void TrafficLightOn(CarTraffic carLight)
        {
            if (carLight.LightState == "Green")
            {
                if (DateTime.Now >= carLight.CurrentTime.AddSeconds(carLight.GreenLightOnSec))
                {
                    carLight.LightState = "Yellow";
                    carLight.GreenLight.BackColor = Color.White;
                    carLight.YellowLight.BackColor = TrafficLightYellow;
                    carLight.CurrentTime = DateTime.Now;
                }
            }
            else if (carLight.LightState == "Yellow")
            {
                if (DateTime.Now >= carLight.CurrentTime.AddSeconds(carLight.YellowLightOnSec))
                {
                    carLight.LightState = "Red";
                    carLight.YellowLight.BackColor = Color.White;
                    carLight.RedLight.BackColor = TrafficLightRed;
                    carLight.CurrentTime = DateTime.Now;
                }
            }
            else if (carLight.LightState == "Red")
            {
                if (DateTime.Now >= carLight.CurrentTime.AddSeconds(carLight.RedLightOnSec))
                {
                    carLight.LightState = "Green";
                    carLight.RedLight.BackColor = Color.White;
                    carLight.GreenLight.BackColor = TrafficLightGreen;
                    carLight.CurrentTime = DateTime.Now;
                }
            }
        }
    }
}
