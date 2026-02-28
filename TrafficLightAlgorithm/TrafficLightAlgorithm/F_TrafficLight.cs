using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TrafficLightAlgorithm
{
    public partial class F_TrafficLight : Form
    {
        /// <summary>
        /// 進行可能秒数の最大値
        /// </summary>
        private const int AvaiSecMax      = 20;

        /// <summary>
        /// 進行可能秒数の最小値
        /// </summary>
        private const int AvaiSecMin      = 5;

        /// <summary>
        /// 矢印信号機の点灯秒数の最大値
        /// </summary>
        private const int ArrowSecMax     = 5;

        /// <summary>
        /// 矢印信号機の点灯秒数の最小値
        /// </summary>
        private const int ArrowSecMin     = 1;

        /// <summary>
        /// 全信号機の赤点灯秒数の最大値
        /// </summary>
        private const int AllRedSecMax    = 5;

        /// <summary>
        /// 全信号機の赤点灯秒数の最小値
        /// </summary>
        private const int AllRedSecMin    = 1;

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
        /// キャンセル要求
        /// </summary>
        private CancellationTokenSource Cts;

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
            IsTrafficEnable = false;  // 信号機アルゴリズムが動かない場合のブール値に設定する
            IsInterrupt     = false;  // 信号機アルゴリズムの中断を無効にする

            // ラベルイメージの回転
            ChangeLabelImageRotate(RotateFlipType.Rotate90FlipNone,  lbl_PNGreTwo, lbl_PNRedTwo, lbl_PSGreTwo, lbl_PSRedTwo, lbl_NECorner);
            ChangeLabelImageRotate(RotateFlipType.Rotate180FlipNone, lbl_PEGreTwo, lbl_PERedTwo, lbl_PWGreTwo, lbl_PWRedTwo, lbl_WSLArrow, 
                                                                     lbl_WRArrow,  lbl_SECorner, lbl_EArrow);
            ChangeLabelImageRotate(RotateFlipType.Rotate270FlipNone, lbl_PNGreOne, lbl_PNRedOne, lbl_PSGreOne, lbl_PSRedOne, lbl_SWCorner);
        }

        /// <summary>
        /// ラベルイメージを回転する
        /// </summary>
        /// <param name="rotate"> ラベルイメージの回転量と反転軸 </param>
        /// <param name="labels"> イメージを回転するラベル       </param>
        private void ChangeLabelImageRotate(RotateFlipType rotate, params Label[] labels)
        {
            foreach (Label label in labels)
            { 
                label.Image.RotateFlip(rotate);  // ラベルイメージを回転する
            }
        }

        /// <summary>
        /// 「開始」ボタンクリック時イベント
        /// </summary>
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            string errMsg = CreateErrMsg();  // エラーメッセージを取得する

            // エラーメッセージを表示する
            if (errMsg != "" && MessageBox.Show(errMsg, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK) return;  
            
            if (IsTrafficEnable) Cts?.Cancel();  // 信号機アルゴリズムが動いている場合はキャンセル要求を出す
            
            IsTrafficEnable = true;   // 信号機アルゴリズムが動く場合のブール値に設定する
            IsInterrupt     = false;  // 信号機アルゴリズムの中断を無効にする

            int.TryParse(txt_NAvaiSec.Text,  out int nSec);  // 交差点北方向への進行可能秒数を表す文字列をint型に変換する
            int.TryParse(txt_SAvaiSec.Text,  out int sSec);  // 交差点南方向への進行可能秒数を表す文字列をint型に変換する
            int.TryParse(txt_EAvaiSec.Text,  out int eSec);  // 交差点東方向への進行可能秒数を表す文字列をint型に変換する
            int.TryParse(txt_WAvaiSec.Text,  out int wSec);  // 交差点西方向への進行可能秒数を表す文字列をint型に変換する
            int.TryParse(txt_ArrowSec.Text,  out int aSec);  // 矢印信号機の点灯秒数を表す文字列をint型に変換する
            int.TryParse(txt_AllRedSec.Text, out int rSec);  // 全信号機の赤点灯秒数を表す文字列をint型に変換する

            WaitMSec waitMSec = new WaitMSec(nSec, sSec, eSec, wSec, aSec, rSec);  // 信号機アルゴリズムの設定値構造体を取得する

            PhaseList = CreateTrafficPhaseList(waitMSec);  // 信号機アルゴリズムのフェーズリストを作成
            ChangeTextInterruptResumeBtn(false);           // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnabled(false);                   // テキストボックスのEnabledプロパティ値変更
            LoopTrafficPhase(0, PhaseList);                // フェーズリストを最初のフェーズから再生する
        }

        /// <summary>
        /// 「中断/再開」ボタンクリック時イベント
        /// </summary>
        private void Btn_InterruptResume_Click(object sender, EventArgs e)
        {
            if (!IsTrafficEnable) return;  // 信号機アルゴリズムが動いていない場合は終了する

            if (IsInterrupt)
            {
                IsInterrupt = false;                          // 信号機アルゴリズムの中断を無効にする
                ChangeTextInterruptResumeBtn(false);          // 「中断/再開」ボタンのTextプロパティ値変更
                LoopTrafficPhase(InterruptPhase, PhaseList);  // 中断フェーズからフェーズリストを再生する
            }
            else
            {
                IsInterrupt = true;                  // 信号機アルゴリズムの中断を有効にする
                ChangeTextInterruptResumeBtn(true);  // 「中断/再開」ボタンのTextプロパティ値変更
                Cts.Cancel();                        // キャンセル要求
            }
        }

        /// <summary>
        /// 「リセット」ボタンクリック時イベント
        /// </summary>
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            IsTrafficEnable = false;              // 信号機アルゴリズムが動かない場合のブール値に設定する
            ChangeTextInterruptResumeBtn(false);  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnabled(true);           // テキストボックスのEnabledプロパティ値変更
            Cts?.Cancel();                        // キャンセル要求

            ChangeSignalLightOn(LightState.NoLight, lbl_NGreen,   lbl_NYellow,  lbl_NRed,     null);          // 北方向の車用信号機を無灯火にする
            ChangeSignalLightOn(LightState.NoLight, lbl_SGreen,   lbl_SYellow,  lbl_SRed,     null);          // 南方向の車用信号機を無灯火にする
            ChangeSignalLightOn(LightState.NoLight, lbl_EGreen,   lbl_EYellow,  lbl_ERed,     lbl_EArrow);    // 東方向の車用信号機を無灯火にする
            ChangeSignalLightOn(LightState.NoLight, lbl_WGreen,   lbl_WYellow,  lbl_WRed,     lbl_WArrow);    // 西方向の車用信号機を無灯火にする
            ChangePedesLightOn(LightState.NoLight,  lbl_PNGreOne, lbl_PNGreTwo, lbl_PNRedOne, lbl_PNRedTwo);  // 北方向の歩行者用信号機を無灯火にする
            ChangePedesLightOn(LightState.NoLight,  lbl_PSGreOne, lbl_PSGreTwo, lbl_PSRedOne, lbl_PSRedTwo);  // 南方向の歩行者用信号機を無灯火にする
            ChangePedesLightOn(LightState.NoLight,  lbl_PEGreOne, lbl_PEGreTwo, lbl_PERedOne, lbl_PERedTwo);  // 東方向の歩行者用信号機を無灯火にする
            ChangePedesLightOn(LightState.NoLight,  lbl_PWGreOne, lbl_PWGreTwo, lbl_PWRedOne, lbl_PWRedTwo);  // 西方向の歩行者用信号機を無灯火にする
        }

        /// <summary>
        /// 「バージョン情報」ボタンクリック時イベント
        /// </summary>
        private void Btn_VersionShow_Click(object sender, EventArgs e)
        {
            F_Version f_Version = new F_Version();

            f_Version.Show();  // バージョン情報フォームを表示する
        }

        /// <summary>
        /// エラーメッセージを作成する
        /// </summary>
        /// <returns> エラーメッセージを表す文字列 </returns>
        private string CreateErrMsg()
        {
            string errMsg = "";  // エラーメッセージが入る

            // テキストボックスのTextプロパティ値をチェックし、チェックを満たさない場合はエラーメッセージを追加する
            if (!CheckSecText(txt_NAvaiSec.Text,  AvaiSecMin,   AvaiSecMax))   errMsg += $"「北方向への進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_SAvaiSec.Text,  AvaiSecMin,   AvaiSecMax))   errMsg += $"「南方向への進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_EAvaiSec.Text,  AvaiSecMin,   AvaiSecMax))   errMsg += $"「東方向への進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_WAvaiSec.Text,  AvaiSecMin,   AvaiSecMax))   errMsg += $"「西方向への進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_ArrowSec.Text,  ArrowSecMin,  ArrowSecMax))  errMsg += $"「矢印信号機の点灯時間」には{ArrowSecMin}から{ArrowSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_AllRedSec.Text, AllRedSecMin, AllRedSecMax)) errMsg += $"「全信号機の赤点灯時間」には{AllRedSecMin}から{AllRedSecMax}の整数を入力してください。\n";

            return errMsg;
        }

        /// <summary>
        /// 文字列が最大値と最小値の範囲内の整数を表す値かチェックを行う
        /// </summary>
        /// <param name="checkText"> チェック対象の文字列         </param>
        /// <param name="minValue">  チェックを満たす整数の最小値 </param>        
        /// <param name="maxValue">  チェックを満たす整数の最大値 </param>
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
        /// <param name="isInterrupt"> 信号機アルゴリズムの中断が有効の場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTextInterruptResumeBtn(bool isInterrupt)
        {
            if (isInterrupt) 
            { 
                btn_InterruptResume.Text = "再開";  // 信号機アルゴリズムの中断が有効の場合は「再開」に設定する
            }
            else
            {
                btn_InterruptResume.Text = "中断";  // 信号機アルゴリズムの中断が無効の場合は「中断」に設定する
            }
        }

        /// <summary>
        /// テキストボックスのEnabledプロパティ値を変更する
        /// </summary>
        /// <param name="enable"> Enabledプロパティを有効にする場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTextBoxEnabled(bool enable)
        {
            txt_NAvaiSec.Enabled  = enable;  // 北方向への進行可能秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_SAvaiSec.Enabled  = enable;  // 南方向への進行可能秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_EAvaiSec.Enabled  = enable;  // 東方向への進行可能秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_WAvaiSec.Enabled  = enable;  // 西方向への進行可能秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_ArrowSec.Enabled  = enable;  // 矢印信号機の点灯秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_AllRedSec.Enabled = enable;  // 全信号機の赤点灯秒数を入力するテキストボックスのEnabledプロパティ値変更
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストを作成する
        /// </summary>
        /// <param name="setMSec"> 信号機の設定値構造体 </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> CreateTrafficPhaseList(WaitMSec setMSec)
        {
            int carNSpdEWMSec = Math.Min(setMSec.NMSec, setMSec.SMSec) - BlinkMSec * BlinkPhaseCount - MinMSec;  // 北南の車用・東西の歩行者用信号機が同時に緑に点灯するミリ秒
            int carEWpdNSMSec = Math.Min(setMSec.EMSec, setMSec.WMSec) - BlinkMSec * BlinkPhaseCount - MinMSec;  // 東西の車用・北南の歩行者用信号機が同時に緑に点灯するミリ秒

            List<TrafficPhase> phaseList = new List<TrafficPhase>
            {
                CreatePhase(setMSec.RMSec, LightState.Red, 
                    Signal.CarNorth,   Signal.CarSouth,   Signal.CarEast,   Signal.CarWest, 
                    Signal.PedesNorth, Signal.PedesSouth, Signal.PedesEast, Signal.PedesWest),  // 全ての車用・歩行者用信号機の赤点灯

                CreatePhase(carNSpdEWMSec, LightState.Green, 
                    Signal.CarNorth,   Signal.CarSouth,   Signal.PedesEast, Signal.PedesWest)   // 北南の車用・東西の歩行者用信号機の緑点灯   
            };

            phaseList.AddRange(PedesBlink(Signal.PedesEast, Signal.PedesWest));                       // 東西の歩行者用信号機の点滅
            phaseList.Add(CreatePhase(MinMSec, LightState.Red, Signal.PedesEast, Signal.PedesWest));  // 東西の歩行者用信号機の赤点灯
            
            phaseList.AddRange(YelRedPhaseList(setMSec.RMSec, setMSec.NMSec - setMSec.SMSec, Signal.CarSouth, Signal.CarNorth));  // 北南の車用信号機の黄・赤点灯
            
            phaseList.Add(CreatePhase(carEWpdNSMSec, LightState.Green, 
                Signal.CarEast, Signal.CarWest, Signal.PedesNorth, Signal.PedesSouth));  // 東西の車用・北南の歩行者用信号機の緑点灯
            
            phaseList.AddRange(PedesBlink(Signal.PedesNorth, Signal.PedesSouth));                       // 北南の歩行者用信号機の点滅
            phaseList.Add(CreatePhase(MinMSec, LightState.Red, Signal.PedesNorth, Signal.PedesSouth));  // 北南の歩行者用信号機の赤点灯
            
            phaseList.AddRange(YelRedPhaseList(MinMSec, setMSec.EMSec - setMSec.WMSec, Signal.CarWest, Signal.CarEast));  // 東西の車用信号機の黄・赤点灯
            phaseList.Add(CreatePhase(setMSec.AMSec, LightState.Arrow,  Signal.CarEast, Signal.CarWest));                 // 東西の矢印信号機の点灯
            phaseList.Add(CreatePhase(YellowMSec,    LightState.Yellow, Signal.CarEast, Signal.CarWest));                 // 東西の車用信号機の黄点灯 

            return phaseList;
        }

        /// <summary>
        /// 複数の車用・歩行者用信号機を同じ時間・同じ色に点灯するフェーズを作成
        /// </summary>
        /// <param name="mSec">     点灯時間ミリ秒             </param>
        /// <param name="state">    点灯状態                   </param>
        /// <param name="traffics"> 点灯する信号機を表す列挙型 </param>
        /// <returns> 作成したフェーズ </returns>
        private TrafficPhase CreatePhase(int mSec, LightState state, params Signal[] traffics)
        {
            TrafficCommand[] commands = new TrafficCommand[traffics.Length];
            
            for (int i = 0; i < commands.Length; i++)
            {
                commands[i] = new TrafficCommand(traffics[i], state);  // 信号機の種類と点灯状態が入る
            }

            return new TrafficPhase(mSec, commands);
        }

        /// <summary>
        /// 歩行者用信号機の点滅フェーズリスト作成
        /// </summary>
        /// <param name="pSigOne"> １つ目の歩行者用信号機を表す列挙型 </param>
        /// <param name="pSigTwo"> ２つ目の歩行者用信号機を表す列挙型 </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> PedesBlink(Signal pSigOne, Signal pSigTwo)
        {
            List<TrafficPhase> phaseList = new List<TrafficPhase>();  // 歩行者用信号機点滅フェーズのリスト

            for (int i = 0; i < BlinkPhaseCount; i++)
            {
                if (i % 2 == 0)
                {
                    phaseList.Add(CreatePhase(BlinkMSec, LightState.NoLight, pSigOne, pSigTwo));  // pSigOneとpSigTwoが表す歩行者用信号機を無灯火にするフェーズを追加
                }
                else
                {
                    phaseList.Add(CreatePhase(BlinkMSec, LightState.Green,   pSigOne, pSigTwo));  // pSigOneとpSigTwoが表す歩行者用信号機を緑に点灯するフェーズを追加
                }
            }

            return phaseList;
        }

        /// <summary>
        /// 車用信号機の黄・赤点灯フェーズリストを作成する
        /// </summary>
        /// <param name="redMSec"> sigTwoが表す車用信号機の赤点灯ミリ秒             </param>
        /// <param name="mSecDif"> sigOneとsigTwoが表す車用信号機の緑点灯ミリ秒の差 </param>
        /// <param name="sigOne">  １つ目の車用信号機を表す列挙型                   </param>
        /// <param name="sigTwo">  ２つ目の車用信号機を表す列挙型                   </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> YelRedPhaseList(int redMSec, int mSecDif, Signal sigOne, Signal sigTwo)
        {
            List<TrafficPhase> pList = new List<TrafficPhase>();  // sigOneとsigTwoが表す車用信号機の黄・赤点灯フェーズが入るフェーズリスト

            Signal sigMin = sigOne;  // 進行可能時間が短い方の車用信号機を表す列挙型が入る
            Signal sigMax = sigTwo;  // 進行可能時間が長い方の車用信号機を表す列挙型が入る

            if (mSecDif < 0)
            {
                sigMin = sigTwo;
                sigMax = sigOne;
            }

            if (mSecDif == 0)
            {
                // sigMinとsigMaxが表す車用信号機の緑点灯ミリ秒が一致する場合
                pList.Add(CreatePhase(YellowMSec, LightState.Yellow, sigMin, sigMax));  // sigMinとsigMaxが表す車用信号機を黄に点灯
                pList.Add(CreatePhase(redMSec,    LightState.Red,    sigMin, sigMax));  // sigMinとsigMaxが表す車用信号機を赤に点灯
            }
            else if (Math.Abs(mSecDif) == MinMSec)
            {
                // sigMinとsigMaxの緑点灯ミリ秒の差がMinMSecと一致する場合
                pList.Add(new TrafficPhase(YellowMSec, new TrafficCommand(sigMin, LightState.Yellow)));  // sigMinが表す車用信号機を黄に点灯
                pList.Add(new TrafficPhase(YellowMSec, new TrafficCommand(sigMin, LightState.Red),
                                                       new TrafficCommand(sigMax, LightState.Yellow)));  // sigMinが表す車用信号機を赤、sigMaxが表す車用信号機を黄に点灯
                pList.Add(new TrafficPhase(redMSec,    new TrafficCommand(sigMax, LightState.Red)));     // sigMaxが表す車用信号機を赤に点灯
            }
            else
            {
                pList.Add(new TrafficPhase(YellowMSec,                     new TrafficCommand(sigMin, LightState.Yellow)));  // sigMinが表す車用信号機を黄に点灯
                pList.Add(new TrafficPhase(Math.Abs(mSecDif) - YellowMSec, new TrafficCommand(sigMin, LightState.Red)));     // sigMinが表す車用信号機を赤に点灯
                pList.Add(new TrafficPhase(YellowMSec,                     new TrafficCommand(sigMax, LightState.Yellow)));  // sigMaxが表す車用信号機を黄に点灯
                pList.Add(new TrafficPhase(redMSec,                        new TrafficCommand(sigMax, LightState.Red)));     // sigMaxが表す車用信号機を赤に点灯
            }

            return pList;
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストをループさせる
        /// </summary>
        /// <param name="phaseNum"> 開始フェーズの番号を表す整数       </param>
        /// <param name="phases">   信号機アルゴリズムのフェーズリスト </param>
        private async void LoopTrafficPhase(int phaseNum, List<TrafficPhase> phases)
        {
            int startPhase = phaseNum;            // 開始フェーズの番号を取得する
            Cts = new CancellationTokenSource();  // Ctsの初期化

            while (!Cts.IsCancellationRequested)
            {
                for (int i = startPhase; i < phases.Count; i++)
                {
                    InterruptPhase = i;  // 現在のフェーズの番号を取得する

                    foreach (TrafficCommand command in phases[i].Commands)
                    {
                        if (command.Signal == Signal.CarNorth)
                        {
                            ChangeSignalLightOn(command.State, lbl_NGreen, lbl_NYellow, lbl_NRed, null);  // 北方向の車用信号機の点灯状態更新
                        }
                        else if (command.Signal == Signal.CarSouth)
                        {
                            ChangeSignalLightOn(command.State, lbl_SGreen, lbl_SYellow, lbl_SRed, null);  // 南方向の車用信号機の点灯状態更新
                        }
                        else if (command.Signal == Signal.CarEast)
                        {
                            ChangeSignalLightOn(command.State, lbl_EGreen, lbl_EYellow, lbl_ERed, lbl_EArrow);  // 東方向の車用信号機の点灯状態更新
                        }
                        else if (command.Signal == Signal.CarWest)
                        {
                            ChangeSignalLightOn(command.State, lbl_WGreen, lbl_WYellow, lbl_WRed, lbl_WArrow);  // 西方向の車用信号機の点灯状態更新
                        }
                        else if (command.Signal == Signal.PedesNorth)
                        {
                            ChangePedesLightOn(command.State, lbl_PNGreOne, lbl_PNGreTwo, lbl_PNRedOne, lbl_PNRedTwo);  // 北方向の歩行者用信号機の点灯状態更新
                        }
                        else if (command.Signal == Signal.PedesSouth)
                        {
                            ChangePedesLightOn(command.State, lbl_PSGreOne, lbl_PSGreTwo, lbl_PSRedOne, lbl_PSRedTwo);  // 南方向の歩行者用信号機の点灯状態更新
                        }
                        else if (command.Signal == Signal.PedesEast)
                        {
                            ChangePedesLightOn(command.State, lbl_PEGreOne, lbl_PEGreTwo, lbl_PERedOne, lbl_PERedTwo);  // 東方向の歩行者用信号機の点灯状態更新
                        }
                        else if (command.Signal == Signal.PedesWest)
                        {
                            ChangePedesLightOn(command.State, lbl_PWGreOne, lbl_PWGreTwo, lbl_PWRedOne, lbl_PWRedTwo);  // 西方向の歩行者用信号機の点灯状態更新
                        }
                    }

                    try
                    {
                        await Task.Delay(phases[i].WaitMSec, Cts.Token);  // キャンセル要求がされていない場合、WaitMSecミリ秒待機する
                    }
                    catch (TaskCanceledException)
                    { 
                        return;  // タスクが取り消された場合終了する
                    }
                }
                
                startPhase = 0;  // 開始フェーズの番号を0に設定する
            }
        }

        /// <summary>
        /// 車用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">      点灯状態を表す列挙型               </param>
        /// <param name="lbl_green">  車用信号機の緑ランプを表すラベル   </param>
        /// <param name="lbl_yellow"> 車用信号機の黄ランプを表すラベル   </param>
        /// <param name="lbl_red">    車用信号機の赤ランプを表すラベル   </param>
        /// <param name="lbl_arrow">  矢印信号機の矢印ランプを表すラベル </param>
        private void ChangeSignalLightOn(LightState state, Label lbl_green, Label lbl_yellow, Label lbl_red, Label lbl_arrow)
        {
            bool greVisible = false;  // lbl_green のVisibleプロパティ値が入る
            bool yelVisible = false;  // lbl_yellowのVisibleプロパティ値が入る
            bool redVisible = false;  // lbl_red   のVisibleプロパティ値が入る
            bool arwVisible = false;  // lbl_arrow のVisibleプロパティ値が入る

            if (state == LightState.Green)  greVisible = true;
            if (state == LightState.Yellow) yelVisible = true;
            if (state == LightState.Red || state == LightState.Arrow) redVisible = true;
            if (state == LightState.Arrow)  arwVisible = true;

            lbl_green.Visible  = greVisible;                        // lbl_green の表示/非表示の設定
            lbl_yellow.Visible = yelVisible;                        // lbl_yellowの表示/非表示の設定
            lbl_red.Visible    = redVisible;                        // lbl_red   の表示/非表示の設定
            if (lbl_arrow != null) lbl_arrow.Visible = arwVisible;  // lbl_arrow の表示/非表示の設定
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">        点灯状態を表す列挙型                         </param>
        /// <param name="lbl_greenOne"> 歩行者用信号機の緑ランプを表す１つ目のラベル </param>
        /// <param name="lbl_greenTwo"> 歩行者用信号機の緑ランプを表す２つ目のラベル </param>
        /// <param name="lbl_redOne">   歩行者用信号機の赤ランプを表す１つ目のラベル </param>
        /// <param name="lbl_redTwo">   歩行者用信号機の赤ランプを表す２つ目のラベル </param>
        private void ChangePedesLightOn(LightState state, Label lbl_greenOne, Label lbl_greenTwo, Label lbl_redOne, Label lbl_redTwo)
        {
            bool greVisible = false;  // lbl_greenOneとlbl_greenTwoのVisibleプロパティ値が入る
            bool redVisible = false;  // lbl_redOne  とlbl_redTwo  のVisibleプロパティ値が入る

            if (state == LightState.Green) greVisible = true;
            if (state == LightState.Red)   redVisible = true;

            lbl_greenOne.Visible = greVisible;  // lbl_greenOneの表示/非表示の設定
            lbl_greenTwo.Visible = greVisible;  // lbl_greenTwoの表示/非表示の設定
            lbl_redOne.Visible   = redVisible;  // lbl_redOne  の表示/非表示の設定
            lbl_redTwo.Visible   = redVisible;  // lbl_redTwo  の表示/非表示の設定
        }
    }
}