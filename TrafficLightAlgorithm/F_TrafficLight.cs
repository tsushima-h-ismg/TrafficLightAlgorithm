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
        /// 歩行者用信号機の点滅間隔ミリ秒
        /// </summary>
        private const int BlinkMSec       = 500;

        /// <summary>
        /// 信号機点灯ミリ秒の最小値
        /// </summary>
        private const int MinMSec         = 1000;

        /// <summary>
        /// 全信号機の赤点灯ミリ秒
        /// </summary>
        private const int AllRedMSec      = 1000;

        /// <summary>
        /// 歩行者用信号機点滅の合計フェーズ数
        /// </summary>
        private const int BlinkPhaseCount = 5;

        /// <summary>
        /// 信号機アルゴリズムのフェーズ再生で中断したフェーズを表す番号
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
                IsTrafficEnable = false;                                       // 信号機アルゴリズムが動かない場合のブール値に設定する
                IsInterrupt     = false;                                       // 信号機アルゴリズムの中断を無効にする
                Cts             = new CancellationTokenSource();               // Ctsの初期化
                SetMSec         = new WaitMSec(5000, 5000, 5000, 5000, 1000);  // 信号機点灯時間の初期設定

                //ピクチャボックスのコントロールコレクションにラベルを追加する
                pib_NSignal.Controls.Add(lbl_NSignal);
                pib_SSignal.Controls.Add(lbl_SSignal);
                pib_ESignal.Controls.AddRange(new Control[] { lbl_ESignal, lbl_EArrow });
                pib_WSignal.Controls.AddRange(new Control[] { lbl_WSignal, lbl_WArrow });

                // ラベルのコントロールコレクションに信号機のランプイメージ画像表示ピクチャボックスを追加する
                lbl_NSignal.Controls.AddRange(new Control[] { pib_NGreen, pib_NYellow, pib_NRed });
                lbl_SSignal.Controls.AddRange(new Control[] { pib_SGreen, pib_SYellow, pib_SRed });
                lbl_ESignal.Controls.AddRange(new Control[] { pib_EGreen, pib_EYellow, pib_ERed });
                lbl_WSignal.Controls.AddRange(new Control[] { pib_WGreen, pib_WYellow, pib_WRed });
            }
            catch (Exception ex)
            {
                string exStr = "フォームのロードでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 信号機イメージ画像クリック時イベント
        /// </summary>
        private void Lbl_CarSignal_Click(object sender, EventArgs e)
        {
            try
            {
                // 設定値入力フォームを表示する
                if (sender == lbl_NSignal || sender == pib_NGreen || sender == pib_NYellow || sender == pib_NRed)
                {
                    SetFormShow(SetMSec.NMSec, SetMSec.AMSec, "北", false, pib_NSignal, Properties.Resources.NSignalHilight);  // 北車用信号機の設定値入力
                }
                else if (sender == lbl_SSignal || sender == pib_SGreen || sender == pib_SYellow || sender == pib_SRed)
                {
                    SetFormShow(SetMSec.SMSec, SetMSec.AMSec, "南", false, pib_SSignal, Properties.Resources.SSignalHilight);  // 南車用信号機の設定値入力
                }
                else if (sender == lbl_ESignal || sender == lbl_EArrow || sender == pib_EGreen || sender == pib_EYellow || sender == pib_ERed || sender == pib_EArrow)
                {
                    SetFormShow(SetMSec.EMSec, SetMSec.AMSec, "東", true,  pib_ESignal, Properties.Resources.ESignalHilight);  // 東車用信号機の設定値入力
                }
                else if (sender == lbl_WSignal || sender == lbl_WArrow || sender == pib_WGreen || sender == pib_WYellow || sender == pib_WRed || sender == pib_WArrow)
                {
                    SetFormShow(SetMSec.WMSec, SetMSec.AMSec, "西", true,  pib_WSignal, Properties.Resources.WSignalHilight);  // 西車用信号機の設定値入力
                }
            }
            catch (Exception ex)
            {
                string exStr = "設定値入力フォームの表示でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 設定値入力フォームを表示する
        /// </summary>
        /// <param name="avaiMSec">     進行可能ミリ秒数                                </param>
        /// <param name="arrMSec">      矢印信号機の点灯ミリ秒数                        </param>
        /// <param name="dir">          方角を表す文字列                                </param>
        /// <param name="isArrow">      trueで矢印信号機を有する、それ以外の場合はfalse </param>
        /// <param name="pib">          信号機イメージ画像を表示するラベル              </param>
        /// <param name="highlightImg"> 強調表示した信号機イメージ画像                  </param>
        private void SetFormShow(int avaiMSec, int arrMSec, string dir, bool isArrow, PictureBox pib, Image highlightImg)
        {
            try
            {
                Image defaultImage  = pib.BackgroundImage;  // ピクチャボックスの背景画像をデフォルト背景画像として取得する
                pib.BackgroundImage = highlightImg;         // ピクチャボックスの背景画像を強調表示した信号機イメージ画像にする

                // 設定値入力フォームを初期化
                F_SetSec f_SetSec = new F_SetSec
                { 
                    AvaiSec       = avaiMSec / 1000,
                    ArrowSec      = arrMSec  / 1000,
                    DirectionName = dir,
                    IsArrow       = isArrow,
                    IsEnable      = !IsTrafficEnable 
                };
                
                Point pibTopLeft = pib.PointToScreen(Point.Empty);            // 信号機イメージ画像表示ピクチャボックスの左上端の座標
                Rectangle scArea = Screen.FromPoint(pibTopLeft).WorkingArea;  // 信号機イメージ画像表示ピクチャボックスを保持するスクリーンの作業領域を取得
                int xlocation    = pibTopLeft.X + pib.Width;                  // 設定値入力フォーム初期表示位置のx座標
                int ylocation    = pibTopLeft.Y;                              // 設定値入力フォーム初期表示位置のy座標
                if (xlocation > scArea.Right  - f_SetSec.Width)  xlocation = scArea.Right  - f_SetSec.Width;
                if (ylocation > scArea.Bottom - f_SetSec.Height) ylocation = scArea.Bottom - f_SetSec.Height; 

                f_SetSec.Location = new Point(xlocation, ylocation);  // 設定値入力フォームの画面全体がスクリーン内に収まるように初期表示位置を設定する
                f_SetSec.ShowDialog();                                // 設定値入力フォームを表示

                int avaimsec = f_SetSec.AvaiSec  * 1000;  // 設定値入力フォームから進行可能秒数を取得してミリ秒に変換する
                int arrmsec  = f_SetSec.ArrowSec * 1000;  // 設定値入力フォームから矢印信号機点灯秒数を取得してミリ秒に変換する

                // 設定値構造体を初期化
                if      (pib == pib_NSignal) SetMSec = new WaitMSec(avaimsec,      SetMSec.SMSec, SetMSec.EMSec, SetMSec.WMSec, arrmsec);
                else if (pib == pib_SSignal) SetMSec = new WaitMSec(SetMSec.NMSec, avaimsec,      SetMSec.EMSec, SetMSec.WMSec, arrmsec);
                else if (pib == pib_ESignal) SetMSec = new WaitMSec(SetMSec.NMSec, SetMSec.SMSec, avaimsec,      SetMSec.WMSec, arrmsec);
                else if (pib == pib_WSignal) SetMSec = new WaitMSec(SetMSec.NMSec, SetMSec.SMSec, SetMSec.EMSec, avaimsec,      arrmsec);

                pib.BackgroundImage = defaultImage;  // ピクチャボックスの背景画像をデフォルト背景画像にする
            }
            catch (Exception ex)
            {
                string exStr = "設定値入力フォームの表示でエラーが発生しました。\n" + ex.Message;
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

                ChangeTextInterruptResumeBtn(false);                        // 「中断/再開」ボタンのTextプロパティ値変更
                PhaseList = CreateTrafficPhaseList(SetMSec);                // フェーズリスト作成
                if (PhaseList == null || PhaseList.Contains(null)) return;  // フェーズリストが作成できなかった場合は終了する

                IsTrafficEnable = true;          // 信号機アルゴリズムが動く場合のブール値に設定する
                IsInterrupt     = false;         // 信号機アルゴリズムの中断を無効にする
                LoopTrafficPhase(0, PhaseList);  // フェーズリストを最初のフェーズから再生する
            }
            catch (Exception ex) 
            {
                string exStr = "信号機の点灯を開始する前にエラーが発生しました。\n" + ex.Message;
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
                string irtext = "再開";
                if (IsInterrupt) irtext = "中断";
                string exStr = "信号機プログラムの" + irtext + "でエラーが発生しました。\n" + ex.Message;
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
                ChangePedesLightOn(LightState.NoLight,  lbl_PNGreOne, lbl_PNGreTwo, lbl_PNRedOne, lbl_PNRedTwo);  // 北方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  lbl_PSGreOne, lbl_PSGreTwo, lbl_PSRedOne, lbl_PSRedTwo);  // 南方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  lbl_PEGreOne, lbl_PEGreTwo, lbl_PERedOne, lbl_PERedTwo);  // 東方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  lbl_PWGreOne, lbl_PWGreTwo, lbl_PWRedOne, lbl_PWRedTwo);  // 西方向の歩行者用信号機を無灯火にする
                
                IsTrafficEnable = false;              // 信号機アルゴリズムが動かない場合のブール値に設定する
                IsInterrupt     = false;              // 信号機アルゴリズムの中断を無効にする
                lbx_StateRecord.Items.Clear();        // 点灯状態変更履歴をクリアにする
                ChangeTextInterruptResumeBtn(false);  // 「中断/再開」ボタンのTextプロパティ値変更
            }
            catch (Exception ex)
            {
                string exStr = "信号機の点灯状態のリセットでエラーが発生しました。\n" + ex.Message;
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
                f_Version.ShowDialog();                 // バージョン情報フォーム表示
            }
            catch (Exception ex)
            {
                string exStr = "バージョン情報フォーム画面の表示でエラーが発生しました。\n" + ex.Message;
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
                if (isInterrupt) btn_InterruptResume.Text = "再開";  // 信号機アルゴリズムの中断が有効の場合は「再開」に設定する 
                else             btn_InterruptResume.Text = "中断";  // 信号機アルゴリズムの中断が無効の場合は「中断」に設定する
            }
            catch (Exception ex)
            {
                string exStr = "「中断/再開」ボタンテキストの変更でエラーが発生しました。\n" + ex.Message;
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
                    // 全ての車用・歩行者用信号機の赤点灯フェーズ
                    CreatePhase(0, AllRedMSec, MSecToSec(AllRedMSec) + "秒待機。全信号機が赤に点灯しました。",
                        LightState.Red, Signal.CarNorth,   Signal.CarSouth,   Signal.CarEast,   Signal.CarWest,
                                        Signal.PedesNorth, Signal.PedesSouth, Signal.PedesEast, Signal.PedesWest),
                    
                    // 北南車用・東西歩行者用信号機の緑点灯フェーズ 
                    CreatePhase(1, carNSpdEWMSec, MSecToSec(carNSpdEWMSec) + "秒待機。北南車用・東西歩行者用信号機が緑に点灯しました。",
                        LightState.Green, Signal.CarNorth, Signal.CarSouth, Signal.PedesEast, Signal.PedesWest)
                };
                
                // 東西歩行者用信号機の点滅フェーズリスト
                phaseList.AddRange(PedesBlink(2, Signal.PedesEast, Signal.PedesWest));
                
                // 東西歩行者用信号機の赤点灯フェーズ
                phaseList.Add(CreatePhase(3, MinMSec, MSecToSec(MinMSec) + "秒待機。東西歩行者用信号機が赤に点灯しました。", 
                    LightState.Red, Signal.PedesEast, Signal.PedesWest));
                
                // 北南車用信号機の黄・赤点灯フェーズリスト
                phaseList.AddRange(YelRedPhaseList(4, AllRedMSec, setMSec.NMSec - setMSec.SMSec, Signal.CarNorth, Signal.CarSouth));

                // 東西車用・北南歩行者用信号機の緑点灯フェーズ 
                phaseList.Add(CreatePhase(phaseList[phaseList.Count - 1].PhaseNum + 1, carEWpdNSMSec, MSecToSec(carEWpdNSMSec) + "秒待機。東西車用・北南歩行者用信号機が緑に点灯しました。",
                    LightState.Green, Signal.CarEast, Signal.CarWest, Signal.PedesNorth, Signal.PedesSouth));
                
                // 北南歩行者用信号機の点滅フェーズリスト
                phaseList.AddRange(PedesBlink(phaseList[phaseList.Count - 1].PhaseNum + 1, Signal.PedesNorth, Signal.PedesSouth));
                
                // 北南歩行者用信号機の赤点灯フェーズ 
                phaseList.Add(CreatePhase(phaseList[phaseList.Count - 1].PhaseNum + 1, MinMSec, MSecToSec(MinMSec) + "秒待機。北南歩行者用信号機が赤に点灯しました。", 
                    LightState.Red, Signal.PedesNorth, Signal.PedesSouth));
                
                // 東西車用信号機の黄・赤点灯フェーズリスト
                phaseList.AddRange(YelRedPhaseList(phaseList[phaseList.Count - 1].PhaseNum + 1, MinMSec, setMSec.EMSec - setMSec.WMSec, Signal.CarEast, Signal.CarWest));

                // 東西矢印信号機の点灯フェーズ
                phaseList.Add(CreatePhase(phaseList[phaseList.Count - 1].PhaseNum + 1, setMSec.AMSec, MSecToSec(setMSec.AMSec) + "秒待機。東西矢印信号機が点灯しました。",
                    LightState.Arrow,  Signal.CarEast, Signal.CarWest));
                
                // 東西車用信号機の黄点灯フェーズ
                phaseList.Add(CreatePhase(phaseList[phaseList.Count - 1].PhaseNum + 1, YellowMSec, MSecToSec(YellowMSec) + "秒待機。東西車用信号機が黄に点灯しました。",
                    LightState.Yellow, Signal.CarEast, Signal.CarWest));

                return phaseList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 複数の車用・歩行者用信号機を同じ時間待機・同じ色に点灯するフェーズを作成
        /// </summary>
        /// <param name="num">     点灯フェーズを表す番号     </param>
        /// <param name="mSec">    待機時間ミリ秒             </param>
        /// <param name="msg">     点灯状態の内容を表す文字列 </param>
        /// <param name="state">   点灯状態を表す列挙型       </param>
        /// <param name="signals"> 点灯する信号機を表す列挙型 </param>
        /// <returns> 作成したフェーズ </returns>
        private TrafficPhase CreatePhase(int num, int mSec, string msg, LightState state, params Signal[] signals)
        {
            try
            {
                TrafficCommand[] commands = new TrafficCommand[signals.Length];
                
                for (int i = 0; i < commands.Length; i++)
                {
                    commands[i] = new TrafficCommand(signals[i], state);  // 信号機を表す列挙型と点灯状態を表す列挙型が入る
                }

                return new TrafficPhase(num, mSec, msg, commands);
            }
            catch (Exception ex)
            {
                string exStr = "信号機点灯フェーズの作成でエラーが発生しました。\n" + ex.Message;
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
        private List<TrafficPhase> PedesBlink(int num, Signal pSigOne, Signal pSigTwo)
        {
            try
            {
                // 歩行者用信号機の点滅を表す文字列
                string pBlinkStr = MSecToSec(BlinkMSec * BlinkPhaseCount) + "秒待機。" + SigToStr(pSigOne) + SigToStr(pSigTwo) + "歩行者用信号機が点滅しました。";
                
                List<TrafficPhase> phaseList = new List<TrafficPhase>();  // 歩行者用信号機点滅フェーズが入るリスト
                
                for (int i = 0; i < BlinkPhaseCount; i++)
                {
                    if (i % 2 == 0) phaseList.Add(CreatePhase(num, BlinkMSec, pBlinkStr, LightState.NoLight, pSigOne, pSigTwo));  // 歩行者用信号機を無灯火にするフェーズ
                    else            phaseList.Add(CreatePhase(num, BlinkMSec, pBlinkStr, LightState.Green,   pSigOne, pSigTwo));  // 歩行者用信号機を緑に点灯するフェーズ
                }

                return phaseList;
            }
            catch (Exception ex)
            {
                string exStr = "歩行者用信号機の点滅フェーズリストの作成でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>
        /// 車用信号機の黄・赤点灯フェーズリストを作成する
        /// </summary>
        /// <param name="num">     点灯フェーズの番号                               </param>
        /// <param name="redMSec"> sigTwoが表す車用信号機の赤点灯ミリ秒             </param>
        /// <param name="mSecDif"> sigOneとsigTwoが表す車用信号機の緑点灯ミリ秒の差 </param>
        /// <param name="sigOne">  １つ目の車用信号機を表す列挙型                   </param>
        /// <param name="sigTwo">  ２つ目の車用信号機を表す列挙型                   </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> YelRedPhaseList(int num, int redMSec, int mSecDif, Signal sigOne, Signal sigTwo)
        {
            try
            {
                Signal sigMax = sigOne;  // 進行可能時間が長い方の車用信号機を表す列挙型が入る
                Signal sigMin = sigTwo;  // 進行可能時間が短い方の車用信号機を表す列挙型が入る
                if (mSecDif < 0) (sigMin, sigMax) = (sigMax, sigMin);

                string yelSecStr = MSecToSec(YellowMSec) + "秒待機。";  // 信号機が黄点灯秒だけ待機することを表す文字列
                string redSecStr = MSecToSec(redMSec)    + "秒待機。";  // 信号機が赤点灯秒だけ待機することを表す文字列
                List<TrafficPhase> phases = new List<TrafficPhase>();   // 信号機点灯フェーズリスト

                if (mSecDif == 0)
                {
                    // sigMinとsigMaxが表す車用信号機の進行可能ミリ秒数が一致する場合
                    string sigDir = SigToStr(sigMax) + SigToStr(sigMin) + "車用信号機";  // ２つの車用信号機を表す文字列

                    phases.Add(CreatePhase(num,     YellowMSec, yelSecStr + sigDir + "が黄に点灯しました。", LightState.Yellow, sigMax, sigMin));  // sigMinとsigMaxの黄点灯
                    phases.Add(CreatePhase(num + 1, redMSec,    redSecStr + sigDir + "が赤に点灯しました。", LightState.Red,    sigMax, sigMin));  // sigMinとsigMaxの赤点灯
                }
                else
                {
                    string sigMaxDir = SigToStr(sigMax) + "車用信号機";  // 進行可能時間が長い方の車用信号機を表す文字列
                    string sigMinDir = SigToStr(sigMin) + "車用信号機";  // 進行可能時間が短い方の車用信号機を表す文字列

                    phases.Add(new TrafficPhase(num, YellowMSec, yelSecStr + sigMinDir + "が黄に点灯しました。", new TrafficCommand(sigMin, LightState.Yellow)));  // sigMinの黄点灯

                    if (Math.Abs(mSecDif) == YellowMSec)
                    {
                        string msg = yelSecStr + sigMinDir + "が赤・" + sigMaxDir + "が黄に点灯しました。";

                        phases.Add(new TrafficPhase(num + 1, YellowMSec, msg, new TrafficCommand(sigMin, LightState.Red),
                                                                              new TrafficCommand(sigMax, LightState.Yellow)));  // sigMinの赤、sigMaxの黄点灯
                    }
                    else
                    {
                        int    minRedMSec = Math.Abs(mSecDif) - YellowMSec;      // 信号機sigMinの赤点灯ミリ秒数
                        string minRedStr  = MSecToSec(minRedMSec) + "秒待機。";  // 信号機sigMinが赤点灯秒だけ待機することを表す文字列
                        
                        phases.Add(new TrafficPhase(num + 1, minRedMSec, minRedStr + sigMinDir + "が赤に点灯しました。", new TrafficCommand(sigMin, LightState.Red)));     // sigMinの赤点灯
                        phases.Add(new TrafficPhase(num + 2, YellowMSec, yelSecStr + sigMaxDir + "が黄に点灯しました。", new TrafficCommand(sigMax, LightState.Yellow)));  // sigMaxの黄点灯
                    }

                    phases.Add(new TrafficPhase(num + phases.Count, redMSec, redSecStr + sigMaxDir + "が赤に点灯しました。", new TrafficCommand(sigMax, LightState.Red)));  // sigMaxの赤点灯
                }

                return phases;
            }
            catch (Exception ex)
            {
                string exStr = "車用信号機の黄・赤点灯フェーズリストの作成でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>
        /// ミリ秒数を秒数に変換
        /// </summary>
        /// <param name="mSec"> ミリ秒数 </param>
        /// <returns> 変換後の秒数 </returns>
        private float MSecToSec(int mSec)
        {
            return mSec / 1000F;
        }

        /// <summary>
        /// 信号機を表す列挙型から方角を表す文字列を作成する
        /// </summary>
        /// <param name="signal"> 信号機の名称を表す列挙型 </param>
        /// <returns> 作成した文字列 </returns>
        private string SigToStr(Signal signal)
        {
            try
            {
                if      (signal == Signal.CarNorth || signal == Signal.PedesNorth) return "北";  // 北方向の信号機の場合は「北」を返す
                else if (signal == Signal.CarSouth || signal == Signal.PedesSouth) return "南";  // 南方向の信号機の場合は「南」を返す
                else if (signal == Signal.CarEast  || signal == Signal.PedesEast)  return "東";  // 東方向の信号機の場合は「東」を返す
                else if (signal == Signal.CarWest  || signal == Signal.PedesWest)  return "西";  // 西方向の信号機の場合は「西」を返す
                return "";
            }
            catch
            {
                return "";
            }
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
                int startPhase = 0;                   // 最初に再生するフェーズのインデックス番号が入る
                Cts = new CancellationTokenSource();  // Ctsの初期化
                
                for (int i = 0; i < phases.Count; i++)
                {
                    if (phaseNum == phases[i].PhaseNum)
                    {
                        startPhase = i;  // phaseNum番目のフェーズのインデックス番号を取得する
                        break;
                    }
                }

                while (!Cts.IsCancellationRequested)
                {
                    for (int j = startPhase; j < phases.Count; j++)
                    {
                        InterruptPhase = phases[j].PhaseNum;  // 現在のフェーズを表す番号を取得する

                        foreach (TrafficCommand cmd in phases[j].Commands)
                        {
                            // 点灯状態更新の結果を取得し、結果がfalseの場合は終了する
                            if (cmd.Signal == Signal.CarNorth)
                            {
                                if (!ChangeSignalLightOn(cmd.State, pib_NGreen, pib_NYellow, pib_NRed, null)) return;  // 北方向の車用信号機の点灯状態更新
                            }
                            else if (cmd.Signal == Signal.CarSouth)
                            {
                                if (!ChangeSignalLightOn(cmd.State, pib_SGreen, pib_SYellow, pib_SRed, null)) return;  // 南方向の車用信号機の点灯状態更新
                            }
                            else if (cmd.Signal == Signal.CarEast)
                            {
                                if (!ChangeSignalLightOn(cmd.State, pib_EGreen, pib_EYellow, pib_ERed, pib_EArrow)) return;  // 東方向の車用信号機の点灯状態更新
                            }
                            else if (cmd.Signal == Signal.CarWest)
                            {
                                if (!ChangeSignalLightOn(cmd.State, pib_WGreen, pib_WYellow, pib_WRed, pib_WArrow)) return;  // 西方向の車用信号機の点灯状態更新
                            }
                            else if (cmd.Signal == Signal.PedesNorth)
                            {
                                if (!ChangePedesLightOn(cmd.State, lbl_PNGreOne, lbl_PNGreTwo, lbl_PNRedOne, lbl_PNRedTwo)) return;  // 北方向の歩行者用信号機の点灯状態更新
                            }
                            else if (cmd.Signal == Signal.PedesSouth)
                            {
                                if (!ChangePedesLightOn(cmd.State, lbl_PSGreOne, lbl_PSGreTwo, lbl_PSRedOne, lbl_PSRedTwo)) return;  // 南方向の歩行者用信号機の点灯状態更新
                            }
                            else if (cmd.Signal == Signal.PedesEast)
                            {
                                if (!ChangePedesLightOn(cmd.State, lbl_PEGreOne, lbl_PEGreTwo, lbl_PERedOne, lbl_PERedTwo)) return;  // 東方向の歩行者用信号機の点灯状態更新
                            }
                            else if (cmd.Signal == Signal.PedesWest)
                            {
                                if (!ChangePedesLightOn(cmd.State, lbl_PWGreOne, lbl_PWGreTwo, lbl_PWRedOne, lbl_PWRedTwo)) return;  // 西方向の歩行者用信号機の点灯状態更新
                            }
                        }

                        if (j == 0 || phases[j].PhaseNum != phases[j - 1].PhaseNum)
                        {
                            lbx_StateRecord.Items.Add(lbx_StateRecord.Items.Count + "：" + phases[j].Message);  // 点灯状態変更内容をリストボックスに追加する
                            lbx_StateRecord.TopIndex = lbx_StateRecord.Items.Count - 1;                         // 最新の履歴を表示する
                        }

                        try
                        {
                            await Task.Delay(phases[j].WaitMSec, Cts.Token);  // キャンセルが要求されていない場合、WaitMSecミリ秒待機する
                        }
                        catch
                        {
                            return;  // 待機中に例外が発生した場合は終了する
                        }
                    }
                    
                    startPhase = 0;  // 次のループで最初から再生するフェーズの番号を0に設定する
                }
            }
            catch (Exception ex)
            {
                string exStr = "信号機の点灯でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 車用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">      点灯状態を表す列挙型                       </param>
        /// <param name="pib_green">  車用信号機の緑ランプを表すピクチャボックス </param>
        /// <param name="pib_yellow"> 車用信号機の黄ランプを表すピクチャボックス </param>
        /// <param name="pib_red">    車用信号機の赤ランプを表すピクチャボックス </param>
        /// <param name="pib_arrow">  矢印信号機を表すピクチャボックス           </param>
        /// <returns> 点灯状態更新に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool ChangeSignalLightOn(LightState state, PictureBox pib_green, PictureBox pib_yellow, PictureBox pib_red, PictureBox pib_arrow)
        {
            try
            {
                bool greVisible = false;  // 車用信号機の緑ランプを点灯する場合はtrue、それ以外の場合はfalse
                bool yelVisible = false;  // 車用信号機の黄ランプを点灯する場合はtrue、それ以外の場合はfalse
                bool redVisible = false;  // 車用信号機の赤ランプを点灯する場合はtrue、それ以外の場合はfalse
                bool arwVisible = false;  // 矢印信号機のランプを点灯する場合はtrue、　それ以外の場合はfalse
                if (state == LightState.Green)  greVisible = true;
                if (state == LightState.Yellow) yelVisible = true;
                if (state == LightState.Red || state == LightState.Arrow) redVisible = true;
                if (state == LightState.Arrow)  arwVisible = true;

                pib_green.Visible  = greVisible;                        // 車用信号機の緑ランプの点灯状態変更
                pib_yellow.Visible = yelVisible;                        // 車用信号機の黄ランプの点灯状態変更
                pib_red.Visible    = redVisible;                        // 車用信号機の赤ランプの点灯状態変更
                if (pib_arrow != null) pib_arrow.Visible = arwVisible;  // 矢印信号機の点灯状態変更
                return true;
            }
            catch (Exception ex)
            {
                string exStr = "車用信号機点灯状態の更新でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">      点灯状態を表す列挙型                         </param>
        /// <param name="lbl_greOne"> 歩行者用信号機の緑ランプを表す１つ目のラベル </param>
        /// <param name="lbl_greTwo"> 歩行者用信号機の緑ランプを表す２つ目のラベル </param>
        /// <param name="lbl_redOne"> 歩行者用信号機の赤ランプを表す１つ目のラベル </param>
        /// <param name="lbl_redTwo"> 歩行者用信号機の赤ランプを表す２つ目のラベル </param>
        /// <returns> 点灯状態更新に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool ChangePedesLightOn(LightState state, Label lbl_greOne, Label lbl_greTwo, Label lbl_redOne, Label lbl_redTwo)
        {
            try
            {
                Color greBackColor = Color.Black;  // 歩行者用信号機の緑ランプを点灯する場合はColor.Transparent、それ以外の場合はColor.Black
                Color redBackColor = Color.Black;  // 歩行者用信号機の赤ランプを点灯する場合はColor.Transparent、それ以外の場合はColor.Black
                if      (state == LightState.Green) greBackColor = Color.Transparent;
                else if (state == LightState.Red)   redBackColor = Color.Transparent;

                lbl_greOne.BackColor = greBackColor;  // １つ目歩行者用信号機の緑ランプの点灯状態変更
                lbl_redOne.BackColor = redBackColor;  // １つ目歩行者用信号機の赤ランプの点灯状態変更
                lbl_greTwo.BackColor = greBackColor;  // ２つ目歩行者用信号機の緑ランプの点灯状態変更
                lbl_redTwo.BackColor = redBackColor;  // ２つ目歩行者用信号機の赤ランプの点灯状態変更
                return true;
            }
            catch (Exception ex)
            {
                string exStr = "歩行者用信号機点灯状態の更新でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }
    }
}