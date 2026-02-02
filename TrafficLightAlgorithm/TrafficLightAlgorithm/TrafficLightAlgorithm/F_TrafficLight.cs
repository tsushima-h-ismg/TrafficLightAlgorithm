using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

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
        private const int ArrowLightSecMax  = 5;

        /// <summary>
        /// 矢印信号機の緑色灯火秒数の最小値
        /// </summary>
        private const int ArrowLightSecMin  = 1;

        /// <summary>
        /// 交差点の進行方向切り替え準備秒数の最大値
        /// </summary>
        private const int PrepareSecMax     = 5;

        /// <summary>
        /// 交差点の進行方向切り替え準備秒数の最小値
        /// </summary>
        private const int PrepareSecMin     = 1;

        /// <summary>
        /// 車用信号機の黄色灯火時間
        /// </summary>
        private const int YellowSec         = 1;

        /// <summary>
        /// 車用信号機の点滅間隔を表すミリ秒
        /// </summary>
        private const int BlinkTime         = 500;

        /// <summary>
        /// 信号機の緑を表す色
        /// </summary>
        private readonly Color TrafficLightGreen  = Color.ForestGreen;

        /// <summary>
        /// 信号機の黄を表す色
        /// </summary>
        private readonly Color TrafficLightYellow = Color.Yellow;

        /// <summary>
        /// 信号機の赤を表す色
        /// </summary>
        private readonly Color TrafficLightRed    = Color.Red;

        /// <summary>
        /// 信号機の無灯火を表す色
        /// </summary>
        private readonly Color TrafficNoLight     = Color.White;

        /// <summary>
        /// 矢印信号機の緑を表す色
        /// </summary>
        private readonly Color ArrowGreen         = Color.Green;

        /// <summary>
        /// 矢印信号機の無灯火を表す色
        /// </summary>
        private readonly Color ArrowDefault       = Color.Black;

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
        /// 信号機アルゴリズムのキャンセル設定
        /// </summary>
        private CancellationTokenSource CtsSource = new CancellationTokenSource();

        /// <summary>
        /// フォーム画面の「開始」ボタンクリックでtrue、「終了」ボタンクリックもしくはフォームロード時でfalse
        /// </summary>
        private bool IsTrafficEnable;

        /// <summary>
        /// 信号機点灯処理を中断している場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsInterrupt;

        /// <summary>
        /// 信号機点灯処理の中断時点のフェーズ
        /// </summary>
        private int InterruptPhase;

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

            int.TryParse(txt_NLightOnSec.Text, out NorthGreenLightOnSec);  // 北車用信号機の青色灯火時間を取得
            int.TryParse(txt_SLightOnSec.Text, out SouthGreenLightOnSec);  // 南車用信号機の青色灯火時間を取得
            int.TryParse(txt_ELightOnSec.Text, out EastGreenLightOnSec);   // 東車用信号機の青色灯火時間を取得
            int.TryParse(txt_WLightOnSec.Text, out WestGreenLightOnSec);   // 西車用信号機の青色灯火時間を取得
            int.TryParse(txt_ArrowSec.Text,    out ArrowLightOnSec);       // 矢印信号機の青色灯火時間を取得
            int.TryParse(txt_Prepare.Text,     out PrepareSec);            // 進行方向切り替え準備時間を取得

            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTlpControlEnable(false);              // TableLayout内コントロールのEnabledプロパティ値変更

            CtsSource = new CancellationTokenSource();
            List<TrafficPhase> phaseList = CreateTrafficPhase();
            LoopTrafficPhase(phaseList, 0);
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
                CtsSource.Cancel();
            }
            else
            {
                CtsSource = new CancellationTokenSource();
                List<TrafficPhase> phaseList = CreateTrafficPhase();
                LoopTrafficPhase(phaseList, InterruptPhase);
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
            ChangeTrafficLblColor(LightState.NoLight, lbl_NorthGreen, lbl_NorthYellow, lbl_NorthRed, null);
            ChangeTrafficLblColor(LightState.NoLight, lbl_SouthGreen, lbl_SouthYellow, lbl_SouthRed, null);
            ChangeTrafficLblColor(LightState.NoLight, lbl_EastGreen,  lbl_EastYellow,  lbl_EastRed,  lbl_EastArrow);
            ChangeTrafficLblColor(LightState.NoLight, lbl_WestGreen,  lbl_WestYellow,  lbl_WestRed,  lbl_WestArrow);

            // 歩行者用信号機をイメージしたラベルの背景色を変更する
            ChangePedesLblColor(LightState.NoLight, lbl_NorthPedesGreenOne, lbl_NorthPedesGreenTwo, lbl_NorthPedesRedOne, lbl_NorthPedesRedTwo);
            ChangePedesLblColor(LightState.NoLight, lbl_SouthPedesGreenOne, lbl_SouthPedesGreenTwo, lbl_SouthPedesRedOne, lbl_SouthPedesRedTwo);
            ChangePedesLblColor(LightState.NoLight, lbl_EastPedesGreenOne,  lbl_EastPedesGreenTwo,  lbl_EastPedesRedOne,  lbl_EastPedesRedTwo);
            ChangePedesLblColor(LightState.NoLight, lbl_WestPedesGreenOne,  lbl_WestPedesGreenTwo,  lbl_WestPedesRedOne,  lbl_WestPedesRedTwo);

            IsTrafficEnable = false;
            IsInterrupt     = false;
            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTlpControlEnable(true);               // TableLayout内コントロールのEnabledプロパティ値変更
            CtsSource.Cancel();
            InterruptPhase  = 0;
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
        /// 信号機アルゴリズムのフェーズリストを作成する
        /// </summary>
        /// <returns> 作成した信号機アルゴリズムのフェーズリスト </returns>
        private List<TrafficPhase> CreateTrafficPhase()
        {
            Traffic nsMax = GetGreenLightOnMaxTraffic(Traffic.CarNorth, Traffic.CarSouth);  // 北南で緑点灯時間が長い車用信号機
            Traffic nsMin = GetGreenLightOnMinTraffic(Traffic.CarNorth, Traffic.CarSouth);  // 北南で緑点灯時間が短い車用信号機 
            Traffic ewMax = GetGreenLightOnMaxTraffic(Traffic.CarEast,  Traffic.CarWest);   // 東西で緑点灯時間が長い車用信号機 
            Traffic ewMin = GetGreenLightOnMinTraffic(Traffic.CarEast,  Traffic.CarWest);   // 東西で緑点灯時間が短い車用信号機 

            int nsDiffer = Math.Abs(NorthGreenLightOnSec - SouthGreenLightOnSec);  // 北南の車用信号機の緑点灯時間の差
            int ewDiffer = Math.Abs(EastGreenLightOnSec  - WestGreenLightOnSec);   // 東西の車用信号機の緑点灯時間の差

            // 北南の車用信号機の黄・赤点灯
            TrafficPhase nsPhaseOne = new TrafficPhase(Math.Min(nsDiffer, YellowSec) * 1000, new TrafficCommand(nsMin, LightState.Yellow));  // 北南で緑点灯時間が短い車用信号機の黄点灯
            TrafficPhase nsPhaseTwo = new TrafficPhase(YellowSec * 1000,                     new TrafficCommand(nsMax, LightState.Yellow));  // 北南で緑点灯時間が長い車用信号機の黄点灯
            TrafficPhase nsPhaseThr = new TrafficPhase(0,                                    new TrafficCommand(nsMin, LightState.Red));     // 北南で緑点灯時間が短い車用信号機の赤点灯
            TrafficPhase nsPhaseFou = new TrafficPhase(PrepareSec * 1000,                    new TrafficCommand(nsMax, LightState.Red));     // 北南で緑点灯時間が長い車用信号機の赤点灯

            // 東西の車用信号機の黄・赤点灯
            TrafficPhase ewPhaseOne = new TrafficPhase(Math.Min(ewDiffer, YellowSec) * 1000, new TrafficCommand(ewMin, LightState.Yellow));  // 東西で緑点灯時間が短い車用信号機の黄点灯
            TrafficPhase ewPhaseTwo = new TrafficPhase(YellowSec * 1000,                     new TrafficCommand(ewMax, LightState.Yellow));  // 東西で緑点灯時間が長い車用信号機の黄点灯
            TrafficPhase ewPhaseThr = new TrafficPhase(0,                                    new TrafficCommand(ewMin, LightState.Red));     // 東西で緑点灯時間が短い車用信号機の赤点灯
            TrafficPhase ewPhaseFou = new TrafficPhase(1000,                                 new TrafficCommand(ewMax, LightState.Red));     // 東西で緑点灯時間が長い車用信号機の赤点灯

            // 北南で車用信号機の緑点灯時間が一致しない場合
            if (nsDiffer != 0)
            {
                nsPhaseTwo = new TrafficPhase((nsDiffer - 1) * 1000, new TrafficCommand(nsMin, LightState.Red));     // 北南で緑点灯時間が短い車用信号機の赤点灯
                nsPhaseThr = new TrafficPhase(YellowSec * 1000,      new TrafficCommand(nsMax, LightState.Yellow));  // 北南で緑点灯時間が長い車用信号機の黄点灯
            }

            // 東西で車用信号機の緑点灯時間が一致しない場合
            if (ewDiffer != 0) 
            {
                ewPhaseTwo = new TrafficPhase((ewDiffer - 1) * 1000, new TrafficCommand(ewMin, LightState.Red));     // 東西で緑点灯時間が短い車用信号機の赤点灯
                ewPhaseThr = new TrafficPhase(YellowSec * 1000,      new TrafficCommand(ewMax, LightState.Yellow));  // 東西で緑点灯時間が長い車用信号機の黄点灯
            }

            return new List<TrafficPhase>
            {
                // 全ての車用・歩行者用信号機を赤に点灯する
                new TrafficPhase(PrepareSec * 1000,
                    new TrafficCommand(Traffic.CarNorth,   LightState.Red),
                    new TrafficCommand(Traffic.CarSouth,   LightState.Red),
                    new TrafficCommand(Traffic.CarEast,    LightState.Red),
                    new TrafficCommand(Traffic.CarWest,    LightState.Red),
                    new TrafficCommand(Traffic.PedesNorth, LightState.Red),
                    new TrafficCommand(Traffic.PedesSouth, LightState.Red),
                    new TrafficCommand(Traffic.PedesEast,  LightState.Red),
                    new TrafficCommand(Traffic.PedesWest,  LightState.Red)),

                // 北南の車用信号機・東西の歩行者用信号機を緑に点灯する
                new TrafficPhase(Math.Min(NorthGreenLightOnSec, SouthGreenLightOnSec) * 1000 - BlinkTime * 5 - 1000,
                    new TrafficCommand(Traffic.CarNorth,  LightState.Green),
                    new TrafficCommand(Traffic.CarSouth,  LightState.Green),
                    new TrafficCommand(Traffic.PedesEast, LightState.Green),
                    new TrafficCommand(Traffic.PedesWest, LightState.Green)),

                // 東西の歩行者用信号機の点滅
                new TrafficPhase(BlinkTime,
                    new TrafficCommand(Traffic.PedesEast, LightState.NoLight),
                    new TrafficCommand(Traffic.PedesWest, LightState.NoLight)),
                new TrafficPhase(BlinkTime,
                    new TrafficCommand(Traffic.PedesEast, LightState.Green),
                    new TrafficCommand(Traffic.PedesWest, LightState.Green)),
                new TrafficPhase(BlinkTime,
                    new TrafficCommand(Traffic.PedesEast, LightState.NoLight),
                    new TrafficCommand(Traffic.PedesWest, LightState.NoLight)),
                new TrafficPhase(BlinkTime,
                    new TrafficCommand(Traffic.PedesEast, LightState.Green),
                    new TrafficCommand(Traffic.PedesWest, LightState.Green)),
                new TrafficPhase(BlinkTime,
                    new TrafficCommand(Traffic.PedesEast, LightState.NoLight),
                    new TrafficCommand(Traffic.PedesWest, LightState.NoLight)),
                new TrafficPhase(1000,
                    new TrafficCommand(Traffic.PedesEast, LightState.Red),
                    new TrafficCommand(Traffic.PedesWest, LightState.Red)),

                // 北南の車用信号機の黄・赤点灯
                nsPhaseOne,
                nsPhaseTwo,
                nsPhaseThr,
                nsPhaseFou, 

                // 東西の車用信号機・北南の歩行者用信号機を緑に点灯する
                new TrafficPhase(Math.Min(EastGreenLightOnSec, WestGreenLightOnSec) * 1000 - BlinkTime * 5 - 1000,
                    new TrafficCommand(Traffic.CarEast,    LightState.Green),
                    new TrafficCommand(Traffic.CarWest,    LightState.Green),
                    new TrafficCommand(Traffic.PedesNorth, LightState.Green),
                    new TrafficCommand(Traffic.PedesSouth, LightState.Green)),
                
                // 北南の歩行者用信号機の点滅
                new TrafficPhase(BlinkTime,
                    new TrafficCommand(Traffic.PedesNorth, LightState.NoLight),
                    new TrafficCommand(Traffic.PedesSouth, LightState.NoLight)),
                new TrafficPhase(BlinkTime,
                    new TrafficCommand(Traffic.PedesNorth, LightState.Green),
                    new TrafficCommand(Traffic.PedesSouth, LightState.Green)),
                new TrafficPhase(BlinkTime,
                    new TrafficCommand(Traffic.PedesNorth, LightState.NoLight),
                    new TrafficCommand(Traffic.PedesSouth, LightState.NoLight)),
                new TrafficPhase(BlinkTime,
                    new TrafficCommand(Traffic.PedesNorth, LightState.Green),
                    new TrafficCommand(Traffic.PedesSouth, LightState.Green)),
                new TrafficPhase(BlinkTime,
                    new TrafficCommand(Traffic.PedesNorth, LightState.NoLight),
                    new TrafficCommand(Traffic.PedesSouth, LightState.NoLight)),
                new TrafficPhase(1000,
                    new TrafficCommand(Traffic.PedesNorth, LightState.Red),
                    new TrafficCommand(Traffic.PedesSouth, LightState.Red)),

                // 東西の車用信号機の黄・赤点灯
                ewPhaseOne,
                ewPhaseTwo,
                ewPhaseThr,
                ewPhaseFou,

                // 東西の矢印信号機を点灯する
                new TrafficPhase(ArrowLightOnSec * 1000,
                    new TrafficCommand(Traffic.CarEast, LightState.Arrow),
                    new TrafficCommand(Traffic.CarWest, LightState.Arrow)),

                // 東西の車用信号機を黄に点灯する
                new TrafficPhase(YellowSec * 1000,
                    new TrafficCommand(Traffic.CarEast, LightState.Yellow),
                    new TrafficCommand(Traffic.CarWest, LightState.Yellow)),
            };
        }

        /// <summary>
        /// 緑点灯時間が短い車用信号機を表す列挙型を返す
        /// </summary>
        /// <param name="directionOne"> 比較対象となる１つ目の車用信号機 </param>
        /// <param name="directionTwo"> 比較対象となる２つ目の車用信号機 </param>
        /// <returns> 緑点灯時間が短い車用信号機を表す列挙型 </returns>
        private Traffic GetGreenLightOnMinTraffic(Traffic directionOne, Traffic directionTwo)
        {
            if (directionOne == Traffic.CarNorth && directionTwo == Traffic.CarSouth)
            {
                if (NorthGreenLightOnSec >= SouthGreenLightOnSec) return Traffic.CarSouth;  // 南車用信号機を表す列挙型
                return Traffic.CarNorth;                                                    // 北車用信号機を表す列挙型
            }
            else
            {
                if (EastGreenLightOnSec >= WestGreenLightOnSec) return Traffic.CarWest;  // 西車用信号機を表す列挙型
                return Traffic.CarEast;                                                  // 東車用信号機を表す列挙型
            }
        }

        /// <summary>
        /// 緑点灯時間が長い車用信号機を表す列挙型を返す
        /// </summary>
        /// <param name="directionOne"> 比較対象となる１つ目の車用信号機 </param>
        /// <param name="directionTwo"> 比較対象となる２つ目の車用信号機 </param>
        /// <returns> 緑点灯時間が長い車用信号機を表す列挙型 </returns>
        private Traffic GetGreenLightOnMaxTraffic(Traffic directionOne, Traffic directionTwo)
        {
            if (directionOne == Traffic.CarNorth && directionTwo == Traffic.CarSouth)
            {
                if (NorthGreenLightOnSec < SouthGreenLightOnSec) return Traffic.CarSouth;  // 南車用信号機を表す列挙型
                return Traffic.CarNorth;                                                   // 北車用信号機を表す列挙型
            }
            else
            {
                if (EastGreenLightOnSec < WestGreenLightOnSec) return Traffic.CarWest;  // 西車用信号機を表す列挙型
                return Traffic.CarEast;                                                 // 東車用信号機を表す列挙型
            }
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストをループさせる
        /// </summary>
        /// <param name="phases">   信号機アルゴリズムのフェーズリスト </param>
        /// <param name="phaseNum"> 開始フェーズを表す数値             </param>
        private async void LoopTrafficPhase(List<TrafficPhase> phases, int phaseNum)
        {
            int startPhase = phaseNum;

            while (true)
            {
                for (int i = startPhase; i < phases.Count; i++)
                {
                    InterruptPhase = i;

                    foreach(TrafficCommand command in phases[i].Commands)
                    {
                        if (CtsSource.IsCancellationRequested) break;

                        if (command.Traffic == Traffic.CarNorth)
                        {
                            // 北車用信号機の点灯
                            ChangeTrafficLblColor(command.State, lbl_NorthGreen, lbl_NorthYellow, lbl_NorthRed, null);
                        }
                        else if (command.Traffic == Traffic.CarSouth)
                        {
                            // 南車用信号機の点灯
                            ChangeTrafficLblColor(command.State, lbl_SouthGreen, lbl_SouthYellow, lbl_SouthRed, null);
                        }
                        else if (command.Traffic == Traffic.CarEast)
                        {
                            // 東車用信号機の点灯
                            ChangeTrafficLblColor(command.State, lbl_EastGreen, lbl_EastYellow, lbl_EastRed, lbl_EastArrow);
                        }
                        else if (command.Traffic == Traffic.CarWest)
                        {
                            // 西車用信号機の点灯
                            ChangeTrafficLblColor(command.State, lbl_WestGreen, lbl_WestYellow, lbl_WestRed, lbl_WestArrow);
                        }
                        else if (command.Traffic == Traffic.PedesNorth)
                        {
                            // 北歩行者用信号機の点灯
                            ChangePedesLblColor(command.State, lbl_NorthPedesGreenOne, lbl_NorthPedesGreenTwo, lbl_NorthPedesRedOne, lbl_NorthPedesRedTwo);
                        }
                        else if (command.Traffic == Traffic.PedesSouth)
                        {
                            // 南歩行者用信号機の点灯
                            ChangePedesLblColor(command.State, lbl_SouthPedesGreenOne, lbl_SouthPedesGreenTwo, lbl_SouthPedesRedOne, lbl_SouthPedesRedTwo);
                        }
                        else if (command.Traffic == Traffic.PedesEast)
                        {
                            // 東歩行者用信号機の点灯
                            ChangePedesLblColor(command.State, lbl_EastPedesGreenOne, lbl_EastPedesGreenTwo, lbl_EastPedesRedOne, lbl_EastPedesRedTwo);
                        }
                        else if (command.Traffic == Traffic.PedesWest)
                        {
                            // 西歩行者用信号機の点灯
                            ChangePedesLblColor(command.State, lbl_WestPedesGreenOne, lbl_WestPedesGreenTwo, lbl_WestPedesRedOne, lbl_WestPedesRedTwo);
                        }
                    }

                    await Task.Delay(phases[i].WaitMilliSeconds);
                    if (CtsSource.IsCancellationRequested) break;
                }
                
                if (CtsSource.IsCancellationRequested) break;
                startPhase = 0;
            }
        }

        /// <summary>
        /// 車用信号機の点灯状態を更新する
        /// </summary>
        /// <param name="carTraffic"> 車用信号機を表すクラス </param>
        private void ChangeTrafficLblColor(LightState state, Label green, Label yellow, Label red, Label arrow)
        {
            Color greenLamp  = TrafficNoLight;
            Color yellowLamp = TrafficNoLight;
            Color redLamp    = TrafficNoLight;
            Color arrowLamp  = ArrowDefault;

            if (state == LightState.Green)
            {
                greenLamp = TrafficLightGreen;    // 点灯状態が緑の場合
            }
            else if (state == LightState.Yellow)
            {
                yellowLamp = TrafficLightYellow;  // 点灯状態が黄の場合
            }
            else if (state == LightState.Red)
            {
                redLamp = TrafficLightRed;        // 点灯状態が赤の場合
            }
            else if (state == LightState.Arrow)
            {
                arrowLamp = ArrowGreen;           // 矢印信号機を点灯する場合
                redLamp   = TrafficLightRed;
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
        private void ChangePedesLblColor(LightState state, Label greenOne, Label greenTwo, Label redOne, Label redTwo)
        {
            if (state == LightState.Green)
            {
                // 点灯状態が緑の場合
                greenOne.BackColor = TrafficLightGreen;
                greenTwo.BackColor = TrafficLightGreen;
                redOne.BackColor   = TrafficNoLight;
                redTwo.BackColor   = TrafficNoLight;
            }
            else if (state == LightState.Red)
            {
                // 点灯状態が赤の場合
                greenOne.BackColor = TrafficNoLight;
                greenTwo.BackColor = TrafficNoLight; 
                redOne.BackColor   = TrafficLightRed;
                redTwo.BackColor   = TrafficLightRed;
            }
            else if (state == LightState.NoLight)
            {
                // 点灯状態が無灯火の場合
                greenOne.BackColor = TrafficNoLight;
                greenTwo.BackColor = TrafficNoLight;
                redOne.BackColor   = TrafficNoLight;
                redTwo.BackColor   = TrafficNoLight;
            }
        }
    }
}