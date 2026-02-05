using System;
using System.Drawing;
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
        /// 車用信号機の黄色灯火時間ミリ秒
        /// </summary>
        private const int YellowMSec        = 1000;

        /// <summary>
        /// 車用信号機の点滅間隔ミリ秒
        /// </summary>
        private const int BlinkMSec         = 500;

        /// <summary>
        /// 信号機点灯ミリ秒の最小値
        /// </summary>
        private const int MinMSec           = 1000;

        /// <summary>
        /// 歩行者用信号機の点滅にかけるフェーズ数
        /// </summary>
        private const int BlinkPhaseCount   = 5;

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
        /// 北車用信号機が緑色に点灯するミリ秒
        /// </summary>
        private int NorthGreenMSec;

        /// <summary>
        /// 南車用信号機が緑色に点灯するミリ秒
        /// </summary>
        private int SouthGreenMSec;

        /// <summary>
        /// 東車用信号機が緑色に点灯するミリ秒
        /// </summary>
        private int EastGreenMSec;

        /// <summary>
        /// 西車用信号機が緑色に点灯するミリ秒
        /// </summary>
        private int WestGreenMSec;

        /// <summary>
        /// 矢印信号機が緑色に点灯するミリ秒
        /// </summary>
        private int ArrowMSec;

        /// <summary>
        /// 進行方向切り替え準備ミリ秒
        /// </summary>
        private int PrepareMSec;

        /// <summary>
        /// 信号機点灯処理の中断時点のフェーズを表す番号
        /// </summary>
        private int InterruptPhase;

        /// <summary>
        /// フォーム画面の「開始」ボタンクリックでtrue、「終了」ボタンクリックもしくはフォームロード時でfalse
        /// </summary>
        private bool IsTrafficEnable;

        /// <summary>
        /// 信号機点灯処理を中断している場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsInterrupt;

        /// <summary>
        /// 信号機点灯処理を停止する場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsCancel;

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
            string inputErrMsg = CreateErrMsg();  // 入力した値についてのエラーメッセージを取得する

            // エラーメッセージ表示
            if (inputErrMsg != "")
            {
                MessageBox.Show(inputErrMsg, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 信号機点灯処理を中断している場合
            if (IsInterrupt)
            {
                string msgStr = "信号機の点灯処理を中断しています。処理を最初から実行しますか？";
                DialogResult dialogResult = MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.No) return;
            }

            int.TryParse(txt_NLightOnSec.Text, out NorthGreenMSec);  // 北車用信号機の青色灯火時間を取得
            int.TryParse(txt_SLightOnSec.Text, out SouthGreenMSec);  // 南車用信号機の青色灯火時間を取得
            int.TryParse(txt_ELightOnSec.Text, out EastGreenMSec);   // 東車用信号機の青色灯火時間を取得
            int.TryParse(txt_WLightOnSec.Text, out WestGreenMSec);   // 西車用信号機の青色灯火時間を取得
            int.TryParse(txt_ArrowSec.Text,    out ArrowMSec);       // 矢印信号機の青色灯火時間を取得
            int.TryParse(txt_Prepare.Text,     out PrepareMSec);     // 進行方向切り替え準備時間を取得

            NorthGreenMSec *= 1000;
            SouthGreenMSec *= 1000;
            EastGreenMSec  *= 1000;
            WestGreenMSec  *= 1000;
            ArrowMSec      *= 1000;
            PrepareMSec    *= 1000;

            IsTrafficEnable = true;
            IsInterrupt     = false;
            IsCancel        = false; 

            ChangeTextInterruptResumeBtn(IsInterrupt);            // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnabled(false);                          // テキストボックスのEnabledプロパティ値変更        
            List<TrafficPhase> phaseList = CreateTrafficPhase();  // 信号機アルゴリズムのフェーズリストを作成
            LoopTrafficPhase(phaseList, 0);                     　// 信号機点灯処理をループさせる
        }

        /// <summary>
        /// 「中断/再開」ボタンクリック時イベント
        /// </summary>
        private void Btn_InterruptResume_Click(object sender, EventArgs e)
        {
            if (IsTrafficEnable)
            {
                // 開始ボタンが押されて終了ボタンが押されていない場合はIsInterruptのtrue/falseを切り替える
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
                IsCancel = true;  // 信号機点灯処理を停止する
            }
            else
            {
                IsCancel = false;
                List<TrafficPhase> phaseList = CreateTrafficPhase();  // 信号機アルゴリズムのフェーズリストを作成
                LoopTrafficPhase(phaseList, InterruptPhase);          // 信号機点灯処理を中断した箇所から再開する
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
            IsCancel        = true;
            InterruptPhase  = 0;

            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnabled(true);                 // テキストボックスのEnabledプロパティ値変更
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
                { BlueLightOnSecMin, BlueLightOnSecMin, BlueLightOnSecMin, BlueLightOnSecMin, ArrowLightSecMin, PrepareSecMin },
                { BlueLightOnSecMax, BlueLightOnSecMax, BlueLightOnSecMax, BlueLightOnSecMax, ArrowLightSecMax, PrepareSecMax }
            };

            for (int i = 0; i < tlp_InputSecField.RowCount - 1; i++)
            {
                // コントロールのTextプロパティ値がチェックを満たさない場合はエラーメッセージを追加する
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
                btn_InterruptResume.Text = "再開";  // isInterruptがtrueの場合Textプロパティ値は「再開」に設定する     
            }
            else
            {
                btn_InterruptResume.Text = "中断";  // isInterruptがfalseの場合Textプロパティ値は「中断」に設定する   
            }
        }

        /// <summary>
        /// テキストボックスのEnabledプロパティ値を変更する
        /// </summary>
        /// <param name="enable"> Enabledプロパティを有効にする場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTextBoxEnabled(bool enable)
        {
            txt_NLightOnSec.Enabled = enable;
            txt_SLightOnSec.Enabled = enable;
            txt_ELightOnSec.Enabled = enable;
            txt_WLightOnSec.Enabled = enable;
            txt_ArrowSec.Enabled    = enable;
            txt_Prepare.Enabled     = enable;
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストを作成する
        /// </summary>
        /// <returns> 作成した信号機アルゴリズムのフェーズリスト </returns>
        private List<TrafficPhase> CreateTrafficPhase()
        {
            List<TrafficPhase> phaseList = new List<TrafficPhase>
            {
                // 全ての車用・歩行者用信号機の赤点灯
                CreateMultiLightOn(PrepareMSec, LightState.Red,
                    Traffic.CarNorth,   Traffic.CarSouth,   Traffic.CarEast,   Traffic.CarWest,
                    Traffic.PedesNorth, Traffic.PedesSouth, Traffic.PedesEast, Traffic.PedesWest),

                // 北南の車用信号機・東西の歩行者用信号機の緑点灯
                CreateMultiLightOn(Math.Min(NorthGreenMSec, SouthGreenMSec) - BlinkMSec * BlinkPhaseCount - MinMSec, LightState.Green,
                    Traffic.CarNorth,  Traffic.CarSouth, 
                    Traffic.PedesEast, Traffic.PedesWest)
            };
            
            // 東西の歩行者用信号機の点滅
            phaseList.AddRange(PedesBlink(Traffic.PedesEast, Traffic.PedesWest));

            // 北南の車用信号機の黄・赤点灯
            if (NorthGreenMSec >= SouthGreenMSec)
            {
                phaseList.AddRange(CarYellowRed(Traffic.CarSouth, Traffic.CarNorth, NorthGreenMSec - SouthGreenMSec, PrepareMSec));
            }
            else
            {
                phaseList.AddRange(CarYellowRed(Traffic.CarNorth, Traffic.CarSouth, SouthGreenMSec - NorthGreenMSec, PrepareMSec));
            }

            // 東西の車用信号機・北南の歩行者用信号機を緑に点灯する
            phaseList.Add(CreateMultiLightOn(Math.Min(EastGreenMSec, WestGreenMSec) - BlinkMSec * BlinkPhaseCount - MinMSec, LightState.Green,
                Traffic.CarEast,    Traffic.CarWest, 
                Traffic.PedesNorth, Traffic.PedesSouth));

            // 北南の歩行者用信号機の点滅
            phaseList.AddRange(PedesBlink(Traffic.PedesNorth, Traffic.PedesSouth));

            // 東西の車用信号機の黄・赤点灯
            if (EastGreenMSec >= WestGreenMSec)
            {
                phaseList.AddRange(CarYellowRed(Traffic.CarWest, Traffic.CarEast, EastGreenMSec - WestGreenMSec, MinMSec));
            }
            else
            {
                phaseList.AddRange(CarYellowRed(Traffic.CarEast, Traffic.CarWest, WestGreenMSec - EastGreenMSec, MinMSec));
            }

            // 東西の矢印信号機の点灯
            phaseList.Add(CreateMultiLightOn(ArrowMSec, LightState.Arrow, Traffic.CarEast, Traffic.CarWest));

            // 東西の車用信号機の黄点灯
            phaseList.Add(CreateMultiLightOn(YellowMSec, LightState.Yellow, Traffic.CarEast, Traffic.CarWest));

            return phaseList;
        }

        /// <summary>
        /// 複数の車用・歩行者用信号機を同じ時間・同じ色に点灯するフェーズを作成
        /// </summary>
        /// <param name="mSec">     点灯時間ミリ秒 </param>
        /// <param name="state">    点灯状態       </param>
        /// <param name="traffics"> 点灯する信号機 </param>
        /// <returns> 作成したフェーズ </returns>
        private TrafficPhase CreateMultiLightOn(int mSec, LightState state, params Traffic[] traffics)
        {
            TrafficCommand[] commands = new TrafficCommand[traffics.Length];

            for (int i = 0; i < commands.Length; i++)
            {
                commands[i] = new TrafficCommand(traffics[i], state);
            }

            return new TrafficPhase(mSec, commands);
        }

        /// <summary>
        /// 歩行者用信号機の点滅フェーズリスト
        /// </summary>
        /// <param name="pedesOne"> 歩行者用信号機を表す１つ目の列挙型 </param>
        /// <param name="pedesTwo"> 歩行者用信号機を表す２つ目の列挙型 </param>
        /// <returns> pedesOneとpedesTwoが表す歩行者用信号機の点滅フェーズリスト </returns>
        private List<TrafficPhase> PedesBlink(Traffic pedesOne, Traffic pedesTwo)
        {
            List<TrafficPhase> pedesBlink = new List<TrafficPhase>();

            // 歩行者用信号機が点滅するようにフェーズを追加する
            for (int i = 0; i < BlinkPhaseCount; i++)
            {
                if (i % 2 == 0) pedesBlink.Add(CreateMultiLightOn(BlinkMSec, LightState.NoLight, pedesOne, pedesTwo));
                if (i % 2 == 1) pedesBlink.Add(CreateMultiLightOn(BlinkMSec, LightState.Green,   pedesOne, pedesTwo));
            }

            // 歩行者用信号機を赤に点灯するフェーズを追加する
            pedesBlink.Add(CreateMultiLightOn(MinMSec, LightState.Red, pedesOne, pedesTwo));

            return pedesBlink;
        }

        /// <summary>
        /// 車用信号機の黄・赤点灯フェーズリスト
        /// </summary>
        /// <param name="carOne">     緑点灯時間が短い車用信号機を表す列挙型                 </param>
        /// <param name="carTwo">     緑点灯時間が長い車用信号機を表す列挙型                 </param>
        /// <param name="differMSec"> carOneとcarTwoが表す車用信号機の緑点灯時間の差(ミリ秒) </param>
        /// <param name="redMSec">    carTwoが表す車用信号機の赤点灯ミリ秒                   </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> CarYellowRed(Traffic carOne, Traffic carTwo, int differMSec, int redMSec)
        {
            if (differMSec == 0)
            {
                // carOneとcarTwoが表す車用信号機の緑点灯時間が一致する場合
                return new List<TrafficPhase>
                {
                    CreateMultiLightOn(YellowMSec, LightState.Yellow, carOne, carTwo),  // carOneとcarTwoが表す車用信号機を黄に点灯
                    CreateMultiLightOn(redMSec,    LightState.Red,    carOne, carTwo)   // carOneとcarTwoが表す車用信号機を黄に点灯
                };
            }
            else if (differMSec == MinMSec)
            {
                // carOneとcarTwoが表す車用信号機の緑点灯時間の差がMinMSecミリ秒の場合
                return new List<TrafficPhase>
                {
                    new TrafficPhase(YellowMSec, 
                        new TrafficCommand(carOne, LightState.Yellow)),  // carOneが表す車用信号機を黄に点灯
                    new TrafficPhase(YellowMSec, 
                        new TrafficCommand(carTwo, LightState.Yellow), 
                        new TrafficCommand(carOne, LightState.Red)),     // carOneが表す車用信号機を赤、carTwoが表す車用信号機を黄に点灯
                    new TrafficPhase(redMSec,
                        new TrafficCommand(carTwo, LightState.Red)),     // carTwoが表す車用信号機を赤に点灯
                };
            }

            return new List<TrafficPhase>
            {
                new TrafficPhase(YellowMSec,
                    new TrafficCommand(carOne, LightState.Yellow)),  // carOneが表す車用信号機を黄に点灯
                new TrafficPhase(differMSec - YellowMSec,
                    new TrafficCommand(carOne, LightState.Red)),     // carOneが表す車用信号機を赤に点灯
                new TrafficPhase(YellowMSec,
                    new TrafficCommand(carTwo, LightState.Yellow)),  // carTwoが表す車用信号機を黄に点灯
                new TrafficPhase(redMSec,
                    new TrafficCommand(carTwo, LightState.Red)),     // carTwoが表す車用信号機を赤に点灯
            };
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストをループさせる
        /// </summary>
        /// <param name="phases">   信号機アルゴリズムのフェーズリスト </param>
        /// <param name="phaseNum"> 開始フェーズを表す数値             </param>
        private async void LoopTrafficPhase(List<TrafficPhase> phases, int phaseNum)
        {
            int startPhase = phaseNum;  // 開始するフェーズを表す番号

            while (!IsCancel)
            {
                for (int i = startPhase; i < phases.Count; i++)
                {
                    InterruptPhase = i;  // 現在の点灯フェーズを取得する

                    foreach(TrafficCommand command in phases[i].Commands)
                    {
                        // 信号機点灯処理でキャンセルが要求された場合はループから脱出する
                        if (IsCancel) break;

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

                    await Task.Delay(phases[i].WaitMSec);  // WaitMilliSecondsミリ秒間待機する

                    if (IsCancel) break;  // 信号機点灯処理でキャンセルが要求された場合はループから脱出する
                }

                if (IsCancel) break;  // 信号機点灯処理でキャンセルが要求された場合はループから脱出する

                startPhase = 0;  // フェーズを最初から繰り返す
            }
        }

        /// <summary>
        /// 車用信号機の点灯状態を更新する
        /// </summary>
        /// <param name="state">  点灯状態を表す列挙型             </param>
        /// <param name="green">  車用信号機の緑ランプを表すラベル </param>
        /// <param name="yellow"> 車用信号機の黄ランプを表すラベル </param>
        /// <param name="red">    車用信号機の赤ランプを表すラベル </param>
        /// <param name="arrow">  矢印信号機を表すラベル           </param>
        private void ChangeTrafficLblColor(LightState state, Label green, Label yellow, Label red, Label arrow)
        {
            Color greenLamp  = TrafficNoLight;  // 車用信号機の緑ランプを表すラベルの背景色
            Color yellowLamp = TrafficNoLight;  // 車用信号機の黄ランプを表すラベルの背景色
            Color redLamp    = TrafficNoLight;  // 車用信号機の赤ランプを表すラベルの背景色
            Color arrowLamp  = ArrowDefault;    // 矢印信号機を表すラベルの背景色

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

            if (arrow != null) arrow.ForeColor = arrowLamp;  // 矢印信号機を表すラベルのフォント色を変更する
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態を更新する
        /// </summary>
        /// <param name="state"> 　 点灯状態を表す列挙型                         </param>
        /// <param name="greenOne"> 歩行者用信号機の緑ランプを表す１つ目のラベル </param>
        /// <param name="greenTwo"> 歩行者用信号機の緑ランプを表す２つ目のラベル </param>
        /// <param name="redOne">   歩行者用信号機の赤ランプを表す１つ目のラベル </param>
        /// <param name="redTwo">   歩行者用信号機の赤ランプを表す２つ目のラベル </param>
        private void ChangePedesLblColor(LightState state, Label greenOne, Label greenTwo, Label redOne, Label redTwo)
        {
            Color greenOneLamp = TrafficNoLight;  // 歩行者用信号機の緑ランプを表す１つ目のラベル背景色
            Color greenTwoLamp = TrafficNoLight;  // 歩行者用信号機の緑ランプを表す２つ目のラベル背景色
            Color redOneLamp   = TrafficNoLight;  // 歩行者用信号機の赤ランプを表す１つ目のラベル背景色
            Color redTwoLamp   = TrafficNoLight;  // 歩行者用信号機の赤ランプを表す２つ目のラベル背景色

            if (state == LightState.Green)
            {
                // 点灯状態が緑の場合
                greenOneLamp = TrafficLightGreen;
                greenTwoLamp = TrafficLightGreen;
            }
            else if (state == LightState.Red)
            {
                // 点灯状態が赤の場合
                redOneLamp = TrafficLightRed;
                redTwoLamp = TrafficLightRed;
            }

            greenOne.BackColor = greenOneLamp;  // 歩行者用信号機の緑ランプを表す１つ目のラベル背景色を変更する
            greenTwo.BackColor = greenTwoLamp;  // 歩行者用信号機の緑ランプを表す２つ目のラベル背景色を変更する
            redOne.BackColor   = redOneLamp;    // 歩行者用信号機の赤ランプを表す１つ目のラベル背景色を変更する
            redTwo.BackColor   = redTwoLamp;    // 歩行者用信号機の赤ランプを表す２つ目のラベル背景色を変更する
        }
    }
}