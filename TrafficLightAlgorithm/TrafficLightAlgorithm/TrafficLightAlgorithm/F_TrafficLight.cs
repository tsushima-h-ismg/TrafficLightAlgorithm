using System;
using System.Drawing;
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
        /// 矢印信号機の緑色灯火秒数の最大値
        /// </summary>
        private const int ArrowLightSecMax = 5;

        /// <summary>
        /// 矢印信号機の緑色灯火秒数の最小値
        /// </summary>
        private const int ArrowLightSecMin = 1;

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
        /// 北車用信号機が緑色に点灯する時間
        /// </summary>
        private int NorthGreenLightOnSec;

        /// <summary>
        /// 南車用信号機が緑色に点灯する時間
        /// </summary>
        private int SouthGreenLightOnSec;

        /// <summary>
        /// 東車用信号機が緑色に点灯する時間
        /// </summary>
        private int EastGreenLightOnSec;

        /// <summary>
        /// 西車用信号機が緑色に点灯する時間
        /// </summary>
        private int WestGreenLightOnSec;

        /// <summary>
        /// 矢印信号機が緑色に点灯する時間
        /// </summary>
        private int ArrowLightOnSec;

        /// <summary>
        /// 進行方向切り替え準備時間
        /// </summary>
        private int PrepareSec;

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
        /// 信号機の無灯火を表す色
        /// </summary>
        private readonly Color TrafficNoLight = Color.White;

        /// <summary>
        /// 矢印信号機の緑を表す色
        /// </summary>
        private readonly Color ArrowGreen = Color.Green;

        /// <summary>
        /// 矢印信号機の無灯火を表す色
        /// </summary>
        private readonly Color ArrowDefault = Color.Black;

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
            string inputErrStr = CreateErrMsg();  // エラーメッセージを取得する

            // エラーメッセージ表示
            if (inputErrStr != "")
            {
                MessageBox.Show(inputErrStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 信号機点灯処理を中断している場合
            if (IsInterrupt)
            {
                string msgStr = "信号機の点灯処理を中断しています。処理を最初から実行しますか？";
                DialogResult dialogResult = MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.No) return;
            }

            IsTrafficEnable = true;
            IsInterrupt     = false;
            
            while (DateTime.Now.Millisecond < 100 || DateTime.Now.Millisecond > 200)
            {
                Thread.Sleep(10);  // 現在時刻のミリ秒が100から200の間の秒数になるまでスレッドを中断する
            }

            int.TryParse(txt_NLightOnSec.Text, out NorthGreenLightOnSec);  // 北車用信号機の青色灯火時間を取得
            int.TryParse(txt_SLightOnSec.Text, out SouthGreenLightOnSec);  // 南車用信号機の青色灯火時間を取得
            int.TryParse(txt_ELightOnSec.Text, out EastGreenLightOnSec);   // 東車用信号機の青色灯火時間を取得
            int.TryParse(txt_WLightOnSec.Text, out WestGreenLightOnSec);   // 西車用信号機の青色灯火時間を取得
            int.TryParse(txt_ArrowSec.Text,    out ArrowLightOnSec);       // 矢印信号機の青色灯火時間を取得
            int.TryParse(txt_Prepare.Text,     out PrepareSec);            // 進行方向切り替え準備時間を取得

            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTlpControlEnable(false);              // TableLayout内コントロールのEnabledプロパティ値変更
            InitializeCarLight();                       // 車用信号機を初期状態に設定する
            InitializePedesLight();                     // 歩行者用信号機を初期状態に設定する
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

            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更

            if (IsInterrupt)
            {
                InterruptTime = DateTime.Now;  // 中断時の時刻を取得する
                Timer_Traffic.Stop();
            }
            else
            {
                while (DateTime.Now.Millisecond < 100 || DateTime.Now.Millisecond > 200)
                {
                    Thread.Sleep(10);  // 現在時刻のミリ秒が100から200の間の秒数になるまでスレッドを中断する
                }
                
                NorthLight.UpdateStateChangeResumeTime(InterruptTime);  // 北方向車用信号機の点灯状態変更時刻を更新する
                SouthLight.UpdateStateChangeResumeTime(InterruptTime);  // 南方向車用信号機の点灯状態変更時刻を更新する
                EastLight.UpdateStateChangeResumeTime(InterruptTime);   // 東方向車用信号機の点灯状態変更時刻を更新する
                WestLight.UpdateStateChangeResumeTime(InterruptTime);   // 西方向車用信号機の点灯状態変更時刻を更新する

                NorthPedesLight.UpdateStateChangeResumeTime(InterruptTime);  // 北方向歩行者用信号機の点灯状態変更時刻を更新する
                SouthPedesLight.UpdateStateChangeResumeTime(InterruptTime);  // 南方向歩行者用信号機の点灯状態変更時刻を更新する
                EastPedesLight.UpdateStateChangeResumeTime(InterruptTime);   // 東方向歩行者用信号機の点灯状態変更時刻を更新する
                WestPedesLight.UpdateStateChangeResumeTime(InterruptTime);   // 西方向歩行者用信号機の点灯状態変更時刻を更新する
                
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

            // 車用信号機をイメージしたラベルの背景色を変更する
            ChangeTrafficLblColor(CarTrafficStateMem.NoLight, lbl_NorthGreen, lbl_NorthYellow, lbl_NorthRed, null);
            ChangeTrafficLblColor(CarTrafficStateMem.NoLight, lbl_SouthGreen, lbl_SouthYellow, lbl_SouthRed, null);
            ChangeTrafficLblColor(CarTrafficStateMem.NoLight, lbl_EastGreen,  lbl_EastYellow,  lbl_EastRed,  lbl_EastArrow);
            ChangeTrafficLblColor(CarTrafficStateMem.NoLight, lbl_WestGreen,  lbl_WestYellow,  lbl_WestRed,  lbl_WestArrow);

            // 歩行者用信号機をイメージしたラベルの背景色を変更する
            ChangePedesLblColor(PedesStateMem.NoLight, lbl_NorthPedesGreenOne, lbl_NorthPedesGreenTwo, lbl_NorthPedesRedOne, lbl_NorthPedesRedTwo);
            ChangePedesLblColor(PedesStateMem.NoLight, lbl_SouthPedesGreenOne, lbl_SouthPedesGreenTwo, lbl_SouthPedesRedOne, lbl_SouthPedesRedTwo);
            ChangePedesLblColor(PedesStateMem.NoLight, lbl_EastPedesGreenOne,  lbl_EastPedesGreenTwo,  lbl_EastPedesRedOne,  lbl_EastPedesRedTwo);
            ChangePedesLblColor(PedesStateMem.NoLight, lbl_WestPedesGreenOne,  lbl_WestPedesGreenTwo,  lbl_WestPedesRedOne,  lbl_WestPedesRedTwo);
            
            IsTrafficEnable = false;
            IsInterrupt     = false;
            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTlpControlEnable(true);               // TableLayout内コントロールのEnabledプロパティ値変更
            Timer_Traffic.Stop();
        }

        /// <summary>
        /// タイマーTickイベント
        /// </summary>
        private void Timer_Traffic_Tick(object sender, EventArgs e)
        {
            ControlTrafficLight();
        }

        /// <summary>
        /// エラーメッセージを作成する
        /// </summary>
        /// <returns> エラーメッセージを表す文字列 </returns>
        private string CreateErrMsg()
        {
            string errStr = "";  // エラーメッセージが入る

            int[,] SecMaxMinArr =
            {
                { BlueLightOnSecMin, BlueLightOnSecMin, BlueLightOnSecMin, BlueLightOnSecMin, ArrowLightSecMin, PrepareSecMin},
                { BlueLightOnSecMax, BlueLightOnSecMax, BlueLightOnSecMax, BlueLightOnSecMax, ArrowLightSecMax, PrepareSecMax}
            };

            for (int i = 0; i < tlp_InputSecField.RowCount - 1; i++)
            {
                // テキストボックスに入力された文字列がチェックを満たさない場合はエラーメッセージを追加する
                if (!CheckSecText(tlp_InputSecField.GetControlFromPosition(1, i + 1).Text, SecMaxMinArr[1, i], SecMaxMinArr[0, i]))
                {
                    errStr += $"「{tlp_InputSecField.GetControlFromPosition(0, i + 1).Text}」には{SecMaxMinArr[0, i]}から{SecMaxMinArr[1, i]}の整数を入力してください。\n";
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
        private bool CheckSecText(string checkText, int maxValue, int minValue)
        {
            if (!int.TryParse(checkText, out int checkValue))   return false;  // チェック対象の文字列がint型に変換できない場合は終了する
            if (checkValue > maxValue || checkValue < minValue) return false;  // int型に変換した値がmaxValueより大きい、もしくはminValueより小さい場合は終了する
            return true;
        }

        /// <summary>
        /// 「中断/再開」ボタンのtextプロパティ値変更
        /// </summary>
        /// <param name="isInterrupt"> 信号機点灯処理が中断している場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTextInterruptResumeBtn(bool isInterrupt)
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
        /// TableLayout内に配置したコントロールのEnabledプロパティ値を変更する
        /// </summary>
        /// <param name="enable"> Enabledプロパティを有効にする場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTlpControlEnable(bool enable)
        {
            for (int i = 1; i < tlp_InputSecField.RowCount; i++)
            {
                tlp_InputSecField.GetControlFromPosition(1, i).Enabled = enable;  // コントロールのEnabledプロパティ値変更
            }
        }

        /// <summary>
        /// 車用信号機の初期状態を設定
        /// </summary>
        private void InitializeCarLight()
        {
            // 車用信号機クラスのインスタンス生成
            NorthLight = new CarTraffic(NorthGreenLightOnSec, YellowSec, ArrowLightOnSec, false, true);
            SouthLight = new CarTraffic(SouthGreenLightOnSec, YellowSec, ArrowLightOnSec, false, true);
            EastLight  = new CarTraffic(EastGreenLightOnSec,  YellowSec, ArrowLightOnSec, true,  false);
            WestLight  = new CarTraffic(WestGreenLightOnSec,  YellowSec, ArrowLightOnSec, true,  false);

            // 車用信号機を初期の点灯状態に設定する
            ChangeTrafficLblColor(CarTrafficStateMem.Green, lbl_NorthGreen, lbl_NorthYellow, lbl_NorthRed, null);
            ChangeTrafficLblColor(CarTrafficStateMem.Green, lbl_SouthGreen, lbl_SouthYellow, lbl_SouthRed, null);
            ChangeTrafficLblColor(CarTrafficStateMem.Red,   lbl_EastGreen,  lbl_EastYellow,  lbl_EastRed,  lbl_EastArrow);
            ChangeTrafficLblColor(CarTrafficStateMem.Red,   lbl_WestGreen,  lbl_WestYellow,  lbl_WestRed,  lbl_WestArrow);
        }

        /// <summary>
        /// 歩行者用信号機の初期状態を設定
        /// </summary>
        private void InitializePedesLight()
        {
            int ns_CarGreenSec = Math.Min(NorthGreenLightOnSec, SouthGreenLightOnSec);  // 北と南の車用信号機が同時に緑色に点灯する時間を取得
            int ew_CarGreenSec = Math.Min(EastGreenLightOnSec,  WestGreenLightOnSec);   // 東と西の車用信号機が同時に緑色に点灯する時間を取得

            // 東方向と西方向の歩行者用信号機の点灯処理をずらす秒数を算出
            int minusSec = Math.Max(NorthGreenLightOnSec, SouthGreenLightOnSec) + YellowSec + PrepareSec + ew_CarGreenSec - EastLight.SecCount() - 1;

            // 歩行者用信号機のインスタンス生成
            NorthPedesLight = new PedesTraffic(ew_CarGreenSec, EastLight.SecCount()  - ew_CarGreenSec + 1, DateTime.Now.AddSeconds(minusSec), false);
            SouthPedesLight = new PedesTraffic(ew_CarGreenSec, EastLight.SecCount()  - ew_CarGreenSec + 1, DateTime.Now.AddSeconds(minusSec), false);
            EastPedesLight  = new PedesTraffic(ns_CarGreenSec, NorthLight.SecCount() - ns_CarGreenSec + 1, DateTime.Now, true);
            WestPedesLight  = new PedesTraffic(ns_CarGreenSec, NorthLight.SecCount() - ns_CarGreenSec + 1, DateTime.Now, true);

            // 歩行者用信号機を初期の点灯状態に設定する
            ChangePedesLblColor(PedesStateMem.Green, lbl_NorthPedesGreenOne, lbl_NorthPedesGreenTwo, lbl_NorthPedesRedOne, lbl_NorthPedesRedTwo);
            ChangePedesLblColor(PedesStateMem.Green, lbl_SouthPedesGreenOne, lbl_SouthPedesGreenTwo, lbl_SouthPedesRedOne, lbl_SouthPedesRedTwo);
            ChangePedesLblColor(PedesStateMem.Red,   lbl_EastPedesGreenOne,  lbl_EastPedesGreenTwo,  lbl_EastPedesRedOne,  lbl_EastPedesRedTwo);
            ChangePedesLblColor(PedesStateMem.Red,   lbl_WestPedesGreenOne,  lbl_WestPedesGreenTwo,  lbl_WestPedesRedOne,  lbl_WestPedesRedTwo);
        }

        /// <summary>
        /// 信号機点灯制御
        /// </summary>
        private void ControlTrafficLight()
        {
            // 車用信号機の点灯状態を更新するか判定し、判定を満たせば点灯状態を更新する
            if (NorthLight.JudgeTrafficLightOn()) UpdateLightOnState(NorthLight);
            if (SouthLight.JudgeTrafficLightOn()) UpdateLightOnState(SouthLight);
            if (EastLight.JudgeTrafficLightOn())  UpdateLightOnState(EastLight);
            if (WestLight.JudgeTrafficLightOn())  UpdateLightOnState(WestLight);

            // 歩行者用信号機の点灯状態を更新するか判定し、判定を満たせば点灯状態を更新する
            if (NorthPedesLight.JudgePedesLightOn()) UpdatePedesLightOnState(NorthPedesLight);
            if (SouthPedesLight.JudgePedesLightOn()) UpdatePedesLightOnState(SouthPedesLight);
            if (EastPedesLight.JudgePedesLightOn())  UpdatePedesLightOnState(EastPedesLight);
            if (WestPedesLight.JudgePedesLightOn())  UpdatePedesLightOnState(WestPedesLight);
        }

        /// <summary>
        /// 車用信号機の点灯状態を更新する
        /// </summary>
        /// <param name="carTraffic"> 車用信号機を表すクラス </param>
        private void UpdateLightOnState(CarTraffic carTraffic)
        {
            carTraffic.UpdateLightOnState();    // 車用信号機の点灯状態を更新する
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態を更新する
        /// </summary>
        /// <param name="pedesTraffic"> 歩行者用信号機を表すクラス </param>
        private void UpdatePedesLightOnState(PedesTraffic pedesTraffic)
        {
            pedesTraffic.UpdateLightOnState();  // 歩行者用信号機の点灯状態を更新する
        }

        /// <summary>
        /// 車用信号機の点灯状態を更新する
        /// </summary>
        /// <param name="carTraffic"> 車用信号機を表すクラス </param>
        private void ChangeTrafficLblColor(CarTrafficStateMem state, Label green, Label yellow, Label red, Label arrow)
        {
            Color greenLamp  = TrafficNoLight;
            Color yellowLamp = TrafficNoLight;
            Color redLamp    = TrafficNoLight;
            Color arrowLamp  = ArrowDefault;

            if (state == CarTrafficStateMem.Green)
            {
                greenLamp = TrafficLightGreen;
            }
            else if (state == CarTrafficStateMem.Yellow)
            {
                yellowLamp = TrafficLightYellow;
            }
            else if (state == CarTrafficStateMem.Red)
            {
                redLamp = TrafficLightRed;
            }
            else if (state == CarTrafficStateMem.Arrow)
            {
                arrowLamp = ArrowGreen;
            }

            green.BackColor  = greenLamp;   // 車用信号機の緑ランプを表すラベルの背景色を変更する
            yellow.BackColor = yellowLamp;  // 車用信号機の黄ランプを表すラベルの背景色を変更する 
            red.BackColor    = redLamp;     // 車用信号機の赤ランプを表すラベルの背景色を変更する

            // 矢印信号機のフォント色を変更する
            if (arrow != null) arrow.ForeColor = arrowLamp;  
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態を更新する
        /// </summary>
        /// <param name="state"> 　 点灯状態を表す列挙型 </param>
        /// <param name="greenOne"> 歩行者用信号機の緑ランプを表す１つ目のラベル </param>
        /// <param name="greenTwo"> 歩行者用信号機の緑ランプを表す２つ目のラベル </param>
        /// <param name="redOne">   歩行者用信号機の赤ランプを表す１つ目のラベル </param>
        /// <param name="redTwo">   歩行者用信号機の赤ランプを表す２つ目のラベル </param>
        private void ChangePedesLblColor(PedesStateMem state, Label greenOne, Label greenTwo, Label redOne, Label redTwo)
        {
            if (state == PedesStateMem.Green)
            {
                greenOne.BackColor = TrafficLightGreen;
                greenTwo.BackColor = TrafficLightGreen;
                redOne.BackColor   = TrafficNoLight;
                redTwo.BackColor   = TrafficNoLight;
            }
            else if (state == PedesStateMem.Red)
            {
                greenOne.BackColor = TrafficNoLight;
                greenTwo.BackColor = TrafficNoLight; 
                redOne.BackColor   = TrafficLightGreen;
                redTwo.BackColor   = TrafficLightGreen;
            }
        }
    }
}