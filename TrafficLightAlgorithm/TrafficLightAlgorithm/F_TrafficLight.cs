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
        /// 信号機アルゴリズムのフェーズ再生で中断時点のフェーズを表す番号
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
            try
            {
                IsTrafficEnable = false;              // 信号機アルゴリズムが動かない場合のブール値に設定する
                IsInterrupt     = false;              // 信号機アルゴリズムの中断を無効にする
                Cts = new CancellationTokenSource();  // Ctsの初期化

                // ラベルイメージの回転
                RotateLabelImage(RotateFlipType.Rotate90FlipNone,  lbl_PNGreTwo, lbl_PNRedTwo, lbl_PSGreTwo, lbl_PSRedTwo, lbl_NECorner);
                RotateLabelImage(RotateFlipType.Rotate180FlipNone, lbl_PEGreTwo, lbl_PERedTwo, lbl_PWGreTwo, lbl_PWRedTwo, lbl_WSLArrow, 
                                                                   lbl_WRArrow,  lbl_SECorner, lbl_EArrow);
                RotateLabelImage(RotateFlipType.Rotate270FlipNone, lbl_PNGreOne, lbl_PNRedOne, lbl_PSGreOne, lbl_PSRedOne, lbl_SWCorner);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nフォームのロードに失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// ラベルイメージを回転する
        /// </summary>
        /// <param name="rotate"> ラベルイメージの回転量と反転軸 </param>
        /// <param name="labels"> イメージを回転するラベル       </param>
        private void RotateLabelImage(RotateFlipType rotate, params Label[] labels)
        {
            try
            {
                foreach (Label label in labels)
                { 
                    label.Image.RotateFlip(rotate);  // ラベルイメージを回転する
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nラベルイメージの回転に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 「開始」ボタンクリック時イベント
        /// </summary>
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            try
            {
                string errMsg = CreateErrMsg();  // エラーメッセージを取得する

                // エラーメッセージ表示
                if (errMsg != "")
                {
                    MessageBox.Show(errMsg, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (IsTrafficEnable)
                {
                    string msgStr = "信号機プログラムを最初から実行しますか？";
                    if (MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;

                    Cts.Cancel();  // 信号機アルゴリズムが動いている場合はキャンセル要求を伝える                        
                }

                IsTrafficEnable = true;               // 信号機アルゴリズムが動く場合のブール値に設定する
                IsInterrupt     = false;              // 信号機アルゴリズムの中断を無効にする
                ChangeTextInterruptResumeBtn(false);  // 「中断/再開」ボタンのTextプロパティ値変更
                ChangeTextBoxEnabled(false);          // テキストボックスのEnabledプロパティ値変更

                // ミリ秒設定値構造体を取得する
                WaitMSec waitMSec = new WaitMSec(ConvertToInt(txt_NAvaiSec.Text), ConvertToInt(txt_SAvaiSec.Text), ConvertToInt(txt_EAvaiSec.Text),
                                                 ConvertToInt(txt_WAvaiSec.Text), ConvertToInt(txt_ArrowSec.Text), ConvertToInt(txt_AllRedSec.Text));

                PhaseList = CreateTrafficPhaseList(waitMSec);  // フェーズリスト作成
                LoopTrafficPhase(0, PhaseList);                // フェーズリストを最初のフェーズから再生する
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message + "\n信号機プログラムの開始に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsTrafficEnable = false;
            }
        }

        /// <summary>
        /// 「中断/再開」ボタンクリック時イベント
        /// </summary>
        private void Btn_InterruptResume_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsTrafficEnable) return;  // 信号機アルゴリズムが動いていない場合は終了する
                
                if (IsInterrupt)
                {
                    IsInterrupt = false;                          // 信号機アルゴリズムの中断を無効にする
                    LoopTrafficPhase(InterruptPhase, PhaseList);  // 中断したフェーズからフェーズリストを再生する
                }
                else
                {
                    IsInterrupt = true;  // 信号機アルゴリズムの中断を有効にする
                    Cts.Cancel();        // キャンセル要求を伝える
                }

                ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            }
            catch (Exception ex)
            {
                string interResume = "再開";
                if (IsInterrupt) interResume = "中断";
                MessageBox.Show(ex.Message + "\n信号機プログラムの" + interResume + "に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 「リセット」ボタンクリック時イベント
        /// </summary>
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsTrafficEnable)
                { 
                    if (MessageBox.Show("点灯状態をリセットしますか？", Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return; 
                }

                Cts.Cancel();  // キャンセル要求を伝える

                ChangeSignalLightOn(LightState.NoLight, lbl_NGreen,   lbl_NYellow,  lbl_NRed,     null);          // 北方向の車用信号機を無灯火にする
                ChangeSignalLightOn(LightState.NoLight, lbl_SGreen,   lbl_SYellow,  lbl_SRed,     null);          // 南方向の車用信号機を無灯火にする
                ChangeSignalLightOn(LightState.NoLight, lbl_EGreen,   lbl_EYellow,  lbl_ERed,     lbl_EArrow);    // 東方向の車用信号機を無灯火にする
                ChangeSignalLightOn(LightState.NoLight, lbl_WGreen,   lbl_WYellow,  lbl_WRed,     lbl_WArrow);    // 西方向の車用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  lbl_PNGreOne, lbl_PNGreTwo, lbl_PNRedOne, lbl_PNRedTwo);  // 北方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  lbl_PSGreOne, lbl_PSGreTwo, lbl_PSRedOne, lbl_PSRedTwo);  // 南方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  lbl_PEGreOne, lbl_PEGreTwo, lbl_PERedOne, lbl_PERedTwo);  // 東方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  lbl_PWGreOne, lbl_PWGreTwo, lbl_PWRedOne, lbl_PWRedTwo);  // 西方向の歩行者用信号機を無灯火にする

                ChangeTextInterruptResumeBtn(false);  // 「中断/再開」ボタンのTextプロパティ値変更
                ChangeTextBoxEnabled(true);           // テキストボックスのEnabledプロパティ値変更
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n点灯状態のリセットに失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            IsTrafficEnable = false;  // 信号機アルゴリズムが動かない場合のブール値に設定する
            IsInterrupt     = false;  // 信号機アルゴリズムの中断を無効にする
        }

        /// <summary>
        /// 「Version」ボタンクリック時イベント
        /// </summary>
        private void Btn_VerInfo_Click(object sender, EventArgs e)
        {
            try
            {
                F_Version f_Version = new F_Version();  // バージョン情報フォームの初期化
                f_Version.ShowDialog();                 // バージョン情報フォーム画面表示
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nバージョン情報の表示に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        /// <summary>
        /// エラーメッセージを作成する
        /// </summary>
        /// <returns> 作成したエラーメッセージ </returns>
        private string CreateErrMsg()
        {
            try
            {
                string errMsg = "";  // エラーメッセージが入る

                // テキストボックスのTextプロパティ値がチェックを満たさない場合はエラーメッセージを追加する
                if (!CheckSecText(txt_NAvaiSec.Text,  AvaiSecMin,   AvaiSecMax))   errMsg += $"「北方向への進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";
                if (!CheckSecText(txt_SAvaiSec.Text,  AvaiSecMin,   AvaiSecMax))   errMsg += $"「南方向への進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";
                if (!CheckSecText(txt_EAvaiSec.Text,  AvaiSecMin,   AvaiSecMax))   errMsg += $"「東方向への進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";
                if (!CheckSecText(txt_WAvaiSec.Text,  AvaiSecMin,   AvaiSecMax))   errMsg += $"「西方向への進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";
                if (!CheckSecText(txt_ArrowSec.Text,  ArrowSecMin,  ArrowSecMax))  errMsg += $"「矢印信号機の点灯時間」には{ArrowSecMin}から{ArrowSecMax}の整数を入力してください。\n";
                if (!CheckSecText(txt_AllRedSec.Text, AllRedSecMin, AllRedSecMax)) errMsg += $"「全信号機の赤点灯時間」には{AllRedSecMin}から{AllRedSecMax}の整数を入力してください。\n";

                return errMsg;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message + "\nエラーメッセージ作成に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
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
            try
            {
                if (!double.TryParse(checkText, out double douvalue))       return false;  // チェック対象文字列がdouble型に変換できない場合は終了する
                if (!int.TryParse(douvalue.ToString(), out int checkValue)) return false;  // double型から変換した文字列がint型に変換できない場合は終了する
                if (checkValue < minValue || checkValue > maxValue)         return false;  // int型に変換した値がminValueより小さい、もしくはmaxValueより大きい場合は終了する
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n文字列のチェックに失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// 文字列を整数に変換
        /// </summary>
        /// <param name="str"> 変換元の文字列 </param>
        /// <returns> 変換後の整数 </returns>
        private int ConvertToInt(string str)
        {
            try
            {
                double.TryParse(str, out double douValue);          // 変換元の文字列をdouble型に変換
                int.TryParse(douValue.ToString(), out int result);  // double型から変換した文字列をint型に変換
                return result;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 「中断/再開」ボタンのtextプロパティ値変更
        /// </summary>
        /// <param name="isInterrupt"> 信号機アルゴリズムの中断が有効の場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTextInterruptResumeBtn(bool isInterrupt)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n中断/再開ボタンテキストの変更に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// テキストボックスのEnabledプロパティ値を変更する
        /// </summary>
        /// <param name="enable"> Enabledプロパティを有効にする場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTextBoxEnabled(bool enable)
        {
            try
            {
                txt_NAvaiSec.Enabled  = enable;  // 北方向への進行可能秒数
                txt_SAvaiSec.Enabled  = enable;  // 南方向への進行可能秒数
                txt_EAvaiSec.Enabled  = enable;  // 東方向への進行可能秒数
                txt_WAvaiSec.Enabled  = enable;  // 西方向への進行可能秒数
                txt_ArrowSec.Enabled  = enable;  // 矢印信号機の点灯秒数
                txt_AllRedSec.Enabled = enable;  // 全信号機の赤点灯秒数
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nテキストボックスEnabledプロパティ値の変更に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストを作成する
        /// </summary>
        /// <param name="setMSec"> 信号機のミリ秒設定値構造体 </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> CreateTrafficPhaseList(WaitMSec setMSec)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nフェーズリストの作成に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>
        /// 複数の車用・歩行者用信号機を同じ時間・同じ色に点灯するフェーズを作成
        /// </summary>
        /// <param name="mSec">    点灯間隔ミリ秒 </param>
        /// <param name="state">   点灯状態       </param>
        /// <param name="signals"> 点灯する信号機 </param>
        /// <returns> 作成したフェーズ </returns>
        private TrafficPhase CreatePhase(int mSec, LightState state, params Signal[] signals)
        {
            try
            {
                TrafficCommand[] commands = new TrafficCommand[signals.Length];
            
                for (int i = 0; i < commands.Length; i++)
                {
                    commands[i] = new TrafficCommand(signals[i], state);  // 信号機と点灯状態を表す列挙型が入る
                }

                return new TrafficPhase(mSec, commands);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nフェーズの作成に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>
        /// 歩行者用信号機の点滅フェーズリスト作成
        /// </summary>
        /// <param name="pSigOne"> １つ目の歩行者用信号機を表す列挙型 </param>
        /// <param name="pSigTwo"> ２つ目の歩行者用信号機を表す列挙型 </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> PedesBlink(Signal pSigOne, Signal pSigTwo)
        {
            try
            {
                List<TrafficPhase> phaseList = new List<TrafficPhase>();  // 歩行者用信号機点滅フェーズリスト

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n点滅フェーズリストの作成に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
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
            try
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
                else if (Math.Abs(mSecDif) == YellowMSec)
                {
                    // sigMinとsigMaxが表す車用信号機の緑点灯ミリ秒の差がYellowMSecと一致する場合
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n黄・赤点灯フェーズリストの作成に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストをループさせる
        /// </summary>
        /// <param name="phaseNum"> 開始フェーズの番号                 </param>
        /// <param name="phases">   信号機アルゴリズムのフェーズリスト </param>
        private async void LoopTrafficPhase(int phaseNum, List<TrafficPhase> phases)
        {
            try
            {
                int  startPhase = phaseNum;           // 開始フェーズの番号を取得する
                bool lightonSuc = false;              // 信号機点灯状態の更新に成功した場合はtrue、それ以外の場合はfalse
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
                                lightonSuc = ChangeSignalLightOn(command.State, lbl_NGreen, lbl_NYellow, lbl_NRed, null);  // 北方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.CarSouth)
                            {
                                lightonSuc = ChangeSignalLightOn(command.State, lbl_SGreen, lbl_SYellow, lbl_SRed, null);  // 南方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.CarEast)
                            {
                                lightonSuc = ChangeSignalLightOn(command.State, lbl_EGreen, lbl_EYellow, lbl_ERed, lbl_EArrow);  // 東方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.CarWest)
                            {
                                lightonSuc = ChangeSignalLightOn(command.State, lbl_WGreen, lbl_WYellow, lbl_WRed, lbl_WArrow);  // 西方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesNorth)
                            {
                                lightonSuc = ChangePedesLightOn(command.State, lbl_PNGreOne, lbl_PNGreTwo, lbl_PNRedOne, lbl_PNRedTwo);  // 北方向の歩行者用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesSouth)
                            {
                                lightonSuc = ChangePedesLightOn(command.State, lbl_PSGreOne, lbl_PSGreTwo, lbl_PSRedOne, lbl_PSRedTwo);  // 南方向の歩行者用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesEast)
                            {
                                lightonSuc = ChangePedesLightOn(command.State, lbl_PEGreOne, lbl_PEGreTwo, lbl_PERedOne, lbl_PERedTwo);  // 東方向の歩行者用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesWest)
                            {
                                lightonSuc = ChangePedesLightOn(command.State, lbl_PWGreOne, lbl_PWGreTwo, lbl_PWRedOne, lbl_PWRedTwo);  // 西方向の歩行者用信号機の点灯状態更新
                            }

                            // 信号機点灯状態の更新に失敗した場合
                            if (!lightonSuc)
                            {
                                MessageBox.Show("信号機点灯状態の更新に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                                IsTrafficEnable = false;     // 信号機アルゴリズムが動かない場合のブール値に設定する
                                ChangeTextBoxEnabled(true);  // テキストボックスのEnabledプロパティ値変更
                                return;
                            }
                        }

                        try
                        {
                            await Task.Delay(phases[i].WaitMSec, Cts.Token);  // キャンセルが要求されていない場合、WaitMSecミリ秒待機する
                        }
                        catch (Exception)
                        {
                            return;  // キャンセル要求で例外が発生した場合は終了する
                        }
                    }
                
                    startPhase = 0;  // 開始フェーズの番号を0に設定する
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nフェーズリストの再生に失敗しました。", Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsTrafficEnable = false;
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
        /// <returns> 点灯状態更新に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool ChangeSignalLightOn(LightState state, Label lbl_green, Label lbl_yellow, Label lbl_red, Label lbl_arrow)
        {
            try
            {
                bool greVisible = false;  // lbl_green のVisibleプロパティ値
                bool yelVisible = false;  // lbl_yellowのVisibleプロパティ値
                bool redVisible = false;  // lbl_red   のVisibleプロパティ値
                bool arwVisible = false;  // lbl_arrow のVisibleプロパティ値

                if (state == LightState.Green)  greVisible = true;
                if (state == LightState.Yellow) yelVisible = true;
                if (state == LightState.Red || state == LightState.Arrow) redVisible = true;
                if (state == LightState.Arrow)  arwVisible = true;

                lbl_green.Visible  = greVisible;                        // lbl_green のVisibleプロパティ値の設定
                lbl_yellow.Visible = yelVisible;                        // lbl_yellowのVisibleプロパティ値の設定
                lbl_red.Visible    = redVisible;                        // lbl_red   のVisibleプロパティ値の設定
                if (lbl_arrow != null) lbl_arrow.Visible = arwVisible;  // lbl_arrow のVisibleプロパティ値の設定

                return true;   // 点灯状態更新成功
            }
            catch
            {
                return false;  // 点灯状態更新失敗
            }
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">        点灯状態を表す列挙型                         </param>
        /// <param name="lbl_greenOne"> 歩行者用信号機の緑ランプを表す１つ目のラベル </param>
        /// <param name="lbl_greenTwo"> 歩行者用信号機の緑ランプを表す２つ目のラベル </param>
        /// <param name="lbl_redOne">   歩行者用信号機の赤ランプを表す１つ目のラベル </param>
        /// <param name="lbl_redTwo">   歩行者用信号機の赤ランプを表す２つ目のラベル </param>
        /// <returns> 点灯状態更新に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool ChangePedesLightOn(LightState state, Label lbl_greenOne, Label lbl_greenTwo, Label lbl_redOne, Label lbl_redTwo)
        {
            try
            {
                bool greVisible = false;  // lbl_greenOneとlbl_greenTwoのVisibleプロパティ値
                bool redVisible = false;  // lbl_redOne  とlbl_redTwo  のVisibleプロパティ値

                if (state == LightState.Green) greVisible = true;
                if (state == LightState.Red)   redVisible = true;

                lbl_greenOne.Visible = greVisible;  // lbl_greenOneのVisibleプロパティ値の設定
                lbl_greenTwo.Visible = greVisible;  // lbl_greenTwoのVisibleプロパティ値の設定
                lbl_redOne.Visible   = redVisible;  // lbl_redOne  のVisibleプロパティ値の設定
                lbl_redTwo.Visible   = redVisible;  // lbl_redTwo  のVisibleプロパティ値の設定

                return true;   // 点灯状態更新成功
            }
            catch
            {
                return false;  // 点灯状態更新失敗
            }
        }
    }
}