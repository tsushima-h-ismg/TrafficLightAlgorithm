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
        private const int GreenSecMax     = 20;

        /// <summary>
        /// 車用信号機の緑色灯火秒数の最小値
        /// </summary>
        private const int GreenSecMin     = 5;

        /// <summary>
        /// 矢印信号機の緑色灯火秒数の最大値
        /// </summary>
        private const int ArrowSecMax     = 5;

        /// <summary>
        /// 矢印信号機の緑色灯火秒数の最小値
        /// </summary>
        private const int ArrowSecMin     = 1;

        /// <summary>
        /// 交差点の進行方向切り替え準備秒数の最大値
        /// </summary>
        private const int PrepaSecMax     = 5;

        /// <summary>
        /// 交差点の進行方向切り替え準備秒数の最小値
        /// </summary>
        private const int PrepaSecMin     = 1;

        /// <summary>
        /// 車用信号機の黄点灯ミリ秒
        /// </summary>
        private const int YellowMSec      = 1000;

        /// <summary>
        /// 車用信号機の点滅間隔ミリ秒
        /// </summary>
        private const int BlinkMSec       = 500;

        /// <summary>
        /// 信号機点灯ミリ秒の最小値
        /// </summary>
        private const int MinMSec         = 1000;

        /// <summary>
        /// 歩行者用信号機点滅の合計フェーズ数
        /// </summary>
        private const int BlinkPhaseCount = 5;

        /// <summary>
        /// 信号機点灯処理の中断時点のフェーズを表す番号
        /// </summary>
        private int InterruptPhase;

        /// <summary>
        /// 信号機アルゴリズムが動いている・中断している場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsTrafficEnable;

        /// <summary>
        /// 信号機アルゴリズムの中断が有効の場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsInterrupt;

        /// <summary>
        /// 信号機アルゴリズムを停止する場合はtrue、それ以外の場合はfalse
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
            IsTrafficEnable = false;  // 信号機アルゴリズムが動かない場合のbool値に設定する
            IsInterrupt     = false;  // 信号機アルゴリズムの中断を無効にする
        }

        /// <summary>
        /// 「開始」ボタンクリック時イベント
        /// </summary>
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            string errMsg = CreateErrMsg();  // エラーメッセージを取得する

            // エラーメッセージ表示
            if (errMsg != "")
            {
                MessageBox.Show(errMsg, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 信号機点灯処理を中断している場合
            if (IsInterrupt)
            {
                string msgStr = "信号機の点灯処理を中断しています。処理を最初から実行しますか？";
                DialogResult diresult = MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (diresult == DialogResult.No) return;
            }
            else
            {
                if (IsTrafficEnable) return;  // 信号機点灯処理が中断されず動いている場合は終了する
            }

            // 信号機アルゴリズムの設定値構造体を取得する
            WaitMSec waitMSec = new WaitMSec(txt_NLightSec.Text, txt_SLightSec.Text, txt_ELightSec.Text,
                                             txt_WLightSec.Text, txt_ArrowSec.Text,  txt_PrepaSec.Text);
            
            IsTrafficEnable = true;   // 信号機アルゴリズムが動く場合のブール値に設定する
            IsInterrupt     = false;  // 信号機アルゴリズムの中断を無効にする
            IsCancel        = false;  // 信号機アルゴリズムの停止を無効にする            

            ChangeTextInterruptResumeBtn();                // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnabled(false);                   // テキストボックスのEnabledプロパティ値変更        
            PhaseList = CreateTrafficPhaseList(waitMSec);  // 信号機アルゴリズムのフェーズリストを作成
            LoopTrafficPhase(0);                     　    // フェーズリストの最初のフェーズからループを開始する
        }

        /// <summary>
        /// 「中断/再開」ボタンクリック時イベント
        /// </summary>
        private void Btn_InterruptResume_Click(object sender, EventArgs e)
        {
            if (IsTrafficEnable)
            {
                if (IsInterrupt)
                {
                    IsInterrupt = false;  // 信号機アルゴリズムの中断を無効にする
                }
                else
                {
                    IsInterrupt = true;   // 信号機アルゴリズムの中断を有効にする
                }

                ChangeTextInterruptResumeBtn();  // 「中断/再開」ボタンのTextプロパティ値変更

                if (IsInterrupt)
                {
                    IsCancel = true;  // 信号機アルゴリズムの停止を有効にする
                }
                else
                {
                    IsCancel = false;                  // 信号機アルゴリズムの停止を無効にする
                    LoopTrafficPhase(InterruptPhase);  // 中断したフェーズからループを開始する
                }
            }
        }

        /// <summary>
        /// 「停止」ボタンクリック時イベント
        /// </summary>
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            if (IsTrafficEnable)
            {
                string msgStr = "信号機の点灯処理を停止しますか？";
                DialogResult diresult = MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (diresult == DialogResult.No) return;
            }

            IsTrafficEnable = false;  // 信号機アルゴリズムが動かない場合のブール値に設定する
            IsInterrupt     = false;  // 信号機アルゴリズムの中断を無効にする
            IsCancel        = true;   // 信号機アルゴリズムの停止を有効にする

            ChangeTextInterruptResumeBtn();  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnabled(true);      // テキストボックスのEnabledプロパティ値変更

            ChangeSignalLightOn(LightState.NoLight, lbl_picNorth, null);              // 北車用信号機を無灯火にする
            ChangeSignalLightOn(LightState.NoLight, lbl_picSouth, null);              // 南車用信号機を無灯火にする
            ChangeSignalLightOn(LightState.NoLight, lbl_picEast,  lbl_picEastArrow);  // 東車用信号機を無灯火にする
            ChangeSignalLightOn(LightState.NoLight, lbl_picWest,  lbl_picWestArrow);  // 西車用信号機を無灯火にする

            ChangePedesLightOn(LightState.NoLight, lbl_PedesNorthOne, lbl_PedesNorthTwo);  // 北歩行者用信号機を無灯火にする
            ChangePedesLightOn(LightState.NoLight, lbl_PedesSouthOne, lbl_PedesSouthTwo);  // 南歩行者用信号機を無灯火にする
            ChangePedesLightOn(LightState.NoLight, lbl_PedesEastOne,  lbl_PedesEastTwo);   // 東歩行者用信号機を無灯火にする
            ChangePedesLightOn(LightState.NoLight, lbl_PedesWestOne,  lbl_PedesWestTwo);   // 西歩行者用信号機を無灯火にする
        }
        
        /// <summary>
        /// 「バージョン情報」ボタンクリック時イベント
        /// </summary>
        private void Btn_VersionShow_Click(object sender, EventArgs e)
        {
            Version ver    = typeof(F_TrafficLight).Assembly.GetName().Version;  // バージョン情報を取得
            string  verstr = $"Ver{ver.Minor}.{ver.Build}{ver.Revision}";        // Ver{マイナーバージョン}.{ビルド番号}{リビジョン} 形式の文字列を作成する

            MessageBox.Show(verstr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// エラーメッセージを作成する
        /// </summary>
        /// <returns> エラーメッセージを表す文字列 </returns>
        private string CreateErrMsg()
        {
            string errMsg = "";  // エラーメッセージが入る

            // テキストボックスのTextプロパティ値がチェックを満たさない場合はエラーメッセージを追加する
            if (!CheckSecText(txt_NLightSec.Text, GreenSecMin, GreenSecMax)) errMsg += $"「{lbl_NLightSec.Text}」には{GreenSecMin}から{GreenSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_SLightSec.Text, GreenSecMin, GreenSecMax)) errMsg += $"「{lbl_SLightSec.Text}」には{GreenSecMin}から{GreenSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_ELightSec.Text, GreenSecMin, GreenSecMax)) errMsg += $"「{lbl_ELightSec.Text}」には{GreenSecMin}から{GreenSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_WLightSec.Text, GreenSecMin, GreenSecMax)) errMsg += $"「{lbl_WLightSec.Text}」には{GreenSecMin}から{GreenSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_ArrowSec.Text,  ArrowSecMin, ArrowSecMax)) errMsg += $"「{lbl_ArrowSec.Text}」には{ArrowSecMin}から{ArrowSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_PrepaSec.Text,  PrepaSecMin, PrepaSecMax)) errMsg += $"「{lbl_PrepaSec.Text}」には{PrepaSecMin}から{PrepaSecMax}の整数を入力してください。\n";

            return errMsg;
        }

        /// <summary>
        /// 文字列が最大値と最小値の範囲内の整数を表す値かチェックを行う
        /// </summary>
        /// <param name="checkText"> チェック対象の文字列         </param>
        /// <param name="maxValue">  チェックを満たす整数の最大値 </param>
        /// <param name="minValue">  チェックを満たす整数の最小値 </param>
        /// <returns> checkTextをint型に変換した値がminValue以上でmaxValue以下の整数の場合はtrue、それ以外の場合はfalse </returns>
        private bool CheckSecText(string checkText, int minValue, int maxValue)
        {
            if (!int.TryParse(checkText, out int checkValue))   return false;  // チェック対象の文字列がint型に変換できない場合は終了する
            if (checkValue < minValue || checkValue > maxValue) return false;  // int型に変換した値がminValueより小さい、もしくはmaxValueより大きい場合は終了する
            return true;
        }

        /// <summary>
        /// 「中断/再開」ボタンのtextプロパティ値変更
        /// </summary>
        private void ChangeTextInterruptResumeBtn()
        {
            // IsInterruptがtrueの場合のTextプロパティ値は「再開」、それ以外の場合は「中断」に設定する
            if (IsInterrupt) 
            { 
                btn_InterruptResume.Text = "再開";  
            }
            else
            {
                btn_InterruptResume.Text = "中断";
            }
        }

        /// <summary>
        /// テキストボックスのEnabledプロパティ値を変更する
        /// </summary>
        /// <param name="enable"> Enabledプロパティを有効にする場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTextBoxEnabled(bool enable)
        {
            txt_NLightSec.Enabled = enable;  // 北車用信号機の緑点灯秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_SLightSec.Enabled = enable;  // 南車用信号機の緑点灯秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_ELightSec.Enabled = enable;  // 東車用信号機の緑点灯秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_WLightSec.Enabled = enable;  // 西車用信号機の緑点灯秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_ArrowSec.Enabled  = enable;  // 矢印信号機の点灯秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_PrepaSec.Enabled  = enable;  // 進行方向切り替え準備秒数を入力するテキストボックスのEnabledプロパティ値変更
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストを作成する
        /// </summary>
        /// <param name="setTraffic"> 信号機の設定値構造体 </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> CreateTrafficPhaseList(WaitMSec setTraffic)
        {
            List<TrafficPhase> phaseList = new List<TrafficPhase>
            {
                // 全ての車用・歩行者用信号機の赤点灯
                CreatePhase(setTraffic.PMSec, LightState.Red,
                    Traffic.CarNorth,   Traffic.CarSouth,   Traffic.CarEast,   Traffic.CarWest,
                    Traffic.PedesNorth, Traffic.PedesSouth, Traffic.PedesEast, Traffic.PedesWest),

                // 北南の車用・東西の歩行者用信号機の緑点灯
                CreatePhase(Math.Min(setTraffic.NMSec, setTraffic.SMSec) - BlinkMSec * BlinkPhaseCount - MinMSec, LightState.Green,
                    Traffic.CarNorth,  Traffic.CarSouth,
                    Traffic.PedesEast, Traffic.PedesWest)
            };

            // 東西の歩行者用信号機の点滅
            for (int i = 0; i < BlinkPhaseCount; i++)
            {
                if (i % 2 == 0) phaseList.Add(CreatePhase(BlinkMSec, LightState.NoLight, Traffic.PedesEast, Traffic.PedesWest));
                if (i % 2 == 1) phaseList.Add(CreatePhase(BlinkMSec, LightState.Green,   Traffic.PedesEast, Traffic.PedesWest));
            }

            // 東西の歩行者用信号機の赤点灯
            phaseList.Add(CreatePhase(MinMSec, LightState.Red, Traffic.PedesEast, Traffic.PedesWest));

            // 北南の車用信号機の黄・赤点灯
            if (setTraffic.NMSec >= setTraffic.SMSec)
            {
                phaseList.AddRange(CreateYellowRedPhaseList(setTraffic.PMSec, setTraffic.NMSec - setTraffic.SMSec, Traffic.CarSouth, Traffic.CarNorth));
            }
            else
            {
                phaseList.AddRange(CreateYellowRedPhaseList(setTraffic.PMSec, setTraffic.SMSec - setTraffic.NMSec, Traffic.CarNorth, Traffic.CarSouth));
            }

            // 東西の車用・北南の歩行者用信号機の緑点灯
            phaseList.Add(CreatePhase(Math.Min(setTraffic.EMSec, setTraffic.WMSec) - BlinkMSec * BlinkPhaseCount - MinMSec, LightState.Green,
                Traffic.CarEast,    Traffic.CarWest, 
                Traffic.PedesNorth, Traffic.PedesSouth));

            // 北南の歩行者用信号機の点滅
            for (int i = 0; i < BlinkPhaseCount; i++)
            {
                if (i % 2 == 0) phaseList.Add(CreatePhase(BlinkMSec, LightState.NoLight, Traffic.PedesNorth, Traffic.PedesSouth));
                if (i % 2 == 1) phaseList.Add(CreatePhase(BlinkMSec, LightState.Green,   Traffic.PedesNorth, Traffic.PedesSouth));
            }

            // 北南の歩行者用信号機の赤点灯
            phaseList.Add(CreatePhase(MinMSec, LightState.Red, Traffic.PedesNorth, Traffic.PedesSouth));

            // 東西の車用信号機の黄・赤点灯
            if (setTraffic.EMSec >= setTraffic.WMSec)
            {
                phaseList.AddRange(CreateYellowRedPhaseList(MinMSec, setTraffic.EMSec - setTraffic.WMSec, Traffic.CarWest, Traffic.CarEast));
            }
            else
            {
                phaseList.AddRange(CreateYellowRedPhaseList(MinMSec, setTraffic.WMSec - setTraffic.EMSec, Traffic.CarEast, Traffic.CarWest));
            }

            // 東西の矢印信号機の点灯
            phaseList.Add(CreatePhase(setTraffic.AMSec, LightState.Arrow, Traffic.CarEast, Traffic.CarWest));

            // 東西の車用信号機の黄点灯
            phaseList.Add(CreatePhase(YellowMSec, LightState.Yellow, Traffic.CarEast, Traffic.CarWest));  

            return phaseList;
        }

        /// <summary>
        /// 複数の車用・歩行者用信号機を同じ時間・同じ色に点灯するフェーズを作成
        /// </summary>
        /// <param name="mSec">     点灯時間ミリ秒 </param>
        /// <param name="state">    点灯状態       </param>
        /// <param name="traffics"> 点灯する信号機 </param>
        /// <returns> 作成したフェーズ </returns>
        private TrafficPhase CreatePhase(int mSec, LightState state, params Traffic[] traffics)
        {
            TrafficCommand[] commands = new TrafficCommand[traffics.Length];  // １フェーズで設定する信号機点灯状態の設定値が入る

            for (int i = 0; i < commands.Length; i++)
            {
                commands[i] = new TrafficCommand(traffics[i], state);  // 点灯状態を表す設定値を信号機ごとに１つずつ入れる
            }

            return new TrafficPhase(mSec, commands);
        }

        /// <summary>
        /// 車用信号機の黄・赤点灯フェーズリストを作成する
        /// </summary>
        /// <param name="redMSec"> carMaxが表す車用信号機の赤点灯ミリ秒             </param>
        /// <param name="difMSec"> carMinとcarMaxが表す車用信号機の緑点灯ミリ秒の差 </param>
        /// <param name="carMin">  緑点灯時間が短い車用信号機を表す列挙型           </param>
        /// <param name="carMax">  緑点灯時間が長い車用信号機を表す列挙型           </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> CreateYellowRedPhaseList(int redMSec, int difMSec, Traffic carMin, Traffic carMax)
        {
            if (difMSec == 0)
            {
                // carMinとcarMaxが表す車用信号機の緑点灯ミリ秒が一致する場合
                return new List<TrafficPhase>
                {
                    CreatePhase(YellowMSec, LightState.Yellow, carMin, carMax),  // carMinとcarMaxが表す車用信号機を黄に点灯
                    CreatePhase(redMSec,    LightState.Red,    carMin, carMax)   // carMinとcarMaxが表す車用信号機を赤に点灯
                };
            }
            else if (difMSec == MinMSec)
            {
                // differMSecとMinMSecが等しい場合
                return new List<TrafficPhase>
                {
                    new TrafficPhase(YellowMSec, 
                        new TrafficCommand(carMin, LightState.Yellow)),  // carMinが表す車用信号機を黄に点灯
                    new TrafficPhase(YellowMSec, 
                        new TrafficCommand(carMin, LightState.Red),
                        new TrafficCommand(carMax, LightState.Yellow)),  // carMinが表す車用信号機を赤、carMaxが表す車用信号機を黄に点灯
                    new TrafficPhase(redMSec,
                        new TrafficCommand(carMax, LightState.Red)),     // carMaxが表す車用信号機を赤に点灯
                };
            }

            return new List<TrafficPhase>
            {
                new TrafficPhase(YellowMSec, 
                    new TrafficCommand(carMin, LightState.Yellow)),  // carMinが表す車用信号機を黄に点灯
                new TrafficPhase(difMSec - YellowMSec,
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
        /// <param name="phaseNum"> 開始フェーズを表す番号 </param>
        private async void LoopTrafficPhase(int phaseNum)
        {
            int startPhase = phaseNum;  // 開始フェーズの番号を取得する

            while (!IsCancel)
            {
                for (int i = startPhase; i < PhaseList.Count; i++)
                {
                    InterruptPhase = i;  // 現在のフェーズの番号を取得する
                    
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

                    await Task.Delay(PhaseList[i].WaitMSec);  // WaitMSecミリ秒待機する
                    if (IsCancel) break;                      // 信号機アルゴリズムを停止する場合はループから脱出する                    
                }

                startPhase = 0;  // 開始フェーズの番号を0に設定する
            }
        }

        /// <summary>
        /// 車用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">      点灯状態を表す列挙型                   </param>
        /// <param name="carLabel">   車用信号機イメージ画像を表示するラベル </param>
        /// <param name="arrowLabel"> 矢印信号機イメージ画像を表示するラベル </param>
        private void ChangeSignalLightOn(LightState state, Label carLabel, Label arrowLabel)
        {
            if (state == LightState.NoLight) carLabel.Image = Resources.NoLightSignal;      // carLabelに無灯火イメージ画像を表示する
            if (state == LightState.Green)   carLabel.Image = Resources.GreenLightSignal;   // carLabelに緑点灯イメージ画像を表示する
            if (state == LightState.Yellow)  carLabel.Image = Resources.YellowLightSignal;  // carLabelに黄点灯イメージ画像を表示する
            if (state == LightState.Red)     carLabel.Image = Resources.RedLightSignal;     // carLabelに赤点灯イメージ画像を表示する

            // 矢印信号機の点灯
            if (arrowLabel != null)
            {
                if (state == LightState.Arrow)
                {
                    carLabel.Image   = Resources.RedLightSignal;  // carLabelに赤点灯イメージ画像を表示する
                    arrowLabel.Image = Resources.ArrowGreen;      // arrowLabelに矢印信号機点灯イメージ画像を表示する
                }
                else
                {
                    arrowLabel.Image = Resources.ArrowDefault;  // arrowLabelに矢印信号機無灯火イメージ画像を表示する
                }
            }
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">         点灯状態を表す列挙型                               </param>
        /// <param name="pedesLabelOne"> 歩行者用信号機イメージ画像を表示する１つ目のラベル </param>
        /// <param name="pedesLabelTwo"> 歩行者用信号機イメージ画像を表示する２つ目のラベル </param>
        private void ChangePedesLightOn(LightState state, Label pedesLabelOne, Label pedesLabelTwo)
        {
            Bitmap bmp = Resources.PedesNoLight;                        // 無灯火イメージ画像を取得する
            if (state == LightState.Green) bmp = Resources.PedesGreen;  // 緑点灯イメージ画像を取得する
            if (state == LightState.Red)   bmp = Resources.PedesRed;    // 赤点灯イメージ画像を取得する

            pedesLabelOne.Image = bmp;  // １つ目のラベルに点灯イメージ画像を表示する
            pedesLabelTwo.Image = bmp;  // ２つ目のラベルに点灯イメージ画像を表示する
        }
    }
}