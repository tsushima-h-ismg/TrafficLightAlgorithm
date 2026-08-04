using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using TrafficLightAlgorithm.Properties;

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
        private const int AllRedMSec 　　 = 1000;
        
        /// <summary>
        /// 歩行者用信号機点滅の合計フェーズ数
        /// </summary>
        private const int BlinkPhaseCount = 5;

        /// <summary>
        /// 時間設定一覧画面表示ボタンのデフォルト背景色
        /// </summary>
        private readonly Color BtnAllValuesDefaultColor = Color.FromArgb(200, 247, 242);

        /// <summary>
        /// 時間設定一覧画面表示ボタンの無効時背景色
        /// </summary>
        private readonly Color BtnAllValuesValidColor   = Color.FromArgb(220, 220, 220);

        /// <summary>
        /// 信号機アルゴリズムで中断が有効になった時点のフェーズ番号
        /// </summary>
        private int InterruptPhase;

        /// <summary>
        /// 信号機アルゴリズムでフェーズが再生されている・中断している場合はtrue、それ以外の場合はfalse
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
        /// 時間設定値
        /// </summary>
        private TrafficMSecValues MSecValues;

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
                IsInterrupt     = false;
                IsTrafficEnable = false;
                Cts             = new CancellationTokenSource();
                MSecValues      = new TrafficMSecValues(1000, 1000, 1000, 1000, 1000, 1000, 1000);

                // ピクチャボックスの信号機イメージ部分が、透明のラベルに表示されるようにする
                pib_NSignal.Controls.Add(lbl_NSignal);
                pib_SSignal.Controls.Add(lbl_SSignal);
                pib_ESignal.Controls.AddRange(new Control[] { lbl_ESignal, lbl_EArrow });
                pib_WSignal.Controls.AddRange(new Control[] { lbl_WSignal, lbl_WArrow });
                pib_PNSignalOne.Controls.Add(lbl_PNOne);
                pib_PNSignalTwo.Controls.Add(lbl_PNTwo);
                pib_PSSignalOne.Controls.Add(lbl_PSOne);
                pib_PSSignalTwo.Controls.Add(lbl_PSTwo);
                pib_PESignalOne.Controls.Add(lbl_PEOne);
                pib_PESignalTwo.Controls.Add(lbl_PETwo);
                pib_PWSignalOne.Controls.Add(lbl_PWOne);
                pib_PWSignalTwo.Controls.Add(lbl_PWTwo);

		        // ラベルと緑・黄・赤ピクチャボックスの背景色を一致させる
                lbl_NSignal.Controls.AddRange(new Control[] { pib_NGreen, pib_NYellow, pib_NRed });
                lbl_SSignal.Controls.AddRange(new Control[] { pib_SGreen, pib_SYellow, pib_SRed });
                lbl_ESignal.Controls.AddRange(new Control[] { pib_EGreen, pib_EYellow, pib_ERed });
                lbl_WSignal.Controls.AddRange(new Control[] { pib_WGreen, pib_WYellow, pib_WRed });
            }
            catch (Exception ex)
            {
                string exStr = "メイン画面の読み込みでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 信号機イメージ部分クリック時イベント
        /// </summary>
        private void Lbl_Signal_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsTrafficEnable) return;
                
                if (sender == lbl_NSignal)
                {
                    SetFormShow(MSecValues.CarNMSec, false, Signal.Car, Direction.North, pib_NSignal);  // 北車用信号機
                }
                else if (sender == lbl_SSignal)
                {
                    SetFormShow(MSecValues.CarSMSec, false, Signal.Car, Direction.South, pib_SSignal);  // 南車用信号機
                }
                else if (sender == lbl_ESignal || sender == lbl_EArrow)
                {
                    SetFormShow(MSecValues.CarEMSec, true, Signal.Car, Direction.East, pib_ESignal);  // 東車用信号機
                }
                else if (sender == lbl_WSignal || sender == lbl_WArrow)
                {
                    SetFormShow(MSecValues.CarWMSec, true, Signal.Car, Direction.West, pib_WSignal);  // 西車用信号機
                }
                else if (sender == lbl_PNOne || sender == lbl_PNGreOne || sender == lbl_PNRedOne)
                {
                    SetFormShow(MSecValues.PedNSMSec, false, Signal.Pedes, Direction.North, pib_PNSignalOne);  // 北歩行者用信号機
                }
                else if (sender == lbl_PNTwo || sender == lbl_PNGreTwo || sender == lbl_PNRedTwo)
                {
                    SetFormShow(MSecValues.PedNSMSec, false, Signal.Pedes, Direction.North, pib_PNSignalTwo);  // 北歩行者用信号機
                }
                else if (sender == lbl_PSOne || sender == lbl_PSGreOne || sender == lbl_PSRedOne)
                {
                    SetFormShow(MSecValues.PedNSMSec, false, Signal.Pedes, Direction.South, pib_PSSignalOne);  // 南歩行者用信号機
                }
                else if (sender == lbl_PSTwo || sender == lbl_PSGreTwo || sender == lbl_PSRedTwo)
                {
                    SetFormShow(MSecValues.PedNSMSec, false, Signal.Pedes, Direction.South, pib_PSSignalTwo);  // 南歩行者用信号機
                }
                else if (sender == lbl_PEOne || sender == lbl_PEGreOne || sender == lbl_PERedOne)
                {
                    SetFormShow(MSecValues.PedEWMSec, false, Signal.Pedes, Direction.East, pib_PESignalOne);  // 東歩行者用信号機
                }
                else if (sender == lbl_PETwo || sender == lbl_PEGreTwo || sender == lbl_PERedTwo)
                {
                    SetFormShow(MSecValues.PedEWMSec, false, Signal.Pedes, Direction.East, pib_PESignalTwo);  // 東歩行者用信号機
                }
                else if (sender == lbl_PWOne || sender == lbl_PWGreOne || sender == lbl_PWRedOne)
                {
                    SetFormShow(MSecValues.PedEWMSec, false, Signal.Pedes, Direction.West, pib_PWSignalOne);  // 西歩行者用信号機
                }
                else if (sender == lbl_PWTwo || sender == lbl_PWGreTwo || sender == lbl_PWRedTwo)
                {
                    SetFormShow(MSecValues.PedEWMSec, false, Signal.Pedes, Direction.West, pib_PWSignalTwo);  // 西歩行者用信号機
                }
            }
            catch (Exception ex)
            {
                string exStr = "信号機イメージ画像のクリックでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 時間設定値入力フォームを表示
        /// </summary>
        /// <param name="avaiMSec">  進行可能ミリ秒　                                    </param>
        /// <param name="isArrow">   矢印信号機を有する場合はtrue、それ以外の場合はfalse </param>
        /// <param name="signal">    信号機の種類                                        </param>
        /// <param name="direction"> 信号機設置方角                                　　　</param>
        /// <param name="pib">       信号機イメージ画像表示ピクチャボックス 　　　       </param>
        private void SetFormShow(int avaiMSec, bool isArrow, Signal signal, Direction direction, PictureBox pib)
        {
            try
            {
                // 信号機イメージ画像を強調表示した画像に変更して、時間設定値の編集開始を示す
                if (!ChangeSignalImage(signal, direction, true)) return;

                F_SetSec f_SetSec = new F_SetSec { AvaiSec = avaiMSec / 1000, 
                                                   ArrowSec = MSecValues.ArwMSec / 1000, 
                                                   IsArrow = isArrow, 
                                                   SetSignal = signal, 
                                                   SetDirection = direction };

                // 時間設定値入力フォームの初期表示位置設定
                Point pibTopLeft = pib.PointToScreen(Point.Empty);
                Rectangle scArea = Screen.FromPoint(pibTopLeft).WorkingArea;
                int xlocation = pibTopLeft.X + pib.Width;
                int ylocation = pibTopLeft.Y;
                if (xlocation > scArea.Right  - f_SetSec.Width)  xlocation = scArea.Right  - f_SetSec.Width;
                if (ylocation > scArea.Bottom - f_SetSec.Height) ylocation = scArea.Bottom - f_SetSec.Height;
                f_SetSec.Location = new Point(xlocation, ylocation);

                f_SetSec.ShowDialog();

                MSecValues = MSecValues.ChangeMSec(f_SetSec.AvaiSec * 1000,
                                                   f_SetSec.ArrowSec * 1000,
                                                   signal, direction);

                // 信号機イメージ画像をデフォルトの画像に戻して、時間設定値の編集終了を示す
                if (!ChangeSignalImage(signal, direction, false)) return;
            }
            catch (Exception ex)
            {
                string exStr = "設定値の入力でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 信号機イメージ画像の変更
        /// </summary>
        /// <param name="signal">    信号機の種類                                              </param>
        /// <param name="direction"> 信号機設置方角   　　                                     </param>
        /// <param name="isHilight"> 強調表示の画像に変更する場合はtrue、それ以外の場合はfalse </param>
        /// <returns> 信号機イメージ画像の変更に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool ChangeSignalImage(Signal signal, Direction direction, bool isHilight)
        {
            try
            {
                if (signal == Signal.Car)
                {
                    if      (direction == Direction.North) pib_NSignal.BackgroundImage = GetImg(Resources.NSignalHilight, Resources.SignalPoleN, isHilight);
                    else if (direction == Direction.South) pib_SSignal.BackgroundImage = GetImg(Resources.SSignalHilight, Resources.SignalPoleS, isHilight);
                    else if (direction == Direction.East)  pib_ESignal.BackgroundImage = GetImg(Resources.ESignalHilight, Resources.SignalPoleE, isHilight);
                    else if (direction == Direction.West)  pib_WSignal.BackgroundImage = GetImg(Resources.WSignalHilight, Resources.SignalPoleW, isHilight);
                    return true;
                }
                else if (signal == Signal.Pedes)
                {
                    if (direction == Direction.North || direction == Direction.South)
                    {
                        pib_PNSignalOne.BackgroundImage = GetImg(Resources.PedesNorthOneHilight, Resources.PedesNorthOneDefault, isHilight);
                        pib_PNSignalTwo.BackgroundImage = GetImg(Resources.PedesNorthTwoHilight, Resources.PedesNorthTwoDefault, isHilight);
                        pib_PSSignalOne.BackgroundImage = GetImg(Resources.PedesSouthOneHilight, Resources.PedesSouthOneDefault, isHilight);
                        pib_PSSignalTwo.BackgroundImage = GetImg(Resources.PedesSouthTwoHilight, Resources.PedesSouthTwoDefault, isHilight);
                    }
                    else if (direction == Direction.East || direction == Direction.West)
                    {
                        pib_PESignalOne.BackgroundImage = GetImg(Resources.PedesEastOneHilight, Resources.PedesEastOneDefault, isHilight);
                        pib_PESignalTwo.BackgroundImage = GetImg(Resources.PedesEastTwoHilight, Resources.PedesEastTwoDefault, isHilight);
                        pib_PWSignalOne.BackgroundImage = GetImg(Resources.PedesWestOneHilight, Resources.PedesWestOneDefault, isHilight);
                        pib_PWSignalTwo.BackgroundImage = GetImg(Resources.PedesWestTwoHilight, Resources.PedesWestTwoDefault, isHilight);
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                string exStr = "信号機イメージ画像の変更でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// 信号機イメージ画像を返す
        /// </summary>
        /// <param name="hilightImg"> 強調表示した信号機イメージ画像                        </param>
        /// <param name="defaultImg"> デフォルトの信号機イメージ画像                        </param>
        /// <param name="isHilight">  強調表示の画像を返す場合はtrue、それ以外の場合はfalse </param>
        /// <returns> 引数isHilightがtrueの場合は引数hilightImg、それ以外の場合は引数defaultImg </returns>
        private Bitmap GetImg(Bitmap hilightImg, Bitmap defaultImg, bool isHilight)
        {
            if (isHilight) return hilightImg;
            return defaultImg;
        }

        /// <summary>
        /// 「開始」ボタンクリック時イベント
        /// </summary>
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsTrafficEnable)
                {
                    string msgStr = "信号機プログラムを最初から実行しますか？";
                    if (MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;

                    Cts.Cancel();  // 複数のフェーズ再生が同時に実行されることを防ぐため、既に実行しているフェーズ再生を終了する
                }

                ChangeTextInterruptResumeBtn(false);
                PhaseList = CreateTrafficPhaseList(MSecValues);
                if (PhaseList == null || PhaseList.Contains(null)) return;  // フェーズリストにフェーズが存在しない、再生できない場合は終了

                IsTrafficEnable = true;
                IsInterrupt     = false;
                btn_SetAllValue.Enabled = false;
                btn_SetAllValue.BackColor = BtnAllValuesValidColor;  // ボタンの応答不可を背景色変更で示す
                ChangeChdCtlCursor(Cursors.Default);                 // 時間設定値の編集不可をカーソル変更で示す
                LoopTrafficPhase(0, PhaseList);                      // フェーズリストを最初のフェーズから再生
            }
            catch (Exception ex)
            {
                string exStr = "「開始」ボタンのクリックでエラーが発生しました。\n" + ex.Message;
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
                if (!IsTrafficEnable) return;

                if (IsInterrupt)
                {
                    IsInterrupt = false;

                    // 点滅の途中で中断した場合、フェーズ再生は点滅開始フェーズから再開する
                    if (PhaseList[InterruptPhase].IsBlink)
                    {
                        for (int i = InterruptPhase; i >= 0; i--)
                        {
                            if (PhaseList[i].IsBlinkStart)
                            {
                                InterruptPhase = i;
                                break;
                            }
                        }
                    }

                    LoopTrafficPhase(InterruptPhase, PhaseList);  // フェーズ再生を中断したフェーズから再開する
                }
                else
                {
                    IsInterrupt = true;
                    Cts.Cancel();  // フェーズ再生を中断する
                }

                ChangeTextInterruptResumeBtn(IsInterrupt);
            }
            catch (Exception ex)
            {
                string exStr = "「中断/再開」ボタンのクリックでエラーが発生しました。\n" + ex.Message;
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
		        if (!IsTrafficEnable) return;

                string msgStr = "信号機プログラムを停止し、信号機の点灯状態をリセットしますか？";
                if (MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;

                Cts.Cancel();

                // 全ての車用・歩行者用信号機を無灯火にする
                ChangeSignalLightOn(LightState.NoLight, pib_NGreen,  pib_NYellow,  pib_NRed,     null);
                ChangeSignalLightOn(LightState.NoLight, pib_SGreen,  pib_SYellow,  pib_SRed,     null);
                ChangeSignalLightOn(LightState.NoLight, pib_EGreen,  pib_EYellow,  pib_ERed,     pib_EArrow);
                ChangeSignalLightOn(LightState.NoLight, pib_WGreen,  pib_WYellow,  pib_WRed,     pib_WArrow);
                ChangePedesLightOn(LightState.NoLight, lbl_PNGreOne, lbl_PNGreTwo, lbl_PNRedOne, lbl_PNRedTwo);
                ChangePedesLightOn(LightState.NoLight, lbl_PSGreOne, lbl_PSGreTwo, lbl_PSRedOne, lbl_PSRedTwo);
                ChangePedesLightOn(LightState.NoLight, lbl_PEGreOne, lbl_PEGreTwo, lbl_PERedOne, lbl_PERedTwo);
                ChangePedesLightOn(LightState.NoLight, lbl_PWGreOne, lbl_PWGreTwo, lbl_PWRedOne, lbl_PWRedTwo);

                IsTrafficEnable = false;
                IsInterrupt     = false;
                btn_SetAllValue.Enabled = true;
                btn_SetAllValue.BackColor = BtnAllValuesDefaultColor;  // ボタンが応答可能になったことを背景色変更で示す
                ChangeChdCtlCursor(Cursors.Hand);                      // 時間設定値が編集可能になったことをカーソル変更で示す
                lbx_SignalControlLog.Items.Clear();                    // 信号機点灯の履歴をリセット
                ChangeTextInterruptResumeBtn(false);

                Cts.Dispose();
            }
            catch (Exception ex)
            {
                string exStr = "「リセット」ボタンのクリックでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// コントロールの表示カーソル変更
        /// </summary>
        /// <param name="cursor"> 変更後の表示カーソル </param>
        private void ChangeChdCtlCursor(Cursor cursor)
        {
            try
            {
                lbl_NSignal.Cursor = cursor;
                lbl_SSignal.Cursor = cursor;
                lbl_ESignal.Cursor = cursor;
                lbl_WSignal.Cursor = cursor;
                lbl_EArrow.Cursor  = cursor;
                lbl_WArrow.Cursor  = cursor;

                lbl_PNOne.Cursor    = cursor;
                lbl_PNGreOne.Cursor = cursor;
                lbl_PNRedOne.Cursor = cursor;
                lbl_PNTwo.Cursor    = cursor;
                lbl_PNGreTwo.Cursor = cursor;
                lbl_PNRedTwo.Cursor = cursor;

                lbl_PSOne.Cursor    = cursor;
                lbl_PSGreOne.Cursor = cursor;
                lbl_PSRedOne.Cursor = cursor;
                lbl_PSTwo.Cursor    = cursor;
                lbl_PSGreTwo.Cursor = cursor;
                lbl_PSRedTwo.Cursor = cursor;

                lbl_PEOne.Cursor    = cursor;
                lbl_PEGreOne.Cursor = cursor;
                lbl_PERedOne.Cursor = cursor;
                lbl_PETwo.Cursor    = cursor;
                lbl_PEGreTwo.Cursor = cursor;
                lbl_PERedTwo.Cursor = cursor;

                lbl_PWOne.Cursor    = cursor;
                lbl_PWGreOne.Cursor = cursor;
                lbl_PWRedOne.Cursor = cursor;
                lbl_PWTwo.Cursor    = cursor;
                lbl_PWGreTwo.Cursor = cursor;
                lbl_PWRedTwo.Cursor = cursor;
            }
            catch (Exception ex) 
            {
                string exStr = "表示カーソルの変更でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 時間設定一覧画面表示ボタンクリック時イベント
        /// </summary>
        private void Btn_SetAllValueShow_Click(object sender, EventArgs e)
        {
            try
            {
                F_SetAllValue f_SetAllValue = new F_SetAllValue { SetMSecValues = MSecValues };
                f_SetAllValue.ShowDialog();
                MSecValues = f_SetAllValue.SetMSecValues;
            }
            catch (Exception ex)
            {
                string exStr = "「時間設定一覧」画面表示ボタンのクリックでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// タイトル表示ラベルクリック時イベント
        /// </summary>
        private void Lbl_FormTitle_Click(object sender, EventArgs e)
        {
            try
            {
                F_Version f_Version = new F_Version();
                f_Version.ShowDialog();
            }
            catch (Exception ex)
            {
                string exStr = "タイトル部分のクリックでエラーが発生しました。\n" + ex.Message;
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
                if (isInterrupt) btn_InterruptResume.Text = "再開";
                else btn_InterruptResume.Text = "中断";
            }
            catch (Exception ex)
            {
                string exStr = "「中断/再開」ボタンテキストの変更でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストを作成
        /// </summary>
        /// <param name="mSecValues"> 信号機のミリ秒設定値 </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> CreateTrafficPhaseList(TrafficMSecValues mSecValues)
        {
            try
            {
                List<TrafficPhase> phases = new List<TrafficPhase>
                {
                    new TrafficPhase(AllRedMSec, new TrafficCommand[]{ new TrafficCommand(Direction.All, Signal.All, LightState.Red) })  // 全信号機の赤点灯フェーズ
                };

                phases.AddRange(DirectionPhaseList(mSecValues, Direction.NorthSouth, false));  // 交差点北南方向の点灯フェーズリスト追加
                phases.AddRange(DirectionPhaseList(mSecValues, Direction.EastWest,   true));   // 交差点東西方向の点灯フェーズリスト追加

                return phases;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 進行方向ごとにフェーズリスト作成
        /// </summary>
        /// <param name="mSecValues"> 信号機のミリ秒設定値                                  </param>
        /// <param name="direction">  交差点内の進行方向                　　　　　　　　　　</param>
        /// <param name="isArrow">    矢印信号機が存在する場合はtrue、それ以外の場合はfalse </param>
        /// <returns> 作成した点灯フェーズリスト </returns>
        private List<TrafficPhase> DirectionPhaseList(TrafficMSecValues mSecValues, Direction direction, bool isArrow)
        {
            try
            {
                int carOneAllMSec = mSecValues.CarNMSec  + YellowMSec;                   // １つ目車用信号機が赤に点灯するまでのミリ秒
                int carTwoAllMSec = mSecValues.CarSMSec  + YellowMSec;                   // ２つ目車用信号機が赤に点灯するまでのミリ秒
                int pedAllMSec    = mSecValues.PedEWMSec + BlinkMSec * BlinkPhaseCount;  // 　歩行者用信号機が赤に点灯するまでのミリ秒
                Direction cOneDir      = Direction.North;                                // １つ目車用信号機の設置方角
                Direction cTwoDir      = Direction.South;                                // ２つ目車用信号機の設置方角
                Direction pedDirection = Direction.EastWest;                             // 　歩行者用信号機の設置方角

                if (direction == Direction.EastWest)
                {
                    carOneAllMSec = mSecValues.CarEMSec + YellowMSec;
                    carTwoAllMSec = mSecValues.CarWMSec + YellowMSec;
                    pedAllMSec    = mSecValues.PedNSMSec + BlinkMSec * BlinkPhaseCount;
                    cOneDir      = Direction.East;
                    cTwoDir      = Direction.West;
                    pedDirection = Direction.NorthSouth;
                }

                if (isArrow)
                {
                    carOneAllMSec += MinMSec + mSecValues.ArwMSec;
                    carTwoAllMSec += MinMSec + mSecValues.ArwMSec;
                }

                // 進行方向で車用・歩行者用信号機が赤に点灯するまでのミリ秒
                int finishMSec = Math.Max(Math.Max(carOneAllMSec, carTwoAllMSec), pedAllMSec) + MinMSec;

                List<TrafficPhase> pList = new List<TrafficPhase>();  // 信号機点灯フェーズリスト

                TrafficCommand[] cmdArr    = null;  // 経過ミリ秒ごとの点灯状態
                TrafficCommand[] befcmdArr = null;  // cmdArrの500ミリ秒前の点灯状態

                // 最後にフェーズリストに追加されたフェーズの点灯状態
                TrafficCommand[] lastcmdArr = new TrafficCommand[] { new TrafficCommand(cOneDir,      Signal.Car,   LightState.Red),
                                                                     new TrafficCommand(cTwoDir,      Signal.Car,   LightState.Red),
                                                                     new TrafficCommand(pedDirection, Signal.Pedes, LightState.Red) };

                int  waitMSec     = 0;      // 点灯フェーズの待機ミリ秒
                bool isBlinkStart = false;  // 歩行者用信号機点滅開始フェーズの場合はtrue、それ以外ではfalse
                bool isBlink      = false;  // 歩行者用信号機点滅フェーズの場合はtrue、それ以外ではfalse
                bool cmdMatch     = true;   // cmdArrとbefcmdArrの点灯状態が一致する場合はtrue、それ以外ではfalse

                // 500ミリ秒ごとに車用・歩行者用信号機の点灯状態を取得する
                for (int elap_msec = 0; elap_msec <= finishMSec; elap_msec += BlinkMSec)
                {
                    cmdArr = new TrafficCommand[]
                    {
                        GetTrafficCmd(elap_msec, mSecValues, Signal.Car,   cOneDir,      isArrow),  // １つ目車用信号機の点灯状態
                        GetTrafficCmd(elap_msec, mSecValues, Signal.Car,   cTwoDir,      isArrow),  // ２つ目車用信号機の点灯状態
                        GetTrafficCmd(elap_msec, mSecValues, Signal.Pedes, pedDirection, false)     //   歩行者用信号機の点灯状態
                    };

                    List<TrafficCommand> cmdList = new List<TrafficCommand>();  // 信号機点灯フェーズに入れるTrafficCommandのリスト
                    cmdMatch = true;

                    if (befcmdArr != null)
                    {
                        // befcmdArrとcmdArrの点灯状態が全て一致するか判定
                        for (int i = 0; i < befcmdArr.Length; i++)
                        {
                            if (befcmdArr[i].State != cmdArr[i].State)
                            {
                                cmdMatch = false;
                                break;
                            }
                        }

                        // 信号機点灯フェーズに追加するTrafficCommandのリストを作成
                        for (int j = 0; j < befcmdArr.Length; j++)
                        {
                            if (befcmdArr[j].State != lastcmdArr[j].State)
                            {
                                cmdList.Add(befcmdArr[j]);

                                if (j > 0)
                                {
                                    // ２つの車用信号機の点灯状態が同じ場合１つのTrafficCommandにまとめる
                                    if (befcmdArr[j].State  == befcmdArr[j - 1].State && 
                                        befcmdArr[j].Signal == befcmdArr[j - 1].Signal)
                                    {
                                        cmdList = new List<TrafficCommand> { new TrafficCommand(direction, Signal.Car, befcmdArr[j].State) };
                                    }
                                }
                            }
                        }
                    }

                    if (elap_msec == finishMSec)
                    {
                        if (!(direction == Direction.EastWest && cmdList[0].Signal == Signal.Pedes))
                        {
                            pList.Add(new TrafficPhase(MinMSec, cmdList.ToArray()));  // フェーズリストに点灯フェーズを追加                       
                        }
                    }
                    else if (cmdMatch)
                    {
                        waitMSec += BlinkMSec;
                        befcmdArr = cmdArr;
                    }
                    else
                    {
                        // フェーズリストに点灯フェーズを追加
                        pList.Add(new TrafficPhase(waitMSec, cmdList.ToArray(), isBlinkStart, isBlink, BlinkPhaseCount));
                        waitMSec = BlinkMSec;

                        // 歩行者用信号機の点滅状態を更新
                        isBlink      = false;
                        isBlinkStart = false;
                        if (elap_msec >= mSecValues.PedEWMSec && elap_msec < pedAllMSec) isBlink = true;
                        if (elap_msec == mSecValues.PedEWMSec) isBlinkStart = true;

                        lastcmdArr = befcmdArr;
                        befcmdArr  = cmdArr;
                    }
                }

                return pList;
            }
            catch (Exception ex)
            {
                string exStr = "信号機点灯の順番表の作成でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>
        /// 信号機の点灯状態・種類・設置方角を取得
        /// </summary>
        /// <param name="elapMSec">   信号機点灯処理の経過ミリ秒                            </param>
        /// <param name="mSecValues"> 信号機のミリ秒設定値    　　　                        </param>
        /// <param name="signal">     信号機の種類                                          </param>
        /// <param name="direction">  信号機設置方角                                        </param>
        /// <param name="isArrow">    矢印信号機が存在する場合はtrue、それ以外の場合はfalse </param>
        /// <returns> 信号機の点灯状態・種類・設置方角 </returns>
        private TrafficCommand GetTrafficCmd(int elapMSec, TrafficMSecValues mSecValues, Signal signal, Direction direction, bool isArrow)
        {
            try
            {
                LightState lightState = LightState.NoLight;

                if (signal == Signal.Car)
                {
                    // 車用信号機の点灯状態変更
                    int carOneMSec = mSecValues.CarNMSec;
                    int carTwoMSec = mSecValues.CarSMSec;
                    
                    if      (direction == Direction.South) (carOneMSec, carTwoMSec) = (mSecValues.CarSMSec, mSecValues.CarNMSec);
                    else if (direction == Direction.East)  (carOneMSec, carTwoMSec) = (mSecValues.CarEMSec, mSecValues.CarWMSec);
                    else if (direction == Direction.West)  (carOneMSec, carTwoMSec) = (mSecValues.CarWMSec, mSecValues.CarEMSec);

                    if (elapMSec < carOneMSec)
                    {
                        lightState = LightState.Green;
                    }
                    else if (elapMSec >= carOneMSec && elapMSec < carOneMSec + YellowMSec)
                    {
                        lightState = LightState.Yellow;
                    }
                    else if (elapMSec >= carOneMSec + YellowMSec)
                    {
                        if (isArrow)
                        {
                            // 矢印信号機が存在する場合
                            if (elapMSec < Math.Max(carOneMSec, carTwoMSec) + YellowMSec + MinMSec)
                            {
                                lightState = LightState.Red;
                            }
                            else if (elapMSec < Math.Max(carOneMSec, carTwoMSec) + YellowMSec + MinMSec + mSecValues.ArwMSec)
                            {
                                lightState = LightState.ArrowRed;
                            }
                            else if (elapMSec < Math.Max(carOneMSec, carTwoMSec) + YellowMSec + MinMSec + mSecValues.ArwMSec + YellowMSec)
                            {
                                lightState = LightState.Yellow;
                            }
                            else if (elapMSec >= Math.Max(carOneMSec, carTwoMSec) + YellowMSec + MinMSec + mSecValues.ArwMSec + YellowMSec)
                            {
                                lightState = LightState.Red;
                            }
                        }
                        else
                        {
                            // 矢印信号機が存在しない場合
                            lightState = LightState.Red;
                        }
                    }
                }
                else if (signal == Signal.Pedes)
                {
                    // 歩行者用信号機の点灯状態変更
                    int pedMSec = mSecValues.PedNSMSec;
                    if (direction == Direction.EastWest) pedMSec = mSecValues.PedEWMSec;

                    int pedAllMSec = pedMSec + BlinkMSec * BlinkPhaseCount;

                    if (elapMSec < pedMSec)
                    {
                        lightState = LightState.Green;
                    }
                    else if (elapMSec >= pedMSec && elapMSec < pedAllMSec)
                    {
                        // 点滅
                        for (int i = 0; i < BlinkPhaseCount; i++)
                        {
                            if (elapMSec == pedMSec + BlinkMSec * i)
                            {
                                if      (i % 2 == 0) lightState = LightState.NoLight;
                                else if (i % 2 == 1) lightState = LightState.Green;
                            }
                        }
                    }
                    else if (elapMSec >= pedAllMSec)
                    {
                        lightState = LightState.Red;
                    }
                }

                return new TrafficCommand(direction, signal, lightState);
            }
            catch (Exception ex)
            {
                string exStr = "信号機点灯の順番表の作成でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>
        /// フェーズリスト再生
        /// </summary>
        /// <param name="phaseNum"> 最初に再生するフェーズの番号           </param>
        /// <param name="phases">   再生するフェーズが入ったフェーズリスト </param>
        private async void LoopTrafficPhase(int phaseNum, List<TrafficPhase> phases)
        {
            try
            {
                int  startPhase  = phaseNum;  // ループで最初に再生するフェーズの番号
                bool isCarChange = false;     // フェーズに車用信号機の点灯状態変更が含まれる場合はtrue、それ以外の場合はfalse
                Cts = new CancellationTokenSource();

                while (!Cts.IsCancellationRequested)
                {
                    for (int i = startPhase; i < phases.Count; i++)
                    {
                        InterruptPhase = i;
                        isCarChange = false;

                        foreach (TrafficCommand cmd in phases[i].Commands)
                        {
                            // 車用信号機の点灯状態更新
                            if (cmd.Signal == Signal.All || cmd.Signal == Signal.Car)
                            {
                                isCarChange = true;

                                if (cmd.Direction == Direction.All || cmd.Direction == Direction.NorthSouth || cmd.Direction == Direction.North)
                                {
                                    if (!ChangeSignalLightOn(cmd.State, pib_NGreen, pib_NYellow, pib_NRed, null)) return;
                                }

                                if (cmd.Direction == Direction.All || cmd.Direction == Direction.NorthSouth || cmd.Direction == Direction.South)
                                {
                                    if (!ChangeSignalLightOn(cmd.State, pib_SGreen, pib_SYellow, pib_SRed, null)) return;
                                }

                                if (cmd.Direction == Direction.All || cmd.Direction == Direction.EastWest || cmd.Direction == Direction.East)
                                {
                                    if (!ChangeSignalLightOn(cmd.State, pib_EGreen, pib_EYellow, pib_ERed, pib_EArrow)) return;
                                }

                                if (cmd.Direction == Direction.All || cmd.Direction == Direction.EastWest || cmd.Direction == Direction.West)
                                {
                                    if (!ChangeSignalLightOn(cmd.State, pib_WGreen, pib_WYellow, pib_WRed, pib_WArrow)) return;
                                }
                            }

                            // 歩行者用信号機の点灯状態更新
                            if (cmd.Signal == Signal.All || cmd.Signal == Signal.Pedes)
                            {
                                if (cmd.Direction == Direction.All || cmd.Direction == Direction.NorthSouth)
                                {
                                    if (!ChangePedesLightOn(cmd.State, lbl_PNGreOne, lbl_PNGreTwo, lbl_PNRedOne, lbl_PNRedTwo)) return;
                                    if (!ChangePedesLightOn(cmd.State, lbl_PSGreOne, lbl_PSGreTwo, lbl_PSRedOne, lbl_PSRedTwo)) return;
                                }

                                if (cmd.Direction == Direction.All || cmd.Direction == Direction.EastWest)
                                {
                                    if (!ChangePedesLightOn(cmd.State, lbl_PEGreOne, lbl_PEGreTwo, lbl_PERedOne, lbl_PERedTwo)) return;
                                    if (!ChangePedesLightOn(cmd.State, lbl_PWGreOne, lbl_PWGreTwo, lbl_PWRedOne, lbl_PWRedTwo)) return;
                                }
                            }
                        }

                        // 車用信号機の点灯状態が変更される、もしくは点滅開始フェーズの場合、点灯状態変更内容を履歴に追加
                        if (isCarChange || !phases[i].IsBlink || (phases[i].IsBlink && phases[i].IsBlinkStart))
                        {
                            lbx_SignalControlLog.Items.Add(lbx_SignalControlLog.Items.Count + "：" + phases[i].GetMsg());
                            
                            lbx_SignalControlLog.TopIndex = lbx_SignalControlLog.Items.Count - 1;  // 最新の履歴が表示されるようにスクロールを調整
                        }

                        try
                        {
                            await Task.Delay(phases[i].WaitMSec, Cts.Token);
                        }
                        catch
                        {
                            return;
                        }
                    }

                    startPhase = 0;  // 信号機点灯が一巡した後、次はフェーズリストの最初のフェーズから再生する
                }
            }
            catch (Exception ex)
            {
                string exStr = "信号機の点灯状態の変更でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 車用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">   信号機の点灯状態                           </param>
        /// <param name="pib_gre"> 車用信号機の緑ランプを表すピクチャボックス </param>
        /// <param name="pib_yel"> 車用信号機の黄ランプを表すピクチャボックス </param>
        /// <param name="pib_red"> 車用信号機の赤ランプを表すピクチャボックス </param>
        /// <param name="pib_arw"> 矢印信号機を表すピクチャボックス           </param>
        /// <returns> 点灯状態更新に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool ChangeSignalLightOn(LightState state, PictureBox pib_gre, PictureBox pib_yel, PictureBox pib_red, PictureBox pib_arw)
        {
            try
            {
                bool greVisible = false;  // 車用信号機の緑ランプを点灯する場合はtrue、それ以外の場合はfalse
                bool yelVisible = false;  // 車用信号機の黄ランプを点灯する場合はtrue、それ以外の場合はfalse
                bool redVisible = false;  // 車用信号機の赤ランプを点灯する場合はtrue、それ以外の場合はfalse
                bool arwVisible = false;  // 矢印信号機のランプを点灯する場合はtrue、　それ以外の場合はfalse
                if (state == LightState.Green) greVisible = true;
                if (state == LightState.Yellow) yelVisible = true;
                if (state == LightState.Red || state == LightState.ArrowRed) redVisible = true;
                if (state == LightState.ArrowRed) arwVisible = true;

                pib_gre.Visible = greVisible;                     
                pib_yel.Visible = yelVisible;                     
                pib_red.Visible = redVisible;                     
                if (pib_arw != null) pib_arw.Visible = arwVisible;
                return true;
            }
            catch (Exception ex)
            {
                string exStr = "車用信号機の点灯状態の更新でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">      信号機の点灯状態　　                         </param>
        /// <param name="lbl_greOne"> 歩行者用信号機の緑ランプを表す１つ目のラベル </param>
        /// <param name="lbl_greTwo"> 歩行者用信号機の緑ランプを表す２つ目のラベル </param>
        /// <param name="lbl_redOne"> 歩行者用信号機の赤ランプを表す１つ目のラベル </param>
        /// <param name="lbl_redTwo"> 歩行者用信号機の赤ランプを表す２つ目のラベル </param>
        /// <returns> 点灯状態更新に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool ChangePedesLightOn(LightState state, Label lbl_greOne, Label lbl_greTwo, Label lbl_redOne, Label lbl_redTwo)
        {
            try
            {
                bool greNotVisible = true;  // 歩行者用信号機の緑ランプを点灯しない場合はtrue、点灯する場合はfalse
                bool redNotVisible = true;  // 歩行者用信号機の赤ランプを点灯しない場合はtrue、点灯する場合はfalse
                if      (state == LightState.Green) greNotVisible = false;
                else if (state == LightState.Red)   redNotVisible = false;

                lbl_greOne.Visible = greNotVisible;
                lbl_greTwo.Visible = greNotVisible;
                lbl_redOne.Visible = redNotVisible;
                lbl_redTwo.Visible = redNotVisible;

                return true;
            }
            catch (Exception ex)
            {
                string exStr = "歩行者用信号機の点灯状態の更新でエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }
    }
}