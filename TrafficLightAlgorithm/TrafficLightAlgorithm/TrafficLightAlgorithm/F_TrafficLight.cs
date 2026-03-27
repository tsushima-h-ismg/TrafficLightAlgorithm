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
        /// 信号機アルゴリズムのフェーズ再生で中断したフェーズを表す番号
        /// </summary>
        private int InterruptPhase;

        /// <summary>
        /// 点灯状態変更履歴のリストから参照する要素の番号
        /// </summary>
        private int LogEleNum;

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

        /// <summary>
        /// 信号機の点灯状態変更履歴
        /// </summary>
        private List<string> LightChangeLog;

        /// <summary>
        /// 設定値構造体
        /// </summary>
        private WaitMSec SetMSec;

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

                //ピクチャボックスイメージの回転
                RotateLabelImage(RotateFlipType.Rotate90FlipNone,  pib_PNGreOne, pib_PNRedOne, pib_PSGreOne, pib_PSRedOne, pib_PSSignalTwo, pib_PNSignalTwo);
                RotateLabelImage(RotateFlipType.Rotate180FlipNone, pib_EArrow,   pib_PEGreTwo, pib_PERedTwo, pib_PWGreTwo, pib_PWRedTwo,    pib_SSignal,
                                                                   pib_ESignal,  pib_PESignalTwo, pib_PWSignalTwo);
                RotateLabelImage(RotateFlipType.Rotate270FlipNone, pib_PNGreTwo, pib_PNRedTwo, pib_PSGreTwo, pib_PSRedTwo, pib_PNSignalOne, pib_PSSignalOne);

                SetMSec = new WaitMSec(5, 5, 5, 5, 1, 1);  // 信号機点灯時間の初期設定

                //ピクチャボックスをラベルの親コントロールに設定する
                lbl_NSignal.Parent = pib_NSignal;
                lbl_SSignal.Parent = pib_SSignal;
                lbl_ESignal.Parent = pib_ESignal;
                lbl_EArrow.Parent  = pib_ESignal;
                lbl_WSignal.Parent = pib_WSignal;
                lbl_WArrow.Parent  = pib_WSignal;

                //ピクチャボックスのコントロールコレクションにラベルを追加する
                pib_NSignal.Controls.Add(lbl_NSignal);
                pib_SSignal.Controls.Add(lbl_SSignal);
                pib_ESignal.Controls.AddRange(new Control[] { lbl_ESignal, lbl_EArrow });
                pib_WSignal.Controls.AddRange(new Control[] { lbl_WSignal, lbl_WArrow });
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\nフォームのロードに失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// ピクチャボックスのイメージを回転する
        /// </summary>
        /// <param name="rotate"> イメージの回転量と反転軸           </param>
        /// <param name="pibs"> イメージ回転を行うピクチャボックス </param>
        private void RotateLabelImage(RotateFlipType rotate, params PictureBox[] pibs)
        {
            try
            {
                foreach (PictureBox pib in pibs)
                {
                    pib.BackgroundImage.RotateFlip(rotate);  // ピクチャボックスのバックグラウンドイメージを回転する
                }
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\nピクチャボックスイメージの回転に失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 信号機イメージ画像表示ラベルクリック時イベント
        /// </summary>
        private void Lbl_CarSignal_Click(object sender, EventArgs e)
        {
            try
            {
                int nSec = SetMSec.NMSec / 1000;  // 北方向車用信号機の秒数
                int sSec = SetMSec.SMSec / 1000;  // 南方向車用信号機の秒数
                int eSec = SetMSec.EMSec / 1000;  // 東方向車用信号機の秒数
                int wSec = SetMSec.WMSec / 1000;  // 西方向車用信号機の秒数

                // 設定値入力フォームの初期化
                F_SetSec f_SetSec = new F_SetSec
                {
                    IsEnable  = !IsTrafficEnable,      // 信号機アルゴリズム実行状態
                    ArrowSec  = SetMSec.AMSec / 1000,  // 矢印信号機の点灯秒
                    AllRedSec = SetMSec.RMSec / 1000   // 全信号機の赤点灯秒
                };
                
                // 設定値入力フォームを表示し、入力した値を取得する
                if (sender == lbl_NSignal)
                {
                    f_SetSec.SetValueName = "北方向への進行可能時間";
                    f_SetSec.IsArrow = false;
                    f_SetSec.SetValue = nSec;
                    f_SetSec.ShowDialog();
                    nSec = f_SetSec.SetValue;
                }
                else if (sender == lbl_SSignal)
                {
                    f_SetSec.SetValueName = "南方向への進行可能時間";
                    f_SetSec.IsArrow = false;
                    f_SetSec.SetValue = sSec;
                    f_SetSec.ShowDialog();
                    sSec = f_SetSec.SetValue;
                }
                else if (sender == lbl_ESignal || sender == lbl_EArrow)
                {
                    f_SetSec.SetValueName = "東方向への進行可能時間";
                    f_SetSec.IsArrow = true;
                    f_SetSec.SetValue = eSec;
                    f_SetSec.ShowDialog();
                    eSec = f_SetSec.SetValue;
                }
                else if (sender == lbl_WSignal || sender == lbl_WArrow)
                {
                    f_SetSec.SetValueName = "西方向への進行可能時間";
                    f_SetSec.IsArrow = true;
                    f_SetSec.SetValue = wSec;
                    f_SetSec.ShowDialog();
                    wSec = f_SetSec.SetValue;
                }

                SetMSec = new WaitMSec(nSec, sSec, eSec, wSec, f_SetSec.ArrowSec, f_SetSec.AllRedSec);
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n設定値入力フォームの表示に失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 「開始」ボタンクリック時イベント
        /// </summary>
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            try
            {
                // 信号機アルゴリズムが動いている場合
                if (IsTrafficEnable)
                {
                    string msgStr = "信号機プログラムを最初から実行しますか？";
                    if (MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;

                    Cts.Cancel();  // 先に実行した信号機アルゴリズムのキャンセル要求を伝える                        
                }

                LogEleNum       = 0;                          // 参照するリストの要素の番号を０に設定する
                IsTrafficEnable = true;                       // 信号機アルゴリズムが動く場合のブール値に設定する
                IsInterrupt     = false;                      // 信号機アルゴリズムの中断を無効にする
                ChangeTextInterruptResumeBtn(false);          // 「中断/再開」ボタンのTextプロパティ値変更
                PhaseList = CreateTrafficPhaseList(SetMSec);  // フェーズリスト作成
                LoopTrafficPhase(0, PhaseList);               // フェーズリストを最初のフェーズから再生する
            }
            catch (Exception ex) 
            {
                string exStr = ex.Message + "\n信号機プログラムの開始に失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                string exStr = ex.Message + "\n信号機プログラムの" + btn_InterruptResume.Text + "に失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string msgStr = "信号機の点灯状態をリセットしますか？";
                    if (MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return; 
                }

                Cts.Cancel();  // キャンセル要求を伝える
                
                ChangeSignalLightOn(LightState.NoLight, pib_NGreen,   pib_NYellow,  pib_NRed,     null);          // 北方向の車用信号機を無灯火にする
                ChangeSignalLightOn(LightState.NoLight, pib_SGreen,   pib_SYellow,  pib_SRed,     null);          // 南方向の車用信号機を無灯火にする
                ChangeSignalLightOn(LightState.NoLight, pib_EGreen,   pib_EYellow,  pib_ERed,     pib_EArrow);    // 東方向の車用信号機を無灯火にする
                ChangeSignalLightOn(LightState.NoLight, pib_WGreen,   pib_WYellow,  pib_WRed,     pib_WArrow);    // 西方向の車用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  pib_PNGreOne, pib_PNGreTwo, pib_PNRedOne, pib_PNRedTwo);  // 北方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  pib_PSGreOne, pib_PSGreTwo, pib_PSRedOne, pib_PSRedTwo);  // 南方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  pib_PEGreOne, pib_PEGreTwo, pib_PERedOne, pib_PERedTwo);  // 東方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  pib_PWGreOne, pib_PWGreTwo, pib_PWRedOne, pib_PWRedTwo);  // 西方向の歩行者用信号機を無灯火にする
                
                IsTrafficEnable = false;              // 信号機アルゴリズムが動かない場合のブール値に設定する
                IsInterrupt     = false;              // 信号機アルゴリズムの中断を無効にする
                lbx_StateRecord.Items.Clear();        // 点灯状態変更履歴をクリアにする
                ChangeTextInterruptResumeBtn(false);  // 「中断/再開」ボタンのTextプロパティ値変更
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n点灯状態のリセットに失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// タイトル表示ラベルクリック時イベント
        /// </summary>
        private void Lbl_SoftTitle_Click(object sender, EventArgs e)
        {
            try
            {
                F_Version f_Version = new F_Version();  // バージョン情報フォームの初期化
                f_Version.ShowDialog();                 // バージョン情報フォーム画面表示
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\nバージョン情報フォーム画面の表示に失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                string exStr = ex.Message + "\n「中断/再開」ボタンテキストの変更に失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                int carNSpdEWMSec = Math.Min(setMSec.NMSec, setMSec.SMSec) - BlinkMSec * BlinkPhaseCount - MinMSec;  // 北南の車用・東西の歩行者用信号機が緑に同時点灯するミリ秒数
                int carEWpdNSMSec = Math.Min(setMSec.EMSec, setMSec.WMSec) - BlinkMSec * BlinkPhaseCount - MinMSec;  // 東西の車用・北南の歩行者用信号機が緑に同時点灯するミリ秒数

                List<TrafficPhase> phaseList = new List<TrafficPhase>
                {
                    CreatePhase(setMSec.RMSec, LightState.Red, 
                        Signal.CarNorth,   Signal.CarSouth,   Signal.CarEast,   Signal.CarWest, 
                        Signal.PedesNorth, Signal.PedesSouth, Signal.PedesEast, Signal.PedesWest),  // 全ての車用・歩行者用信号機の赤点灯フェーズ

                    CreatePhase(carNSpdEWMSec, LightState.Green, 
                        Signal.CarNorth,   Signal.CarSouth,   Signal.PedesEast, Signal.PedesWest)   // 北南の車用・東西の歩行者用信号機の緑点灯フェーズ 
                };

                LightChangeLog = new List<string>
                {
                    "全信号機が赤に点灯しました。",
                    "北南車用・東西歩行者用信号機が緑に点灯しました。"
                };

                phaseList.AddRange(PedesBlink(Signal.PedesEast, Signal.PedesWest));                        // 東西の歩行者用信号機の点滅フェーズリスト
                phaseList.Add(CreatePhase(MinMSec, LightState.Red, Signal.PedesEast,  Signal.PedesWest));  // 東西の歩行者用信号機の赤点灯フェーズ
                LightChangeLog.AddRange(new List<string> { "東西歩行者用信号機が点滅しました。", 
                                                           "東西歩行者用信号機が赤に点灯しました。"});

                phaseList.AddRange(YelRedPhaseList(setMSec.RMSec, setMSec.NMSec - setMSec.SMSec, Signal.CarSouth, Signal.CarNorth));  // 北南の車用信号機の黄・赤点灯フェーズリスト
                

                phaseList.Add(CreatePhase(carEWpdNSMSec, LightState.Green, 
                    Signal.CarEast, Signal.CarWest, Signal.PedesNorth, Signal.PedesSouth));  // 東西の車用・北南の歩行者用信号機の緑点灯フェーズ
                LightChangeLog.Add("東西車用・北南歩行者用信号機が緑に点灯しました。");

                phaseList.AddRange(PedesBlink(Signal.PedesNorth, Signal.PedesSouth));                       // 北南の歩行者用信号機の点滅フェーズリスト
                phaseList.Add(CreatePhase(MinMSec, LightState.Red, Signal.PedesNorth, Signal.PedesSouth));  // 北南の歩行者用信号機の赤点灯フェーズ
                LightChangeLog.AddRange(new List<string> { "北南歩行者用信号機が点滅しました。",
                                                           "北南歩行者用信号機が赤に点灯しました。"});

                phaseList.AddRange(YelRedPhaseList(MinMSec, setMSec.EMSec - setMSec.WMSec, Signal.CarWest, Signal.CarEast));  // 東西の車用信号機の黄・赤点灯フェーズリスト

                phaseList.Add(CreatePhase(setMSec.AMSec, LightState.Arrow,  Signal.CarEast, Signal.CarWest));  // 東西の矢印信号機の点灯フェーズ
                phaseList.Add(CreatePhase(YellowMSec,    LightState.Yellow, Signal.CarEast, Signal.CarWest));  // 東西の車用信号機の黄点灯フェーズ
                LightChangeLog.AddRange(new List<string> { "東西矢印信号機が点灯しました。", 
                                                           "東西車用信号機が黄に点灯しました。" });

                return phaseList;
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\nフェーズリストの作成に失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>
        /// 複数の車用・歩行者用信号機を同じ時間待機・同じ色に点灯するフェーズを作成
        /// </summary>
        /// <param name="mSec">    待機時間ミリ秒 </param>
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
                    commands[i] = new TrafficCommand(signals[i], state);  // 信号機を表す列挙型と点灯状態を表す列挙型が入る
                }

                return new TrafficPhase(mSec, commands);
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\nフェーズの作成に失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<TrafficPhase> phaseList = new List<TrafficPhase>();  // 歩行者用信号機点滅フェーズのリスト

                for (int i = 0; i < BlinkPhaseCount; i++)
                {
                    if (i % 2 == 0)
                    {
                        phaseList.Add(CreatePhase(BlinkMSec, LightState.NoLight, pSigOne, pSigTwo));  // 歩行者用信号機を無灯火にするフェーズ
                    }
                    else
                    {
                        phaseList.Add(CreatePhase(BlinkMSec, LightState.Green,   pSigOne, pSigTwo));  // 歩行者用信号機を緑に点灯するフェーズ
                    }
                }

                return phaseList;
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n歩行者用信号機点滅フェーズリストの作成に失敗しました。" ;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                string sigStr = "北南";
                if (sigMin == Signal.CarWest || sigMin == Signal.CarEast) sigStr = "東西";

                if (mSecDif == 0)  
                {
                    // sigMinとsigMaxが表す車用信号機の緑点灯ミリ秒が一致する場合
                    pList.Add(CreatePhase(YellowMSec, LightState.Yellow, sigMin, sigMax));  // sigMinとsigMaxが表す車用信号機の黄点灯フェーズ
                    pList.Add(CreatePhase(redMSec,    LightState.Red,    sigMin, sigMax));  // sigMinとsigMaxが表す車用信号機の赤点灯フェーズ
                    LightChangeLog.Add(sigStr + "車用信号機が黄に点灯しました。");
                    LightChangeLog.Add(sigStr + "車用信号機が赤に点灯しました。");
                }
                else if (Math.Abs(mSecDif) == YellowMSec)  
                {
                    // sigMinとsigMaxが表す車用信号機の緑点灯ミリ秒の差がYellowMSecと一致する場合
                    pList.Add(new TrafficPhase(YellowMSec, new TrafficCommand(sigMin, LightState.Yellow)));  // sigMinが表す車用信号機の黄点灯フェーズ
                    pList.Add(new TrafficPhase(YellowMSec, new TrafficCommand(sigMin, LightState.Red),
                                                           new TrafficCommand(sigMax, LightState.Yellow)));  // sigMinが表す車用信号機の赤点灯、sigMaxが表す車用信号機の黄点灯フェーズ
                    pList.Add(new TrafficPhase(redMSec,    new TrafficCommand(sigMax, LightState.Red)));     // sigMaxが表す車用信号機の赤点灯フェーズ
                    LightChangeLog.Add(SigStr(sigMin) + "車用信号機が黄に点灯しました。");
                    LightChangeLog.Add(SigStr(sigMin) + "車用信号機が赤・" + SigStr(sigMax) + "車用信号機が黄に点灯しました。");
                    LightChangeLog.Add(SigStr(sigMax) + "車用信号機が赤に点灯しました。");
                }
                else
                {
                    pList.Add(new TrafficPhase(YellowMSec,                     new TrafficCommand(sigMin, LightState.Yellow)));  // sigMinが表す車用信号機の黄点灯フェーズ
                    pList.Add(new TrafficPhase(Math.Abs(mSecDif) - YellowMSec, new TrafficCommand(sigMin, LightState.Red)));     // sigMinが表す車用信号機の赤点灯フェーズ
                    pList.Add(new TrafficPhase(YellowMSec,                     new TrafficCommand(sigMax, LightState.Yellow)));  // sigMaxが表す車用信号機の黄点灯フェーズ
                    pList.Add(new TrafficPhase(redMSec,                        new TrafficCommand(sigMax, LightState.Red)));     // sigMaxが表す車用信号機の赤点灯フェーズ
                    LightChangeLog.Add(SigStr(sigMin) + "車用信号機が黄に点灯しました。");
                    LightChangeLog.Add(SigStr(sigMax) + "車用信号機が赤に点灯しました。");
                    LightChangeLog.Add(SigStr(sigMin) + "車用信号機が黄に点灯しました。");
                    LightChangeLog.Add(SigStr(sigMax) + "車用信号機が赤に点灯しました。");
                }

                return pList;
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n車用信号機の黄・赤点灯フェーズリストの作成に失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>
        /// 信号機列挙型から方角を表す文字列を返す
        /// </summary>
        /// <param name="signal"> 信号機を表す列挙型 </param>
        /// <returns> 方角を表す文字列 </returns>
        private string SigStr(Signal signal)
        {
            if (signal == Signal.CarNorth) return "北";
            if (signal == Signal.CarSouth) return "南";
            if (signal == Signal.CarEast)  return "東";
            if (signal == Signal.CarWest)  return "西";
            return "";
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストをループさせる
        /// </summary>
        /// <param name="phaseNum"> 最初に再生するフェーズを表す番号 </param>
        /// <param name="phases">   ループ再生するフェーズリスト     </param>
        private async void LoopTrafficPhase(int phaseNum, List<TrafficPhase> phases)
        {
            try
            {
                int  startPhase  = phaseNum;          // 最初に再生するフェーズの番号を取得する
                bool isSucUpdate = false;             // 信号機点灯状態の更新に成功した場合はtrue、それ以外の場合はfalse
                Cts = new CancellationTokenSource();  // Ctsの初期化

                while (!Cts.IsCancellationRequested)
                {
                    for (int i = startPhase; i < phases.Count; i++)
                    {
                        InterruptPhase = i;  // 現在のフェーズを表す番号を取得する

                        foreach (TrafficCommand command in phases[i].Commands)
                        {
                            if (command.Signal == Signal.CarNorth)
                            {
                                isSucUpdate = ChangeSignalLightOn(command.State, pib_NGreen, pib_NYellow, pib_NRed, null);  // 北方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.CarSouth)
                            {
                                isSucUpdate = ChangeSignalLightOn(command.State, pib_SGreen, pib_SYellow, pib_SRed, null);  // 南方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.CarEast)
                            {
                                isSucUpdate = ChangeSignalLightOn(command.State, pib_EGreen, pib_EYellow, pib_ERed, pib_EArrow);  // 東方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.CarWest)
                            {
                                isSucUpdate = ChangeSignalLightOn(command.State, pib_WGreen, pib_WYellow, pib_WRed, pib_WArrow);  // 西方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesNorth)
                            {
                                isSucUpdate = ChangePedesLightOn(command.State, pib_PNGreOne, pib_PNGreTwo, pib_PNRedOne, pib_PNRedTwo);  // 北方向の歩行者用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesSouth)
                            {
                                isSucUpdate = ChangePedesLightOn(command.State, pib_PSGreOne, pib_PSGreTwo, pib_PSRedOne, pib_PSRedTwo);  // 南方向の歩行者用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesEast)
                            {
                                isSucUpdate = ChangePedesLightOn(command.State, pib_PEGreOne, pib_PEGreTwo, pib_PERedOne, pib_PERedTwo);  // 東方向の歩行者用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesWest)
                            {
                                isSucUpdate = ChangePedesLightOn(command.State, pib_PWGreOne, pib_PWGreTwo, pib_PWRedOne, pib_PWRedTwo);  // 西方向の歩行者用信号機の点灯状態更新
                            }

                            // 信号機点灯状態の更新に失敗した場合
                            if (!isSucUpdate)
                            {
                                string failStr = "信号機点灯状態の更新に失敗しました。";
                                MessageBox.Show(failStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        try
                        {
                            CreateStateRecord(phases, i);                     // リストボックスに点灯状態を書き込む
                            await Task.Delay(phases[i].WaitMSec, Cts.Token);  // キャンセルが要求されていない場合、WaitMSecミリ秒待機する
                        }
                        catch
                        {
                            LogEleNum--;
                            return;  // 待機中に例外が発生した場合は終了する
                        }
                    }
                    
                    startPhase = 0;  // 次のループで最初から再生するフェーズの番号を0に設定する
                }
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\nフェーズリストの再生に失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// リストボックスに状態変更履歴を表す文字列を追加する
        /// </summary>
        /// <param name="phases">   信号機アルゴリズムの点灯フェーズリスト     </param>
        /// <param name="phaseNum"> 信号機アルゴリズムの点灯フェーズを表す番号 </param>
        private void CreateStateRecord(List<TrafficPhase> phases, int phaseNum)
        {
            try
            {
                int listItemCount = lbx_StateRecord.Items.Count;  // リストボックスの項目数を取得
                
                // 点灯状態変更履歴を追加する
                if (phases[phaseNum].WaitMSec > BlinkMSec)
                {
                    // 点灯フェーズ phases[phaseNum] の待機ミリ秒がBlinkMSecより大きい場合
                    // 現在の点灯フェーズの待機ミリ秒と点灯状態変更内容を追加する
                    lbx_StateRecord.Items.Add(LogEleNum + "：" + phases[phaseNum].WaitMSec + "ミリ秒待機。" + LightChangeLog[LogEleNum]);
                }
                else if (phases[phaseNum].WaitMSec == BlinkMSec && phases[phaseNum - 1].WaitMSec > BlinkMSec)
                {
                    // 現在の点灯フェーズの待機ミリ秒がBlinkMSecと等しく、１つ前の点灯フェーズの待機ミリ秒がBlinkMSecより大きい場合
                    // 点滅を表すフェーズを１行の文字列にまとめて履歴に追加する
                    lbx_StateRecord.Items.Add(LogEleNum + "：" + BlinkMSec * BlinkPhaseCount + "ミリ秒待機。" + LightChangeLog[LogEleNum]);
                }

                // リストボックスの項目数が増えた場合にLogEleNumの値を更新する
                if (lbx_StateRecord.Items.Count > listItemCount)
                {
                    LogEleNum++;
                    if (LogEleNum >= LightChangeLog.Count) LogEleNum = 0;        // LogEleNumを０に戻す
                    lbx_StateRecord.TopIndex = lbx_StateRecord.Items.Count - 1;  // 最新の履歴を表示する
                }
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n信号機の点灯状態変更履歴の追加に失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 車用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">      点灯状態を表す列挙型                         </param>
        /// <param name="pib_green">  車用信号機の緑ランプを表すピクチャボックス   </param>
        /// <param name="pib_yellow"> 車用信号機の黄ランプを表すピクチャボックス   </param>
        /// <param name="pib_red">    車用信号機の赤ランプを表すピクチャボックス   </param>
        /// <param name="pib_arrow">  矢印信号機の矢印ランプを表すピクチャボックス </param>
        /// <returns> 点灯状態更新に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool ChangeSignalLightOn(LightState state, PictureBox pib_green, PictureBox pib_yellow, PictureBox pib_red, PictureBox pib_arrow)
        {
            try
            {
                bool greVisible = false;  // pib_green のVisibleプロパティ値
                bool yelVisible = false;  // pib_yellowのVisibleプロパティ値
                bool redVisible = false;  // pib_red   のVisibleプロパティ値
                bool arwVisible = false;  // pib_arrow のVisibleプロパティ値

                if (state == LightState.Green) greVisible = true;
                if (state == LightState.Yellow) yelVisible = true;
                if (state == LightState.Red || state == LightState.Arrow) redVisible = true;
                if (state == LightState.Arrow) arwVisible = true;

                pib_green.Visible  = greVisible;                        // pib_green のVisibleプロパティ値の設定
                pib_yellow.Visible = yelVisible;                        // pib_yellowのVisibleプロパティ値の設定
                pib_red.Visible    = redVisible;                        // pib_red   のVisibleプロパティ値の設定
                if (pib_arrow != null) pib_arrow.Visible = arwVisible;  // pib_arrow のVisibleプロパティ値の設定

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
        /// <param name="state">        点灯状態を表す列挙型                                   </param>
        /// <param name="pib_greenOne"> 歩行者用信号機の緑ランプを表す１つ目のピクチャボックス </param>
        /// <param name="pib_greenTwo"> 歩行者用信号機の緑ランプを表す２つ目のピクチャボックス </param>
        /// <param name="pib_redOne">   歩行者用信号機の赤ランプを表す１つ目のピクチャボックス </param>
        /// <param name="pib_redTwo">   歩行者用信号機の赤ランプを表す２つ目のピクチャボックス </param>
        /// <returns> 点灯状態更新に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool ChangePedesLightOn(LightState state, PictureBox pib_greenOne, PictureBox pib_greenTwo, PictureBox pib_redOne, PictureBox pib_redTwo)
        {
            try
            {
                bool greVisible = false;  // pib_greenOneとpib_greenTwoのVisibleプロパティ値
                bool redVisible = false;  // pib_redOne  とpib_redTwo  のVisibleプロパティ値

                if (state == LightState.Green) greVisible = true;
                if (state == LightState.Red)   redVisible = true;

                pib_greenOne.Visible = greVisible;  // pib_greenOneのVisibleプロパティ値の設定
                pib_greenTwo.Visible = greVisible;  // pib_greenTwoのVisibleプロパティ値の設定
                pib_redOne.Visible   = redVisible;  // pib_redOne  のVisibleプロパティ値の設定
                pib_redTwo.Visible   = redVisible;  // pib_redTwo  のVisibleプロパティ値の設定

                return true;   // 点灯状態更新成功
            }
            catch
            {
                return false;  // 点灯状態更新失敗
            }
        }
    }
}