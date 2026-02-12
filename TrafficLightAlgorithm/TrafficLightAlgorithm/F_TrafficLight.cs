using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using TrafficLightAlgorithm.Properties;

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
        /// 歩行者用信号機の点滅のフェーズ数
        /// </summary>
        private const int BlinkPhaseCount   = 5;

        /// <summary>
        /// 信号機点灯処理の中断時点のフェーズを表す番号
        /// </summary>
        private int InterruptPhase;

        /// <summary>
        /// 信号機アルゴリズムを動かす場合はtrue、それ以外の場合はfalse
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

        /// <summary>
        /// 信号機アルゴリズムのフェーズリスト
        /// </summary>
        private List<TrafficPhase> PhaseList;

        public F_TrafficLight()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロードイベント
        /// </summary>
        private void F_TrafficLight_Load(object sender, EventArgs e)
        {
            IsTrafficEnable = false;  // 信号機アルゴリズムを無効にする
            IsInterrupt     = false;  // 信号機アルゴリズムの中断を無効にする
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

            // テキストボックスTextプロパティを数値に変換し、設定値を構造体で受け取る
            WaitMSec waitMSec = new WaitMSec(txt_NLightOnSec.Text, txt_SLightOnSec.Text, txt_ELightOnSec.Text, 
                                             txt_WLightOnSec.Text, txt_ArrowSec.Text,    txt_Prepare.Text);

            IsTrafficEnable = true;   // 信号機アルゴリズムを有効にする
            IsInterrupt     = false;  // 信号機アルゴリズムの中断を無効にする
            IsCancel        = false;  // 信号機アルゴリズムのキャンセルを無効にする

            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnabled(false);                // 値を入力するテキストボックスEnabledプロパティ値変更        
            PhaseList = CreateTrafficPhase(waitMSec);   // 信号機アルゴリズムのフェーズリストを作成
            LoopTrafficPhase(0);                     　 // 信号機点灯処理をループさせる
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
                return;  // IsTrafficEnableがfalseの場合は終了する
            }

            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更

            if (IsInterrupt)
            {
                IsCancel = true;  // 信号機アルゴリズムのキャンセルを有効にする
            }
            else
            {
                IsCancel = false;                  // 信号機アルゴリズムのキャンセルを無効にする
                LoopTrafficPhase(InterruptPhase);  // 信号機点灯処理を中断した箇所から再開する
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

            IsTrafficEnable = false;  // 信号機アルゴリズムを無効にする
            IsInterrupt     = false;  // 信号機アルゴリズム中断を無効にする
            IsCancel        = true;   // 信号機アルゴリズムのキャンセルを有効にする

            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnabled(true);                 // テキストボックスのEnabledプロパティ値変更

            // 車用信号機を無灯火にする
            ChangeSignalLightOn(LightState.NoLight, lbl_picNorth, null);
            ChangeSignalLightOn(LightState.NoLight, lbl_picSouth, null);
            ChangeSignalLightOn(LightState.NoLight, lbl_picEast,  lbl_picEastArrow);
            ChangeSignalLightOn(LightState.NoLight, lbl_picWest,  lbl_picWestArrow);

            // 歩行者用信号機を無灯火にする
            ChangePedesLightOn(LightState.NoLight, lbl_PedesNorthOne, lbl_PedesNorthTwo);
            ChangePedesLightOn(LightState.NoLight, lbl_PedesSouthOne, lbl_PedesSouthTwo);
            ChangePedesLightOn(LightState.NoLight, lbl_PedesEastOne,  lbl_PedesEastTwo);
            ChangePedesLightOn(LightState.NoLight, lbl_PedesWestOne,  lbl_PedesWestTwo);
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
        /// <param name="stTraffic"> 信号機の設定値を表す構造体 </param>
        /// <returns> 作成した信号機アルゴリズムのフェーズリスト </returns>
        private List<TrafficPhase> CreateTrafficPhase(WaitMSec stTraffic)
        {
            List<TrafficPhase> phaseList = new List<TrafficPhase>
            {
                // 全ての車用・歩行者用信号機の赤点灯
                CreateMultiLightOn(stTraffic.PMSec, LightState.Red,
                    Traffic.CarNorth,   Traffic.CarSouth,   Traffic.CarEast,   Traffic.CarWest,
                    Traffic.PedesNorth, Traffic.PedesSouth, Traffic.PedesEast, Traffic.PedesWest),

                // 北南の車用信号機・東西の歩行者用信号機の緑点灯
                CreateMultiLightOn(Math.Min(stTraffic.NMSec, stTraffic.SMSec) - BlinkMSec * BlinkPhaseCount - MinMSec, LightState.Green,
                    Traffic.CarNorth,  Traffic.CarSouth, 
                    Traffic.PedesEast, Traffic.PedesWest)
            };
            
            // 東西の歩行者用信号機の点滅
            for (int i = 0; i < BlinkPhaseCount; i++)
            {
                if (i % 2 == 0) phaseList.Add(CreateMultiLightOn(BlinkMSec, LightState.NoLight, Traffic.PedesEast, Traffic.PedesWest));
                if (i % 2 == 1) phaseList.Add(CreateMultiLightOn(BlinkMSec, LightState.Green,   Traffic.PedesEast, Traffic.PedesWest));
            }

            // 東西の歩行者用信号機を赤に点灯
            phaseList.Add(CreateMultiLightOn(MinMSec, LightState.Red, Traffic.PedesEast, Traffic.PedesWest));

            // 北南の車用信号機の黄・赤点灯
            if (stTraffic.NMSec >= stTraffic.SMSec)
            {
                phaseList.AddRange(CarYellowRed(stTraffic.PMSec, stTraffic.NMSec - stTraffic.SMSec, Traffic.CarSouth, Traffic.CarNorth));
            }
            else
            {
                phaseList.AddRange(CarYellowRed(stTraffic.PMSec, stTraffic.SMSec - stTraffic.NMSec, Traffic.CarNorth, Traffic.CarSouth));
            }

            // 東西の車用信号機・北南の歩行者用信号機を緑に点灯する
            phaseList.Add(CreateMultiLightOn(Math.Min(stTraffic.EMSec, stTraffic.WMSec) - BlinkMSec * BlinkPhaseCount - MinMSec, LightState.Green,
                Traffic.CarEast,    Traffic.CarWest, 
                Traffic.PedesNorth, Traffic.PedesSouth));

            // 北南の歩行者用信号機の点滅
            for (int i = 0; i < BlinkPhaseCount; i++)
            {
                if (i % 2 == 0) phaseList.Add(CreateMultiLightOn(BlinkMSec, LightState.NoLight, Traffic.PedesNorth, Traffic.PedesSouth));
                if (i % 2 == 1) phaseList.Add(CreateMultiLightOn(BlinkMSec, LightState.Green,   Traffic.PedesNorth, Traffic.PedesSouth));
            }

            // 北南の歩行者用信号機を赤に点灯
            phaseList.Add(CreateMultiLightOn(MinMSec, LightState.Red, Traffic.PedesNorth, Traffic.PedesSouth));

            // 東西の車用信号機の黄・赤点灯
            if (stTraffic.EMSec >= stTraffic.WMSec)
            {
                phaseList.AddRange(CarYellowRed(MinMSec, stTraffic.EMSec - stTraffic.WMSec, Traffic.CarWest, Traffic.CarEast));
            }
            else
            {
                phaseList.AddRange(CarYellowRed(MinMSec, stTraffic.WMSec - stTraffic.EMSec, Traffic.CarEast, Traffic.CarWest));
            }

            // 東西の矢印信号機の点灯
            phaseList.Add(CreateMultiLightOn(stTraffic.AMSec, LightState.Arrow, Traffic.CarEast, Traffic.CarWest));

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
            TrafficCommand[] commands = new TrafficCommand[traffics.Length];  // １フェーズで行う信号機点灯設定が入る

            for (int i = 0; i < commands.Length; i++)
            {
                commands[i] = new TrafficCommand(traffics[i], state);
            }

            return new TrafficPhase(mSec, commands);
        }

        /// <summary>
        /// 車用信号機の黄・赤点灯フェーズリストを作成する
        /// </summary>
        /// <param name="redMSec">    carTwoが表す車用信号機の赤点灯ミリ秒                   </param>
        /// <param name="differMSec"> carOneとcarTwoが表す車用信号機の緑点灯時間の差(ミリ秒) </param>
        /// <param name="carMin">     緑点灯時間が短い車用信号機を表す列挙型                 </param>
        /// <param name="carMax">     緑点灯時間が長い車用信号機を表す列挙型                 </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> CarYellowRed(int redMSec, int differMSec, Traffic carMin, Traffic carMax)
        {
            if (differMSec == 0)
            {
                // carOneとcarTwoが表す車用信号機の緑点灯時間が一致する場合
                return new List<TrafficPhase>
                {
                    CreateMultiLightOn(YellowMSec, LightState.Yellow, carMin, carMax),  // carMinとcarMaxが表す車用信号機を黄に点灯
                    CreateMultiLightOn(redMSec,    LightState.Red,    carMin, carMax)   // carMinとcarMaxが表す車用信号機を黄に点灯
                };
            }
            else if (differMSec == MinMSec)
            {
                // differMSecとMinMSecが等しい場合
                return new List<TrafficPhase>
                {
                    new TrafficPhase(YellowMSec, 
                        new TrafficCommand(carMin, LightState.Yellow)),  // carMinが表す車用信号機を黄に点灯
                    new TrafficPhase(YellowMSec, 
                        new TrafficCommand(carMax, LightState.Yellow), 
                        new TrafficCommand(carMin, LightState.Red)),     // carMinが表す車用信号機を赤、carMaxが表す車用信号機を黄に点灯
                    new TrafficPhase(redMSec,
                        new TrafficCommand(carMax, LightState.Red)),     // carMaxが表す車用信号機を赤に点灯
                };
            }

            return new List<TrafficPhase>
            {
                new TrafficPhase(YellowMSec,
                    new TrafficCommand(carMin, LightState.Yellow)),  // carMinが表す車用信号機を黄に点灯
                new TrafficPhase(differMSec - YellowMSec,
                    new TrafficCommand(carMin, LightState.Red)),     // carMinが表す車用信号機を赤に点灯
                new TrafficPhase(YellowMSec,
                    new TrafficCommand(carMax, LightState.Yellow)),  // carMaxが表す車用信号機を黄に点灯
                new TrafficPhase(redMSec,
                    new TrafficCommand(carMax, LightState.Red)),     // carMaxが表す車用信号機を赤に点灯
            };
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストをループさせる
        /// </summary>
        /// <param name="phaseNum"> 開始フェーズを表す数値 </param>
        private async void LoopTrafficPhase(int phaseNum)
        {
            int startPhase = phaseNum;  // 開始フェーズを表す番号

            while (!IsCancel)
            {
                for (int i = startPhase; i < PhaseList.Count; i++)
                {
                    InterruptPhase = i;   // 現在の点灯フェーズを取得する

                    foreach(TrafficCommand command in PhaseList[i].Commands)
                    {
                        if (command.Traffic == Traffic.CarNorth)
                        {
                            ChangeSignalLightOn(command.State, lbl_picNorth, null);  // 北車用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.CarSouth)
                        {
                            ChangeSignalLightOn(command.State, lbl_picSouth, null);  // 南車用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.CarEast)
                        {
                            ChangeSignalLightOn(command.State, lbl_picEast, lbl_picEastArrow);  // 東車用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.CarWest)
                        {
                            ChangeSignalLightOn(command.State, lbl_picWest, lbl_picWestArrow);  // 西車用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.PedesNorth)
                        {
                            ChangePedesLightOn(command.State, lbl_PedesNorthOne, lbl_PedesNorthTwo);  // 北歩行者用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.PedesSouth)
                        {
                            ChangePedesLightOn(command.State, lbl_PedesSouthOne, lbl_PedesSouthTwo);  // 南歩行者用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.PedesEast)
                        {
                            ChangePedesLightOn(command.State, lbl_PedesEastOne, lbl_PedesEastTwo);  // 東歩行者用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.PedesWest)
                        {
                            ChangePedesLightOn(command.State, lbl_PedesWestOne, lbl_PedesWestTwo);  // 西歩行者用信号機の点灯
                        }
                    }

                    await Task.Delay(PhaseList[i].WaitMSec);  // WaitMSecミリ秒間待機する
                    if (IsCancel) break;                      // 信号機点灯処理でキャンセルが要求された場合はループから脱出する
                }

                startPhase = 0;  // フェーズを最初から繰り返す
            }
        }

        /// <summary>
        /// 車用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">      点灯状態を表す列挙型               </param>
        /// <param name="picLabel">   車用信号機の画像を貼り付けたラベル </param>
        /// <param name="arrowLabel"> 矢印信号機の画像を貼り付けたラベル </param>
        private void ChangeSignalLightOn(LightState state, Label picLabel, Label arrowLabel)
        {
            if (state == LightState.NoLight) picLabel.Image = Resources.NoLightSignal;      // 無灯火イメージ画像を取得する
            if (state == LightState.Green)   picLabel.Image = Resources.GreenLightSignal;   // 緑点灯イメージ画像を取得する
            if (state == LightState.Yellow)  picLabel.Image = Resources.YellowLightSignal;  // 黄点灯イメージ画像を取得する
            if (state == LightState.Red)     picLabel.Image = Resources.RedLightSignal;     // 赤点灯イメージ画像を取得する

            // 矢印信号機の点灯
            if (arrowLabel != null)
            {
                if (state == LightState.Arrow)
                {
                    picLabel.Image   = Resources.RedLightSignal;  // 赤点灯イメージ画像を取得する
                    arrowLabel.Image = Resources.ArrowGreen;      // 矢印信号機点灯イメージ画像を取得する
                }
                else
                {
                    arrowLabel.Image = Resources.ArrowDefault;  // 矢印信号機無灯火イメージ画像を取得する
                }
            }
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">         点灯状態を表す列挙型                           </param>
        /// <param name="pedesLabelOne"> 歩行者用信号機の画像を貼り付けた１つ目のラベル </param>
        /// <param name="pedesLabelTwo"> 歩行者用信号機の画像を貼り付けた２つ目のラベル </param>
        private void ChangePedesLightOn(LightState state, Label pedesLabelOne, Label pedesLabelTwo)
        {
            Bitmap bmp = Resources.PedesNoLight;  // 無灯火イメージ画像を取得する

            if (state == LightState.Green) bmp = Resources.PedesGreen;  // 緑点灯イメージ画像を取得する
            if (state == LightState.Red)   bmp = Resources.PedesRed;    // 赤点灯イメージ画像を取得する

            pedesLabelOne.Image = bmp;
            pedesLabelTwo.Image = bmp;
        }
    }
}